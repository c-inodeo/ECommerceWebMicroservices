using ECommerceWebMicroservices.DataAccess.Repository.IRepository;
using ProductCatalog.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceWebMicroservices.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IProductRepository Product { get; private set; }
        public ICartRepository Cart { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Product = new ProductRepository(_context);
            Cart = new CartRepository(_context);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
