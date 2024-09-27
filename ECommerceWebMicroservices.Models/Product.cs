using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceWebMicroservices.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        [DisplayName("Product Name")]
        public string? ProductName { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public double Price { get; set; }
    }
}
