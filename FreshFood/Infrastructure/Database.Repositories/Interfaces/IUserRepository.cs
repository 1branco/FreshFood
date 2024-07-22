using Models.Registration;

namespace Database.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<Guid> RegisterNewCustomer(UserRegistration user);

        Guid GetUserId(string email);
    }
}
