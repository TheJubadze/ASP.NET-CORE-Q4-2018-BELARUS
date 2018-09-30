using DataAccess.Models;

namespace Core.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(NorthwindContext context) : base(context)
        {
        }
    }
}
