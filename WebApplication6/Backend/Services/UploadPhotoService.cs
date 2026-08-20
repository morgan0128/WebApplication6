using Microsoft.AspNetCore.Mvc;
using WebApplication6.Backend.Models;
using WebApplication6.Backend.Repositories;

namespace WebApplication6.Backend.Services;

public class UploadPhotoService(IPhotoRepository photoRepository, IImageRepository imageRepository, IFileHostingService fileHostingService) : IUploadPhotoService
{
    public async Task<IActionResult> UploadPhoto(Album album, IFormFile file, PhotoSpecDto photoSpec)
    {
        var altText = photoSpec.Name;

            var hostImageResult = await fileHostingService.HostImageAsync(file);

            if (hostImageResult.Value == null) return new UnprocessableEntityResult();

            var untrackedImage = hostImageResult.Value;
            
            var image = new Image
            {
                FileName = untrackedImage.UploadedWithFileName,
                FileSize = untrackedImage.FileSize,
                StorageFileName = untrackedImage.StorageFileName,
                ContentType = untrackedImage.ContentType,
                Height = untrackedImage.Height,
                Width = untrackedImage.Width,
                AltText = photoSpec.Name ?? ""
            };
        
            var imageResult = await imageRepository.SaveImageAsync(image);
            var savedImage = imageResult.Value;
            // if (imageResult.Result?.GetType() == typeof(ForbidResult)) // TODO
            if (savedImage == null)
            {
                return new UnprocessableEntityResult(); // TODO
            }

            // var savedImage = imageResult.Value;

            var photo = new Photo
            {
                // CreatedAt = System.DateTime.Now,
                ImageId = savedImage.Id,
                Name = photoSpec.Name ?? "unnamed",
                Description = photoSpec.Description ?? "",
                YearContentCreated = photoSpec.YearContentCreated ?? 1999
            };

            var photoResult = await photoRepository.GetPhotoByIdAsync(savedImage.Id);
            if (photoResult.Value == null)
            {
                return new UnprocessableEntityResult(); // TODO
            }
            
            return new OkResult();
    }
    
    
}