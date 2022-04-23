using System.Linq.Expressions;

namespace Infrastructure.Repository
{
    /// <summary>
    /// Generic Repository interface
    /// </summary>
    /// <typeparam name="T">Type of entity</typeparam>
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Method to get entity by Guid Id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>T entity</returns>
        Task<T> Get(Guid id);

        /// <summary>
        /// Method to get entity by string Id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>T entity</returns>
        Task<T> Get(string id);

        /// <summary>
        /// Method to get first entity by filter and ordering
        /// </summary>
        /// <param name="filter">filter expression</param>
        /// <param name="ordering">ordering expression</param>
        /// <param name="includedProperties">include properties string</param>
        /// <returns>T entity</returns>
        Task<T> Get(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> ordering, string includedProperties);

        /// <summary>
        /// Method that counts entities by filter 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Entity count</returns>
        Task<long> Count(Expression<Func<T, bool>> filter);

        /// <summary>
        /// Method to get all entities by filter and ordering
        /// </summary>
        /// <param name="filter">filter expression</param>
        /// <param name="ordering">ordering expression</param>
        /// <param name="includedProperties">include properties string</param>
        /// <returns>IEnumeralble of T entity</returns>
        Task<(IEnumerable<T>, long)> GetAll(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> ordering, string includedProperties, int page, int pageSize);

        /// <summary>
        /// Method to create an entity
        /// </summary>
        /// <param name="entity">T entity</param>
        /// <returns>Entity result of creation</returns>
        Task<T> Create(T entity);

        /// <summary>
        /// Method to update an entity 
        /// </summary>
        /// <param name="entity">T entity</param>
        /// <returns>Entity result of update</returns>
        Task<T> Update(T entity);

        /// <summary>
        /// Method to delete and entity by Id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Entity result of delete</returns>
        Task<T> Delete(Guid id);

        /// <summary>
        /// Method to delete and entity by Id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Entity result of delete</returns>
        Task<T> Delete(string id);

        /// <summary>
        /// Method to delete and entity
        /// </summary>
        /// <param name="entity">T entity</param>
        /// <returns>Entity result of delete</returns>
        Task<T> Delete(T entity);
    }
}
