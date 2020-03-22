using Microsoft.EntityFrameworkCore;
using RepositoryManager.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RepositoryManager
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _dbContext;
        private DbSet<T> _dbSet;

        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void AddMany(List<T> entities)
        {
            _dbSet.AddRange(entities);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void DeleteMany(List<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public List<T> GetAll(Func<T, bool> filterExpression = null, List<Expression<Func<T, object>>> includeProperties = null)
        {
            List<T> items = null;

            if(includeProperties != null)
            {
                LoadIncludeProperties(_dbSet, includeProperties);
            }

            if(filterExpression != null)
            {
                items = _dbSet.Where(filterExpression).ToList();
            }
            else
            {
                items = _dbSet.ToList();
            }

            return items;
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void UpdateMany(List<T> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        private void LoadIncludeProperties(DbSet<T> dbSet, List<Expression<Func<T, object>>> includeProperties)
        {
            foreach (var includeProperty in includeProperties)
            {
                var body = includeProperty.Body as MethodCallExpression;
                
                if (body != null && body.Arguments != null && body.Arguments.Count > 1)
                {
                    var includeString = GetIncludeString(body);

                    dbSet.Include(includeString).Load();
                }
                else
                {
                    dbSet.Include(includeProperty).Load();
                }
            }
        }

        private string GetIncludeString(MethodCallExpression expression)
        {
            var includeString = "";

            for (var i = 0; i < expression.Arguments.Count; i++)
            {
                var argument = expression.Arguments[i];
                var memberExpression = argument as MemberExpression;

                if (memberExpression != null)
                {
                    includeString += memberExpression.Member.Name + ".";
                }
                else
                {
                    var lambdaExpression = argument as LambdaExpression;
                    includeString += lambdaExpression.Body.Type.Name;
                }
            }

            return includeString;
        }
    }
}
