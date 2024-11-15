using ECommerceWebMicroservices.Models;
using ProductCatalog.Repository.IRepository;
using ProductCatalog.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace ProductCatalog.Repository
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context) :base(context)
        {
            _context =context;
        }

        public async Task<Cart> GetCartByUserId(string userId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }
        public async Task<IEnumerable<Cart>> GetAll(Expression<Func<Cart, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<Cart> query = _context.Carts;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.ToListAsync();
        }

        public async Task Upsert(string userId, CartItem cartItem)
        {
            //Get user ID to add to cart items
            var cart = await GetCartByUserId(userId) ?? 
                new Cart
                {
                    UserId = userId,
                    CartItems = new List<CartItem>()
                };

            //Check ID of ProductsID then add to CarItem ang info sa product nga ni match

            var existingCartItem = cart.CartItems
                .FirstOrDefault(c => c.ProductId == cartItem.ProductId);
            var productPrice = await _context.Products.FindAsync(cartItem.ProductId);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity += cartItem.Quantity;
                if (existingCartItem.Product != null)
                {
                    existingCartItem.Price = existingCartItem.Product.Price * existingCartItem.Quantity;
                }
                else 
                {
                    existingCartItem.Price = productPrice.Price * existingCartItem.Quantity;
                }
            }
            else 
            {
                var product = await _context.Products.FindAsync(cartItem.ProductId);
                if (product != null) 
                { 
                    cartItem.Price = product.Price * cartItem.Quantity;
                    cart.CartItems.Add(cartItem);
                }
            }
            if (cart.Id == 0) 
            { 
                _context.Carts.Add(cart);
            }
            await _context.SaveChangesAsync();
        }
    }
}
