namespace Marketplace.Admin.Data.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}
