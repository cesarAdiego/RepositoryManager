using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace RepositoryManager.Interface
{
    public interface IRepository<T>
    {
        List<T> GetAll(Func<T, bool> filterExpression = null, List<Expression<Func<T, object>>> includeProperties = null);

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        void AddMany(List<T> entities);

        void UpdateMany(List<T> entities);

        void DeleteMany(List<T> entities);
    }
}
