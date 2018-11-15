using System.Collections.Generic;
using DataAccess.Models;
using WebApp.ViewModels;

namespace WebApp.Services
{
    public interface IProductService
    {
        IEnumerable<Product> GetAll();
        Product Get(int id);
        Product GetFullProduct(int id);

        Product Create(ProductEditViewModel productEditViewModel);
        Product Update(ProductEditViewModel productEditViewModel);
        ProductEditViewModel ProductEditViewModel { get; }
    }
}
