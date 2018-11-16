using System;
using System.IO;
using System.Threading.Tasks;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using WebApp.Common;
using WebApp.Services;
using WebApp.ViewModels;

namespace WebApp.Controllers.API
{
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
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
            Category category;
            try
            {
                category = await _categoryService.UpdateAsync(categoryEditViewModel);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(category);
        }

        [HttpGet]
        [Route("image/{id}")]
        public IActionResult Image(int id)
        {
            var pic = _categoryService.GetPicture(id);

            if (pic == null)
                return BadRequest($"There is no image with ID={id}");

            return File(new MemoryStream(pic), Constants.CONTENT_TYPE_IMAGE);
        }
    }
}
