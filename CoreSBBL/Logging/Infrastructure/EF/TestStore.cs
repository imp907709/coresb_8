using System;
using System.Linq;
using System.Threading.Tasks;
using CoreSBBL.LogingTest.Models;
using CoreSBShared.Universal.Infrastructure.EF.Store;
using Microsoft.EntityFrameworkCore;

namespace CoreSBBL.Logging.Infrastructure.EF;
using Serilog;
using Serilog.Sinks.Elasticsearch;

public class TestStore : ITestStore
{
    private IEFStoreGeneric<TestContext> _context;
    
    public TestStore(IEFStoreGeneric<TestContext> context)
    {
        _context = context;
    }

    public async Task GO()
    {
        var rnd = new Random();

        var dt = DateTime.Now;
        var tests = Enumerable.Range(1, 150).Select(s => new LoginTest() {
            Name = $"name_{rnd.Next(1, 1000)}", CreatedAd = dt
        });
    }

    public void SerilogSingCheck()
    {
        var connStr = "http://localhost:9222";
        var logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(connStr))
            {
                AutoRegisterTemplate = true,
                IndexFormat = "test-logs-{0:yyyy.MM.dd}",
                EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog
            })
            .CreateLogger();

        // send a test log with custom properties
        logger.Information("Test log to Elastic", new {userId = 42, action = "login"});
    }
}
