using Microsoft.AspNetCore.Mvc;

namespace WebApplication6.Backend.Services;

public class FileHostingService(IWebHostEnvironment environment) : IFileHostingService
{
    public async Task HostFileAsync(IFormFile file)
    {
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var path = Path.Combine(environment.ContentRootPath, fileName);
        
        await using (var stream = System.IO.File.Create(path))
        {
            await file.CopyToAsync(stream);
        }
        
    }
}