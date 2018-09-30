using Core.Repositories;
using DataAccess.Models;

namespace Core
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly NorthwindContext _context;

        public UnitOfWork(NorthwindContext context)
        {
            _context = context;
            Suppliers = new SupplierRepository(_context);
            Categories = new CategoryRepository(_context);
            Products = new ProductRepository(_context);
        }

        public ISupplierRepository Suppliers { get; }
        public ICategoryRepository Categories { get; }
        public IProductRepository Products { get; }

        public int Complete()
        {
            return _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
