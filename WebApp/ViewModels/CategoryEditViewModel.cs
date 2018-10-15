using System.Collections.Generic;
using DataAccess.Models;
using Microsoft.AspNetCore.Http;

namespace WebApp.ViewModels
{
    public class CategoryEditViewModel
    {
        public Category Category { get; set; }
        public IFormFile File { get; set; }
    }
}
