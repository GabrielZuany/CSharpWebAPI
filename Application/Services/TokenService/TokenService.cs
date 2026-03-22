using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Services.TokenService.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services.TokenService
{
    public class TokenService : ITokenService
    {
        public object GenerateToken<T>(T objectBase, string secret)
        {
            var key = Encoding.ASCII.GetBytes(secret); // encrypt the secret key

            var tokenConfig = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("claimObject", objectBase.GetHashCode().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenConfig);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}
