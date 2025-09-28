using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using CoreSBBL.Logging.Services;
using CoreSBShared.Universal.Checkers.Threading;
using Live;
using Microsoft.AspNetCore.Mvc;

namespace CoreSBServer.Controllers
{
    public class TestController : ControllerBase
    {
        private ILoggingService _loggingService;

        public TestController(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        [HttpGet]
        [Route("test")]
        public ActionResult  Test()
        {
            return Ok("up and running");
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

            var res = await mth.GO(req.UrlsNumber, req.maxParallel);
            return res;
        }
        
    }
}
