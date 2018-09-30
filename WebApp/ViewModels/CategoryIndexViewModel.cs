using System.Collections.Generic;
using DataAccess.Models;

namespace WebApp.ViewModels
{
    public class CategoryIndexViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
    }
}
