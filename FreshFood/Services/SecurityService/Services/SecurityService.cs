using Database.Repositories.Interfaces;
using Security.Interfaces;

namespace Security.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly ISecurityRepository _securityRepository;
        public SecurityService(ISecurityRepository securityRepository) 
        {
            _securityRepository = securityRepository;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            if (!_securityRepository.CheckIfEmailIsValid(email))
            {
                return false;
            }

            var credential = await _securityRepository.GetCustomersCredentials(email);

            if(credential is null)
            {
                throw new InvalidOperationException($"User {email} does not have a single credential active.");
            }

            if(!SecurityHelper.HashingHelper.Verify(credential.Value, password))
            {
                return false;
            }

            return true;
        }

        public async Task<bool> Register(string email, string password)
        {
            return true;
        }
    }
}
