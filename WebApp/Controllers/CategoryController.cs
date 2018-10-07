using Core;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            ViewBag.Title = "Categories";
            var model = new CategoryIndexViewModel
            {
                Categories = _unitOfWork.Categories.GetAll()
            };

            return View(model);
        }
    }
}
