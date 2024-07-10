namespace Security.Interfaces
{
    public interface ISecurityService
    {
        Task<bool> LoginAsync(string username, string password);
    }
}
