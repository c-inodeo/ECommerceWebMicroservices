using ECommerceWebMicroservices.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Repository.IRepository;
using ProductCatalog.Services.Interface;
using System.Security.Claims;

namespace ProductCatalog.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
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

            var cart = await _cartService.GetCartItems(userId);
            if (cart == null)
            {
                Console.WriteLine($"====Authorization Header: {token}");
                return NotFound(new { message = "Cart not found" });
            }
            return Ok(cart);
        }
        [HttpPost("upsert-cart")]
        public async Task<IActionResult> Upsert([FromBody]CartItemDto cartItemDto)
        {
            var userId = GetUserIdFromToken();
            var token = Request.Headers["Authorization"].ToString();

            Console.WriteLine("=====Adding cart items=====");

            await _cartService.UpsertCart(userId, cartItemDto);
            return Ok(new { message = "Added to cart"});
        }

        [HttpDelete("delete-item/{productId}")]
        public async Task<IActionResult> DeleteItem(int productId)
        {
            var userId = GetUserIdFromToken();
            Console.WriteLine("=====Deleting item(s)=====");
            await _cartService.RemoveCartItem(productId, userId);
            return Ok(new { message = "Deleted item" });
        }
        [HttpGet("cart-test")]
        public IActionResult GetTest()
        {
            Console.WriteLine("====>Test endpoint hit via Ocelot Gateway!");
            return Ok("====>Test endpoint hit via Ocelot Gateway!");
        }
    }
}