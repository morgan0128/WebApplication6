using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication6.Backend.Data;
using WebApplication6.Backend.Models;
using Image = WebApplication6.Backend.Models.Image;

using ImageShrp = SixLabors.ImageSharp.Image;

namespace WebApplication6.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ImageController(ApplicationDbContext dbContext, IWebHostEnvironment? environment = null) : ControllerBase
{
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<Image>>> GetAllImages()
    {
        var images = await dbContext.Images
            // .Select(image => ImageToDto(image))
            .ToListAsync();

        return images;
    }
    
    
    [HttpGet("all-ids")]
    public async Task<ActionResult<IEnumerable<int>>> GetAllIds()
    {
        var imageIds = await dbContext.Images
            .AsNoTracking()
            .Select(i => i.Id)
            .ToListAsync();
        
        return imageIds;
    }


    
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Image>> GetImageById(int id)
    {
        var image = await dbContext.Images
            .FindAsync(id);

        var dto = ImageToDto(image);

        // return Ok(dto);
        return image;
        
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
    public async Task<ActionResult<Image>> UploadImage(IFormFile file)
    {
        if (environment is null)
        {
            return BadRequest("Environment is null.");
        }
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var path = Path.Combine(environment.ContentRootPath, fileName);
        
        await using (var stream = System.IO.File.Create(path))
        {
            await file.CopyToAsync(stream);
        }

        try
        {
            var imageInfo = await ImageShrp.IdentifyAsync(path);
            if (imageInfo == null)
            {
                throw new Exception("Null returned in IdentifyAsync. Likely threw exception that failed to propagate. " +
                                    "Throwing exception.");
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

            // return Ok(image);
            return image;
        }
        catch (Exception ex)
        {
            if (!System.IO.File.Exists(path)) throw;
            var untracked = new UntrackedFile()
            {
                OccurredInClass = "Controllers/ImageController",
                OccurredElaboration = "Error in UploadImage. File was saved but exception thrown before added to db.",
                FileName = $"{fileName}",
                FileLocation = "ClientApp/public"
            };
            dbContext.UntrackedFiles.Add(untracked);
            await dbContext.SaveChangesAsync();
            throw;
        }
        
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