using Infrastructure.Repository;
using Microsoft.AspNetCore.Identity;

namespace ApplicationCore.Repositories
{
    public interface IRolesRepository : IGenericRepository<IdentityRole>
    {
    }
}
