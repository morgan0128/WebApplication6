using Microsoft.AspNetCore.Mvc;
using WebApplication6.Backend.Models;

namespace WebApplication6.Backend.Repositories;

public interface IPhotoRepository
{
    Task<ActionResult<IEnumerable<Photo>>> GetAllPhotosAsync();

    Task<ActionResult<IEnumerable<int>>> GetAllPhotosIdsAsync();
    
    Task<ActionResult<Photo?>> GetPhotoByIdAsync(int id);

    Task<int> SavePhotoAsync(Photo photo);
    
    Task<IActionResult> DeletePhotoByIdAsync(int id);
}