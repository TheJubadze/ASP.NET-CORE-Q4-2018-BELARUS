using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoFixture;
using DataAccess.Models;
using Moq;
using Xunit;

namespace Core.UnitTests
{
    public class RepositoryTests
    {
        private readonly Fixture _fixture;
        private readonly IMockService _mockService;
        private readonly IUnitOfWork _unitOfWork;

        public RepositoryTests()
        {
            _mockService = new MockService();
            _unitOfWork = _mockService.UnitOfWork;
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public void Repository_Add()
        {
            //Arrange
            var newCategory = new Mock<Category>().SetupAllProperties().Object;

            //Act
            _unitOfWork.Categories.Add(newCategory);
            _unitOfWork.Complete();

            //Assert
            _mockService.CategoryDbSetMock.Verify(x => x.Add(newCategory));
            _mockService.DbContextMock.Verify(x => x.SaveChanges());
        }

        [Fact]
        public void Repository_AddRange()
        {
            //Arrange
            var newCategories = _fixture.Create<IEnumerable<Category>>().ToList();

            //Act
            _unitOfWork.Categories.AddRange(newCategories);
            _unitOfWork.Complete();

            //Assert
            _mockService.CategoryDbSetMock.Verify(x => x.AddRange(newCategories));
            _mockService.DbContextMock.Verify(x => x.SaveChanges());
        }

        [Fact]
        public void Repository_Delete()
        {
            //Arrange
            var categoryToRemove = _unitOfWork.Categories.GetAll().FirstOrDefault();

            //Act
            _unitOfWork.Categories.Delete(categoryToRemove);
            _unitOfWork.Complete();

            //Assert
            _mockService.CategoryDbSetMock.Verify(x => x.Remove(categoryToRemove));
            _mockService.DbContextMock.Verify(x => x.SaveChanges());
        }

        [Fact]
        public void Repository_DeleteRange()
        {
            //Arrange
            var categoriesToRemove = _unitOfWork.Categories.GetAll().Take(2).ToList();

            //Act
            _unitOfWork.Categories.DeleteRange(categoriesToRemove);
            _unitOfWork.Complete();

            //Assert
            _mockService.CategoryDbSetMock.Verify(x => x.RemoveRange(categoriesToRemove));
            _mockService.DbContextMock.Verify(x => x.SaveChanges());
        }

        [Fact]
        public void Repository_Find()
        {
            //Arrange
            Expression<Func<Category, bool>> predicate = x => x.CategoryId > 100;
            var expected = _mockService.CategoriesFixture.Where(predicate.Compile());

            //Act
            var actual = _unitOfWork.Categories.Find(predicate);

            //Assert
            Assert.Equal(expected.Count(), actual.Count());
        }

        [Fact]
        public void Repository_Get()
        {
            //Arrange
            var id = 0;

            //Act
            _unitOfWork.Categories.Get(id);

            //Assert
            _mockService.CategoryDbSetMock.Verify(x => x.Find(id));
        }
    }
}
