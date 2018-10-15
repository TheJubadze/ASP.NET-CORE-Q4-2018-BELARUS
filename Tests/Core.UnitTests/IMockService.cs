using System.Collections.Generic;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Core.UnitTests
{
    public interface IMockService
    {
        IUnitOfWork UnitOfWork { get; }
        Mock<NorthwindContext> DbContextMock { get; }

        IEnumerable<Category> CategoriesFixture { get; }
        IEnumerable<Product> ProductsFixture { get; }
        IEnumerable<Supplier> SuppliersFixture { get; }

        
        Mock<DbSet<Category>> CategoryDbSetMock { get; }
        Mock<DbSet<Product>> ProductDbSetMock { get; }
        Mock<DbSet<Supplier>> SupplierDbSetMock { get; }
    }
}
