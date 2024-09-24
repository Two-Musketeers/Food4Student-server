using System;
using System.Text.Json;
using API.Entities;
using Microsoft.EntityFrameworkCore;

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

        // Read and parse restaurant data
        var restaurantData = await File.ReadAllTextAsync("Data/restaurant.json");
        var restaurants = JsonSerializer.Deserialize<List<Restaurant>>(restaurantData, JsonOptions);

        if (users == null || restaurants == null) return;

        // Add users to context
        context.Users.AddRange(users);

        // Assign restaurants to users
        var random = new Random();
        foreach (var restaurant in restaurants)
        {
            // Randomly assign a restaurant to a user
            var owner = users[random.Next(users.Count)];
            restaurant.OwnerId = owner.Id;
            restaurant.Owner = owner;
        }

        // Add restaurants to context
        context.Restaurants.AddRange(restaurants);

        // Save changes to context
        await context.SaveChangesAsync();
    }
}