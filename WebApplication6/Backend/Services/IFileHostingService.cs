using Microsoft.AspNetCore.Mvc;

namespace WebApplication6.Backend.Services;

public interface IFileHostingService
{
    Task HostFileAsync(IFormFile file);
}