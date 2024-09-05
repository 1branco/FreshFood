using Asp.Versioning;
using CacheService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SecurityAPI.Attributes;

namespace WebAPI.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IConfiguration _config;
        private readonly ICacheService _cache;

        public ProductController(ILogger<ProductController> logger, 
            IConfiguration config, ICacheService cache)
        {
            _logger = logger;
            _config = config;
            _cache = cache;
        }

        [HttpGet]
        [Route("products")]
        [JwtAuthorize]
        [ProducesResponseType(typeof(OkObjectResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestObjectResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProductsAsync() 
        {
            try
            {
                // Get products from cache
                //var products = await _cache.GetProductsAsync();
                return Ok();
                //return Ok(products);
            }
            catch(Exception ex)
            {
                _logger.LogError("An error occurred while fetching products.", ex);
                return StatusCode(500, "An error occurred while fetching products.");
            }
        }
    }
}