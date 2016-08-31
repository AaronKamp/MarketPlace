namespace Marketplace.Admin.Data.Infrastructure
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// Save Db Changes.
        /// </summary>
        void Commit();
    }
}
