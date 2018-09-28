using DataAccess.Models;

namespace Core.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(NorthwindContext context) : base(context)
        {
        }
    }
}
