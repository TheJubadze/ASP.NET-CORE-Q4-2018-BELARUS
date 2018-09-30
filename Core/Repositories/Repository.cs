using System;
using System.Collections.Generic;
using DataAccess.Models;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbSet<TEntity> _dbSet;

        protected Repository(NorthwindContext context)
        {
            _dbSet = context.Set<TEntity>();
        }

        public TEntity Get(int id)
        {
            return _dbSet.Find(id);
        }

        public IEnumerable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includes)
        {
            return includes.Aggregate<Expression<Func<TEntity, object>>, IQueryable<TEntity>>(_dbSet, (current, include) => current.Include(include));
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }
    }
}
