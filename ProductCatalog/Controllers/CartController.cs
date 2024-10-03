using ECommerceWebMicroservices.DataAccess.Repository.IRepository;
using ECommerceWebMicroservices.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ProductCatalog.Controllers
{
    //[Authorize]
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
            var cart = await _unitOfWork.Cart.GetCartByUserId(userId);
            if (cart == null)
            {
                return NotFound(new { message = "Cart not found" });
            }
            return Ok(cart);
        }
        [HttpPost("add-to-cart")]
        public async Task<IActionResult> AddToCart([FromBody]CartItemDto cartItemDto)
        {
            var userId = GetUserIdFromToken();
            await _unitOfWork.Cart.AddToCart(userId, cartItemDto.ProductId, cartItemDto.Quantity);
            await _unitOfWork.Save();
            return Ok(new { message = "Added to cart"});
        }
        [HttpPost("update-cart")]
        public async Task<IActionResult> UpdateCart([FromBody] CartItemDto cartItemDto)
        {
            var userId = GetUserIdFromToken();
            await _unitOfWork.Cart.UpdateCartItem(userId, cartItemDto.ProductId, cartItemDto.Quantity);
            await _unitOfWork.Save();
            return Ok(new { message = "Added to cart" });
        }
        [HttpDelete("delete-item/{productId}")]
        public async Task<IActionResult> DeleteItem(int productId, CartItemDto cartItemDto)
        {
            var userId = GetUserIdFromToken();
            if (productId == cartItemDto.ProductId)
            {
                await _unitOfWork.Cart.RemoveFromCart(userId, productId);
                await _unitOfWork.Save();
                return Ok(new { message = "Deleted item" });
            }
            return NotFound(new { message = "Item does not exist" });
        }
    }
}
