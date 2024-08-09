using System.Text;
using Database.DbContexts;
using Database.Entities.Entities;
using Database.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories
{
    public class SecurityRepository : BaseRepository<User> , ISecurityRepository
    {
        private readonly FreshFoodContext context;

        public SecurityRepository(FreshFoodContext _context) : base(_context)
        {
            context = _context;
        }

        public bool CheckIfEmailExists(string email)
        {
            return context.Users.Where(x => x.Email == email).Any();
        }

        public async Task<string> GetUsersPassword(Guid userId)
        {
            var password = await context.Users.Where(x => x.Id == userId).Select(x => x.Password).FirstAsync();

            return Encoding.UTF8.GetString(password);
        }

        public async Task<string> GetUsersPassword(string email)
        {
            var password = await context.Users.Where(x => x.Email == email).Select(x => x.Password).FirstAsync();

            return Encoding.UTF8.GetString(password);
        }

    }
}
