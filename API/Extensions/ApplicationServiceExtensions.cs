using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
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

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPhotoService, PhotoService>();
        services.AddScoped<IRestaurantRepository, RestaurantRepository>();
        services.AddScoped<ILikeRepository, LikesRepository>();
        services.AddScoped<IRatingRepository, RatingRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IShippingAddressRepository, ShippingAddressRepository>();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));

        // Add Firebase services
        services.AddSingleton<FirebaseInitializer>();
        services.AddSingleton<FirebaseAuthenticationService>();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        return services;
    }
}