using Core;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    [Route("api/category")]
    public class CategoryApiController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryApiController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Categories()
        {
            var categories = _unitOfWork.Categories.GetAll();

            return Ok(categories);
        }
    }
}
