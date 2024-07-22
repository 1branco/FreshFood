using Database.Repositories.Interfaces;
using Security.Interfaces;
using System.Text;

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
            if (!_securityRepository.CheckIfEmailExists(email))
            {
                return false;
            }

            var credential = await _securityRepository.GetUsersPassword(email);

            if (credential is null)
            {
                throw new InvalidOperationException($"User with email {email} does not have a valid credential.");
            }

            if (!SecurityHelper.HashingHelper.Verify(password, Encoding.UTF8.GetString(credential)))
            {
                return false;
            }

            return true;
        }
    }
}
