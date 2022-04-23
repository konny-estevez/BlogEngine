using Entities;
using Infrastructure;
using Infrastructure.Repository;

namespace ApplicationCore.Repositories
{
    public class PostsRepository : GenericRepository<Post>, IPostsRepository
    {
        public PostsRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {

        }
    }
}
