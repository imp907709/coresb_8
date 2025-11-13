using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreSBShared.Universal.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoreSBShared.Universal.Infrastructure.EF
{
    //GN
    //Class level store generic id
    public class EFStoreG<T, K> : IEFStore<T, K>
        where T : class, ICoreDal<K>
    {
        private readonly DbContext _dbContext;

        public EFStoreG(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> GetByIdAsync(K id)
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<T> AddAsync(T item)
        {
            await _dbContext.Set<T>().AddAsync(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task<IEnumerable<T>> AddManyAsync(IEnumerable<T> items)
        {
            await _dbContext.Set<T>().AddRangeAsync(items);
            await _dbContext.SaveChangesAsync();
            return items;
        }

        public async Task<IEnumerable<T>> GetByFilterAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbContext.Set<T>().Where(expression).ToListAsync();
        }

        public async Task<T> UpdateAsync(T item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task<bool> DeleteAsync(T item)
        {
            _dbContext.Set<T>().Remove(item);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<T>> DeleteManyAsync(IEnumerable<T> items)
        {
            _dbContext.Set<T>().RemoveRange(items);
            await _dbContext.SaveChangesAsync();
            return items;
        }

        public void CreateDB()
        {
            _dbContext.Database.EnsureCreated();
        }

        public void DropDB()
        {
            _dbContext.Database.EnsureDeleted();
        }
    }

    //GN
    //Method lvl
    public class EFStoreG : IEFStoreG
    {
        private readonly DbContext _dbContext;

        public EFStoreG(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> GetByIdAsync<T, K>(K id)
            where T : class, ICoreDal<K>
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<T> AddAsync<T, K>(T item)
            where T : class, ICoreDal<K>
        {
            await _dbContext.Set<T>().AddAsync(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task<IEnumerable<T>> AddManyAsync<T, K>(IEnumerable<T> items)
            where T : class, ICoreDal<K>
        {
            await _dbContext.Set<T>().AddRangeAsync(items);
            await _dbContext.SaveChangesAsync();
            return items;
        }

        public async Task<IEnumerable<T>> GetByFilterAsync<T, K>(Expression<Func<T, bool>> expression)
            where T : class, ICoreDal<K>
        {
            return await _dbContext.Set<T>().Where(expression).ToListAsync();
        }

        public async Task<T> UpdateAsync<T, K>(T item)
            where T : class, ICoreDal<K>
        {
            _dbContext.Entry(item).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task<bool> DeleteAsync<T, K>(T item)
            where T : class, ICoreDal<K>
        {
            _dbContext.Set<T>().Remove(item);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<T>> DeleteManyAsync<T, K>(IEnumerable<T> items)
            where T : class, ICoreDal<K>
        {
            _dbContext.Set<T>().RemoveRange(items);
            await _dbContext.SaveChangesAsync();
            return items;
        }

        public void CreateDB()
        {
            _dbContext.Database.EnsureCreated();
        }

        public void DropDB()
        {
            _dbContext.Database.EnsureDeleted();
        }
    }

    //TS via GN
    //Method level store, type specific id
    //Default method based int id store - general SQL usage
    public class EFStore : IEFStore
    {
        private readonly DbContext _dbContext;

        public EFStore(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> GetByIdAsync<T, TKey>(TKey id) where T : class, ICoreDal<TKey>
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<T?> GetByIdAsync<T>(int id) where T : class, ICoreDalGnInt
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        }


        public async Task<T> AddAsync<T>(T item) where T : class
        {
            await _dbContext.Set<T>().AddAsync(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task<IEnumerable<T>> AddManyAsync<T>(IEnumerable<T> items) where T : class
        {
            await _dbContext.Set<T>().AddRangeAsync(items);
            await _dbContext.SaveChangesAsync();
            return items;
        }

        public async Task<IEnumerable<T>> GetByFilterAsync<T>(Expression<Func<T, bool>> expression)
            where T : class, ICoreDalGnInt
        {
            return await _dbContext.Set<T>().Where(expression).ToListAsync();
        }

        public async Task<T> UpdateAsync<T>(T item) where T : class
        {
            _dbContext.Entry(item).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task<bool> DeleteAsync<T>(T item) where T : class
        {
            _dbContext.Set<T>().Remove(item);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<T>> DeleteManyAsync<T>(IEnumerable<T> items) where T : class
        {
            _dbContext.Set<T>().RemoveRange(items);
            await _dbContext.SaveChangesAsync();
            return items;
        }

        public void CreateDB()
        {
            _dbContext.Database.EnsureCreated();
        }

        public void DropDB()
        {
            _dbContext.Database.EnsureDeleted();
        }
    }
    
    //!!!failed on EF insert
    //TS via GN
    //class lvl
    //int id
    public class EFStoreGInt : EFStoreG<ICoreDalGnInt, int>, IEFStoreInt
    {
        private readonly DbContext _dbContext;

        public EFStoreGInt(DbContext dbContext)
            :base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}

namespace CoreSBShared.Universal.Infrastructure.EF.Store
{
    public class EFStoreGeneric<TContext> 
        : IEFStoreGeneric<TContext> where TContext : DbContext
    {
        private readonly TContext _context;

        public EFStoreGeneric()
        {}

        public EFStoreGeneric(TContext context)
        {
            _context = context;
        }
        
        public async Task<T> GetByIdAsync<T, TKey>(TKey id) where T : class, ICoreDal<TKey>
        {
            return await _context.Set<T>().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<T?> GetByIdAsync<T>(int id) where T : class, ICoreDalGnInt
        {
            return await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<T?> GetByIdAsync<T>(Guid id) where T : class, ICoreDalGuid
        {
            return await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        }


        public async Task<T> AddAsync<T>(T item) where T : class
        {
            await _context.Set<T>().AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<IEnumerable<T>> AddManyAsync<T>(IEnumerable<T> items) where T : class
        {
            await _context.Set<T>().AddRangeAsync(items);
            await _context.SaveChangesAsync();
            return items;
        }

        public async Task<IEnumerable<T>> GetByFilterAsync<T>(Expression<Func<T, bool>> expression)
            where T : class, ICoreDalGnInt
        {
            return await _context.Set<T>().Where(expression).ToListAsync();
        }

        public async Task<T> UpdateAsync<T>(T item) where T : class
        {
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> DeleteAsync<T>(T item) where T : class
        {
            _context.Set<T>().Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<T>> DeleteManyAsync<T>(IEnumerable<T> items) where T : class
        {
            _context.Set<T>().RemoveRange(items);
            await _context.SaveChangesAsync();
            return items;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        
        public async Task<bool> CreateDB()
        {
            return await _context.Database.EnsureCreatedAsync();
        }

        public async Task<bool> DropDB()
        {
            return await _context.Database.EnsureDeletedAsync();
        }
    }
}
