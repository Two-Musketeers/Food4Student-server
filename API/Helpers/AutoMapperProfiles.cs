using API.DTOs;
using API.Entities;
using AutoMapper;

namespace API.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Restaurant, LikeRestaurantDto>();
        CreateMap<Restaurant, RestaurantDto>()
            .ForMember(dest => dest.Menu, opt => opt.MapFrom(src => src.Menu))
            .ForMember(dest => dest.TotalRatings, opt => opt.MapFrom(src => src.Ratings.Count))
            .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => src.Ratings.Count != 0 ? src.Ratings.Average(r => r.Stars) : 0));
        CreateMap<Restaurant, GeneralRestaurantDto>();

        //Automapper for AppUser
        CreateMap<AppUser, UserDto>();

        //Automapper for PhotoDto (if applicable)
        CreateMap<Photo, PhotoDto>();

        //Automapper for FoodItem
        CreateMap<FoodItem, FoodItemDto>();
        CreateMap<FoodItemDto, FoodItem>();
        CreateMap<FoodItemRegisterDto, FoodItem>();

        //Automapper for Order
        CreateMap<Order, OrderDto>();
        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(dest => dest.FoodItemId, opt => opt.MapFrom(src => src.OriginalFoodItemId))
            .ForMember(dest => dest.FoodItemPhoto, opt => opt.MapFrom(src => src.FoodItemPhoto));

        CreateMap<OrderItemDto, OrderItem>();

        CreateMap<Rating, RatingDto>();
        CreateMap<RatingDto, Rating>();

        //Automapper for ShippingAddress
        CreateMap<ShippingAddressDto, ShippingAddress>();
        CreateMap<ShippingAddress, ShippingAddressDto>();
    }
}
