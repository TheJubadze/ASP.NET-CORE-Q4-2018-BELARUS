using AutoMapper;
using Core;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        private readonly int _productsCount;

        private readonly ProductEditViewModel _productEditViewModel;
        private ProductEditViewModel ProductEditViewModel
        {
            get
            {
                _productEditViewModel.Categories = _unitOfWork.Categories.GetAll();
                _productEditViewModel.Suppliers = _unitOfWork.Suppliers.GetAll();
                return _productEditViewModel;
            }
        }

        public ProductController(IUnitOfWork unitOfWork,
            IMapper mapper,
            IConfiguration configuration,
            ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;

            var cfg = configuration.GetSection("RepositorySettings");
            _productsCount = cfg.GetValue<int>("ProductsCountMax");

            _productEditViewModel = new ProductEditViewModel();
        }

        public IActionResult Index()
        {
            _logger.LogInformation(
                "ProductController Index opening!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            var model = new ProductIndexViewModel {Products = _unitOfWork.Products.GetFirst(_productsCount)};

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(ProductEditViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductEditViewModel createViewModel)
        {
            if (ModelState.IsValid)
            {
                var product = _unitOfWork.Products.Add(_mapper.Map<Product>(createViewModel.Product));
                _unitOfWork.Complete();

                return RedirectToAction(nameof(Details), new {id = product.ProductId});
            }
            else
            {
                return View(ProductEditViewModel);
            }
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var product = _unitOfWork.Products.Get(id);

            return View(product);
        }
    }
}