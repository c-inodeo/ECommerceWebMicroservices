using ECommerceWebMicroservices.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Repository.IRepository;
using System.Security.Claims;

namespace ProductCatalog.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private string GetUserIdFromToken()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
        [HttpGet("get-cart")]
        public async Task<IActionResult> GetCart()
        {
            var userId = GetUserIdFromToken();
            var token = Request.Headers["Authorization"].ToString();
            Console.WriteLine($"=====UserId {userId}=====");
            Console.WriteLine("=====Getting cart items from user=====");
            var cart = await _unitOfWork.Cart.GetCartByUserId(userId);
            if (cart == null)
            {
                Console.WriteLine($"====Authorization Header: {token}");
                return NotFound(new { message = "Cart not found" });
            }
            Console.WriteLine($"====Authorization Header: {token}");
            return Ok(cart);
        }
        [HttpPost("add-to-cart")]
        public async Task<IActionResult> AddToCart([FromBody]CartItemDto cartItemDto)
        {
            var userId = GetUserIdFromToken();
            var token = Request.Headers["Authorization"].ToString();

            Console.WriteLine($"=====UserId {userId.ToString()}=====");
            Console.WriteLine("=====Adding cart items=====");
            Console.WriteLine($"====Authorization Header: {token}");

            await _unitOfWork.Cart.AddToCart(userId, cartItemDto.ProductId, cartItemDto.Quantity);
            await _unitOfWork.Save();
            return Ok(new { message = "Added to cart"});
        }
        [HttpPost("update-cart")]
        public async Task<IActionResult> UpdateCart([FromBody] CartItemDto cartItemDto)
        {
            var userId = GetUserIdFromToken();
            Console.WriteLine($"=====UserId {userId}=====");
            Console.WriteLine("=====Updating cart items=====");
            await _unitOfWork.Cart.UpdateCartItem(userId, cartItemDto.ProductId, cartItemDto.Quantity);
            await _unitOfWork.Save();
            return Ok(new { message = "Added to cart" });
        }
        [HttpDelete("delete-item/{productId}")]
        public async Task<IActionResult> DeleteItem(int productId, CartItemDto cartItemDto)
        {
            var userId = GetUserIdFromToken();
            Console.WriteLine($"=====UserId {userId}=====");
            Console.WriteLine("=====Deleting item(s)=====");
            if (productId == cartItemDto.ProductId)
            {
                await _unitOfWork.Cart.RemoveFromCart(userId, productId);
                await _unitOfWork.Save();
                return Ok(new { message = "Deleted item" });
            }
            return NotFound(new { message = "Item does not exist" });
        }
        [HttpGet("cart-test")]
        public IActionResult GetTest()
        {
            Console.WriteLine("TEEEEEEEESSTT--2");
            Console.WriteLine("====>Test endpoint hit via Ocelot Gateway!");
            return Ok("====>TEEEEEEEESSTT--2");
        }
    }
}