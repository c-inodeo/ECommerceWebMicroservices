using ECommerceWebMicroservices.Models;
using ECommerceWebMicroservices.Models.DTO;
using Mapster;

namespace ProductCatalog.Profiles
{
    public static class MappingFunctions
    {
        public static void RegisterMappings()
        {
            /*Source, Destination*/
            TypeAdapterConfig<CartItem, CartItemDto>.NewConfig()
                .Map(dest => dest.ProductId, src => src.ProductId)
                .Map(dest => dest.Quantity, src => src.Quantity)
                .Map(dest => dest.Price, src => src.Price);
        }
    }
}