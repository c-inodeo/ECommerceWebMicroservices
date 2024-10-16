using ECommerceWebMicroservices.Models;
using ProductCatalog.DTO;
using AutoMapper;

namespace ProductCatalog.Profiles
{
    public class ProductProfiles : Profile
    {
        public ProductProfiles() 
        {
            CreateMap<Product, ProductDto>();
            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();
        }
    }
}