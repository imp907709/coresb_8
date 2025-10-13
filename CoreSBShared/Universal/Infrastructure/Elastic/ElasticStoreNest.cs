using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreSBShared.Registrations;
using CoreSBShared.Universal.Infrastructure.Interfaces;
using Elasticsearch.Net;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Nest;

namespace CoreSBShared.Universal.Infrastructure.Elastic
{
    public class ElasticStoreNest : IElasticStoreNest
    {
        private readonly ElasticClient _client;

        public ElasticStoreNest(string? connStr, string? defaultIndex)
        {
            var pool = string.IsNullOrEmpty(connStr)
                ? new SingleNodeConnectionPool(new Uri(DefaultConfigurationValues.DefaultElasticConnStr))
                : new SingleNodeConnectionPool(new Uri(connStr));

            var idxName = string.IsNullOrEmpty(defaultIndex)
                ? DefaultConfigurationValues.DefaultElasticIndex
                : defaultIndex;

            var connSettings = new ConnectionSettings(pool).DefaultIndex(idxName);

            _client = new ElasticClient(connSettings);
            SetIndex(idxName);
        }

        private string _indexName { get; set; }

        public void SetIndex(string indexName)
        {
            _indexName = indexName;
        }

        public string CreateindexIfNotExists<T>(string indexName)
            where T : class, ICoreDalGnStr
        {
            if (string.IsNullOrEmpty(indexName))
            {
                return "No index info provided";
            }

            var indexExists = _client.Indices.Exists(indexName).Exists;

            if (!indexExists)
            {
                var createIndexResponse = _client.Indices.Create(indexName, c => c
                    .Map<T>(m => m.AutoMap())
                );

                if (!createIndexResponse.IsValid)
                {
                    return $"Index not created: {createIndexResponse.DebugInformation}";
                }

                return $"Index created: {createIndexResponse.DebugInformation}";
            }

            return $"indexExists: {indexExists}";
        }


        public async Task<T> GetByIdAsync<T, TKey>(TKey id) where T : class, ICoreDal<TKey>
        {
            var response = await _client.GetAsync<T>(id.ToString(), idx => idx.Index(_indexName));
            return BsonSerializer.Deserialize<T>(response.ToBsonDocument());
        }

        public async Task<T?> GetByIdAsync<T>(string id) where T : class, ICoreDalGnStr
        {
            var result = await _client.GetAsync<T>(id);
            return result.Source;
        }

        public async Task<IndexResponse> AddAsyncElk<T>(T item) where T : class
        {
            var response = await _client.IndexAsync(item, idx => idx.Index<T>());
            return response;
        }

        public async Task<T> AddAsync<T>(T item) where T : class
        {
            var response = await _client.IndexAsync(item, idx => idx.Index<T>());
            if (response.IsValid && response.Result == Result.Created)
            {
                return item;
            }

            return null;
        }

        public async Task<IEnumerable<T>> AddManyAsync<T>(IEnumerable<T> items) where T : class
        {
            var response = await _client.IndexManyAsync(items, _indexName);
            return BsonSerializer.Deserialize<IEnumerable<T>>(response.Items.ToBsonDocument());
        }


        public async Task<IEnumerable<T>> GetByFilterAsync<T>(Expression<Func<T, bool>> expression)
            where T : class, ICoreDalGnStr
        {
            var searchResponse = await _client.SearchAsync<T>(s => s
                .Index(_indexName)
                .Query(q => q.MatchAll()));
            return searchResponse.Documents;
        }

        public async Task<T> UpdateAsync<T>(T item) where T : class
        {
            var indexResponse = await _client
                .IndexAsync(item, idx => idx.Index(_indexName));
            return BsonSerializer.Deserialize<T>(indexResponse.ToBsonDocument());
        }

        public async Task<bool> DeleteAsync<T>(T item) where T : class, ICoreDalGnStr
        {
            var response = await _client.DeleteAsync<T>(item.Id,
                d => d.Index(_indexName));
            return response.Result == Result.Deleted;
        }

        public async Task<bool> DeleteManyAsync<T>(IEnumerable<T> items) where T : class, ICoreDalGnStr
        {
            var response = await _client.DeleteByQueryAsync<T>(d => d
                .Index(_indexName)
                .Query(q => q
                    .Ids(i => i
                        .Values(items.Select(s => s.Id))
                    )
                )
            );
            return response?.Deleted > 0;
        }

        public void CreateDB()
        {
            if (!_client.Indices.Exists(_indexName).Exists)
            {
                _client.Indices.Create(_indexName);
            }
        }

        public void DropDB()
        {
            if (_client.Indices.Exists(_indexName).Exists)
            {
                _client.Indices.Delete(_indexName);
            }
        }

        public string SetIndex<T>(string indexName)
            where T : class, ICoreDalGnStr
        {
            _indexName = indexName;
            return CreateindexIfNotExists<T>(_indexName);
        }

        private T ReturnDocument<T>(T doc) where T : BsonDocument
        {
            return BsonSerializer.Deserialize<T>(doc);
        }

        private IEnumerable<T> ReturnDocument<T>(List<T> doc) where T : BsonDocument
        {
            var result = doc.Select(s => BsonSerializer.Deserialize<T>(s));
            return result;
        }

        private IndexDescriptor<T> BuildIndexDescriptor<T>(string _indexName) where T : class, ICoreDalGnStr
        {
            var index = IndexName.From<T>(_indexName);
            var indexDescriptor = new IndexDescriptor<T>(index);
            return indexDescriptor;
        }
    }
}
