using System.Collections.Generic;
using DataAccess.Models;

namespace WebApp.ViewModels
{
    public class ProductEditViewModel
    {
        public Product Product { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Supplier> Suppliers { get; set; }
    }
}
