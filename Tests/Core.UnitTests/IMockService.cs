using System.Collections.Generic;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Core.UnitTests
{
    public interface IMockService
    {
        IEnumerable<Category> CategoriesFixture { get; }
        IUnitOfWork UnitOfWork { get; }
        Mock<NorthwindContext> DbContextMock { get; }
        Mock<DbSet<Category>> CategoryDbSetMock { get; }
    }
}
