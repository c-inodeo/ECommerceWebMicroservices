﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceWebMicroservices.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string UserId { get; set; }  // UserId from JWT token

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }

    public class CartItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }  // Reference to Product
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }  // Price of the item at the time of adding to cart

    }

}
