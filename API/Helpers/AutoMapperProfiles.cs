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

        CreateMap<Restaurant, RestaurantDetailDto>()
            .ForMember(dest => dest.LogoUrl, opt => opt.MapFrom(src => src.Logo != null ? src.Logo.Url : null))
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
        CreateMap<ShippingAddress, OrderShippingAddressDto>();

        //Automapper for Order
        CreateMap<CreateOrderItemDto, OrderItem>();

        CreateMap<OrderItemVariationDto, OrderItemVariation>();

        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(dest => dest.SelectedVariations, opt => opt.MapFrom(src => src.OrderItemVariations));

        CreateMap<OrderItemVariation, OrderItemVariationDto>();

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

        //Automapper for FoodItemVariation
        CreateMap<FoodItemVariation, FoodItemVariationDto>()
            .ForMember(dest => dest.VariationId, opt => opt.MapFrom(src => src.Variation.Id))
            .ForMember(dest => dest.VariationName, opt => opt.MapFrom(src => src.Variation.Name))
            .ForMember(dest => dest.VariationOptionId, opt => opt.MapFrom(src => src.VariationOption.Id))
            .ForMember(dest => dest.VariationOptionName, opt => opt.MapFrom(src => src.VariationOption.Name))
            .ForMember(dest => dest.PriceAdjustment, opt => opt.MapFrom(src => src.VariationOption.PriceAdjustment));
    }
}
