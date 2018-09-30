using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Core.UnitTests
{
    public class MockService : IMockService
    {
        public MockService()
        {
            var fixture = new Fixture();
            
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            CategoriesFixture = fixture.Create<IEnumerable<Category>>().ToList();
            fixture.Customizations.Clear();
            fixture.Customizations.Add(new ElementsBuilder<Category>(CategoriesFixture));

            DbContextMock = new Mock<NorthwindContext>();
            CategoryDbSetMock = GetDbSetMock(CategoriesFixture.AsQueryable());
            DbContextMock.Setup(_ => _.Set<Category>()).Returns(CategoryDbSetMock.Object);
            UnitOfWork = new UnitOfWork(DbContextMock.Object);
        }

        public IEnumerable<Category> CategoriesFixture { get; }
        public IUnitOfWork UnitOfWork { get; }
        public Mock<NorthwindContext> DbContextMock { get; }
        public Mock<DbSet<Category>> CategoryDbSetMock { get; }

        private Mock<DbSet<TEntity>> GetDbSetMock<TEntity>(IQueryable<TEntity> entities) where TEntity : class
        {
            var mockSet = new Mock<DbSet<TEntity>>();
            mockSet.As<IQueryable<TEntity>>().Setup(x => x.Provider).Returns(entities.Provider);
            mockSet.As<IQueryable<TEntity>>().Setup(x => x.Expression).Returns(entities.Expression);
            mockSet.As<IQueryable<TEntity>>().Setup(x => x.ElementType).Returns(entities.ElementType);
            mockSet.As<IQueryable<TEntity>>().Setup(x => x.GetEnumerator()).Returns(entities.GetEnumerator);
            mockSet.Setup(_ => _.Add(It.IsAny<TEntity>()));
            return mockSet;
        }
    }
}
