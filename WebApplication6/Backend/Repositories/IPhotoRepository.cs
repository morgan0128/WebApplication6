using Microsoft.AspNetCore.Mvc;
using WebApplication6.Backend.Models;

namespace WebApplication6.Backend.Repositories;

public interface IPhotoRepository
{
    Task<IEnumerable<Photo>> GetAllPhotosAsync();

    Task<IEnumerable<int>> GetAllPhotosIdsAsync();
    
    Task<Photo?> GetPhotoByIdAsync(int id);

    /// <returns>The Id of the saved photo on success, or null on exception thrown or failure.</returns>
    Task<int?> SavePhotoAsync(Photo photo);
    
    /// <returns>true on success, false on not found, or null on found but either exception thrown or failure</returns>
    Task<bool?> DeletePhotoByIdAsync(int id);
}