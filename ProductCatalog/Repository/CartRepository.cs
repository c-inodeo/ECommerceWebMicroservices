using ECommerceWebMicroservices.Models;
using ProductCatalog.Repository.IRepository;
using ProductCatalog.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context =context;
        }
        public async Task AddToCart(string userId, int productId, int quantity)
        {
            var cart = await GetCartByUserId(userId);
            if (cart == null) 
            {
                cart = new Cart
                {
                    UserId = userId,
                    CartItems = new List<CartItem>()
                };
                _context.Carts.Add(cart);
            }
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity
                };
                cart.CartItems.Add(cartItem);
            }
            else
            { 
                cartItem.Quantity += quantity;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<Cart> GetCartByUserId(string userId)
        {
            return await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c=> c.UserId == userId);
        }

        public async Task RemoveFromCart(string userId, int productId)
        {
            var cart = await GetCartByUserId(userId);
            var cartItem = cart?.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (cartItem == null) 
            {
                cart.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateCartItem(string userId, int productId, int quantity)
        {
            var cart = await GetCartByUserId(userId);
            var cartItem = cart?.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (cartItem != null)
            {
                cartItem.Quantity = quantity; 
                await _context.SaveChangesAsync();
            }
        }
    }
}
