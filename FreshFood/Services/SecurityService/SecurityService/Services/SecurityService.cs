using CacheService.Interfaces;
using Database.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using SecurityService.Interfaces;

namespace SecurityService.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly ICacheService _cache;
        private readonly ILogger<ISecurityService> _logger;
        private readonly ISecurityRepository _securityRepository;

        public SecurityService(ISecurityRepository securityRepository,
            ICacheService cache, ILogger<ISecurityService> logger)
        {
            _securityRepository = securityRepository;
            _cache = cache;
            _logger = logger;
        }

        public async Task<bool> LoginAsync(string email, string password, string jwToken)
        {
            if (!_securityRepository.CheckIfEmailExists(email))
            {
                _logger.LogInformation($"User's email: {email} doesn't exists in database.");
                return false;
            }

            var credential = await _securityRepository.GetUsersPassword(email);

            if (credential is null)
            {
                _logger.LogInformation($"User with email {email} does not have a valid credential.");
                throw new InvalidOperationException($"User with email {email} does not have a valid credential.");
            }

            if (!SecurityHelper.HashingHelper.Verify(password, credential))
            {
                return false;
            }

            return StoreJwtToken(email, jwToken);
        }

        public bool StoreJwtToken(string username, string token)
        {
            var key = string.Format($"{username}_jwt");
            var value = string.Format($"{username}_jwtoken_{token}");

            return _cache.Set(key, value);
        }

        public void RemoveJwtToken(string username)
        {
            var key = string.Format($"{username}_jwt");

            _cache.Remove(key);
        }

        public void LogoutAsync(string email)
        {
            RemoveJwtToken(email);
        }

    }
}
