using System.Collections.Generic;
using DataAccess.Models;

namespace Core.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Product GetFullProduct(int id);
        IEnumerable<Product> GetFirst(int count);
    }
}
