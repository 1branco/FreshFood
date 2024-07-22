namespace Security.Interfaces
{
    public interface ISecurityService
    {
        Task<bool> LoginAsync(string username, string password);

        bool StoreJwtToken(string username, string token);

        void RemoveJwtToken(string username);
    }
}
