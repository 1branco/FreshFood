using Database.Entities.Customer;

namespace Database.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer> RegisterNewCustomer(Customer customer);
    }
}
