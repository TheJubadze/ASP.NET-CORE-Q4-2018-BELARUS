using DataAccess.Models;

namespace Core.Repositories
{
    public class CategoryRepository : Repository<Categories>, ICategoryRepository
    {
        public CategoryRepository(NorthwindContext context) : base(context)
        {
        }
    }
}
