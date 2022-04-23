using Entities;
using Infrastructure;
using Infrastructure.Repository;

namespace ApplicationCore.Repositories
{
    public class CommentsRepository : GenericRepository<Comment>, ICommentsRepository
    {
        public CommentsRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
