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
        CreateMap<FoodItem, FoodItemDto>();
        CreateMap<Rating, RatingDto>();
        CreateMap<RatingDto, Rating>();
    }
}
