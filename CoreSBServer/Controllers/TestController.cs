using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CoreSBBL.Logging.Services;
using CoreSBShared.Universal.Infrastructure.HTTP.MyApp.Services.Http;
using CoreSBShared.Universal.Checkers.Threading;
using Live;
using Microsoft.AspNetCore.Mvc;

namespace CoreSBServer.Controllers
{
    public class TestController : ControllerBase
    {
        private readonly IHttpService _http;
        public TestController(IHttpService http) {
            _http = http;
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
        
        [HttpGet]
        [Route("test")]
        public async Task<ActionResult> Test()
        {
            var tsk = await Task.FromResult("up and running !!");
            var resp = await _http.GetAsync<string>("https://api.restful-api.dev/objects");
            return Ok(resp);
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
        
    }
}
