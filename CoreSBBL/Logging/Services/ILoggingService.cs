using System;
using System.Threading.Tasks;
using CoreSBBL.Logging.Models.TC.BL;

namespace CoreSBBL.Logging.Services
{
    public interface ILoggingServiceNew
    {

        Task<LoggingResp> AddToAll(LoggingGenericBLAdd item);

        Task<bool> RecreateDB();
    }
}
