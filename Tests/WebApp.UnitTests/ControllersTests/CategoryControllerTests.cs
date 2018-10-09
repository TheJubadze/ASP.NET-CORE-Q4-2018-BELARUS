using System.Linq;
using Core;
using Core.UnitTests;
using Microsoft.AspNetCore.Mvc;
using WebApp.Controllers;
using WebApp.ViewModels;
using Xunit;

namespace WebApp.UnitTests.ControllersTests
{
    public class CategoryControllerTests
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryControllerTests()
        {
            IMockService mockService = new MockService();
            _unitOfWork = mockService.UnitOfWork;
        }
        [Fact]
        public void Index_Returns_A_ViewResult_And_List_Of_Categories()
        {
            //Arrange
            var categoryController = new CategoryController(_unitOfWork);

            //Act
            var result = categoryController.Index();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CategoryIndexViewModel>(viewResult.Model);
            Assert.Equal(3, model.Categories.Count());
        }
    }
}
