using System.Security.Claims;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Geometries;

namespace API.Controllers;
/// <summary>
/// Controller for managing user accounts and device tokens
/// </summary>
[Authorize]
public class AccountController(IUserRepository userRepository,
    IFirebaseService firebaseService,
    IRestaurantRepository restaurantRepository) : BaseApiController
{
    [HttpPost("user-register")]
    public async Task<ActionResult> RegisterUser([FromBody] RegisterAccountDto userDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        if (await userRepository.UserExists(userId)) return BadRequest("How did you even get here ?");

        if (string.IsNullOrEmpty(userDto.PhoneNumber)) return BadRequest("Invalid phone number");

        var user = new AppUser
        {
            Id = userId,
            PhoneNumber = userDto.PhoneNumber,
        };

        userRepository.AddUser(user);

        var result = await userRepository.SaveAllAsync();

        if (!result) return BadRequest("Failed to register user");

        await firebaseService.AssignRoleAsync(userId, "User");

        return Ok("User has successfully registered");
    }

    [HttpPost("restaurantOwner-register")]
    public async Task<ActionResult> RegisterRestaurantOwner([FromBody] RegisterRestaurantAccountDto userDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        if (await userRepository.UserExists(userId)) return BadRequest("How did you even get here ?");

        var user = new AppUser
        {
            Id = userId,
            PhoneNumber = userDto.OwnerPhoneNumber,
        };

        var LocationPoint = new Point(userDto.Longitude, userDto.Latitude) { SRID = 4326 };

        var restaurant = new Restaurant
        {
            Id = userId,
            Name = userDto.Name,
            PhoneNumber = userDto.PhoneNumber,
            Address = userDto.Address,
            Description = userDto.Description,
            Location = LocationPoint
        };

        userRepository.AddUser(user);

        restaurantRepository.AddRestaurant(restaurant);

        var result = await userRepository.SaveAllAsync();

        if (!result) return BadRequest("Failed to register restaurant owner");

        await firebaseService.AssignRoleAsync(userId, "RestaurantOwner");

        return Ok("Restaurant Owner has successfully registered");
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteUser()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var user = await userRepository.GetUserByIdAsync(userId);

        if (user == null) return NotFound();

        userRepository.RemoveUser(user);

        var result = await userRepository.SaveAllAsync();

        if (!result) return BadRequest("Failed to delete user");

        return Ok("User has been deleted");
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(UserDto userDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var user = await userRepository.GetUserByIdAsync(userId);

        if (user == null) return NotFound();

        user.PhoneNumber = userDto.PhoneNumber;

        var result = await userRepository.SaveAllAsync();

        if (!result) return BadRequest("Failed to update user");

        return Ok("User has been updated");
    }

    [HttpPost("device-token")]
    public async Task<ActionResult> RegisterDeviceToken(DeviceTokenDto deviceTokenDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        if (await userRepository.TokenExists(deviceTokenDto.Token, userId)) return Ok("Device token already exists");

        if (string.IsNullOrEmpty(deviceTokenDto.Token)) return BadRequest("Invalid device token");

        var deviceToken = new DeviceToken
        {
            Token = deviceTokenDto.Token,
            AppUserId = userId
        };

        var user = await userRepository.GetMemberAsync(userId);

        if (user == null) return NotFound();

        user.DeviceTokens.Add(deviceToken);

        var result = await userRepository.SaveAllAsync();

        if (!result) return BadRequest("Failed to add device token");

        return Ok();
    }

    [HttpDelete("device-token/{token}")]
    public async Task<ActionResult<UserDto>> DeleteDeviceToken(string token)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var user = await userRepository.GetMemberAsync(userId);

        if (user == null) return NotFound();

        var deviceToken = user.DeviceTokens.FirstOrDefault(x => x.Token == token);

        if (deviceToken == null) return NotFound();

        user.DeviceTokens.Remove(deviceToken);

        var result = await userRepository.SaveAllAsync();

        if (!result) return BadRequest("Failed to delete device token");

        return Ok(user);
    }

#if DEBUG
    [HttpPost("admin-register")]
    public async Task<ActionResult> RegisterAdmin(UserDto userDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        if (await userRepository.UserExists(userId)) return BadRequest("How did you even get here ?");

        var user = new AppUser
        {
            Id = userId,
            PhoneNumber = userDto.PhoneNumber,
        };

        userRepository.AddUser(user);

        var result = await userRepository.SaveAllAsync();

        if (!result) return BadRequest("Failed to register admin");

        await firebaseService.AssignRoleAsync(userId, "Admin");

        return Ok("Admin has successfully registered");
    }

    [HttpPost("moderator-register")]
    public async Task<ActionResult> RegisterModerator(UserDto userDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        if (await userRepository.UserExists(userId)) return BadRequest("How did you even get here ?");

        var user = new AppUser
        {
            Id = userId,
            PhoneNumber = userDto.PhoneNumber,
        };

        userRepository.AddUser(user);

        var result = await userRepository.SaveAllAsync();

        if (!result) return BadRequest("Failed to register moderator");

        await firebaseService.AssignRoleAsync(userId, "Moderator");

        return Ok("Moderator has successfully registered");
    }

    [HttpPost("test-notification")]
    public async Task<ActionResult> SendTestNotification()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var user = await userRepository.GetMemberAsync(userId);

        if (user == null) return NotFound();

        var userDeviceToken = await userRepository.GetDeviceTokens(userId!);
        if (userDeviceToken.Count > 0)
        {
            var notification = new MulticastMessage
            {
                Tokens = userDeviceToken,
                Data = new Dictionary<string, string>
                {
                    { "userId", user.Id },
                    { "phoneNumber", user.PhoneNumber },
                },
                Notification = new Notification
                {
                    Title = "Thông tin của bạn",
                    Body = " Coi thử body như nào"
                }
            };

            var response = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(notification);
        }

        return Ok("Test notification has been sent");
    }

    [HttpPost("test-notification/{userId}")]
    public async Task<ActionResult> SendTestNotification(string userId)
    {
        var user = await userRepository.GetMemberAsync(userId);

        if (user == null) return NotFound();

        var userDeviceToken = await userRepository.GetDeviceTokens(userId!);
        if (userDeviceToken.Count > 0)
        {
            var notification = new MulticastMessage
            {
                Tokens = userDeviceToken,
                Data = new Dictionary<string, string>
                {
                    { "userId", user.Id },
                    { "phoneNumber", user.PhoneNumber },
                },
                Notification = new Notification
                {
                    Title = "Thông tin của bạn",
                    Body = " Coi thử body như nào"
                }
            };

            var response = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(notification);
        }

        return Ok("Test notification has been sent");
    }
#endif   
}
