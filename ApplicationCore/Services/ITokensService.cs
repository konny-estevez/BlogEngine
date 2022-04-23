using ApplicationCore.Models;
using Entities;

namespace ApplicationCore.Services
{
    public interface ITokensService
    {
        (string, JwtOptions) CreateToken(string email, User user);
        bool ValidateToken(string token);
    }
}
