using DataBrokerAPI.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace DataBrokerAPI.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction _transaction;

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

        public void BeginTransaction()
        {
            _transaction ??= _context.Database.BeginTransaction();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
            }
        }

        public void Save()
        {
            _context.SaveChanges();
            _transaction.Commit();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
            if (_transaction != null) 
            {
                await _transaction.CommitAsync();
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }


    }
}
