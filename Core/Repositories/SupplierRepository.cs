using DataAccess.Models;

namespace Core.Repositories
{
    public class SupplierRepository : Repository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(NorthwindContext context) : base(context)
        {
        }
    }
}
