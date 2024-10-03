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

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        //Retrieve product information
        [HttpGet("get-products")]
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
        [HttpDelete("delete-product")]
        public async Task<IActionResult> Delete(int? id)
        {
            await _productService.DeleteProduct(id.Value);
            return Ok(new { message = "Delete Successful!" });
        }
        [HttpPost("add-product")]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            if (ModelState.IsValid) 
            {
                await _productService.AddProduct(product);
                return Ok(new { message = "Product added successfully" });
            }
            return BadRequest(new { message = "Invalid product data"});
        }
        [HttpPut("update-product/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            if (id != product.Id) 
            {
                return BadRequest(new { message = "Id mismatch!" });
            }

            var existingProduct = await _productService.GetProductById(id);
            if (existingProduct == null) 
            {
                return NotFound(new { message = "Product not found" });
            }
            existingProduct.ProductName = product.ProductName;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;

            await _productService.UpdateProduct(existingProduct);

            return Ok(new { message = "Product updated!" });
        }
    }
}
