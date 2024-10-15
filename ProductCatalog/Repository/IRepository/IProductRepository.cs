using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceWebMicroservices.Models;

namespace ProductCatalog.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        Task Update(Product obj);
    }
}
