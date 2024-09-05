using CacheService.Interfaces;
using CustomerService.Interfaces;
using Database.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using Models.Registration;
using NSubstitute;
using System.Text;

namespace UnitTests.Services
{
    public class RegistrationUnitTest
    {
        #region Variables

        private const string _emailInvalid = "joao812@hotmail.com";

        #endregion

        #region Moqs

        private readonly IUserRepository _repo;
        private readonly ICacheService _cache;
        private readonly ILogger<ICustomerService> _logger;

        #endregion
        public RegistrationUnitTest()
        {
            _repo = Substitute.For<IUserRepository>();
            _cache = Substitute.For<ICacheService>();
            _logger = Substitute.For<ILogger<ICustomerService>>();
        }

        [Fact]
        public async void Register_WithValidData_ReturnsSuccess()
        {
            #region Arrange
            _repo.RegisterNewCustomer(Arg.Any<UserRegistration>()).Returns(Guid.NewGuid());

            UserRegistration user = new UserRegistration
            {
                Email = "joao97@gmail.com",
                Password = Encoding.UTF8.GetBytes("teste"),
                Address = "Rua 1",
                Avatar = null,
                VATNumber = "123456789",
                Cellphone = "912345678",
                City = "Porto",
                CountryId = Guid.Parse("ACE4FE18-C673-4FAD-B2CF-0BCDF5E59AE9"),
                State = "Porto",
                Name = "João",
                Zipcode = "4000-000"
            };
            #endregion

            #region Act                  
            ICustomerService customerService = new CustomerService.Services.CustomerService(_repo);
            var result = await customerService.RegisterCustomer(user);
            #endregion

            #region Assert
            Assert.NotEmpty(result.ToString());
            Assert.IsType<Guid>(result);
            #endregion
        }       
    }
}
