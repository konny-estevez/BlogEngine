using Entities;

namespace ApplicationCore.Services
{
    public interface IUsersService
    {
        Task<(IEnumerable<User>, long)> GetUsers(int page, int pageSize, string name);
        Task<User> GetUser(Guid id);
        Task<(User, string)> CreateUser(User newObject);
        Task<(bool, string)> UpdateUser(User updatedObject);
        Task<(bool, string)> DeleteUser(Guid id);
        Task<(User, string)> LoginUser(string email, string password);
        Task<(bool, string)> ConfirmEmail(Guid id, string email);
        Task<(IEnumerable<string>, string)> GetRole(Guid userId);
        Task<(bool, string)> UpdateUserRole(Guid id, string newRole);
        Task<IEnumerable<string>> GetRoles();
    }
}
