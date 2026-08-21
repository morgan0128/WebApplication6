using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication6.Backend.Data;
using WebApplication6.Backend.Models;
using WebApplication6.Backend.Repositories;
using WebApplication6.Backend.Services;
using Image = WebApplication6.Backend.Models.Image;

namespace WebApplication6.Backend.Controllers;

// TODO: (probably) FULLY DEPRECATE, once app-edit-album is complete (no ImageController API)
[ApiController]
[Route("api/Image")]
public sealed class ImageController(IImageRepository imageRepository) : ControllerBase
{
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<Image>>> GetAllImages()
    {
        var images = await imageRepository.GetAllImagesAsync();
        return Ok(images);
    }
    
    
    [HttpGet("all-ids")]
    public async Task<ActionResult<IEnumerable<int>>> GetAllImagesIds()
    {
        var imageIds = await imageRepository.GetAllImagesIdsAsync();
        return Ok(imageIds);
    }
    
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Image>> GetImageById(int id)
    {
        var image = await imageRepository.GetImageByIdAsync(id);
        if (image == null)
        {
            return NotFound();
        }

        return Ok(image);
    }

    // [HttpPost]
    // public async Task<IActionResult> PostImage(IFormFile file, string? altText = null)
    // {
    //     var alt = altText ?? "alt text";
    //     try
    //     {
    //         var untracked = await service.HostImageAsync(file);
    //     
    //         // TODO: move all logic to service
    //         var image = new Image
    //         {
    //             FileName = untracked.UploadedWithFileName,
    //             FileSize = untracked.FileSize,
    //             StorageFileName = untracked.StorageFileName,
    //             ContentType = untracked.ContentType,
    //             Height = untracked.Height,
    //             Width = untracked.Width,
    //             AltText = alt
    //         };
    //     
    //         await repository.SaveImageAsync(image);
    //         return Ok(image);
    //     }
    //     catch (Exception ex)
    //     {
    //         return Problem("Error posting image.");
    //     }
    //
    //
    // }
    
    // [HttpPost]
    // public async Task<ActionResult<Image>> UploadImage(IFormFile file)
    // {
    //     if (environment is null)
    //     {
    //         return BadRequest("Environment is null.");
    //     }
    //     var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
    //     var path = Path.Combine(environment.ContentRootPath, fileName);
    //     
    //     await using (var stream = System.IO.File.Create(path))
    //     {
    //         await file.CopyToAsync(stream);
    //     }
    //
    //     try
    //     {
    //         var imageInfo = await ImageShrp.IdentifyAsync(path);
    //         if (imageInfo == null)
    //         {
    //             throw new Exception("Null returned in IdentifyAsync. Likely threw exception that failed to propagate. " +
    //                                 "Throwing exception.");
    //         }
    //
    //         var width = imageInfo.Width;
    //         var height = imageInfo.Height;
    //         var image = new Image
    //         {
    //             FileName = file.FileName,
    //             ContentType = file.ContentType,
    //             FileSize = file.Length,
    //             StorageFileName = fileName,
    //             AltText = "alt text",
    //             Width = width,
    //             Height = height
    //         };
    //
    //         dbContext.Images.Add(image);
    //         await dbContext.SaveChangesAsync();
    //         
    //         return image;
    //     }
    //     catch (Exception ex)
    //     {
    //         if (!System.IO.File.Exists(path)) throw;
    //         var untracked = new UntrackedFile()
    //         {
    //             OccurredInClass = "Controllers/ImageController",
    //             OccurredElaboration = "Error in UploadImage. File was saved but exception thrown before added to db.",
    //             FileName = $"{fileName}",
    //             FileLocation = "ClientApp/public"
    //         };
    //         dbContext.UntrackedFiles.Add(untracked);
    //         await dbContext.SaveChangesAsync();
    //         throw;
    //     }
    //     
    // }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteImageById(int id)
    {
        var status = await imageRepository.DeleteImageByIdAsync(id);
        return status switch
        {
            true => Ok(),
            null => Problem(),
            false => NotFound()
        };
    }
    
    
    // private static ImageItemDto ImageToDto(Image image)
    // {
    //     return new ImageItemDto(image.Id, image.FileName, image.ContentType, image.FileSize, image.StorageFileName, image.Width, image.Height, image.AltText);
    // }
    //
    // public sealed record ImageItemDto(int Id, string FileName, string ContentType, long FileSize, string StorageFileName, int Width, int Height, string? AltText = null);
    //
}