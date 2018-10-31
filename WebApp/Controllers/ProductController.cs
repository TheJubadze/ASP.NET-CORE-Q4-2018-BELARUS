using System;
using AutoMapper;
using Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartBreadcrumbs;
using WebApp.Services;
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
            IConfigurationService configurationService,
            ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _productsCount = configurationService.ProductsCount;
            _productEditViewModel = new ProductEditViewModel();
        }

        [Breadcrumb("Products")]
        public IActionResult Index()
        {
            ViewBag.Title = "Products";
            _logger.LogInformation("Products list");
            var model = new ProductIndexViewModel {Products = _unitOfWork.Products.GetFirst(_productsCount)};

            return View(model);
        }

        [HttpGet]
        [Breadcrumb("Create", FromAction = "Index")]
        public IActionResult Create()
        {
            ViewBag.Title = "New Product";

            return View(ProductEditViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductEditViewModel createViewModel)
        {
            if (!ModelState.IsValid) 
                return View(ProductEditViewModel);

            var product = _unitOfWork.Products.Add(createViewModel.Product);
            _unitOfWork.Complete();

            return RedirectToAction(nameof(Details), new {id = product.ProductId});
        }

        [HttpGet]
        [Breadcrumb("Edit", FromAction = "Index")]
        public IActionResult Edit(int id)
        {
            ViewBag.Title = "Edit Product";
            _productEditViewModel.Product = _unitOfWork.Products.Get(id);

            return View(ProductEditViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductEditViewModel createViewModel)
        {
            if (!ModelState.IsValid) 
                return View(ProductEditViewModel);

            var product = _unitOfWork.Products.Update(createViewModel.Product);
            _unitOfWork.Complete();

            return RedirectToAction(nameof(Details), new {id = product.ProductId});
        }

        [HttpGet]
        [Breadcrumb("Details", FromAction = "Index")]
        public IActionResult Details(int id)
        {
            var product = _unitOfWork.Products.Get(id) ?? throw new ArgumentOutOfRangeException();
            
            if (product.CategoryId.HasValue)
                product.Category = _unitOfWork.Categories.Get(product.CategoryId.Value);

            if (product.SupplierId.HasValue)
                product.Supplier = _unitOfWork.Suppliers.Get(product.SupplierId.Value);
                
            return View(product);
        }
    }
}