using Cache.Interfaces;
using Database.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using Security.Interfaces;
using Security.Services;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace UnitTests.Services
{
    public class LoginUnitTest
    {
        #region Variables
        private const string _emailInvalid = "joao812@hotmail.com";
        private const string _emailValid = "joao@gmail.com";
        private const string _passwordValid = "J_p#A.b1997";
        private const string _passwordInvalid = "password#&_";
        private const string _jwToken = "jwtToken_teste_123"; 

        #endregion
        #region Moqs
        private readonly ISecurityRepository _repo;
        private readonly ICacheService _cache;
        private readonly ILogger<ISecurityService> _logger;
        #endregion
        public LoginUnitTest()
        {
            _repo = Substitute.For<ISecurityRepository>();
            _cache = Substitute.For<ICacheService>();
            _logger = Substitute.For<ILogger<ISecurityService>>();
        }

        [Fact]
        public async void Login_WithRightCredentials_ReturnsTrue()
        {
            #region Arrange
            
            _repo.CheckIfEmailExists(_emailValid).Returns(true);
            _repo.GetUsersPassword(_emailValid).Returns("$MYHASH$V1$100$aJRiB1vO7guOyJYRdXjjTtNNs8u0SDyVNMPjHPTLdqJkdC72");

            _cache.Set(Arg.Any<string>(), Arg.Any<string>()).Returns(true);
            #endregion

            #region Act                  
            ISecurityService securityService = new SecurityService(_repo, _cache, _logger);
            var result = await securityService.LoginAsync(_emailValid, _passwordValid, _jwToken);
            #endregion

            #region Assert
            Assert.True(result);
            #endregion
        }

        [Fact]
        public async void Login_WithInvalidEmail_ReturnsFalse()
        {
            #region Arrange
            _repo.CheckIfEmailExists(_emailInvalid).Returns(false);

            #endregion

            #region Act                  
            ISecurityService securityService = new SecurityService(_repo, _cache, _logger);
            var result = await securityService.LoginAsync(_emailInvalid, _passwordValid, _jwToken);
            #endregion

            #region Assert
            Assert.False(result);
            #endregion
        }

        [Fact]
        public async void Login_WithInvalidCredentials_ReturnsFalse()
        {
            #region Arrange
            _repo.CheckIfEmailExists(_emailInvalid).Returns(false);

            #endregion

            #region Act                  
            ISecurityService securityService = new SecurityService(_repo, _cache, _logger);
            var result = await securityService.LoginAsync(_emailInvalid, _passwordInvalid, _jwToken);
            #endregion

            #region Assert
            Assert.False(result);
            #endregion
        }

        [Fact]
        public async void Login_WithInvalidPassword_ThrowsException()
        {
            #region Arrange
            _repo.CheckIfEmailExists(_emailValid).Returns(true);
            _repo.GetUsersPassword(_emailValid).ReturnsNull();

            #endregion

            #region Act                  
            ISecurityService securityService = new SecurityService(_repo, _cache, _logger);
            #endregion

            #region Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await securityService.LoginAsync(_emailValid, _passwordInvalid, _jwToken));
            #endregion
        }
    }
}