using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs;
using WebApp.Common;
using WebApp.Services;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [Breadcrumb("Categories")]
        public IActionResult Index()
        {
            ViewBag.Title = "Categories";
            var model = new CategoryIndexViewModel
            {
                Categories = _categoryService.GetAll()
            };

            return View(model);
        }

        [HttpGet]
        [Breadcrumb("Edit", FromAction = "Index")]
        public IActionResult Edit(int id)
        {
            ViewBag.Title = "Edit Category";

            var model = new CategoryEditViewModel
            {
                Category = _categoryService.Get(id)
            };

            model.Category.Picture = _categoryService.GetPicture(id);
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryEditViewModel categoryEditViewModel)
        {
            var category = await _categoryService.UpdateAsync(categoryEditViewModel);

            return RedirectToAction(nameof(Edit), new {id = category.CategoryId});
        }

        [HttpGet]
        [Route("/Category/Image")]
        public IActionResult Image(int id)
        {
            return File(new MemoryStream(_categoryService.GetPicture(id)), Constants.CONTENT_TYPE_IMAGE);
        }
    }
}
