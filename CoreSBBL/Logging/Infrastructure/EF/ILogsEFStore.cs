using CoreSBShared.Universal.Infrastructure.EF;
using CoreSBShared.Universal.Infrastructure.EF.Stores;
using CoreSBShared.Universal.Infrastructure.Interfaces;

namespace CoreSBBL.Logging.Infrastructure.TS
{
    // GN
    // class lvl
    // Interface for LogsEFStoreG<T, K>
    public interface ILogsEFStoreG<T, K> : IEFStore<T, K>
        where T : class, ICoreDal<K>
    {
    }

    // GN
    // Method lvl
    // Interface for LogsEFStoreG
    public interface ILogsEFStoreG : IEFStoreG
    {
    }

    // TS via GN
    // Method lvl
    // Interface for LogsEFStore
    public interface ILogsEFStore : IEFStore
    {
    }

    // !!!failed on EF insert
    // TS via GN
    // class lvl
    // Interface for LogsEFStoreGInt
    public interface ILogsEFStoreGInt : IEFStoreInt
    {
    }
}
