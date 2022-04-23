namespace Infrastructure.Repository
{
    /// <summary>
    /// Unit of Work interface
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Commit method to repository
        /// </summary>
        /// <returns>Number of affected records</returns>
        public Task<int> Commit();
    }
}
