using AutoMapper;

namespace ProductCatalog.DTO
{
    public class ProductDto
    { 
        public int Id { get; set; }
        public string ProductName { get; set; }    
        public string Description { get; set; }
        public double Price { get; set; }
    }
}
