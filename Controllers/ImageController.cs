using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using WebApplication6.Data;
using WebApplication6.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using Image = WebApplication6.Models.Image;

// using Image = SixLabors.ImageSharp.Image;

namespace WebApplication6.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ImageController(ApplicationDbContext dbContext, IWebHostEnvironment environment) : ControllerBase
{
    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var images = await dbContext.Images
            .Select(image => ImageToDto(image))
            .ToListAsync();

        return Ok(images);
    }
    
    
    [HttpGet("all-ids")]
    public async Task<IActionResult> GetAllIds()
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

        var dto = ImageToDto(image);

        return Ok(dto);

        // if (image is null) return NotFound();
        //
        // var pr = environment.WebRootFileProvider;
        //
        // var path = pr.GetFileInfo(image.StorageFileName).PhysicalPath;
        //
        // if (!System.IO.File.Exists(path)) return NotFound();
        //
        // var bz = await System.IO.File.ReadAllBytesAsync(path);
        // return File(bz, image.ContentType);
    }
    
    [HttpPost]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var path = Path.Combine(environment.ContentRootPath, fileName);

        await using (var stream = System.IO.File.Create(path))
        {
            await file.CopyToAsync(stream);
            Task.WaitAll()
        }
        
        ImageInfo imageInfo;
        try
        {
            imageInfo = await SixLabors.ImageSharp.Image.IdentifyAsync(path);
        }
        catch (Exception ex || AggregateException aex) // TODO
        {
            if (System.IO.File.Exists(path))
            {
                var untracked = new UntrackedFile()
                {
                    OccurredInClass = "Controllers/ImageController",
                    OccurredElaboration = "Error in UploadImage. File was saved but exception thrown before added to db.",
                    LikelyFileLocation = "ClientApp/public"
                };
                dbContext.UntrackedFiles.Add(untracked);
                await dbContext.SaveChangesAsync();
            }
            throw;
        }

        var width = imageInfo.Width;
        var height = imageInfo.Height;
        
        var image = new Image
        {
            FileName = file.FileName,
            ContentType = file.ContentType,
            FileSize = file.Length,
            StorageFileName = fileName,
            AltText = "alt text",
            Width = width,
            Height = height
        };

        dbContext.Images.Add(image);
        await dbContext.SaveChangesAsync();
        
        return Ok(image);
    }

    // private async Task<IResult> UntrackedFileThrow(string? elaboration)
    // {
    //     var untracked = new UntrackedFile()
    //     {
    //         OccurredInClass = "Controllers/ImageController",
    //         OccurredElaboration = "Error in UploadImage. File was saved but exception thrown before added to db.",
    //         LikelyFileLocation = "ClientApp/public"
    //     };
    //     dbContext.UntrackedFiles.Add(untracked);
    //     await dbContext.SaveChangesAsync();
    //     return Ok();
    // }

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
        return new ImageItemDto(image.Id, image.FileName, image.ContentType, image.FileSize, image.StorageFileName, image.Width, image.Height, image.AltText);
    }
    
    public sealed record ImageItemDto(int Id, string FileName, string ContentType, long FileSize, string StorageFileName, int Width, int Height, string? AltText = null);
    
}