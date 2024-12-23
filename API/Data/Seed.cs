using System.Text.Json;
using API.DTOs;
using API.Entities;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace API.Data;

public class Seed
{
    // Cache and reuse JsonSerializerOptions instance
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public static async Task SeedUsersAndRestaurants(DataContext context)
    {
        if (await context.Users.AnyAsync() || await context.Restaurants.AnyAsync()) return;

        // Read and parse user data
        var userData = await File.ReadAllTextAsync("Data/appuser.json");
        var users = JsonSerializer.Deserialize<List<AppUser>>(userData, JsonOptions);

        var restaurantData = await File.ReadAllTextAsync("Data/restaurant.json");
        var restaurantDtos = JsonSerializer.Deserialize<List<RestaurantSeedDto>>(restaurantData, JsonOptions);
        if (users == null || restaurantDtos == null) return;

        var restaurants = restaurantDtos.Select(dto => new Restaurant
        {
            Id = dto.Id,
            Name = dto.Name,
            Address = dto.Address,
            PhoneNumber = dto.PhoneNumber,
            Description = dto.Description,
            Location = new Point(dto.Longitude, dto.Latitude) { SRID = 4326 },
            Logo = dto.Logo,
            Banner = dto.Banner,
            FoodCategories = dto.FoodCategories,
            IsApproved = dto.IsApproved
        }).ToList();
        restaurants[0].Id = "jS15TdaITTfjZB7eycCXOfzp7WW2";
        users[2].OwnedRestaurant = restaurants[0];

        // Add users to context
        context.Users.AddRange(users);
        context.Restaurants.AddRange(restaurants);

        // Save changes to context
        await context.SaveChangesAsync();

        Console.WriteLine("Seeding completed successfully.");
    }
}