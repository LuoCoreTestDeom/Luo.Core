using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Luo.Core.Api.Version1
{
    [ApiExplorerSettings(GroupName = "V1")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        [HttpGet(Name = "GetWeatherForecast2222")]
        [Authorize]
        public IEnumerable<WeatherForecast> Get13123123()
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
