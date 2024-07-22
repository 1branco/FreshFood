using Cache.Interfaces;
using Database.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Security.Interfaces;
using System.Text;

namespace Security.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly ICacheService _cache;
        private readonly ISecurityRepository _securityRepository;

        public SecurityService(ISecurityRepository securityRepository,
            ICacheService cache) 
        {
            _securityRepository = securityRepository;
            _cache = cache;
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

        public bool StoreJwtToken(string username, string token)
        {
            var value = string.Format($"{username}_token_{token}");
            var key = string.Format($"{username}_jwt");
            
            return _cache.Set(key, value);
        }

        public void RemoveJwtToken(string username)
        {
            var key = string.Format($"{username}_jwt");

            _cache.Remove(key);
        }
    }
}
