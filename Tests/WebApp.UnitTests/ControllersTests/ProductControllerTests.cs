using System;
using System.Linq;
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
        private readonly Mock<ILogger> _loggerMock;
        private readonly Mock<IProductService> _productServiceMock;

        public ProductControllerTests()
        {
            _mockService = new MockService();
            _productServiceMock = new Mock<IProductService>();
            _loggerMock = new Mock<ILogger>();

            var product = _mockService.ProductsFixture.First();

            var createViewModel  = new ProductEditViewModel
            {
                Product = new Product(),
                Categories = _mockService.CategoriesFixture,
                Suppliers = _mockService.SuppliersFixture
            };

            _productServiceMock.Setup(x => x.ProductEditViewModel).Returns(createViewModel);
            _productServiceMock.Setup(x => x.GetMany()).Returns(_mockService.ProductsFixture);
            _productServiceMock.Setup(x => x.Create(It.IsAny<Product>())).Returns(product);
            _productServiceMock.Setup(x => x.Update(It.IsAny<Product>())).Returns(product);
            _productServiceMock.Setup(x => x.GetFullProduct(It.IsAny<int>())).Returns(product);
        }

        [Fact]
        public void Index_Returns_A_ViewResult_And_List_Of_Products()
        {
            //Arrange
            var productController = new ProductController(_loggerMock.Object, _productServiceMock.Object);

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
            var productController = new ProductController(_loggerMock.Object, _productServiceMock.Object);

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
            var productController = new ProductController(_loggerMock.Object, _productServiceMock.Object);


            //Act
            var result = productController.Create(_productServiceMock.Object.ProductEditViewModel);

            //Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Details", viewResult.ActionName);
        }

        [Fact]
        public void Edit_Get_Returns_ViewResult()
        {
            //Arrange
            var productController = new ProductController(_loggerMock.Object, _productServiceMock.Object);

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
            var productController = new ProductController(_loggerMock.Object, _productServiceMock.Object);


            //Act
            var result = productController.Edit(_productServiceMock.Object.ProductEditViewModel);

            //Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Details", viewResult.ActionName);
        }

        [Fact]
        public void Details_Returns_ViewResult()
        {
            //Arrange
            var productController = new ProductController(_loggerMock.Object, _productServiceMock.Object);
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
            var productController = new ProductController(_loggerMock.Object, _productServiceMock.Object);
            _productServiceMock.Setup(x => x.GetFullProduct(It.IsAny<int>())).Returns((Product) null);

            //Act
            //Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => productController.Details(1));
        }   
    }
}
