using API.Data;
using API.Extensions;
using API.Middleware;
using API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

// Initialize Firebase
var firebaseInitializer = app.Services.GetRequiredService<FirebaseInitializer>();
firebaseInitializer.Initialize();

// Configure the HTTP request pipeline.
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod()
    .WithOrigins("http://localhost:5000", "https://localhost:5001"));

// Use the custom Firebase authentication middleware
app.UseMiddleware<FirebaseAuthenticationMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedUsersAndRestaurants(context);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration");
}

app.Run();