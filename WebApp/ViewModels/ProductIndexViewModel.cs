using System.Collections.Generic;
using DataAccess.Models;

namespace WebApp.ViewModels
{
    public class ProductIndexViewModel
    {
        public IEnumerable<Product> Products { get; set; }
    }
}
