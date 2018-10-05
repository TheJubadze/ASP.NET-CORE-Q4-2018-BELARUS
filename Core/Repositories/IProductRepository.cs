using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DataAccess.Models;

namespace Core.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        IEnumerable<Product> GetFirst(int count, params Expression<Func<Product, object>>[] includes);
    }
}
