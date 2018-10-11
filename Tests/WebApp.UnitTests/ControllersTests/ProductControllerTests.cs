using System.Linq;
using AutoFixture;
using AutoMapper;
using Core;
using Core.UnitTests;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using WebApp.Controllers;
using WebApp.Services;
using WebApp.ViewModels;
using Xunit;

namespace WebApp.UnitTests.ControllersTests
{
    public class ProductControllerTests
    {
        private const int CountOfProductsPerPage = 3;

        private readonly IUnitOfWork _unitOfWork;
        private readonly Mock<IConfigurationService> _configurationMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger> _loggerMock;
        
        //private readonly Fixture _fixture = new Fixture();
        
        public ProductControllerTests()
        {
            IMockService mockService = new MockService();
            _unitOfWork = mockService.UnitOfWork;

            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger>();

            _configurationMock = new Mock<IConfigurationService>();
            _configurationMock
                .Setup(x => x.ProductsCount)
                .Returns(CountOfProductsPerPage);
        }

        [Fact]
        public void Index_Returns_A_ViewResult_And_List_Of_Products()
        {
            //Arrange
            var productController = new ProductController(_unitOfWork, _mapperMock.Object, _configurationMock.Object, _loggerMock.Object);

            //Act
            var result = productController.Index();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ProductIndexViewModel>(viewResult.Model);
            Assert.Equal(CountOfProductsPerPage, model.Products.Count());
        }
        
        [Fact]
        public void Create_Get_Returns_ViewResult()
        {
            //Arrange
            var productController = new ProductController(_unitOfWork, _mapperMock.Object, _configurationMock.Object, _loggerMock.Object);

            //Act
            var result = productController.Create();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ProductEditViewModel>(viewResult.Model);
            Assert.Equal(CountOfProductsPerPage, model.Categories.Count());
            Assert.Equal(CountOfProductsPerPage, model.Suppliers.Count());
        }        

        [Fact]
        public void Create_Post_Returns_ViewResult()
        {
            //Arrange
            var productController = new ProductController(_unitOfWork, _mapperMock.Object, _configurationMock.Object, _loggerMock.Object);
            var createViewModel = new ProductEditViewModel()
            {
                Product = new Product(),
                Categories = _unitOfWork.Categories.GetAll(),
                Suppliers = _unitOfWork.Suppliers.GetAll()
            };

            //Act
            var result = productController.Create(createViewModel);

            //Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Details", viewResult.ActionName);
        }
    }
}
