using Microsoft.AspNetCore.Mvc;
using WebApplication6.Backend.Data;
using WebApplication6.Backend.Models;
using WebApplication6.Backend.Repositories;
using ImageShrp = SixLabors.ImageSharp.Image;

namespace WebApplication6.Backend.Services;

// TODO:
// make controller for file hosting;
// dependency injections here;
// or perform other similar refactorings
public class ImageHostingService(IImageRepository repository, IWebHostEnvironment environment)
{
    public async Task<int> HostImageAsync(IFormFile file)
    {
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
            
            return await repository.PostImageAsync(image);
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
            // todo
            // dbContext.UntrackedFiles.Add(untracked);
            // await dbContext.SaveChangesAsync();
            throw;
        }
    }
}