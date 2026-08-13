using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication6.Data;
using WebApplication6.Models;

namespace WebApplication6.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ImageController(ApplicationDbContext dbContext, IWebHostEnvironment environment) : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetImages()
    {
        return Ok(await dbContext.Images.ToListAsync());
    }
    
    [HttpPost]
    public async Task<IActionResult> UploadImage(IFormFile file)
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

        return Ok(image);
    }
    
}