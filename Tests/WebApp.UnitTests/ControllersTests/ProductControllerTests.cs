using System;
using System.Linq;
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

        private readonly IMockService _mockService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly Mock<IConfigurationService> _configurationMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger> _loggerMock;
        
        private readonly ProductEditViewModel _createViewModel;

        public ProductControllerTests()
        {
            _mockService = new MockService();
            _unitOfWork = _mockService.UnitOfWork;

            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger>();

            _configurationMock = new Mock<IConfigurationService>();
            _configurationMock
                .Setup(x => x.ProductsCount)
                .Returns(CountOfProductsPerPage);

            _createViewModel  = new ProductEditViewModel
            {
                Product = new Product(),
                Categories = _unitOfWork.Categories.GetAll(),
                Suppliers = _unitOfWork.Suppliers.GetAll()
            };
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
        public void Create_Post_Redirects_To_Details()
        {
            //Arrange
            var productController = new ProductController(_unitOfWork, _mapperMock.Object, _configurationMock.Object, _loggerMock.Object);


            //Act
            var result = productController.Create(_createViewModel);

            //Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Details", viewResult.ActionName);
        }

        [Fact]
        public void Edit_Get_Returns_ViewResult()
        {
            //Arrange
            var productController = new ProductController(_unitOfWork, _mapperMock.Object, _configurationMock.Object, _loggerMock.Object);

            //Act
            var result = productController.Edit(1);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ProductEditViewModel>(viewResult.Model);
            Assert.Equal(CountOfProductsPerPage, model.Categories.Count());
            Assert.Equal(CountOfProductsPerPage, model.Suppliers.Count());
        }    

        [Fact]
        public void Edit_Post_Redirects_To_Details()
        {
            //Arrange
            var productController = new ProductController(_unitOfWork, _mapperMock.Object, _configurationMock.Object, _loggerMock.Object);


            //Act
            var result = productController.Edit(_createViewModel);

            //Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Details", viewResult.ActionName);
        }

        [Fact]
        public void Details_Returns_ViewResult()
        {
            //Arrange
            var productController = new ProductController(_unitOfWork, _mapperMock.Object, _configurationMock.Object, _loggerMock.Object);
            _mockService.ProductDbSetMock.Setup(x => x.Find(It.IsAny<int>())).Returns(new Product());

            //Act
            var result = productController.Details(1);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<Product>(viewResult.Model);
        }

        [Fact]
        public void Details_Throws_ArgumentOutOfRangeException()
        {
            //Arrange
            var productController = new ProductController(_unitOfWork, _mapperMock.Object, _configurationMock.Object, _loggerMock.Object);

            //Act
            //Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => productController.Details(1));
        }   
    }
}
