using Microsoft.AspNetCore.Mvc;
using WebApplication6.Backend.Models;

namespace WebApplication6.Backend.Repositories;

public interface IImageRepository
{
    Task<IEnumerable<Image>> GetAllImagesAsync();

    Task<IEnumerable<int>> GetAllImagesIdsAsync();
    
    Task<Image?> GetImageByIdAsync(int id);
    
    /// <returns>Id of the saved image on success, or null on exception thrown or failure.</returns>
    Task<int?> SaveImageAsync(Image image);
    
    /// <returns>true on success, or false</returns>
    Task<bool> DeleteImageByIdAsync(int id);
    
}