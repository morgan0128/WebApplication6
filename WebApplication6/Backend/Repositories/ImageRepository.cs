using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication6.Backend.Data;
using WebApplication6.Backend.Models;

namespace WebApplication6.Backend.Repositories;

public class ImageRepository(ApplicationDbContext context) : IImageRepository
{
    public async Task<ActionResult<IEnumerable<Image>>> GetAllImagesAsync()
    {
        var images = await context.Images
            .ToListAsync();

        return images;
    }

    public async Task<ActionResult<IEnumerable<int>>> GetAllImagesIdsAsync()
    {
        var imageIds = await context.Images
            .AsNoTracking()
            .Select(i => i.Id)
            .ToListAsync();
        
        return imageIds;
    }

    public async Task<ActionResult<Image?>> GetImageByIdAsync(int id)
    {
        var image = await context.Images
            .FindAsync(id);
        
        return image;
    }

    public async Task<ActionResult<Image>> SaveImageAsync(Image image)
    {
        context.Images.Add(image);
        await context.SaveChangesAsync();

        var savedImage = await context.Images.FindAsync(image);

        if (savedImage == null)
        {
            return new ForbidResult();
        }
        
        return savedImage;
    }

    public async Task<IActionResult> DeleteImageByIdAsync(int id)
    {
        var image = await context.Images.FindAsync(id);
        if (image == null)
        {
            return new NotFoundResult();
        }

        context.Images.Remove(image);
        await context.SaveChangesAsync();
        
        return new OkResult();
    }

}