using Microsoft.AspNetCore.Mvc;
using WebApplication6.Backend.Models;
using WebApplication6.Backend.Repositories;

namespace WebApplication6.Backend.Services;

public class UploadPhotoService(IPhotoRepository photoRepository, IImageRepository imageRepository, IFileHostingService fileHostingService) : IUploadPhotoService
{
    public async Task<bool> UploadPhoto(Album album, IFormFile file, PhotoSpecDto photoSpec)
    {
        var altText = photoSpec.Name;

        var hostImageResult = await fileHostingService.HostImageAsync(file);

        if (hostImageResult == null) return false;

        var untrackedImage = hostImageResult;
        
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
    
        var nullableImageId = await imageRepository.SaveImageAsync(image);
        if (nullableImageId is null)
        {
            return false;
        }

        var imageId = nullableImageId.Value;
        
        var photo = new Photo
        {
            // CreatedAt = System.DateTime.Now,
            ImageId = imageId,
            Name = photoSpec.Name ?? "unnamed",
            Description = photoSpec.Description ?? "",
            YearContentCreated = photoSpec.YearContentCreated ?? 1999
        };

        var photoResult = await photoRepository.GetPhotoByIdAsync(imageId);
        return photoResult != null;
    }
    
    
}