using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreSBBL.Logging.Infrastructure.Generic;
using CoreSBBL.Logging.Infrastructure.TS;
using CoreSBBL.Logging.Infrastructure.Mongo;
using CoreSBBL.Logging.Models.DAL.GN;
using CoreSBBL.Logging.Models.TC.BL;
using CoreSBBL.Logging.Models.DAL.TS;
using CoreSBBL.Logging.Models.TC.DAL;
using CoreSBShared.Universal.Infrastructure.EF;
using CoreSBShared.Universal.Infrastructure.Elastic;
using CoreSBShared.Universal.Infrastructure.Interfaces;
using CoreSBShared.Universal.Infrastructure.Models;
using CoreSBShared.Universal.Infrastructure.Mongo;

namespace CoreSBBL.Logging.Services
{
    public class LoggingService : ILoggingServiceNew
    {
        private readonly ILogsServiceGeneric _logsServiceGeneric;
        
        private IMongoStore _mongoStore { get; }
        private IElasticStoreNest _elasticStore { get; }
        
        public LoggingService(ILogsServiceGeneric logsServiceGeneric)
        {
            _logsServiceGeneric = logsServiceGeneric;
        }
        
        public async Task<LoggingResp> AddToAll(LoggingGenericBLAdd item)
        {
            await _logsServiceGeneric.CheckCreated();
           
            
            var toAdd = new LoggingGenericBLAdd() { Message = item.Message, CreatedBy = "Default"};
            var resp = await _logsServiceGeneric.AddItem(toAdd);

            var secondItem = await _logsServiceGeneric.AddToSecond(toAdd);
            
            var toAddGuid = new LoggingGenericGuid() { Message = item.Message};
            var respGuid = await _logsServiceGeneric.AddItem(toAddGuid);
            
            var ret = new LoggingGenericBLGetInt() {Id = resp.Id, Created = resp.Created, Modified = resp.Modified};

            
            return new LoggingResp(){name = "Ids added", id1 = resp?.Id, id2 = secondItem?.Id, id3 = respGuid?.Id} ;
        }

        public async Task<bool> RecreateDB()
        {
            await _logsServiceGeneric.RecreateDB2();
            return await _logsServiceGeneric.RecreateDB();
            
        }

        public async Task Add(LogsBL item)
        {
            _mongoStore.CreateDB();

            _elasticStore.CreateDB();

            
            
            LogsMongo _itemMng = new () { Message = item.Message 
                ,Label = new LabelMongo(){Text= "label 1" }
                ,Tags = new List<TagMongo>(){new (){Text = "tag 1"},new (){Text = "tag 2"}}};
            var resp2 = await _mongoStore.AddAsync(_itemMng);

            LogsElk _call = new () { Message = item.Message ?? DefaultModelValues.Logging.MessageEmpty 
                ,Label = new LabelElk(){Text= "label 1" }
                ,Tags = new List<TagElk>(){new (){Text = "tag 1"},new (){Text = "tag 2"}}};
            try
            {
                var res = _elasticStore.CreateindexIfNotExists<LogsElk>(DefaultConfigurationValues
                    .DefaultElasticIndex);
                var respelk = await _elasticStore.AddAsyncElk(_call);
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
