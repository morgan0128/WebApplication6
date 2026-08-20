using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication6.Backend.Data;
using WebApplication6.Backend.Models;

namespace WebApplication6.Backend.Repositories;

public class PhotoRepository(ApplicationDbContext context) : IPhotoRepository
{
    public async Task<ActionResult<IEnumerable<Photo>>> GetAllPhotosAsync()
    {
        var photos = await context.Photos
            .ToListAsync();

        return photos;
    }

    public async Task<ActionResult<IEnumerable<int>>> GetAllPhotosIdsAsync()
    {
        var photoIds = await context.Photos
            .Select(p => p.Id)
            .ToListAsync();

        return photoIds;
    }

    public async Task<ActionResult<Photo?>> GetPhotoByIdAsync(int id)
    {
        var photo = await context.Photos
            .FindAsync(id);

        return photo;
    }

    public async Task<int> SavePhotoAsync(Photo photo)
    {
        await context.AddAsync(photo);

        return await context.SaveChangesAsync();
    }

    public async Task<IActionResult> DeletePhotoByIdAsync(int id)
    {
        var photo = await context.Photos.FindAsync(id);
        if (photo == null)
        {
            return new NotFoundResult();
        }

        context.Photos.Remove(photo);
        await context.SaveChangesAsync();
        return new OkResult();
    }
}