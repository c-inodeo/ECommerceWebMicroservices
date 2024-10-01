using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ECommerceWebMicroservices.Models;
using ECommerceWebMicroservices.DataAccess.Repository.IRepository;
using ECommerceWebMicroservices.Services.Interface;

namespace ProductCatalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController( IProductService productService)
        {
            _productService = productService;
        }
        //Retrieve product information, add to cart, manage inventory
        [HttpGet("products")]
        public async Task<IActionResult> GetAllProducts()
        {
            List<Product> products = await _productService.GetAllProducts();
            var result = products.Select(p => new
            {
                p.Id,
                p.ProductName,
                p.Description,
                p.Price,
            }).ToList();

            return Ok(new { data = result });
        }
    }
}
