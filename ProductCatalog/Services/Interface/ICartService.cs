using ECommerceWebMicroservices.Models;
using ECommerceWebMicroservices.Models.DTO;

namespace ProductCatalog.Services.Interface
{
    public interface ICartService
    {
        Task<List<CartItemDto>> GetCartItems(string userId);
        Task UpsertCart(string userId, CartItemDto cartItemDto);
        Task RemoveCartItem(int cartItemId, string userId);
    }
}
