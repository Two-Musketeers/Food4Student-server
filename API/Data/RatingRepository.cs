using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class RatingRepository(DataContext context, IMapper mapper) : IRatingRepository
{
    public async Task<double> GetAverageRatingAsync(string restaurantId)
    {
        var averageRating = await context.Ratings
            .Where(r => r.RestaurantId == restaurantId)
            .AverageAsync(r => r.Stars);

        return averageRating;
    }

    public async Task<bool> AddOrUpdateRatingAsync(RatingDto ratingDto)
    {
        var existingRating = await context.Ratings
            .FirstOrDefaultAsync(r => r.UserId == ratingDto.UserId && r.RestaurantId == ratingDto.RestaurantId);

        if (existingRating != null)
        {
            existingRating.Stars = ratingDto.Stars;
            context.Ratings.Update(existingRating);
        }
        else
        {
            var rating = mapper.Map<Rating>(ratingDto);
            context.Ratings.Add(rating);
        }

        return await context.SaveChangesAsync() > 0;
    }

    public async Task<ActionResult<RatingDto>> AddRating(RatingDto ratingDto)
    {
        var rating = mapper.Map<Rating>(ratingDto);
        context.Ratings.Add(rating);

        if (await context.SaveChangesAsync() > 0)
            return new CreatedAtActionResult("GetRating", "Rating", new { id = rating.Id }, mapper.Map<RatingDto>(rating));

        return new BadRequestResult();
    }

    public async Task<ActionResult> UpdateRating(int id, RatingDto ratingDto)
    {
        var rating = await context.Ratings.FindAsync(id);
        if (rating == null) return new NotFoundResult();

        mapper.Map(ratingDto, rating);

        if (await context.SaveChangesAsync() > 0) return new NoContentResult();

        return new BadRequestResult();
    }

    public async Task<ActionResult> DeleteRating(int id)
    {
        var rating = await context.Ratings.FindAsync(id);
        if (rating == null) return new NotFoundResult();

        context.Ratings.Remove(rating);

        if (await context.SaveChangesAsync() > 0) return new NoContentResult();

        return new BadRequestResult();
    }
}
