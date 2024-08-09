using Asp.Versioning;
using Cache.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using Security.Interfaces;
using SecurityAPI.Attributes;
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
        private readonly ICacheService _cache;

        //private readonly IMapper _mapper;

        public SecurityController(ILogger<SecurityController> logger, 
            IConfiguration config, ISecurityService securityService, ICacheService cache)
            //IMapper mapper)
        {
            _logger = logger;
            _config = config;
            _securityService = securityService;
            _cache = cache;
            //_mapper = mapper;
        }

        /// <summary>
        /// Customer's login into the platform
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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
                _logger.LogError("Model State is in invalid state.", JsonConvert.SerializeObject(request));
                
                return BadRequest(ModelState);
            }

            try
            {
                var jwtUtils = new JwtUtils(_config);
                var jwToken = jwtUtils.GenerateJwt(request.Email);
                //var refreshToken = jwtUtils.GenerateRefreshToken();

                var result = await _securityService.LoginAsync(request.Email, request.Password, jwToken.Token);

                if (result)
                {
                    return Ok(new LoginResponse()
                    {
                        Email = request.Email,
                        Token = jwToken.Token
                    });
                }
                else
                {
                    _logger.LogError($"Invalid credentials for user {request.Email}");
                    return Problem("Invalid credentials.");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.StackTrace);
                return Problem(e.Message, e.StackTrace);
            }
        }

        /// <summary>
        /// Endpoint for the customer's logout   
        /// Removes token's from cache and eliminates server-side cookies
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [Route("logout")]
        [HttpPost]
        [JwtAuthorize]
        [ProducesResponseType(typeof(OkObjectResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestObjectResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status500InternalServerError)]
        public IActionResult LogoutAsync([FromBody] string email)
        {
            try
            {
                _securityService.LogoutAsync(email);
                return Ok();       
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.StackTrace);
                return Problem(e.Message, e.StackTrace);
            }
        }
    }
}
