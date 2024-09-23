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
            .ForMember(dest => dest.Menu, opt => opt.MapFrom(src => src.Menu));
        CreateMap<FoodItem, FoodItemDto>();
    }
}
