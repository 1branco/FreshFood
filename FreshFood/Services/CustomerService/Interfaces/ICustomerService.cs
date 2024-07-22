using Models.Registration;

namespace Customer.Interfaces
{
    public interface ICustomerService
    {
        Task<Guid> RegisterCustomer(UserRegistration user);

        Guid GetCustomerId(string email);
    }
}
