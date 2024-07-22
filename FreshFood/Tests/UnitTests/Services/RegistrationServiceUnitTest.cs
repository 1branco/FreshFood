using Customer.Interfaces;
using Customer.Services;
using Database.Repositories.Interfaces;
using Models.Registration;
using Moq;
using System.Text;

namespace UnitTests.Services
{
    public class RegistrationServiceUnitTest
    {
        [Fact]
        public async void Register_WithValidData_ReturnsSuccess()
        {
            #region Arrange
            var moq = new Mock<IUserRepository>();
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
            ICustomerService customerService = new CustomerService(moq.Object);
            var result = await customerService.RegisterCustomer(user);
            #endregion

            #region Assert
            Assert.NotEmpty(result.ToString());
            Assert.IsType<Guid>(result);
            #endregion
        }

        [Fact]
        public async void Register_WithInvalidCountryId_ThrowsArgumentNullException()
        {
            #region Arrange
            var moq = new Mock<IUserRepository>();
            UserRegistration user = new UserRegistration
            {
                Email = "joao@gmail.com",
                Password = Encoding.UTF8.GetBytes("teste"),
                Address = "Rua 1",
                Avatar = null,
                VATNumber = "123456789",
                Cellphone = "912345678",
                City = "Porto",
                CountryId = Guid.Parse("ACE4FE18-0000-0000-0111-0BCDF5E59AE9"),
                State = "Porto",
                Name = "João",
                Zipcode = "4000-000"
            };
            #endregion

            #region Act                  
            ICustomerService customerService = new CustomerService(moq.Object);
            #endregion

            #region Assert
            Assert.ThrowsAsync<ArgumentNullException>(() 
                => customerService.RegisterCustomer(user));
            #endregion
        }

        [Fact]
        public async void Register_WithAlreadyRegisteredEmail_ThrowsInvalidOperationException()
        {
            #region Arrange
            var moq = new Mock<IUserRepository>();
            UserRegistration user = new UserRegistration
            {
                Email = "joao@gmail.com",
                Password = Encoding.UTF8.GetBytes("teste"),
                Address = "Rua 1",
                Avatar = null,
                VATNumber = "123456789",
                Cellphone = "912345678",
                City = "Porto",
                CountryId = Guid.Parse("ACE4FE18-0000-0000-0111-0BCDF5E59AE9"),
                State = "Porto",
                Name = "João",
                Zipcode = "4000-000"
            };
            #endregion

            #region Act                  
            ICustomerService customerService = new CustomerService(moq.Object);
            #endregion

            #region Assert
            Assert.ThrowsAsync<InvalidOperationException>(() =>
                customerService.RegisterCustomer(user));
            #endregion
        }

        [Fact]
        public async void Register_WithNullObject_ThrowsArgumentNullException()
        {
            #region Arrange
            var moq = new Mock<IUserRepository>();
            UserRegistration user = null;
            #endregion

            #region Act                  
            ICustomerService customerService = new CustomerService(moq.Object);
            #endregion

            #region Assert
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                customerService.RegisterCustomer(user));
            #endregion
        }
    }
}
