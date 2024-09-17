using System;
using System.Text.Json;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class Seed
{
    public static async Task SeedUsers(DataContext context)
    {
        if (await context.Users.AnyAsync()) return;

        var userData = await File.ReadAllTextAsync("Data/appuser.json");
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

        if (users == null) return;

        context.Users.AddRange(users);

        await context.SaveChangesAsync();
    }

    public static async Task SeedRestaurants(DataContext context)
    {
        if (await context.Restaurants.AnyAsync()) return;

        var restaurantData = await File.ReadAllTextAsync("Data/restaurant.json");
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var restaurants = JsonSerializer.Deserialize<List<Restaurant>>(restaurantData, options);

        if (restaurants == null) return;

        context.Restaurants.AddRange(restaurants);

        await context.SaveChangesAsync();
    }
}