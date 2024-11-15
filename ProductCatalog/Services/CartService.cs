using ECommerceWebMicroservices.Models;
using ECommerceWebMicroservices.Models.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ProductCatalog.Repository;
using ProductCatalog.Repository.IRepository;
using ProductCatalog.Services.Interface;

namespace ProductCatalog.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task UpsertCart(string userId, CartItemDto cartItemDto)
        {
            var cartItem = await _unitOfWork.Cart.Get(c => c.UserId == userId);
            if (cartItem == null) 
            {
                cartItem = new Cart
                {
                    UserId = userId,
                    CartItems = new List<CartItem>()
                };
            }
            var existingItem= cartItem.CartItems.FirstOrDefault(ci => ci.ProductId == cartItemDto.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += cartItemDto.Quantity;
            }
            else
            {
                var newCartItems = new CartItem
                { 
                    ProductId = cartItemDto.ProductId,
                    Quantity = cartItemDto.Quantity

                };
                await _unitOfWork.Cart.Upsert(userId, newCartItems);
            }
            await _unitOfWork.Save();
        }

        public async Task<List<Cart>> GetCartItems(string userId)
        {
            var cartItems = await _unitOfWork.Cart.GetAll(c => c.UserId == userId, includeProperties: "CartItems.Product");
            foreach (var cart in cartItems)
            {
                foreach (var item in cart.CartItems)
                {
                    item.Price = item.Product.Price * item.Quantity;
                }
            }
            return cartItems.ToList();
        }

        public async Task RemoveCartItem(int cartItemId, string userId)
        {

            var item = await _unitOfWork.Cart.Get(c => c.UserId == userId, includeProperties: "CartItems.Product");
            var itemToBeDeleted = item.CartItems.FirstOrDefault(ci => ci.ProductId == cartItemId);
            if (itemToBeDeleted != null)
            {
                item.CartItems.Remove(itemToBeDeleted);
                await _unitOfWork.Save();
            }
            else
            {
                 Console.WriteLine("Item does not exist!");
            }
        }
    }
}
