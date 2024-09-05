using CustomerService.Interfaces;
using Database.Repositories.Interfaces;
using Models.Registration;
using System.Text;

namespace CustomerService.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUserRepository customerRepository;
        public CustomerService(IUserRepository _customerRepository)
        {
            customerRepository = _customerRepository;
        }

        /// <summary>
        /// Hashes password and registers a new customer
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Returns the guid of the new customer</returns>
        public async Task<Guid> RegisterCustomer(UserRegistration user)
        {
            try
            {
                var hashedPassword = SecurityHelper.HashingHelper.Hash(Encoding.UTF8.GetString(user.Password));

                user.Password = Encoding.UTF8.GetBytes(hashedPassword);

                return await customerRepository.RegisterNewCustomer(user);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Guid GetCustomerId(string email)
        {
            return customerRepository.GetUserId(email);
        }

    }
}
