using System;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using WebApp.Services;

namespace WebApp.Controllers.API
{
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var products = _productService.GetAll();

            return Ok(products);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            var product = _productService.Get(id);

            return Ok(product);
        }

        [HttpPost]
        public IActionResult Post([FromBody]Product product)
        {
            product.ProductId = 0;
            var newProduct = _productService.Create(product);

            return Ok(newProduct);
        }

        [HttpPut]
        public IActionResult Put([FromBody]Product product)
        {
            Product p;
            try
            {
                p = _productService.Update(product);
            }
            catch (Exception e)
            {
                return BadRequest($"ERROR updating Product with ID={product?.ProductId}. {e.Message}");
            }

            return Ok(p);
        }

        [HttpDelete]
        public IActionResult Delete([FromBody]Product product)
        {
            try
            {
                if (_productService.Delete(product) == 1)
                    return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return BadRequest();
        }
    }
}
