using System.Threading.Tasks;
using CoreSBBL.Logging.Services;
using CoreSBShared.Registrations;
using CoreSBShared.Universal.Infrastructure.Clouds;
using CoreSBShared.Universal.Infrastructure.Geo;
using CoreSBShared.Universal.Infrastructure.HTTP.MyApp.Services.Http;
using CoreSBShared.Universal.Infrastructure.Rabbit;
using Microsoft.AspNetCore.Mvc;

namespace CoreSBServer.Controllers
{
    public class TestController : ControllerBase
    {
        private readonly IHttpService _http;
        private readonly IRabbitClient _rabbit;
        private readonly GoogleCloud _googleCloud;
        private readonly IGoogleGeoApiService _googleGeoApi;

        public TestController(
            IHttpService http,
            IRabbitClient rabbit,
            GoogleCloud cloud,
            IGoogleGeoApiService googleGeoApi)
        {
            _http = http;
            _rabbit = rabbit;
            _googleCloud = cloud;
            _googleGeoApi = googleGeoApi;
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

        [HttpGet]
        [Route("googlekey")]
        public async Task<ActionResult> TestGoogleApi()
        {
            try
            {
                var key = _googleCloud.GetApiKey();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        /// <summary>Sample Geocoding call; returns Google API JSON body; failures propagate.</summary>
        [HttpGet]
        [Route("googlegeo")]
        [Produces("application/json")]
        public async Task<IActionResult> GoogleGeoSample(CancellationToken cancellationToken)
        {
            try
            {
                var json = await _googleGeoApi.GeocodeTestAsync( "1600 Amphitheatre Parkway, Mountain View, CA", cancellationToken);
                return Ok(json);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}
