using Microsoft.AspNetCore.Mvc;
using WebApplication6.Backend.Models;
using WebApplication6.Backend.Repositories;
using ImageShrp = SixLabors.ImageSharp.Image;

namespace WebApplication6.Backend.Services;

public class LocalFileHostingService(string uploadRoot, IUntrackedFileRepository untrackedFileRepository) : IFileHostingService
{
    public async Task<UntrackedImageFileDto?> HostImageAsync(IFormFile file)
    {
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var path = Path.Combine(uploadRoot, fileName);
        
        await using (var stream = System.IO.File.Create(path))
        {
            await file.CopyToAsync(stream);
        }

        if (!File.Exists(path))
        {
            return null;
        }

        // try
        // {
            var imageInfo = await ImageShrp.IdentifyAsync(path);
            if (imageInfo == null)
            {
                var untracked = new UntrackedFile
                {
                    FileName = file.FileName,
                    FileStorageLocation = fileName
                };
                var untrackedId = await untrackedFileRepository.SaveUntrackedAsync(untracked);
                if (untrackedId == null)
                {
                    throw new Exception("!!!ERROR!!!\n" +
                                        "IMAGE FILE SAVED ON HOST YET FAILED TO BE STORED IN IMAGE TABLE" +
                                        "THEN FAILED TO BE STORED IN UNTRACKED_FILES TABLE.\n" +
                                        "SEVERITY: LOW (HOSTING FILES LOCALLY)");
                }
                return null;
            }

            return new UntrackedImageFileDto(fileName, ("/uploads" + "/" + fileName), file.FileName, file.ContentType, file.Length, imageInfo.Width, imageInfo.Height);

            
    
    }
}