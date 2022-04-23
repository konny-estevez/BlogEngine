using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public UnitOfWork(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
        }

        public async Task<int> Commit()
        {
            try
            {
                return await _applicationDbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.InnerException?.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Dispose method 
        /// </summary>
        public void Dispose()
        {
            _applicationDbContext.Dispose();
        }
    }
}
