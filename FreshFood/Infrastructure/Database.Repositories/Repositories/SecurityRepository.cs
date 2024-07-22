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

        public async Task<byte[]> GetUsersPassword(Guid userId)
        {
            return await context.Users.Where(x => x.Id == userId).Select(x => x.Password).FirstAsync();
        }

        public async Task<byte[]> GetUsersPassword(string email)
        {
            return await context.Users.Where(x => x.Email == email).Select(x => x.Password).FirstAsync();
        }

    }
}
