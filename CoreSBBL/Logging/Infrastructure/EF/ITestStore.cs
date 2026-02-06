namespace CoreSBBL.Logging.Infrastructure.EF;

public interface ITestStore
{
    Task GO();

    void SerilogSingCheck();
}
