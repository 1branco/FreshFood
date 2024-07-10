using Microsoft.IdentityModel.Tokens;
using SecurityAPI.Models.Responses;

namespace SecurityAPI.Interfaces
{
    public interface IJwtUtils
    {
        JwtToken GenerateJwt(string email);
        SecurityToken DecodeJwt(string token);
    }
}
