using System;
using System.IO;
using System.Threading.Tasks;
using Core;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private const int PictureBytesToSkip = 78;

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

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Title = "Edit Category";

            var model = new CategoryEditViewModel
            {
                Category = _unitOfWork.Categories.Get(id)
            };

            model.Category.Picture = GetPicture(id);

            return View(model);
        }

        private byte[] GetPicture(int id)
        {
            var category = _unitOfWork.Categories.Get(id);
            var newArray = new byte[category.Picture.Length - PictureBytesToSkip];
            Array.Copy(category.Picture, PictureBytesToSkip, newArray, 0, newArray.Length);
            return newArray;
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryEditViewModel categoryEditViewModel)
        {
            var category = categoryEditViewModel.Category;

            if (categoryEditViewModel.File.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await categoryEditViewModel.File.CopyToAsync(memoryStream);

                    var newPic = memoryStream.ToArray();
                    var picture = new byte[PictureBytesToSkip + newPic.Length];

                    Array.Copy(newPic, 0, picture, PictureBytesToSkip, newPic.Length);

                    category.Picture = picture;
                    category = _unitOfWork.Categories.Update(category);
                    _unitOfWork.Complete();
                }
            }

            return RedirectToAction(nameof(Edit), new {id = category.CategoryId});
        }

        [HttpGet]
        public IActionResult Image(int id)
        {
            var model = new CategoryEditViewModel {Category = _unitOfWork.Categories.Get(id)};
            model.Category.Picture = GetPicture(id);
            return View(model);
        }
    }
}
