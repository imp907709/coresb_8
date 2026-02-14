using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CoreSBBL.Logging.Infrastructure.EF;
using CoreSBBL.Logging.Services;
using CoreSBShared.Universal.Infrastructure.HTTP.MyApp.Services.Http;
using CoreSBShared.Universal.Checkers.Threading;
using Live;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CoreSBBL.Logging.Services;
using CoreSBShared.Universal.Infrastructure.HTTP.MyApp.Services.Http;
using CoreSBShared.Universal.Infrastructure.Rabbit;
using Microsoft.AspNetCore.Mvc;


namespace CoreSBServer.Controllers
{
    public class TestController : ControllerBase
    {
        private readonly IHttpService _http;
        private readonly ITestStore _testStore;
        private readonly IRabbitClient _rabbit;
        
        private static CancellationTokenSource _cts;
        
        public TestController(IHttpService http, ITestStore testStore, IRabbitClient rabbit) {
            _http = http;
            _testStore = testStore;
            _rabbit = rabbit;
        }

        [HttpGet]
        [Route("runstart")]
        public async Task<IActionResult> RunStart()
        {
            _cts = new CancellationTokenSource();
            var hs = _cts?.Token.GetHashCode();
            Console.WriteLine("Start token: " + _cts?.Token.GetHashCode());

            try
            {
                await Task.Delay(15000, _cts.Token);
            }
            catch (TaskCanceledException)
            {
                return Ok($"cancelled with {hs}");
            }
            return Ok($"finished with {hs}");
        }
        [HttpGet]
        [Route("runcancel")]
        public IActionResult RunCancel()
        {
            var hs = _cts?.Token.GetHashCode();
            Console.WriteLine("Cancel token: " + _cts?.Token.GetHashCode());
            
            _cts.Cancel();
            return Ok($"cancelled with {hs}");
        }
        
        
        
        [HttpGet]
        [Route("test")]
        public async Task<ActionResult> Test()
        {
            var tsk = await Task.FromResult("up and running !!");
            var resp = await _http.GetAsync<string>("https://api.restful-api.dev/objects");
            return Ok(resp);
        }

        [HttpGet]
        [Route("checklog")]
        public ActionResult CheckLog()
        {
            _testStore.SerilogSingCheck();
            return Ok();
        }
        
        private async void DoWorkAsync()
        {
            await Task.Delay(100);
            throw new Exception("Boom!");
        }
        
        [HttpGet]
        [Route("AsyncVoid")]
        public IActionResult AsyncVoid()
        {
            DoWorkAsync(); // async void
            return Ok();
        }
        
        public class TestParallelReq
        {
            public int UrlsNumber { get; set; } = 4;
            public int maxParallel { get; set; } = 4;
        }
        
        [HttpPost]
        [Route("testprallel")]
        public async Task<IEnumerable<string>> testprallel(TestParallelReq req)
        {
            var mth = new MultithreadingCheck();
            var ct = new CancellationToken();
            // var res = await mth.GO(req.UrlsNumber, req.maxParallel);

            var lv = new LiveCheck();
            var enm = Enumerable.Range(0, 1).Select(n => n.ToString()).ToList();
            return await Task.FromResult(enm);
        }

        [HttpGet]
        [Route("testStoreGO")]
        public async Task<ActionResult> testStoreGO()
        {
            await _testStore.GO();
            return Ok();
        }
    
        [HttpGet]
        [Route("Connected")]
        public async Task<ActionResult> Connected()
        {
            try
            {
                return Ok($"Rabbit is connected : {_rabbit.IsConnected()}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }
        
        [HttpGet]
        [Route("Channel")]
        public async Task<ActionResult> Get()
        {
            try
            {
                var ch = await _rabbit.ChannelOpen();
                var num = ch.ChannelNumber;
                await _rabbit.ChannelClose(ch);
                return Ok($"Channel : {num}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }
    }
}
