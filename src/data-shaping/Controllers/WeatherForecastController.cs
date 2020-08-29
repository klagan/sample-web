using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Sample.WebApi.Shaping.Controllers
{
    using System.Reflection.Metadata;

    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger
        )
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] string fields = null)
        {
            var requestedFields = fields == null ? new string[] {} : fields.Split(",");

            var rng = new Random();
            var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToList();

            return requestedFields.Any() ? Ok(result.Shape(requestedFields)) : Ok(result);
        }

        [HttpGet]
        [Route("GetForecast")]
        public IActionResult GetForecast([FromQuery] string fields=null)
        {
            var requestedFields = fields == null ? new string[] {} : fields.Split(",");
            var rng = new Random();

            return Ok(new WeatherForecast
            {
                Date = DateTime.Now.AddDays(1),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }.Shape(requestedFields));
        }
    }
}
