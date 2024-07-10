using Database.Entities.Customer;
using Database.Entities.Security;

namespace Database.Repositories.Interfaces
{
    public interface ISecurityRepository
    {   
        bool CheckIfEmailIsValid(string email);
        Task<Credential> GetCustomersCredentials(Guid userId);
        Task<Credential> GetCustomersCredentials(string email);
        IQueryable GetCustomersCredentialById(Guid credentialId);
    }
}
