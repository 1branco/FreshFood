using Database.DbContexts;
using Database.Entities.Entities;
using Database.Repositories.Interfaces;
using Models.Registration;

namespace Database.Repositories.Repositories
{
    public class CustomerRepository : BaseRepository<UserRegistration>, ICustomerRepository
    {
        private readonly FreshFoodContext context;
        public CustomerRepository(FreshFoodContext _context) : base(_context)
        {
            context = _context;
        }

        public async Task<Guid> RegisterNewCustomer(UserRegistration newUser)
        {
            if (newUser == null)
                throw new ArgumentNullException(nameof(newUser));

            var userEmail = context.Users.Any(c => c.Email == newUser.Email);
            if (userEmail)
                throw new InvalidOperationException($"Email {newUser.Email} already exists in database.");

            var country = context.Country.FirstOrDefault(c => c.Id == newUser.CountryId);
            if (country == null)
                throw new ArgumentNullException($"Country with id {newUser.CountryId} not found");

            User user = new User()
            {
                Email = newUser.Email,
                Name = newUser.Name,
                Password = newUser.Password,
                CountryId = newUser.CountryId,
                Country = country,
                BillingInfo = new UserBillingInfo()
                {
                    Address = newUser.Address,
                    City = newUser.City,
                    State = newUser.State,
                    VatNumber = newUser.VATNumber,
                    Zipcode = newUser.Zipcode
                },
                ShippingInfo = new UserShippingInfo()
                {
                    Address = newUser.Address,
                    City = newUser.City,
                    Zipcode = newUser.Zipcode,
                    State = newUser.State
                    
                }
            };

            var entity = context.Users.Add(user).Entity;

            await SaveChangesAsync();

            return entity.Id;
        }
    }
}
