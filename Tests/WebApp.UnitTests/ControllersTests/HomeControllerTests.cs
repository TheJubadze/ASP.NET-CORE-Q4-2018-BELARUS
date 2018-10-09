using Microsoft.AspNetCore.Mvc;
using WebApp.Controllers;
using Xunit;

namespace WebApp.UnitTests.ControllersTests
{
    public class HomeControllerTests
    {
        [Fact]
        public void Index_Returns_A_ViewResult()
        {
            //Arrange
            var homeController = new HomeController();

            //Act
            var result = homeController.Index();

            //Assert
            Assert.IsType<ViewResult>(result);
        }
    }
}
