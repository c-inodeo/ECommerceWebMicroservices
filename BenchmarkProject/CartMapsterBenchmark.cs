using BenchmarkDotNet.Attributes;
using ECommerceWebMicroservices.Models;
using ECommerceWebMicroservices.Models.DTO;
using Mapster;
using AutoMapper;
using Microsoft.Diagnostics.Tracing.Parsers.MicrosoftWindowsWPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkProject
{
    
    public class CartMapsterBenchmark
    {
        private List<CartItem>? cartItems;
        private IMapper _mapper;

        [GlobalSetup]
        public void Setup() 
        {
            cartItems = new List<CartItem>();
            for (int i = 0; i < 10; i++) 
            {
                cartItems.Add(new CartItem
                {
                    Id = i,
                    Price = i + 100,
                    Product = new Product { Id = i, ProductName = $"Product - {i}" }
                });
            }
            var config = new MapperConfiguration(config =>
            {
                config.CreateMap<CartItem, CartItemDto>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price * src.Quantity))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId));
            });
            _mapper = config.CreateMapper();
        }
        [Benchmark]
        public List<CartItemDto> MapsterMapping() 
        {
            return cartItems.Adapt<List<CartItemDto>>();
        }
        [Benchmark]
        public List<CartItemDto> ManualMapping()
        {
            return cartItems.Select(c => new CartItemDto
            {
                ProductId = c.ProductId,
                Quantity = c.Quantity,
                Price = c.Price * c.Quantity
            }).ToList();
        }
        [Benchmark]
        public List<CartItemDto> AutoMapperMapping()
        {
            return _mapper.Map<List<CartItemDto>>(cartItems);
        }
    }
}
