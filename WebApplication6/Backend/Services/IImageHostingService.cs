namespace WebApplication6.Backend.Services;

public interface IImageHostingService : IFileHostingService
{
    Task<int> RetrieveImageWidthAsync(string path);
    
    Task<int> RetrieveImageHeightAsync(string path);
}