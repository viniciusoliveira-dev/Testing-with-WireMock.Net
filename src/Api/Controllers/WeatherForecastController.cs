using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly WeatherForecastServices _service;

        public WeatherForecastController(WeatherForecastServices service)
        {
            _service = service;
        }

        [HttpGet("basecontroller")]
        public IActionResult Get()
        {
            return Ok(_service.GetBaseWeatherForecast());
        }

        [HttpGet("client")]
        public async Task<IActionResult> Client()
        {
            var clientResponse = await _service.GetWeatherForecastAsync();
            return Ok(clientResponse);
        }
    }
}
