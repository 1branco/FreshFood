using Microsoft.IdentityModel.Tokens;
using SecurityAPI.Interfaces;
using SecurityAPI.Models.Responses;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SecurityAPI.Utils
{
    public class JwtUtils : IJwtUtils
    {
        private readonly IConfiguration _config;

        public JwtUtils(IConfiguration configuration)
        {
            _config = configuration;
        }

        public JwtToken GenerateJwt(string email)
        {
            var credentials = CreateSigningCredentials();

            //claim is used to add identity to JWT token
            var claims = CreateClaims(email);

            var jwtSecurityToken = new JwtSecurityToken(_config["JwtAuth:Issuer"],
                                            _config["JwtAuth:Audience"],
                                            claims,
                                            expires: DateTime.Now.AddMinutes(double.TryParse(_config["JwtAuth:ExpirationTime"], out double result) ? result : 30),
                                            signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            
            double expirationTime;
            double.TryParse(jwtSecurityToken.Payload["exp"].ToString(), out expirationTime);

            return new JwtToken()
            {
                Id = Guid.Parse(jwtSecurityToken.Id),
                Token = token,
                TokenTTL = DateTimeOffset.FromUnixTimeSeconds((long)expirationTime).UtcDateTime.ToString()
            };
        }

        public SecurityToken DecodeJwt(string token)
        {
            if (token == null)
                throw new ArgumentNullException("Token is null", "JwtToken cannot be null!");

            try
            {
                var key = Encoding.UTF8.GetBytes(_config["JwtAuth:Key"]);
                var tokenHandler = new JwtSecurityTokenHandler();

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jsonToken = tokenHandler.ReadToken(token);

                //var jwtToken = validatedToken as JwtSecurityToken;
                var jwtToken = jsonToken as JwtSecurityToken;

                if (jwtToken.Claims.First(p => p.Type == JwtRegisteredClaimNames.Jti).Value != null)
                    return validatedToken;
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using (var generator = new RNGCryptoServiceProvider())
            {
                generator.GetBytes(randomNumber);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomNumber),
                    Expires = DateTime.Now.AddMinutes(30),
                    Created = DateTime.Now
                };
            }
        }

        public List<Claim> CreateClaims(string email)
        {
            return new List<Claim>() {
                new Claim(JwtRegisteredClaimNames.Sub, "TokenForTheApiWithAuth"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                new Claim(ClaimTypes.Email, email)
            };
        }

        private SigningCredentials CreateSigningCredentials()
        {
            try
            {
                return new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_config["JwtAuth:Key"])
                    ),
                    SecurityAlgorithms.HmacSha256
                );
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
