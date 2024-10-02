using ECommerceWebMicroservices.DataAccess.Repository.IRepository;
using ECommerceWebMicroservices.Models;
using ECommerceWebMicroservices.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceWebMicroservices.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddProduct(Product product)
        {
            await _unitOfWork.Product.Add(product);
            await _unitOfWork.Save();
        }

        public async Task DeleteProduct(int id)
        {
            var prod = await _unitOfWork.Product.Get(u => u.Id == id);
            if (prod != null) 
            {
                await _unitOfWork.Product.Remove(prod);
                await _unitOfWork.Save();
            }
        }

        public async Task<List<Product>> GetAllProducts()
        {
            var allProducts = await _unitOfWork.Product.GetAll();
            return allProducts.ToList();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _unitOfWork.Product.GetById(id);
        }

        public async Task UpdateProduct(Product product)
        {
            await _unitOfWork.Product.Update(product);
            await _unitOfWork.Save();
        }
    }
}
