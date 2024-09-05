using System.Text;


namespace Product.Services;
public class ProductService : IProductService
{
    public ProductService(ILogger<IProductService> logger, IConfiguration config, ICacheService cache)
    {
    
    }

    public IList<Product> GetProducts()
    {
        throw new NotImplementedException();
    }
}
