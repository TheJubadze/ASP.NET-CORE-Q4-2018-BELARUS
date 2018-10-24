using System.Linq;
using AutoMapper;
using Core;
using Core.UnitTests;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using WebApp.Controllers;
using WebApp.ViewModels;
using Xunit;
using Moq;

namespace WebApp.UnitTests.ControllersTests
{
    public class CategoryControllerTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryControllerTests()
        {
            IMockService mockService = new MockService();
            _unitOfWork = mockService.UnitOfWork;
            _mapper = new Mock<IMapper>().Object;
        }

        [Fact]
        public void Index_Returns_A_ViewResult_And_List_Of_Categories()
        {
            //Arrange
            var categoryController = new CategoryController(_unitOfWork, _mapper);

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
            var categoryController = new CategoryController(uow.Object, _mapper);

            //Act
            var result = categoryController.Edit(1);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CategoryEditViewModel>(viewResult.Model);
            Assert.IsType<Category>(model.Category);
        }    
    }
}
