using System.Linq;
using Core;
using Core.UnitTests;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using WebApp.Controllers;
using WebApp.ViewModels;
using Xunit;
using Moq;
using WebApp.Services;

namespace WebApp.UnitTests.ControllersTests
{
    public class CategoryControllerTests
    {
        private readonly Mock<ICategoryService> _categoryServiceMock;

        public CategoryControllerTests()
        {
            IMockService mockService = new MockService();
            _categoryServiceMock = new Mock<ICategoryService>();
            _categoryServiceMock.Setup(x => x.GetAll()).Returns(mockService.CategoriesFixture);
            _categoryServiceMock.Setup(x => x.Get(It.IsAny<int>())).Returns(mockService.CategoriesFixture.First());
        }

        [Fact]
        public void Index_Returns_A_ViewResult_And_List_Of_Categories()
        {
            //Arrange
            var categoryController = new CategoryController(_categoryServiceMock.Object);

            //Act
            var result = categoryController.Index();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CategoryIndexViewModel>(viewResult.Model);
            Assert.Equal(3, model.Categories.Count());
        }

        [Fact]
        public void Edit_Get_Returns_ViewResult()
        {
            //Arrange
            var uow = new Mock<IUnitOfWork>();
            var cat = new Category
            {
                Picture = new byte[128]
            }; 
            uow.Setup(x => x.Categories.Get(It.IsAny<int>())).Returns(cat);
            var categoryController = new CategoryController(_categoryServiceMock.Object);

            //Act
            var result = categoryController.Edit(1);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CategoryEditViewModel>(viewResult.Model);
            Assert.IsType<Category>(model.Category);
        }    
    }
}
