using Asp.Versioning;
using AutoMapper;
using Customer.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Customer;
using Models.Registration;
using SecurityAPI.Utils;

namespace SecurityAPI.Controllers.v1
{

    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CustomerController : Controller
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly IConfiguration _config;
        private readonly ICustomerService _customerService;
        private readonly IValidator<RegisterRequest> _validator;
        private readonly IMapper _mapper;

        public CustomerController(ILogger<CustomerController> logger,
            IConfiguration config, ICustomerService customerService, 
            IValidator<RegisterRequest> validator,
            IMapper mapper)
        {            
            _logger = logger;
            _config = config;
            _customerService = customerService;
            _validator = validator;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new customer
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Guid of the new customer</returns>
        [Route("register")]
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(OkObjectResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestObjectResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegisterCustomer([FromBody] RegisterRequest user)
        {
            ValidationResult result = await _validator.ValidateAsync(user);

            if (!ModelState.IsValid || !result.IsValid)
            {
                result.AddToModelState(ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                 return Ok(_customerService.RegisterCustomer(_mapper.Map<UserRegistration>(user)));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, ex.StackTrace);
            }
        }
    }
}
