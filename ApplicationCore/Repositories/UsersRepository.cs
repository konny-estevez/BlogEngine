using Entities;
using Infrastructure;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Identity;

namespace ApplicationCore.Repositories
{
    public class UsersRepository : GenericRepository<User>, IUsersRepository
    {
        private readonly string[] roles = new string[] { "Admin", "Public", "Writer", "Editor" };

        public UsersRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            applicationDbContext.Database.EnsureCreated();
            var rolesSet = applicationDbContext.Set<IdentityRole>();
            if (rolesSet != null)
            {
                var currentRoles = rolesSet.Select(x => x.Name).ToList();
                if (roles.Except(currentRoles).Any())
                {
                    if (rolesSet.FirstOrDefault(x => x.Name.Equals("Admin")) == null)
                    {
                        rolesSet.Add(new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = DateTime.Now.Ticks.ToString() });
                    }
                    if (rolesSet.FirstOrDefault(x => x.Name.Equals("Public")) == null)
                    {
                        rolesSet.Add(new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Public", NormalizedName = "PUBLIC", ConcurrencyStamp = DateTime.Now.Ticks.ToString()});
                    }
                    if (rolesSet.FirstOrDefault(x => x.Name.Equals("Writer")) == null)
                    {
                        rolesSet.Add(new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Writer", NormalizedName = "WRITER", ConcurrencyStamp = DateTime.Now.Ticks.ToString() });
                    }
                    if (rolesSet.FirstOrDefault(x => x.Name.Equals("Editor")) == null)
                    {
                        rolesSet.Add(new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Editor", NormalizedName = "EDITOR", ConcurrencyStamp = DateTime.Now.Ticks.ToString() });
                    }
                    applicationDbContext.SaveChanges();
                }
            }
        }
    }
}
