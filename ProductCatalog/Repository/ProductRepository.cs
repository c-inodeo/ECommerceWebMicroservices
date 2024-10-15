using ECommerceWebMicroservices.Models;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Repository.IRepository;
using ProductCatalog.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task Update(Product obj)
        {
            var objFromDb = await _context.Products.FirstOrDefaultAsync(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.ProductName = obj.ProductName;
                objFromDb.Description = obj.Description;
                objFromDb.Price = obj.Price;
            }
        }
    }
}
