namespace DataBrokerAPI.Repositories
{
    public interface IUnitOfWork
    {
        ICustomerRepo CustomerRepo
        {
            get;
        }
        void Save();
        Task SaveAsync();
        void Rollback();
        Task RollbackAsync();
        void BeginTransaction();
        void Dispose();
    }
}
