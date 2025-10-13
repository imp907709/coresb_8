using System;
using System.Threading.Tasks;
using CoreSBBL.Logging.Infrastructure.GN;
using CoreSBBL.Logging.Infrastructure.TS;
using CoreSBBL.Logging.Models.TC.BL;
using CoreSBBL.Logging.Models.TC.DAL;
using CoreSBShared.Universal.Infrastructure.EF;
using CoreSBShared.Universal.Infrastructure.EF.Store;
using CoreSBShared.Universal.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CoreSBBL.Logging.Infrastructure.GN
{
    //GN
    //Class level
    public class LogsEFStoreG<T,K> : EFStoreG<T, K>,
        ILogsEFStoreG<T, K>
        where T : class, ICoreDal<K>
    {
        public LogsEFStoreG(LogsContextGN dbContext) : base(dbContext)
        {
        }
    }
    
    //GN
    //Method lvl
    public class LogsEFStoreG : EFStoreG, ILogsEFStoreG
    {
        public LogsEFStoreG(LogsContextGN dbContext) : base(dbContext)
        {
        }
    }
    
}

namespace CoreSBBL.Logging.Infrastructure.TS
{
    //TS via GN
    //Method lvl
    public class LogsEFStore : EFStore, ILogsEFStore
    {
        public LogsEFStore(LogsContextTC logsContextTC) : base(logsContextTC)
        {
        }
    }

    //???Incorrect inheritance 
    // !!!failed on EF insert
    // TS via GN
    // class lvl
    //class level store generic id int
    public class LogsEFStoreGInt : EFStoreGInt, ILogsEFStoreGInt
    {
        public LogsEFStoreGInt(LogsContextGN dbContext) : base(dbContext)
        {
        }
    }
}

namespace CoreSBBL.Logging.Infrastructure.Generic
{
    public interface ILogsServiceGeneric
    {
        Task<bool> RecreateDB2();
        Task<LoggingGenericBLGetInt> AddToSecond(LoggingGenericBLAdd item);
        
        Task<LoggingGenericBLGetInt> AddItem(LoggingGenericBLAdd item);
        Task<LoggingGenericGuid> AddItem(LoggingGenericGuid item);
        Task<LoggingGenericString> AddItem(LoggingGenericString item);

        Task<bool> CheckCreated();
        Task<bool> RecreateDB();
    }

    public class LogsServiceGeneric : ILogsServiceGeneric
    {
        private readonly IEFStoreGeneric<LogsContextGeneric> _storeEF;
        private readonly IEFStoreGeneric<LogsContextGeneric2> _storeEF2;
        public LogsServiceGeneric(
            IEFStoreGeneric<LogsContextGeneric> storeEF,
            IEFStoreGeneric<LogsContextGeneric2> storeEF2
            )
        {
            _storeEF = storeEF;
            _storeEF2 = storeEF2;
        }

        public async Task<bool> RecreateDB2()
        {
            await _storeEF2.DropDB();
            await _storeEF2.CreateDB();
            return true;
        }
        public async Task<LoggingGenericBLGetInt> AddToSecond(LoggingGenericBLAdd item)
        {
            var toAdd = new LoggingGenericInt()
            {
                Created = DateTime.Now, Modified = DateTime.Now, Message = item.Message, CreatedBy = item.CreatedBy
            };
            var res = await _storeEF2.AddItemAsync(toAdd);
            return new LoggingGenericBLGetInt()
            {
                Id = res.Id, Created = res.Created, Message = res.Message
                ,CreatedBy = res.CreatedBy, Modified = res.Modified
            };
        }
        public async Task<LoggingGenericBLGetInt> AddItem(LoggingGenericBLAdd item)
        {
            var toAdd = new LoggingGenericInt()
            {
                Created = DateTime.Now, Modified = DateTime.Now, Message = item.Message, CreatedBy = item.CreatedBy
            };
            var res = await _storeEF.AddItemAsync(toAdd);
            return new LoggingGenericBLGetInt()
            {
                Id = res.Id, Created = res.Created, Message = res.Message
                ,CreatedBy = res.CreatedBy, Modified = res.Modified
            };
        }
        
        public async Task<LoggingGenericString> AddItem(LoggingGenericString item)
        {
            var res = await _storeEF.AddItemAsync(item);
            return res;
        }
        
        public async Task<LoggingGenericGuid> AddItem(LoggingGenericGuid item)
        {
            var res = await _storeEF.AddItemAsync(item);
            return res;
        }

        
        
        public async Task<bool> CheckCreated()
        {
            return await _storeEF.CreateDB();
        }
        public async Task<bool> RecreateDB()
        {
            await _storeEF.DropDB();
            await _storeEF.CreateDB();
            return true;
        }
        
        
     
    }
}
