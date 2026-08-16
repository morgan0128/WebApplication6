using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using WebApplication6.Data;
using WebApplication6.Models;

namespace WebApplication6.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ImageController(ApplicationDbContext dbContext, IWebHostEnvironment environment) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllImageIds()
    {
        var images = await dbContext.Images
            .AsNoTracking()
            .Select(i => i.Id)
            .ToListAsync();
        
        return Ok(images);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetImage(int id)
    {
        var image = await dbContext.Images
            .FindAsync(id);
        
        if (image is null) return NotFound();

        var pr = environment.WebRootFileProvider;
        
        var path = pr.GetFileInfo(image.StoragePath).PhysicalPath;
        
        if (!System.IO.File.Exists(path)) return NotFound();
        
        var bz = await System.IO.File.ReadAllBytesAsync(path);
        return File(bz, image.ContentType);
    }
    
    [HttpPost]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var path = Path.Combine(environment.ContentRootPath, fileName);
        
        await using var stream = System.IO.File.Create(path);
        await file.CopyToAsync(stream);

        var image = new Image
        {
            FileName = file.FileName,
            ContentType = file.ContentType,
            FileSize = file.Length,
            StoragePath = fileName, // TODO rename to StorageFileName 
            AltText = "alt text"
        };

        dbContext.Images.Add(image);
        await dbContext.SaveChangesAsync();
        
        return Ok(image);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteImage(int id)
    {
        var image = await dbContext.Images.FindAsync(id);
        if (image is null)
        {
            return NotFound();
        }
        
        dbContext.Images.Remove(image);
        await dbContext.SaveChangesAsync();
        
        return NoContent();
    }
    
    
    private static ImageItemDto ImageToDto(Image image)
    {
        return new ImageItemDto(image.Id, image.FileName, image.ContentType, image.FileSize, image.StoragePath, image.AltText);
    }
    
    public sealed record ImageItemDto(int Id, string FileName, string ContentType, long FileSize, string StoragePath, string? AltText = null);
    
}