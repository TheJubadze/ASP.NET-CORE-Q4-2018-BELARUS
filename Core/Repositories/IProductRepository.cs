using System.Collections.Generic;
using DataAccess.Models;

namespace Core.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        IEnumerable<Product> GetFirst(int count);
    }
}
