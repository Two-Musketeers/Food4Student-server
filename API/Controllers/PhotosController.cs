using System.Security.Claims;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[Route("api/restaurants/[controller]")]
[ApiController]
public class PhotosController(IUserRepository userRepository,
    IPhotoService photoService,
    IRestaurantRepository restaurantRepository,
    IFoodItemRepository foodItemRepository) : ControllerBase
{
    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpPost("logo")]
    public async Task<ActionResult<Photo>> AddPhoto(IFormFile file)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var user = await userRepository.GetUserByIdAsync(userId);

        if (user.OwnedRestaurant == null) return BadRequest("User does not own a restaurant");

        // Delete the old photo
        if (user.OwnedRestaurant.Logo != null && user.OwnedRestaurant.Logo.PublicId != null)
        {
            var deleteResult = await photoService.DeletePhotoAsync(user.OwnedRestaurant.Logo.PublicId);

            if (deleteResult.Error != null) return BadRequest(deleteResult.Error.Message);
        }

        var result = await photoService.AddPhotoAsync(file);

        if (result.Error != null) return BadRequest(result.Error.Message);

        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };

        user.OwnedRestaurant.Logo = photo;

        if (await restaurantRepository.SaveAllAsync())
            return Ok(photo);

        return BadRequest("Problem adding photo");
    }

    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpPost("banner")]
    public async Task<ActionResult<Photo>> AddBannerPhoto(IFormFile file)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var user = await userRepository.GetUserByIdAsync(userId);

        if (user.OwnedRestaurant == null) return BadRequest("User does not own a restaurant");

        // Delete the old photo
        if (user.OwnedRestaurant.Banner != null && user.OwnedRestaurant.Banner.PublicId != null)
        {
            var deleteResult = await photoService.DeletePhotoAsync(user.OwnedRestaurant.Banner.PublicId);

            if (deleteResult.Error != null) return BadRequest(deleteResult.Error.Message);
        }

        var result = await photoService.AddPhotoAsync(file);

        if (result.Error != null) return BadRequest(result.Error.Message);

        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };

        user.OwnedRestaurant.Banner = photo;

        if (await restaurantRepository.SaveAllAsync())
            return Ok(photo);

        return BadRequest("Problem adding photo");
    }

    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpPost("food-items/{foodItemId}")]
    public async Task<ActionResult<Photo>> AddFoodItemPhoto(IFormFile file, string foodItemId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var user = await userRepository.GetUserByIdAsync(userId);

        if (user.OwnedRestaurant == null) return BadRequest("User does not own a restaurant");

        var foodItem = await foodItemRepository.GetFoodItemByIdAsync(foodItemId);
        if (foodItem == null)
            return BadRequest("Food item not found");

        // Delete the old photo
        if (foodItem.FoodItemPhoto != null && foodItem.FoodItemPhoto.PublicId != null)
        {
            var deleteResult = await photoService.DeletePhotoAsync(foodItem.FoodItemPhoto.PublicId);

            if (deleteResult.Error != null) return BadRequest(deleteResult.Error.Message);
        }

        var result = await photoService.AddPhotoAsync(file);

        if (result.Error != null) return BadRequest(result.Error.Message);

        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };

        foodItem.FoodItemPhoto = photo;

        if (await foodItemRepository.SaveAllAsync())
            return Ok(photo);

        return BadRequest("Problem adding photo");
    }

    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpDelete("logo")]
    public async Task<ActionResult> DeleteLogoPhoto()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var restaurantOwned = await restaurantRepository.GetRestaurantWithDetailsAsync(userId);

        if (restaurantOwned == null) return BadRequest("User does not own a restaurant");

        var photo = restaurantOwned.Logo;
        if (photo == null) return BadRequest("Logo photo not found");

        if (photo.PublicId == null) return BadRequest("There's no photo mate");

        var result = await photoService.DeletePhotoAsync(photo.PublicId);

        if (result.Error != null) return BadRequest(result.Error.Message);

        restaurantOwned.Logo = null;

        if (await restaurantRepository.SaveAllAsync()) return Ok();

        return BadRequest("Problem deleting photo");
    }

    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpDelete("banner")]
    public async Task<ActionResult> DeleteBannerPhoto()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var restaurantOwned = await restaurantRepository.GetRestaurantWithDetailsAsync(userId);

        if (restaurantOwned == null) return BadRequest("User does not own a restaurant");

        var photo = restaurantOwned.Banner;
        if (photo == null) return BadRequest("Banner photo not found");

        if (photo.PublicId == null) return BadRequest("There's no photo mate");

        var result = await photoService.DeletePhotoAsync(photo.PublicId);

        if (result.Error != null) return BadRequest(result.Error.Message);

        restaurantOwned.Banner = null;

        if (await restaurantRepository.SaveAllAsync()) return Ok();

        return BadRequest("Problem deleting photo");
    }

    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpDelete("food-items/{foodItemId}")]
    public async Task<ActionResult> DeleteFoodItemPhoto(string foodItemId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var user = await userRepository.GetUserByIdAsync(userId);

        if (user.OwnedRestaurant == null) return BadRequest("User does not own a restaurant");

        var foodItem = await foodItemRepository.GetFoodItemByIdAsync(foodItemId);
        if (foodItem == null)
            return BadRequest("Food item is not found");

        var photo = foodItem.FoodItemPhoto;
        if (photo == null) return BadRequest("Food item photo not found");

        if (photo.PublicId == null) return BadRequest("There's no photo mate");

        var result = await photoService.DeletePhotoAsync(photo.PublicId);

        if (result.Error != null) return BadRequest(result.Error.Message);

        foodItem.FoodItemPhoto = null;

        if (await foodItemRepository.SaveAllAsync()) return Ok();

        return BadRequest("Problem deleting photo");
    }
}
