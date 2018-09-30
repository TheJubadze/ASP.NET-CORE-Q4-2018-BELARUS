using System.Linq;
using Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public ProductController(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var cfg = _configuration.GetSection("RepositorySettings");
            var count = cfg.GetValue<int>("ProductsCountMax");
            var products = count > 0
                ? _unitOfWork.Products
                    .GetAll(x => x.Category, x => x.Supplier)
                    .Take(count)
                : _unitOfWork.Products
                    .GetAll(x => x.Category, x => x.Supplier);

            var model = new ProductIndexViewModel { Products = products };
            
            return View(model);
        }

        public IActionResult Create()
        {
            var model = new ProductCreateViewModel
            {
                Categories = _unitOfWork.Categories.GetAll(),
                Suppliers = _unitOfWork.Suppliers.GetAll()
            };

            return View(model);
        }
    }
}
