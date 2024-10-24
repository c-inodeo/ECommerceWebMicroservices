using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ECommerceWebMicroservices.Models;
using ProductCatalog.Repository.IRepository;
using ProductCatalog.Services.Interface;
using AutoMapper;
using ProductCatalog.DTO;

namespace ProductCatalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }
        //Retrieve product information
        [HttpGet("get-products")]
        public async Task<IActionResult> GetAllProducts()
        {
            List<Product> products = await _productService.GetAllProducts();

            var productDtos =_mapper.Map<List<ProductDto>>(products);
            Console.WriteLine("====>Getting the data.....");
            return Ok(new { data = productDtos });
        }
        [HttpDelete("delete-product")]
        public async Task<IActionResult> Delete(int? id)
        {
            await _productService.DeleteProduct(id.Value);
            Console.WriteLine($"====Deleted {id}");
            return Ok(new { message = "Delete Successful!" });
        }
        [HttpPost("add-product")]
        public async Task<IActionResult> AddProduct([FromBody] CreateProductDto productDto)
        {
            if (ModelState.IsValid) 
            {
                var product = _mapper.Map<Product>(productDto);
                await _productService.AddProduct(product);
                return Ok(new { message = "Product added successfully" });
            }
            return BadRequest(new { message = "Invalid product data"});
        }
        [HttpPut("update-product/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto productdto)
        {
            if (id != productdto.Id) 
            {
                return BadRequest(new { message = "Id mismatch!" });
            }

            var existingProduct = await _productService.GetProductById(id);
            if (existingProduct == null) 
            {
                return NotFound(new { message = "Product not found" });
            }
            _mapper.Map(productdto, existingProduct);
            await _productService.UpdateProduct(existingProduct);

            return Ok(new { message = "Product updated!" });
        }
    }
}
