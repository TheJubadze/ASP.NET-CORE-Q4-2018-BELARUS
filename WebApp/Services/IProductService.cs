using System.Collections.Generic;
using DataAccess.Models;
using WebApp.ViewModels;

namespace WebApp.Services
{
    public interface IProductService
    {
        IEnumerable<Product> GetAll();
        IEnumerable<Product> GetMany();
        Product Get(int id);
        Product GetFullProduct(int id);

        Product Create(Product product);
        Product Update(Product product);
        int Delete(Product product);

        ProductEditViewModel ProductEditViewModel { get; }
    }
}
