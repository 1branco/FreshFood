using Models.Registration;

namespace Database.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Guid> RegisterNewCustomer(UserRegistration user);
    }
}
