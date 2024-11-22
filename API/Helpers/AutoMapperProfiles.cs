using API.DTOs;
using API.Entities;
using AutoMapper;

namespace API.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Restaurant, RestaurantDto>()
            .ForMember(dest => dest.LogoUrl, opt => opt.MapFrom(src => src.Logo != null ? src.Logo.Url : null))
            .ForMember(dest => dest.BannerUrl, opt => opt.MapFrom(src => src.Banner != null ? src.Banner.Url : null))
            .ForMember(dest => dest.TotalRatings, opt => opt.MapFrom(src => src.Ratings.Count))
            .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => src.Ratings.Count != 0 ? src.Ratings.Average(r => r.Stars) : 0));

        //Automapper for AppUser
        CreateMap<AppUser, UserDto>();

        //Automapper for PhotoDto (if applicable)
        CreateMap<Photo, PhotoDto>();

        //Automapper for FoodItem
        CreateMap<FoodItem, FoodItemDto>();
        CreateMap<FoodItemDto, FoodItem>();
        CreateMap<FoodItemRegisterDto, FoodItem>();

        CreateMap<ShippingAddress, CreateShippingAddressDto>();

        //Automapper for Order
        CreateMap<Order, OrderDto>();
        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(dest => dest.FoodItemId, opt => opt.MapFrom(src => src.OriginalFoodItemId));

        CreateMap<OrderItemDto, OrderItem>();

        CreateMap<Rating, RatingDto>();
        CreateMap<RatingDto, Rating>();

        //Automapper for ShippingAddress
        CreateMap<ShippingAddressDto, ShippingAddress>();
        CreateMap<ShippingAddress, ShippingAddressDto>();
    }
}
