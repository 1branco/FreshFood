using Database.Contexts;
using Database.Entities.Customer;
using Database.Entities.Security;
using Database.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories
{
    public class SecurityRepository : BaseRepository<Credential> , ISecurityRepository
    {
        private readonly CoreDbContext context;

        public SecurityRepository(CoreDbContext _context) : base(_context)
        {
            context = _context;
        }

        public bool CheckIfEmailIsValid(string email)
        {
            return context.Customers.Where(x => x.Email == email).Any();
        }

        public async Task<Credential> GetCustomersCredentials(Guid userId)
        {
            return (Credential) await context.Customers.Where(x => x.Id == userId).Select(x => x.Credentials.Where(x => x.IsActive)).FirstAsync();
        }

        public async Task<Credential> GetCustomersCredentials(string email)
        {
            return (Credential) await context.Customers.Where(x => x.Email == email).Select(x => x.Credentials.Where(x => x.IsActive)).FirstAsync();
        }

        public IQueryable GetCustomersCredentialById(Guid credentialId)
        {
            return context.CustomersCredentials.Where(x => x.Id == credentialId).Select(x => x.Value);
        }

    }
}
