using System.Threading.Tasks;
using CoreSBBL.Logging.Models.TC.API;
using CoreSBBL.Logging.Models.TC.BL;
using CoreSBBL.Logging.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoreSBServer.Controllers
{
    public class LoggingController : ControllerBase
    {
        private readonly ILoggingServiceNew _loggingService;

        public LoggingController(ILoggingServiceNew loggingService)
        {
            _loggingService = loggingService;
        }

        [HttpPost]
        [Route("AddToAll")]
        public async Task<ActionResult> AddToAll(LogsAPI item)
        {
            var resp = await _loggingService.AddToAll(new LoggingGenericBLAdd() {Message = item.Message});

            return Ok(resp);
        }
        
        [HttpGet]
        [Route("RecreateDB")]
        public async Task<ActionResult> RecreateDB()
        {
            var resp = await _loggingService.RecreateDB();

            return Ok($"Recreated : {resp}");
        }
    }
}
