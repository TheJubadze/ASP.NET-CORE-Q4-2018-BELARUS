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

        public int Delete(Product product)
        {
            _unitOfWork.Products.Delete(product);

            return _unitOfWork.Complete();
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
            return _unitOfWork.Products.GetAll();
        }

        public IEnumerable<Product> GetMany()
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

        public Product Create(Product product)
        {
            var p = _unitOfWork.Products.Add(product);
            _unitOfWork.Complete();

            return p;
        }

        public Product Update(Product product)
        {
            var p = _unitOfWork.Products.Update(product);
            _unitOfWork.Complete();

            return p;
        }
    }
}
