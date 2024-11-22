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

        // Assign unique Ids to users and set their roles
        for (int i = 0; i < 20; i++)
        {
            var user = users[i];

            if (i < restaurants.Count - 1)
            {
                // Assign restaurant to user
                var restaurant = restaurants[i];
                restaurant.Id = user.Id; // Use user Id as restaurant Id
                user.OwnedRestaurant = restaurant;
            }
        }

        restaurants[9].Id = users[22].Id;
        users[22].OwnedRestaurant = restaurants[9];
        
        // Add users to context
        context.Users.AddRange(users);

        // Add restaurants to context
        context.Restaurants.AddRange(restaurants);

        // Save changes to context
        await context.SaveChangesAsync();
    }
}