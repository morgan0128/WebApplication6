using Microsoft.AspNetCore.Mvc;
using WebApplication6.Data;
using WebApplication6.Models;

namespace WebApplication6.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class PhotoController(ApplicationDbContext dbContext, IWebHostEnvironment environment) : ControllerBase
{

    // [HttpGet]
    // public async Task<IActionResult> GetPhotos()
    // {
    //     // await EnsureDevelopmentDatabaseCreatedAsync();
    // }

    [HttpPost]
    public async Task<IActionResult> AddPhoto(IFormFile file, string? title, string? description, int? fromYear)
    {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var path = Path.Combine("wwwroot", "uploads", fileName);

            await using var stream = System.IO.File.Create(path);
            await file.CopyToAsync(stream);


            var image = new Image
            {
                FileName = file.FileName,
                ContentType = file.ContentType,
                FileSize = file.Length,
                StoragePath = $"/uploads/{fileName}"
            };

            dbContext.Images.Add(image);

            await dbContext.SaveChangesAsync();


            var photo = new Photo
            {
                Title = title,
                CreatedAt = DateTime.UtcNow,
                Description = description,
                YearContentCreated = fromYear,
                ImageId = image.Id
            };
            
            dbContext.Photos.Add(photo);
            
            await dbContext.SaveChangesAsync();
        
        return Ok(image);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddPhoto(Image image, string? title, string? description, int? fromYear)
    {
        if (!dbContext.Images.Any(i => i.Id == image.Id))
        {
            return BadRequest("Error adding photo: Image not found in database.");
        }

        var photo = new Photo
        {
            Title = title,
            CreatedAt = DateTime.UtcNow,
            Description = description,
            YearContentCreated = fromYear,
            ImageId = image.Id
        };
            
        dbContext.Photos.Add(photo);
            
        await dbContext.SaveChangesAsync();
        
        return Ok(image);
    }
    
    
    
}