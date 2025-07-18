namespace DataBrokerAPI.Repositories
{
    public interface IUnitOfWork
    {
        ICustomerRepo CustomerRepo
        {
            get;
        }
        void Save();
        void Rollback();
    }
}
