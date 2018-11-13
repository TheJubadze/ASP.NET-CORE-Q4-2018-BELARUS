using Core;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    [Route("api/product")]
    public class ProductApiController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductApiController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Categories()
        {
            var categories = _unitOfWork.Products.GetAll();

            return Ok(categories);
        }
    }
}
