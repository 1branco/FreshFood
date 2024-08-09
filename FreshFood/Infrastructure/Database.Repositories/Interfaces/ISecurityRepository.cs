namespace Database.Repositories.Interfaces
{
    public interface ISecurityRepository
    {   
        bool CheckIfEmailExists(string email);
        Task<string> GetUsersPassword(Guid userId);
        Task<string> GetUsersPassword(string email);
    }
}
