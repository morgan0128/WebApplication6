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
    // private readonly IAmazonS3 _s3Client = s3Client;

    // public async Task<IActionResult> UploadFileAsync(IFormFile file, string bucketName, string? prefix)
    // {
    //     var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, bucketName);
    //     if (!bucketExists) return NotFound($"Bucket {bucketName} does not exist.");
    //     var request = new PutObjectRequest
    //     {
    //         BucketName = bucketName,
    //         Key = string.IsNullOrEmpty(prefix) ? file.FileName : $"{prefix.TrimEnd('/')}/{file.FileName}",
    //         ContentType = file.ContentType,
    //         InputStream = file.OpenReadStream()
    //     };
    //     request.Metadata.Add("Content-Type", file.ContentType);
    //     await _s3Client.PutObjectAsync(request);
    //
    //     return Ok($"File {prefix}/{file.FileName} uploaded to S3 successfully.");
    // }

    [HttpGet]
    public async Task<ActionResult<List<ImageItemDto>>> GetImages()
    {
        var images = await dbContext.Images
            .AsNoTracking()
            .Where(i => i.Id == 5 || i.Id == 6)
            .Select(image => ImageToDto(image))
            .ToListAsync();
        
        return images;
    }
    
    // [HttpGet]
    // public async Task<List<IActionResult>> GetImages()
    // {
    //     var images = await dbContext.Images
    //         .AsNoTracking()
    //         .Select(image => ImageToDto(image))
    //         .ToListAsync();
    //
    //     var files = new List<IActionResult>();
    //     foreach (var image in images)
    //     {
    //         var l = await GetImage(image.Id);
    //     }
    //     
    //     return files;
    // }
    
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
    
    // private async Task<IActionResult> GetImage(IFileProvider fileProvider, Image? image)
    // {
    //     if (image is null) return NotFound();
    //
    //     // var pr = environment.WebRootFileProvider;
    //     
    //     var imageFile = fileProvider.GetFileInfo(image.StoragePath).PhysicalPath;
    //     if (!System.IO.File.Exists(imageFile)) return NotFound();
    //     
    //     var bz = await System.IO.File.ReadAllBytesAsync(imageFile);
    //     return File(bz, image.ContentType);
    // }
    
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
            StoragePath = $"/uploads/{fileName}",
            AltText = "alt text"
        };

        dbContext.Images.Add(image);
        await dbContext.SaveChangesAsync();

        // await UploadFileAsync(file, "amzn-s3-webapp6-file-storage-468670609491-us-east-2-an", "uploads");
        
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

    // private FileContentResult GetBlobbable(Image image)
    // {
    //     var asBytes = System.IO.File.ReadAllBytes(image.StoragePath);
    //     return File(asBytes, image.ContentType);
    // }
    
    private static ImageItemDto ImageToDto(Image image)
    {
        return new ImageItemDto(image.Id, image.FileName, image.ContentType, image.FileSize, image.StoragePath, image.AltText);
    }
    
    public sealed record ImageItemDto(int Id, string FileName, string ContentType, long FileSize, string StoragePath, string? AltText = null);
    
}