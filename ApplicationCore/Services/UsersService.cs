using Entities;
using ApplicationCore.Repositories;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;

namespace ApplicationCore.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IRolesRepository _rolesRepository;

        /// <summary>
        /// Users Service class constructor
        /// </summary>
        /// <param name="usersRepository">Users Repository interface</param>
        /// <param name="unitOfWork">Repository Unit of Work</param>
        /// <param name="signInManager">SignIn Manager</param>
        /// <param name="rolesRepository">Roles Repository interface</param>
        public UsersService(IUsersRepository usersRepository, IUnitOfWork unitOfWork, SignInManager<IdentityUser> signInManager, IRolesRepository rolesRepository)
        {
            _usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _rolesRepository = rolesRepository ?? throw new ArgumentNullException(nameof(rolesRepository));
        }

        /// <summary>
        /// Gets a user by Id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>User</returns>
        public async Task<User> GetUser(Guid id)
        {
            var result = await _usersRepository.Get(id.ToString());
            if (result != null)
            {
                var roles = await _signInManager.UserManager.GetRolesAsync(result);
                result.Role = roles.FirstOrDefault();
            }
            return result;
        }

        /// <summary>
        /// Get all users according to name order by username
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="name">Name filter</param>
        /// <returns>IEnumerable of Users with total count</returns>
        public async Task<(IEnumerable<User>, long)> GetUsers(int page, int pageSize, string name)
        {
            Expression<Func<User, bool>> filter = null;
            if (!string.IsNullOrEmpty(name))
                filter = x => x.UserName.Contains(name);
            return await _usersRepository.GetAll(filter, x => x.OrderBy(o => o.UserName), null, page, pageSize);
        }

        /// <summary>
        /// Gets the roles of a user according to the userId
        /// </summary>
        /// <param name="userId">userIld</param>
        /// <returns>IEnumerable of Roles and status</returns>
        public async Task<(IEnumerable<string>, string)> GetRole(Guid userId)
        {
            var user = await _usersRepository.Get(userId.ToString());
            if (user != null)
            {
                return (await _signInManager.UserManager.GetRolesAsync(user), string.Empty);
            }
            return (null, ErrorMessages.UserIdNotExists);
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="user">New user</param>
        /// <returns>Created result</returns>
        public async Task<(User, string)> CreateUser(User user)
        {
            ArgumentNullException.ThrowIfNull(user);

            user.CreatedDate = DateTime.Now;
            user.Id = Guid.NewGuid().ToString();
            try
            {
                user.UserName = user.Email;
                user.EmailConfirmed = true;
                var result = await _signInManager.UserManager.CreateAsync(user, user.PasswordHash);
                if (result.Succeeded)
                {
                    var createdUser = await _usersRepository.Get(f => f.Email.Equals(user.Email), null, null);
                    if (createdUser != null)
                    {
                        await _signInManager.UserManager.AddToRoleAsync(createdUser, Constants.PublicRole);
                        return (createdUser, string.Empty);
                    }
                    return (null, ErrorMessages.UserNotFound);
                }
                else
                {
                    return (null, string.Join(" ", result.Errors.Select(x => x.Description)));
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(ErrorMessages.DuplicatedRecordPattern))
                {
                    return (null, ErrorMessages.DuplicatedUser);
                }
                return (null, ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing user
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Result of updated user</returns>
        public async Task<(bool, string)> UpdateUser(User user)
        {
            ArgumentNullException.ThrowIfNull(user);
            try
            {
                var updatedUser = await GetUser(Guid.Parse(user.Id));
                if (updatedUser == null)
                {
                    return (false, ErrorMessages.UserNotFound);
                }
                else
                {
                    updatedUser.UpdatedDate = DateTime.Now;
                    updatedUser.Address = user.Address;
                    updatedUser.Birthday = user.Birthday;
                    updatedUser.FirstName = user.FirstName;
                    updatedUser.LastName = user.LastName;
                    updatedUser.City = user.City;
                    updatedUser.State = user.State;
                    updatedUser.Country = user.Country;
                    updatedUser.PostalCode = user.PostalCode;
                    updatedUser.MobilePhone = user.MobilePhone;
                    updatedUser.PhoneNumber = user.PhoneNumber;
                    updatedUser.EmailConfirmed = user.EmailConfirmed;
                    await _usersRepository.Update(updatedUser);
                    var result = await _unitOfWork.Commit();
                    return (result > 0, string.Empty);
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        /// <summary>
        /// Deletes a user by Id
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns>Delete result</returns>
        public async Task<(bool, string)> DeleteUser(Guid id)
        {
            ArgumentNullException.ThrowIfNull(id);

            var user = await GetUser(id);
            if (user == null)
            {
                return (false, ErrorMessages.UserNotFound);
            }
            else
            {
                try
                {
                    ArgumentNullException.ThrowIfNull(_signInManager);
                    await _signInManager.UserManager.DeleteAsync(user);
                    await _unitOfWork.Commit();
                    return (true, string.Empty);
                }
                catch (Exception ex)
                {
                    return (false, ex.Message);
                }
            }
        }

        /// <summary>
        /// Login the user with email and password
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="password">Password</param>
        /// <returns>Result of login</returns>
        public async Task<(User, string)> LoginUser(string email, string password)
        {
            var errorMessage = string.Empty;
            ArgumentNullException.ThrowIfNull(_signInManager);
            var user = await _usersRepository.Get(f => f.Email.Equals(email), null, null);
            if (user != null)
            {
                var loginAttempt = await _signInManager.CheckPasswordSignInAsync(user, password, true);
                if (loginAttempt.Succeeded)
                {
                    var roles = await _signInManager.UserManager.GetRolesAsync(user);
                    user.Role = roles.FirstOrDefault();
                    return (user, errorMessage);
                }
                else if (loginAttempt.IsLockedOut)
                {
                    errorMessage = ErrorMessages.AccountLockedOut;
                }
                else if (loginAttempt.IsNotAllowed)
                {
                    errorMessage = ErrorMessages.AccountNotConfirmed;
                }
                else if (loginAttempt.RequiresTwoFactor)
                {
                    errorMessage = ErrorMessages.AccountRequires2FA;
                }
                else
                {
                    errorMessage = ErrorMessages.FailedLogin;
                }
                return (null, errorMessage);
            }
            else
            {
                return (null, ErrorMessages.UserNotFound);
            }
        }

        /// <summary>
        /// Confirm the user account with the id and email
        /// </summary>
        /// <param name="id">User id</param>
        /// <param name="email">User email</param>
        /// <returns>Result of account confirmation</returns>
        public async Task<(bool, string)> ConfirmEmail(Guid id, string email)
        {
            ArgumentNullException.ThrowIfNull(_signInManager);
            var user = await _signInManager.UserManager.FindByEmailAsync(email);
            if (user != null && user.Id.Equals(id.ToString()))
            {
                var confirmationToken = await _signInManager.UserManager.GenerateEmailConfirmationTokenAsync(user);
                if (confirmationToken != null)
                {
                    var updatedUser = await _signInManager.UserManager.ConfirmEmailAsync(user, confirmationToken);
                    if (updatedUser != null)
                    {
                        if (updatedUser.Succeeded)
                        {
                            return (true, string.Empty);
                        }
                        else
                        {
                            return (false, string.Join(" ", updatedUser.Errors.Select(x => x.Description)));
                        }
                    }
                    else
                    {
                        return (false, ErrorMessages.ErrorConfirmAccount);
                    }
                }
                else
                {
                    return (false, ErrorMessages.ConfirmTokenInvalid);
                }
            }
            else
            {
                return (false, ErrorMessages.AccountTokenError);
            }
        }

        /// <summary>
        /// Updates the user role
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newRole"></param>
        /// <returns>Result of user's role update</returns>
        public async Task<(bool, string)> UpdateUserRole(Guid id, string newRole)
        {
            ArgumentNullException.ThrowIfNull(newRole);
            var user = await _usersRepository.Get(id.ToString());
            if (user != null)
            {
                user.UpdatedDate = DateTime.Now;
                var roles = await _signInManager.UserManager.GetRolesAsync(user);
                var result = await _signInManager.UserManager.RemoveFromRolesAsync(user, roles);
                if (result.Succeeded)
                {
                    result = await _signInManager.UserManager.AddToRoleAsync(user, newRole);
                    if (result.Succeeded)
                    {
                        return (true, string.Empty);
                    }
                    return (false, string.Join("", result.Errors.Select(x => x.Description)));
                }
                return (false, string.Join("", result.Errors.Select(x => x.Description)));
            }
            return (false, ErrorMessages.UserNotFound);
        }

        /// <summary>
        /// Gets the roles for the users
        /// </summary>
        /// <returns>IEnumeranle of roles</returns>
        public async Task<IEnumerable<string>> GetRoles()
        {
            var roles = await _rolesRepository.GetAll(null, null, null, 1, 1000);
            return roles.Item1.Select(x => x.Name);
        }
    }
}
