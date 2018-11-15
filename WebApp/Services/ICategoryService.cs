using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Models;
using WebApp.ViewModels;

namespace WebApp.Services
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAll();
        Category Get(int id);
        int Delete(Category category);
        Task<Category> UpdateAsync(CategoryEditViewModel categoryEditViewModel);
        byte[] GetPicture(int id);
    }
}
