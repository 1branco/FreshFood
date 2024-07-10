using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Security.Interfaces;
using SecurityAPI.Utils;
using Shared.Models.Security;

namespace SecurityAPI.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SecurityController : ControllerBase
    {
        private readonly ILogger<SecurityController> _logger;
        private readonly IConfiguration _config;
        private readonly ISecurityService _securityService;
        //private readonly IMapper _mapper;

        public SecurityController(ILogger<SecurityController> logger, 
            IConfiguration config, ISecurityService securityService)
            //IMapper mapper)
        {
            _logger = logger;
            _config = config;
            _securityService = securityService;
            //_mapper = mapper;
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
        [Route("login")]
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestObjectResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var jwtUtils = new JwtUtils(_config);
                var jwtToken = jwtUtils.GenerateJwt(request.Email);
                //var refreshToken = jwtUtils.GenerateRefreshToken();

                var result = await _securityService.LoginAsync(request.Email, request.Password);

                if (result)
                {
                    return Ok(new LoginResponse()
                    {
                        Email = request.Email,
                        Token = jwtToken.Token
                    });
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception e)
            {
                return Problem(e.Message, e.StackTrace);
            }
        }

        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                return Problem(e.Message, e.StackTrace);
            }
        }
    }
}
