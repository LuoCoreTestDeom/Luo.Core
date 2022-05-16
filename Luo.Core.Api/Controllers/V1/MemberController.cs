using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Luo.Core.Api.Controllers.V1
{
    [ApiExplorerSettings(GroupName = "V1")]
    //[ApiVersion("1.0")]
    [Route("api/v1/[controller]/[action]")]
    //[Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    [Authorize(Policy = "Permission")]
    //[Authorize]
    public class MemberController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
      {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
        [HttpGet(Name = "GetWeatherForecast2222")]
        public IEnumerable<WeatherForecast> Get13123123()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
           .ToArray();
        }
    }
}
