using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Core;
using DataAccess.Models;
using WebApp.ViewModels;

namespace WebApp.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private const int PictureBytesToSkip = 78;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<Category> GetAll()
        {
            return _unitOfWork.Categories.GetAll();
        }

        public Category Get(int id)
        {
            return _unitOfWork.Categories.Get(id);
        }

        public int Delete(Category category)
        {
            _unitOfWork.Categories.Delete(category);

            return _unitOfWork.Complete();
        }

        public async Task<Category> UpdateAsync(CategoryEditViewModel categoryEditViewModel)
        {
            var category = _unitOfWork.Categories.Get(categoryEditViewModel.Category.CategoryId);
            _mapper.Map(categoryEditViewModel, category);

            if (categoryEditViewModel.File != null && categoryEditViewModel.File.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await categoryEditViewModel.File.CopyToAsync(memoryStream);

                    var newPic = memoryStream.ToArray();
                    var picture = new byte[PictureBytesToSkip + newPic.Length];

                    Array.Copy(newPic, 0, picture, PictureBytesToSkip, newPic.Length);

                    category.Picture = picture;
                }
            }

            category = _unitOfWork.Categories.Update(category);
            _unitOfWork.Complete();

            return category;
        }

        public byte[] GetPicture(int id)
        {
            var category = _unitOfWork.Categories.Get(id);
            if (category?.Picture == null)
                return null;

            var newArray = new byte[category.Picture.Length - PictureBytesToSkip];
            Array.Copy(category.Picture, PictureBytesToSkip, newArray, 0, newArray.Length);
            return newArray;
        }

    }
}
