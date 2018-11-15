using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.Common;
using WebApp.Services;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    [Route("api/category")]
    public class CategoryApiController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryApiController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var categories = _categoryService.GetAll();

            return Ok(categories);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            var category = _categoryService.Get(id);

            return Ok(category);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(CategoryEditViewModel categoryEditViewModel)
        {
            var category = await _categoryService.UpdateAsync(categoryEditViewModel);

            return Ok(category);
        }

        [HttpGet]
        [Route("image/{id}")]
        public IActionResult Image(int id)
        {
            return File(new MemoryStream(_categoryService.GetPicture(id)), Constants.CONTENT_TYPE_IMAGE);
        }
    }
}
