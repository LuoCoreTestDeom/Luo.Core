using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Luo.Core.Api.Version2
{
    [ApiExplorerSettings(GroupName = "V2")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        [HttpGet(Name = "GetWeatherForecast111111")]
        public IEnumerable<WeatherForecast> Get2222()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
              
            })
            .ToArray();
        }
    }
}
