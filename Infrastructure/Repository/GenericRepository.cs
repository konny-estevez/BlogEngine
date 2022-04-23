using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly DbSet<T> _dataSet;

        /// <summary>
        /// GenericRepository class constructor
        /// </summary>
        /// <param name="applicationDbContext">ApplicationDbContext</param>
        /// <exception cref="ArgumentNullException">Exception</exception>
        public GenericRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
            _dataSet = _applicationDbContext.Set<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> Get(Guid id)
        {
            ArgumentNullException.ThrowIfNull(id);
            try
            {
                return await _dataSet.FindAsync(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<T> Get(string id)
        {
            ArgumentNullException.ThrowIfNull(id);
            try
            {
                return await _dataSet.FindAsync(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> ordering = null, string includedProperties = null)
        {
            try
            {
                IQueryable<T> query = _dataSet;
                if (filter != null)
                {
                    query = query.Where(filter);
                }
                if (!string.IsNullOrEmpty(includedProperties))
                {
                    var propiedades = includedProperties.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                    propiedades.ForEach(x => query = query.Include(x));
                }
                if (ordering != null)
                {
                    return await ordering(query).FirstOrDefaultAsync();
                }
                return await query.FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<long> Count(Expression<Func<T, bool>> filter = null)
        {
            try
            {
                IQueryable<T> query = _dataSet;
                if (filter != null)
                {
                    query = query.Where(filter);
                }
                return await query.LongCountAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<(IEnumerable<T>, long)> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> ordering = null, string includedProperties = null, int page = 1, int pageSize = 20)
        {
            if (page < 1)
                throw new ArgumentOutOfRangeException("page");
            if (pageSize < 1)
                throw new ArgumentOutOfRangeException("pageSize");

            try
            {
                IQueryable<T> query = _dataSet;
                if (filter != null)
                {
                    query = query.Where(filter);
                }
                if (!string.IsNullOrEmpty(includedProperties))
                {
                    var propiedades = includedProperties.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                    propiedades.ForEach(x => query = query.Include(x));
                }
                page = page > 1 ? --page : 0;
                IEnumerable<T> results;
                var count = await query.LongCountAsync();
                if (ordering != null)
                {
                    results = await ordering(query).Skip(pageSize * page).Take(pageSize).ToListAsync();
                }
                else
                {
                    results = await query.Skip(pageSize * page).Take(pageSize).ToListAsync();
                }

                return (results, count);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<T> Create(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            var result = await _dataSet.AddAsync(entity);
            return result.Entity;
        }

        public async Task<T> Update(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            return _dataSet.Update(entity).Entity;
        }

        public async Task<T> Delete(Guid id)
        {
            ArgumentNullException.ThrowIfNull(id);
            var entity = await Get(id);
            return _dataSet.Remove(entity).Entity;
        }

        public async Task<T> Delete(string id)
        {
            ArgumentNullException.ThrowIfNull(id);
            var entity = await Get(id);
            return _dataSet.Remove(entity).Entity;
        }

        public async Task<T> Delete(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            return _dataSet.Remove(entity).Entity;
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
