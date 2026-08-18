using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication6.Backend.Data;
using WebApplication6.Backend.Models;

namespace WebApplication6.Backend.Repositories;

public class ImageRepository(ApplicationDbContext context) : IImageRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<ActionResult<IEnumerable<Image>>> GetAllImagesAsync()
    {
        var images = await _context.Images
            .ToListAsync();

        return images;
    }

    public async Task<ActionResult<IEnumerable<int>>> GetAllImagesIdsAsync()
    {
        var imageIds = await _context.Images
            .AsNoTracking()
            .Select(i => i.Id)
            .ToListAsync();
        
        return imageIds;
    }

    public async Task<ActionResult<Image>> GetImageByIdAsync(int id)
    {
        var image = await _context.Images
            .FindAsync(id);
        
        return image;
    }

    public async Task<int> PostImageAsync(Image image)
    {
        _context.Images.Add(image);
        var returned = await _context.SaveChangesAsync();
        return returned;
    }

    public async Task<IActionResult> DeleteImageByIdAsync(int id)
    {
        var image = await _context.Images.FindAsync(id);
        if (image == null)
        {
            return new NotFoundResult();
        }

        _context.Images.Remove(image);
        await _context.SaveChangesAsync();
        return new OkResult();
    }

}