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

        // Seed roles
        await SeedRoles(context);

        // Read and parse user data
        var userData = await File.ReadAllTextAsync("Data/appuser.json");
        var users = JsonSerializer.Deserialize<List<AppUser>>(userData, JsonOptions);

        // Read and parse restaurant data
        var restaurantData = await File.ReadAllTextAsync("Data/restaurant.json");
        var restaurants = JsonSerializer.Deserialize<List<Restaurant>>(restaurantData, JsonOptions);

        if (users == null || restaurants == null) return;

        // Get the roles
        var restaurantOwnerRole = await context.Roles.SingleOrDefaultAsync(r => r.Name == "RestaurantOwner");
        var userRole = await context.Roles.SingleOrDefaultAsync(r => r.Name == "User");
        if (restaurantOwnerRole == null || userRole == null) return;

        // Assign unique Ids to users and set their roles
        for (int i = 0; i < users.Count; i++)
        {
            var user = users[i];
            user.Id = Guid.NewGuid().ToString();

            // Assign role and restaurant
            if (i < restaurants.Count)
            {
                user.AppRole = restaurantOwnerRole;

                // Assign restaurant to user
                var restaurant = restaurants[i];
                restaurant.Id = user.Id; // Use user Id as restaurant Id
                user.OwnedRestaurant = restaurant;
            }
            else user.AppRole = userRole;
        }

        // Add users to context
        context.Users.AddRange(users);

        // Add restaurants to context
        context.Restaurants.AddRange(restaurants);

        // Save changes to context
        await context.SaveChangesAsync();
    }

    private static async Task SeedRoles(DataContext context)
    {
        var roles = new List<AppRole>
        {
            new() { Name = "User" },
            new() { Name = "RestaurantOwner" },
            new() { Name = "Admin" },
            new() { Name = "Banned" }
        };

        foreach (var role in roles)
        {
            if (!await context.Roles.AnyAsync(r => r.Name == role.Name))
            {
                context.Roles.Add(role);
            }
        }

        await context.SaveChangesAsync();
    }
}