using Microsoft.EntityFrameworkCore;
using RepositoryManager.Interface;

namespace RepositoryManager.Impl
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;

        public UnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void BeginTransaction()
        {
            _dbContext.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _dbContext.Database.CommitTransaction();
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            return new Repository<T>(_dbContext);
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
