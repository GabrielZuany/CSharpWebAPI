using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Application.Services.AuthService;
using Application.Services.TokenService.Interfaces;
using Domain.Users.Model;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services.TokenService
{
    public class TokenService : ITokenService
    {

        public object GenerateToken<T>(T objectBase, string secret)
        {
            var key = Encoding.ASCII.GetBytes(secret);

            var serialized = JsonSerializer.Serialize(objectBase);

            var tokenConfig = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("claimObject", serialized)
                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenConfig);
            return tokenHandler.WriteToken(token);
        }
        
        public string GenerateRefreshToken<T>(T entity)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var randomBytes = new byte[64];
                rng.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }

        public T ValidateToken<T>(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Key.Secret);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var claimValue = jwtToken.Claims.First(x => x.Type == "claimObject").Value;

                return JsonSerializer.Deserialize<T>(claimValue);
            }
            catch
            {
                return default;
            }
        }
    }
}
