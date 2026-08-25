using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication6.Backend.Data;
using WebApplication6.Backend.Models;

namespace WebApplication6.Backend.Repositories;

public class ImageRepository(ApplicationDbContext context) : IImageRepository
{
    public async Task<IEnumerable<Image>> GetAllImagesAsync()
    {
        var images = await context.Images
            .ToListAsync();

        return images;
    }

    public async Task<IEnumerable<int>> GetAllImagesIdsAsync()
    {
        var imageIds = await context.Images
            .AsNoTracking()
            .Select(i => i.Id)
            .ToListAsync();
        
        return imageIds;
    }

    public async Task<Image?> GetImageByIdAsync(int id)
    {
        var image = await context.Images
            .FindAsync(id);
        
        return image;
    }

    public async Task<int?> SaveImageAsync(Image image)
    {
        context.Images.Add(image);
        
        try
        {
            await context.SaveChangesAsync();
            return image.Id;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> DeleteImageByIdAsync(int id)
    {
        var image = await context.Images.FindAsync(id);
        if (image == null) return false;
        
        context.Images.Remove(image);

        try
        {
            await context.SaveChangesAsync();
            return true;
        }
        catch (AggregateException)
        {
            return false;
        }
    }

}