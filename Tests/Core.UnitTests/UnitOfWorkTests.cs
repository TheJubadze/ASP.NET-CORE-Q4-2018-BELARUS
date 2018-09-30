using System.Linq;
using Xunit;

namespace Core.UnitTests
{
    public class UnitOfWorkTests
    {
        private readonly IMockService _mockService;
        private readonly IUnitOfWork _unitOfWork;

        public UnitOfWorkTests()
        {
            _mockService = new MockService();
            _unitOfWork = _mockService.UnitOfWork;
        }

        [Fact]
        public void UnitOfWork_Categories_GetAll()
        {
            //Arrange
            var cat = _mockService.CategoriesFixture.FirstOrDefault();

            //Act
            var actualCategories = _unitOfWork.Categories.GetAll().ToList();

            //Assert
            Assert.Equal(_mockService.CategoriesFixture.Count(), actualCategories.Count);
            Assert.Contains(cat, actualCategories);
        }

        [Fact]
        public void UnitOfWork_Complete()
        {
            //Arrange

            //Act
            using (_unitOfWork)
            {
                _unitOfWork.Complete();
            }
            
            //Assert
            _mockService.DbContextMock.Verify(_ => _.SaveChanges());
        }
    }
}
