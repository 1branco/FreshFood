using Models.Registration;

namespace CustomerService.Interfaces
{
    public interface ICustomerService
    {
        Task<Guid> RegisterCustomer(UserRegistration user);

        Guid GetCustomerId(string email);
    }
}
