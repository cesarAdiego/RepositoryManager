using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryManager.Interface
{
    public interface IUnitOfWork
    {
        IRepository<T> GetRepository<T>() where T : class;

        void SaveChanges();

        void BeginTransaction();

        void CommitTransaction();
    }
}
