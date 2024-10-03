using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceWebMicroservices.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        public Product ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public Product Product { get; set; }
    }
}
