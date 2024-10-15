using ECommerceWebMicroservices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Repository.IRepository
{
    public interface ICartRepository
    {
        Task<Cart> GetCartByUserId(string userId);
        Task AddToCart(string userId, int productId, int quantity);
        Task RemoveFromCart(string userId, int productId);
        Task UpdateCartItem(string userId, int productId, int quantity);
    }
}
