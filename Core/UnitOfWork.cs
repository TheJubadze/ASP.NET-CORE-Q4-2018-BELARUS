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
            Categories = new CategoryRepository(_context);
        }

        public ICategoryRepository Categories { get; }
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
