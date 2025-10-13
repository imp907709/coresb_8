using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreSBShared.Universal.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoreSBShared.Universal.Infrastructure.EF
{
    //GN
    //class lvl
    public interface IEFStore<T, K> : IStore
        where T : ICoreDal<K>
    {
        Task<T> AddAsync(T item);
        Task<IEnumerable<T>> AddManyAsync(IEnumerable<T> items);

        Task<T?> GetByIdAsync(K id);
        Task<IEnumerable<T>> GetByFilterAsync(Expression<Func<T, bool>> expression);

        Task<T> UpdateAsync(T item);

        Task<bool> DeleteAsync(T item);
        Task<IEnumerable<T>> DeleteManyAsync(IEnumerable<T> items);
    }

    //GN
    //Method lvl
    //method level store generic id int
    public interface IEFStoreG
    {
        Task<T> GetByIdAsync<T, K>(K id)
            where T : class, ICoreDal<K>;

        Task<T> AddAsync<T, K>(T item)
            where T : class, ICoreDal<K>;

        Task<IEnumerable<T>> AddManyAsync<T, K>(IEnumerable<T> items)
            where T : class, ICoreDal<K>;

        Task<IEnumerable<T>> GetByFilterAsync<T, K>(Expression<Func<T, bool>> expression)
            where T : class, ICoreDal<K>;

        Task<T> UpdateAsync<T, K>(T item)
            where T : class, ICoreDal<K>;

        Task<bool> DeleteAsync<T, K>(T item)
            where T : class, ICoreDal<K>;

        Task<IEnumerable<T>> DeleteManyAsync<T, K>(IEnumerable<T> items)
            where T : class, ICoreDal<K>;

        void CreateDB();
        void DropDB();
    }
    
    //TS via GN
    //Method lvl
    public interface IEFStore : IStore
    {
        Task<T> AddAsync<T>(T item) where T : class;
        Task<IEnumerable<T>> AddManyAsync<T>(IEnumerable<T> items) where T : class;
   
        Task<T?> GetByIdAsync<T>(int id) where T : class, ICoreDalGnInt;
        Task<IEnumerable<T>> GetByFilterAsync<T>(Expression<Func<T, bool>> expression) where T : class, ICoreDalGnInt;
        
        Task<T> UpdateAsync<T>(T item) where T : class;
        
        Task<bool> DeleteAsync<T>(T item) where T : class;
        Task<IEnumerable<T>> DeleteManyAsync<T>(IEnumerable<T> items) where T : class;
    }

    //!!!failed on EF insert
    //TS via GN
    //class lvl
    public interface IEFStoreInt : IEFStore<ICoreDalGnInt, int>
    {
    }
}

namespace CoreSBShared.Universal.Infrastructure.EF.Store
{
    public interface IEFStoreCore<TContext> : IStore
        where TContext : DbContext
    {
    }

    public interface IEFStoreGeneric<TContext>
        where TContext : DbContext
    {
        Task<T> AddItemAsync<T>(T item)
            where T : class;

        Task<int> SaveChangesAsync();

        Task<bool> CreateDB();

        Task<bool> DropDB();
    }
}
