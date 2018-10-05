using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DataAccess.Models;

namespace Core.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(NorthwindContext context) : base(context)
        {
        }

        public IEnumerable<Product> GetFirst(int count, params Expression<Func<Product, object>>[] includes)
        {
            return count > 0 ? GetAll(includes).Take(count) : GetAll(includes);
        }
    }
}
