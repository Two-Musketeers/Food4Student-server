using API.DTOs;
using API.Entities;
using AutoMapper;

namespace API.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Restaurant, RestaurantDto>()
            .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Location.Y))
            .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Location.X))
            .ForMember(dest => dest.LogoUrl, opt => opt.MapFrom(src => src.Logo != null ? src.Logo.Url : null))
            .ForMember(dest => dest.BannerUrl, opt => opt.MapFrom(src => src.Banner != null ? src.Banner.Url : null))
            .ForMember(dest => dest.TotalRatings, opt => opt.MapFrom(src => src.Ratings.Count))
            .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => src.Ratings.Count != 0 ? src.Ratings.Average(r => r.Stars) : 0));

        CreateMap<Restaurant, RestaurantDetailDto>()
            .ForMember(dest => dest.LogoUrl, opt => opt.MapFrom(src => src.Logo != null ? src.Logo.Url : null))
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Location.Y))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Location.X))
                .ForMember(dest => dest.BannerUrl, opt => opt.MapFrom(src => src.Banner != null ? src.Banner.Url : null))
                .ForMember(dest => dest.TotalRatings, opt => opt.MapFrom(src => src.Ratings.Count))
                .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => src.Ratings.Count != 0 ? src.Ratings.Average(r => r.Stars) : 0));

        //Automapper for AppUser
        CreateMap<AppUser, UserDto>();

        //Automapper for PhotoDto (if applicable)
        CreateMap<Photo, PhotoDto>();

        //Automapper for FoodItem
        CreateMap<FoodItem, FoodItemDto>()
            .ForMember(dest => dest.FoodItemPhotoUrl, opt => opt.MapFrom(src => src.FoodItemPhoto != null ? src.FoodItemPhoto.Url : null));
        CreateMap<FoodItemDto, FoodItem>();
        CreateMap<FoodItemRegisterDto, FoodItem>();

        CreateMap<FoodCategory, FoodCategoryDto>()
            .ForMember(dest => dest.FoodItems, opt => opt.MapFrom(src => src.FoodItems));

        CreateMap<CreateShippingAddressDto, ShippingAddress>();

        CreateMap<ShippingAddress, CreateShippingAddressDto>();

        //Automapper for Order
        CreateMap<OrderCreateDto, Order>()
                .ForMember(dest => dest.OrderItems, opt => opt.Ignore());

        CreateMap<CreateOrderItemDto, OrderItem>()
                .ForMember(dest => dest.OriginalFoodItem, opt => opt.Ignore());

        CreateMap<Order, OrderDto>();

        CreateMap<OrderItem, OrderItemDto>();

        CreateMap<Rating, RatingDto>();
        CreateMap<RatingDto, Rating>();
        CreateMap<CreatingRatingDto, RatingDto>();

        //Automapper for ShippingAddress
        CreateMap<ShippingAddressDto, ShippingAddress>();
        CreateMap<ShippingAddress, ShippingAddressDto>();

        CreateMap<VariationCreateDto, Variation>();
        CreateMap<Variation, VariationDto>()
            .ForMember(dest => dest.VariationOptions, opt => opt.MapFrom(src => src.VariationOptions));
        CreateMap<VariationOptionCreateDto, VariationOption>();
        CreateMap<VariationOption, VariationOptionDto>();
        CreateMap<VariationOptionUpdateDto, VariationOption>()
            .ForMember(dest => dest.Id, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Id)));
        CreateMap<VariationUpdateDto, Variation>()
            .ForMember(dest => dest.Id, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Id)));
        CreateMap<FoodItemUpdateDto, FoodItem>()
            .ForMember(dest => dest.Variations, opt => opt.Ignore());

        CreateMap<FoodCategory, FoodCategoryDto>();
        CreateMap<FoodCategoryCreateDto, FoodCategory>();

        CreateMap<UserNotification, UserNotificationDto>()
            .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => src.Timestamp));
    }
}
