using System.Collections.Generic;
using Core;
using DataAccess.Models;
using WebApp.ViewModels;

namespace WebApp.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly int _productsCount;
        private readonly ProductEditViewModel _productEditViewModel;

        public ProductService(IUnitOfWork unitOfWork, IConfigurationService configurationService)
        {
            _unitOfWork = unitOfWork;
            _productsCount = configurationService.ProductsCount;
            _productEditViewModel = new ProductEditViewModel();
        }

        public ProductEditViewModel ProductEditViewModel
        {
            get
            {
                _productEditViewModel.Categories = _unitOfWork.Categories.GetAll();
                _productEditViewModel.Suppliers = _unitOfWork.Suppliers.GetAll();
                return _productEditViewModel;
            }
        }

        public IEnumerable<Product> GetAll()
        {
            return _unitOfWork.Products.GetFirst(_productsCount);
        }

        public Product Get(int id)
        {
            return _unitOfWork.Products.Get(id);
        }

        public Product GetFullProduct(int id)
        {
            return _unitOfWork.Products.GetFullProduct(id);
        }

        public Product Create(ProductEditViewModel productEditViewModel)
        {
            var product = _unitOfWork.Products.Add(productEditViewModel.Product);
            _unitOfWork.Complete();

            return product;
        }

        public Product Update(ProductEditViewModel productEditViewModel)
        {
            var product = _unitOfWork.Products.Update(productEditViewModel.Product);
            _unitOfWork.Complete();

            return product;
        }
    }
}
