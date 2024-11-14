using ECommerceWebMicroservices.Models;
using ECommerceWebMicroservices.Models.DTO;

namespace ProductCatalog.Services.Interface
{
    public interface ICartService
    {
        Task<List<Cart>> GetCartItems(string userId);
        Task AddCartItem(string userId, CartItemDto cartItemDto);

    }
}
