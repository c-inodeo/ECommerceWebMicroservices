using ECommerceWebMicroservices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceWebMicroservices.Services.Interface
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProducts();
        Task DeleteProduct(int id);
        Task AddProduct(Product product);
        Task UpdateProduct(Product product);
        Task<Product> GetProductById(int id);
    }
}
