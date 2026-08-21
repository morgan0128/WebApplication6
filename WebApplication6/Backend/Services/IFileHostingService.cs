using Microsoft.AspNetCore.Mvc;
using WebApplication6.Backend.Models;

namespace WebApplication6.Backend.Services;

public interface IFileHostingService
{
    Task<UntrackedImageFileDto?> HostImageAsync(IFormFile file);
}

public record UntrackedImageFileDto(string StorageFileName, string UploadedWithFileName, string ContentType, long? FileSize, int Width, int Height);
