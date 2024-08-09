namespace Security.Interfaces
{
    public interface ISecurityService
    {
        Task<bool> LoginAsync(string username, string password, string jwToken);

        bool StoreJwtToken(string username, string token);

        void RemoveJwtToken(string username);

        void LogoutAsync(string email);
    }
}
