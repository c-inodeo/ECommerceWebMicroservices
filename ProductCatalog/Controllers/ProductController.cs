using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ECommerceWebMicroservices.Models;
using ECommerceWebMicroservices.DataAccess.Repository.IRepository;

namespace ProductCatalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //Retrieve product information, add to cart, manage inventory
        [HttpPost("products")]
        public async Task<IActionResult> GetAllProducts()
        { 
            var allProducts = await _unitOfWork.Product.GetAll();
            return Ok(allProducts.ToList());
        }
    }
}
