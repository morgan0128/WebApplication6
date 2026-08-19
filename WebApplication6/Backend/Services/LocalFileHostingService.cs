using WebApplication6.Backend.Models;
using WebApplication6.Backend.Repositories;
using ImageShrp = SixLabors.ImageSharp.Image;

namespace WebApplication6.Backend.Services;

public class LocalFileHostingService(IWebHostEnvironment environment, IUntrackedFileRepository untrackedFileRepository) : IFileHostingService
{
    public async Task<UntrackedImageFileDto> HostImageAsync(IFormFile file)
    {
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var path = Path.Combine(environment.ContentRootPath, fileName);
        
        await using (var stream = System.IO.File.Create(path))
        {
            await file.CopyToAsync(stream);
        }

        if (!File.Exists(path))
        {
            throw new Exception("File not found after upload. Throwing exception.");
        }

        try
        {
            var imageInfo = await ImageShrp.IdentifyAsync(path);
            return new UntrackedImageFileDto(fileName, file.FileName, file.ContentType, file.Length, imageInfo.Width, imageInfo.Height);
        }
        catch (Exception ex)
        {
            UntrackedFile untracked = new UntrackedFile
            {
                FileName = file.FileName,
                FileStorageLocation = fileName
            };
            await untrackedFileRepository.PostUntrackedAsync(untracked);
            throw;
        }
    
    }
}