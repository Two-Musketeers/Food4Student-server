using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
public class UsersController(IUserRepository userRepository, IMapper mapper, 
    IPhotoService photoService) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
        var users = await userRepository.GetMembersAsync();

        return Ok(users);
    }

    [HttpGet("{username}")]  // /api/users/2
    public async Task<ActionResult<AppUser>> GetUser(string username)
    {
        var user = await userRepository.GetMemberAsync(username);

        if (user == null) return NotFound();

        return user;
    }


    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
    {
        var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());

        if (user == null) return BadRequest("Cannot update user");

        var result = await photoService.AddPhotoAsync(file);
        
        if (result.Error != null) return BadRequest(result.Error.Message);

        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };

        user.Avatar = photo;

        if (await userRepository.SaveAllAsync()) 
            return CreatedAtAction(nameof(GetUser), 
                new {username = user.UserName}, mapper.Map<PhotoDto>(photo));

        return BadRequest("Problem adding photo");
    }

    [HttpPut("set-main-photo/{photoId:int}")]
    public async Task<ActionResult> SetMainPhoto(int photoId)
    {
        var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());

        if (user == null) return BadRequest("Could not find user");

        var photo = user.Avatar;

        if (await userRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Problem setting main photo");
    }

    // [HttpDelete("delete-photo/{photoId:int}")]
    // public async Task<ActionResult> DeletePhoto(int photoId)
    // {
    //     var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());

    //     if (user == null) return BadRequest("User not found");

    //     var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

    //     if (photo == null || photo.IsMain) return BadRequest("This photo cannot be deleted");

    //     if (photo.PublicId != null)
    //     {
    //         var result = await photoService.DeletePhotoAsync(photo.PublicId);
    //         if (result.Error != null) return BadRequest(result.Error.Message);
    //     }

    //     user.Photos.Remove(photo);

    //     if (await userRepository.SaveAllAsync()) return Ok();

    //     return BadRequest("Problem deleting photo");
    // }
}