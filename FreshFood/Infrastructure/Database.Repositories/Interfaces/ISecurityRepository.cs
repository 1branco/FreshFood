namespace Database.Repositories.Interfaces
{
    public interface ISecurityRepository
    {   
        bool CheckIfEmailExists(string email);
        Task<byte[]> GetUsersPassword(Guid userId);
        Task<byte[]> GetUsersPassword(string email);
    }
}
