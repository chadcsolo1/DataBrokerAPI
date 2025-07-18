using DataBrokerAPI.Data;

namespace DataBrokerAPI.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public ICustomerRepo customerRepo;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public ICustomerRepo CustomerRepo
        {
            get
            {
                return customerRepo ??= new CustomerRepo(_context);
            }
        }

        public void Rollback()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
