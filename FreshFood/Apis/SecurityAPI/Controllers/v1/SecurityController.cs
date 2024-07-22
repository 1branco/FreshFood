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
                    return Problem("Invalid credentials.");
                }
            }
            catch (Exception e)
            {
                return Problem(e.Message, e.StackTrace);
            }
        }
    }
}
