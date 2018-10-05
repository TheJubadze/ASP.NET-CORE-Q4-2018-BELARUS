using AutoMapper;
using Core;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly int _productsCount;

        public ProductController(IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

            var cfg = configuration.GetSection("RepositorySettings");
            _productsCount = cfg.GetValue<int>("ProductsCountMax");
        }

        public IActionResult Index()
        {
            var model = new ProductIndexViewModel { Products = _unitOfWork.Products.GetFirst(_productsCount) };
            
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new ProductCreateViewModel
            {
                Categories = _unitOfWork.Categories.GetAll(),
                Suppliers = _unitOfWork.Suppliers.GetAll()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(ProductCreateViewModel createViewModel)
        {
            _unitOfWork.Products.Add(_mapper.Map<Product>(createViewModel.Product));
            _unitOfWork.Complete();

            var model = new ProductIndexViewModel { Products = _unitOfWork.Products.GetFirst(_productsCount) };

            return View("Index", model);
        }
    }
}
