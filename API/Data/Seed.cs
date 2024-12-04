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

        // Identify the restaurant owner
        string ownerUserId = "jS15TdaITTfjZB7eycCXOfzp7WW2";
        var owner = users.FirstOrDefault(u => u.Id == ownerUserId);

        if (owner == null)
        {
            Console.WriteLine("Restaurant owner not found in the user data.");
            return;
        }

        if (restaurants.Count == 0)
        {
            Console.WriteLine("No restaurants found in the restaurant data.");
            return;
        }

        // Assign the first (and only) restaurant to the owner
        var restaurant = restaurants[0];
        restaurant.Id = owner.Id; // Use user Id as restaurant Id to maintain relationship
        owner.OwnedRestaurant = restaurant;

        // Optionally, set other properties or relationships if needed
        // For example, assign the restaurant to the user's navigation property

        // Add users to context
        context.Users.AddRange(users);

        // Add restaurants to context
        context.Restaurants.AddRange(restaurants);

        // Save changes to context
        await context.SaveChangesAsync();

        Console.WriteLine("Seeding completed successfully.");
    }
}