using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SecurityController : ControllerBase
    {
        public SecurityController()
        {

        }

        /// <summary>
        /// This API returns a list of weather forecasts.
        /// </summary>
        /// <remarks>
        /// Possible values could be:
        ///
        ///     "Freezing", "Bracing", "Chilly", "Cool", "Mild",
        ///     "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ///
        /// Just for demonstration
        ///
        ///     GET api/v1/WeatherForecast
        ///     {
        ///     }
        ///     curl -X GET "https://server-url/api/v1/WeatherForecast" -H  "accept: text/plain"
        ///
        /// </remarks>
        [HttpPost]
        public IActionResult Get() 
        {
            return Ok();
        }
    }
}
