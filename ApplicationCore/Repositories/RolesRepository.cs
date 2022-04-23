using Infrastructure;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Identity;

namespace ApplicationCore.Repositories
{
    public class RolesRepository : GenericRepository<IdentityRole>, IRolesRepository
    {
        public RolesRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
