using ApplicationCore.Models;
using Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApplicationCore.Services
{
    public class TokensService : ITokensService
    {
        private readonly IOptions<JwtOptions> _options;

        public TokensService(IOptions<JwtOptions> options)
        {
            _options = options;
        }

        public (string, JwtOptions) CreateToken(string email, User user)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.CreateJwtSecurityToken(
                audience: _options.Value.Audience,
                issuer: _options.Value.Issuer,
                subject: new ClaimsIdentity(claims),
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_options.Value.ExpireTime)),
                signingCredentials: credentials);
            var tokenString = tokenHandler.WriteToken(jwtSecurityToken);
            var options = new JwtOptions { ExpireTime = _options.Value.ExpireTime, Issuer = _options.Value.Issuer };

            return (tokenString, options);
        }

        public bool ValidateToken(string token)
        {
            var mySecret = Encoding.UTF8.GetBytes(_options.Value.Key);
            var mySecurityKey = new SymmetricSecurityKey(mySecret);
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _options.Value.Issuer,
                    ValidAudience = _options.Value.Audience,
                    IssuerSigningKey = mySecurityKey,
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
