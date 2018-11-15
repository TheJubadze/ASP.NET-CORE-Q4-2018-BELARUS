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

        public Product GetFullProduct(int id)
        {
            Expression<Func<Product, object>> [] includes = { x => x.Category, x => x.Supplier };

            return GetAll(includes).FirstOrDefault(x => x.ProductId == id);
        }

        public IEnumerable<Product> GetFirst(int count)
        {
            Expression<Func<Product, object>> [] includes = { x => x.Category, x => x.Supplier };

            return count > 0 ? GetAll(includes).Take(count) : GetAll(includes);
        }
    }
}
