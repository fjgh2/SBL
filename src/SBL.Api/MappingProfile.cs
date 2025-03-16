using AutoMapper;
using SBL.Api.Dtos;
using SBL.Domain.Entities;

namespace SBL.Api;

public class MappingProfile : Profile
{
    
    public MappingProfile()
    {
        // // Product DTOs
        // CreateMap<Product, CreateProductDto>().ReverseMap();
        // CreateMap<Product, UpdateProductDto>().ReverseMap();
        // // User DTOs
        CreateMap<User, RegisterDto>().ReverseMap();
        CreateMap<User, LoginDto>().ReverseMap();
        // // Coupon DTOs
        // CreateMap<Coupon, CreateCouponDto>().ReverseMap();
        // CreateMap<Coupon, UpdateCouponDto>().ReverseMap();
        // // Order DTOs
        // CreateMap<Order, CreateOrderDto>().ReverseMap();
        // // Basket DTOs
        // CreateMap<BasketItem, CreateBasketItemDto>().ReverseMap();
    }
}