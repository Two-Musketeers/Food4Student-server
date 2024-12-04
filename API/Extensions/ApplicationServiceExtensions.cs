using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Middleware;
using API.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration config)
    {
        services.AddControllers();
        services.AddDbContext<DataContext>(opt =>
        {
            opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
        });
        services.AddCors();

        // Add repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRestaurantRepository, RestaurantRepository>();
        services.AddScoped<ILikeRepository, LikesRepository>();
        services.AddScoped<IRatingRepository, RatingRepository>();
        services.AddScoped<IShippingAddressRepository, ShippingAddressRepository>();
        services.AddScoped<IFoodItemRepository, FoodItemRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IVariationRepository, VariationRepository>();
        services.AddScoped<IVariationOptionRepository, VariationOptionRepository>();
        services.AddScoped<IFoodItemVariationRepository, FoodItemVariationRepository>();
        services.AddScoped<IFoodCategoryRepository, FoodCategoryRepository>();

        // Add Photo services
        services.AddScoped<IPhotoService, PhotoService>();
        services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        // Add Firebase services
        services.AddSingleton<FirebaseInitializer>();
        services.AddScoped<IFirebaseService, FirebaseService>();

        //Add Firebase custom tokenKey
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = "Firebase";
            options.DefaultChallengeScheme = "Firebase";
        })
        .AddScheme<AuthenticationSchemeOptions, FirebaseAuthenticationMiddleware>("Firebase", options => { });

        //Add role based authorization
        services.AddAuthorizationBuilder()
            .AddPolicy("RequireAdminRole", policy =>
                policy.RequireRole("Admin"))
            .AddPolicy("RequireModeratorRole", policy =>
                policy.RequireRole("Moderator", "Admin"))
            .AddPolicy("RequireRestaurantOwnerRole", policy =>
                policy.RequireRole("RestaurantOwner", "Admin", "Moderator"))
            .AddPolicy("RequireUserRole", policy =>
                policy.RequireRole("User", "Admin", "Moderator"));

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        return services;
    }
}