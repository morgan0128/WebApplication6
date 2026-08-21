using Microsoft.AspNetCore.Mvc;
using WebApplication6.Backend.Models;

namespace WebApplication6.Backend.Repositories;

public interface IImageRepository
{
    Task<IEnumerable<Image>> GetAllImagesAsync();

    Task<IEnumerable<int>> GetAllImagesIdsAsync();
    
    Task<Image?> GetImageByIdAsync(int id);
    
    /// <returns>The Id of the saved image on success, or null on exception thrown or failure.</returns>
    Task<int?> SaveImageAsync(Image image);
    
    /// <returns>true on success, false on not found, or null on found but either exception thrown or failure</returns>
    Task<bool?> DeleteImageByIdAsync(int id);
    
}