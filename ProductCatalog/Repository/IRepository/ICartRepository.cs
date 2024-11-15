using ECommerceWebMicroservices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Repository.IRepository
{
    public interface ICartRepository :IRepository<Cart>
    {
        Task<Cart> GetCartByUserId(string userId);
        Task<IEnumerable<Cart>> GetAll(Expression<Func<Cart, bool>>? filter = null, string? includeProperties = null);
        Task Upsert(string userId, CartItem cart);
    }
}
