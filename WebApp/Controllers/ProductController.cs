using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartBreadcrumbs;
using WebApp.Services;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ILogger _logger;

        public ProductController(ILogger logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [Breadcrumb("Products")]
        public IActionResult Index()
        {
            ViewBag.Title = "Products";
            _logger.LogInformation("Products list");
            var model = new ProductIndexViewModel {Products = _productService.GetMany()};

            return View(model);
        }

        [HttpGet]
        [Breadcrumb("Create", FromAction = "Index")]
        public IActionResult Create()
        {
            ViewBag.Title = "New Product";

            return View(_productService.ProductEditViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductEditViewModel createViewModel)
        {
            if (!ModelState.IsValid) 
                return View(_productService.ProductEditViewModel);

            var product = _productService.Create(createViewModel.Product);

            return RedirectToAction(nameof(Details), new {id = product.ProductId});
        }

        [HttpGet]
        [Breadcrumb("Edit", FromAction = "Index")]
        public IActionResult Edit(int id)
        {
            ViewBag.Title = "Edit Product";
            _productService.ProductEditViewModel.Product = _productService.Get(id);

            return View(_productService.ProductEditViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductEditViewModel createViewModel)
        {
            if (!ModelState.IsValid) 
                return View(_productService.ProductEditViewModel);

            var product = _productService.Update(createViewModel.Product);

            return RedirectToAction(nameof(Details), new {id = product.ProductId});
        }

        [HttpGet]
        [Breadcrumb("Details", FromAction = "Index")]
        public IActionResult Details(int id)
        {
            var product = _productService.GetFullProduct(id) ?? throw new ArgumentOutOfRangeException();
            
            return View(product);
        }
    }
}