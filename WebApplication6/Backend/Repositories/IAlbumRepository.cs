using Microsoft.AspNetCore.Mvc;
using WebApplication6.Backend.Models;

namespace WebApplication6.Backend.Repositories;

public interface IAlbumRepository
{
    Task<ActionResult<IEnumerable<Album>>> GetAllAlbumsAsync();

    Task<ActionResult<IEnumerable<int>>> GetAllAlbumsIdsAsync();
    
    Task<ActionResult<Album?>> GetAlbumByIdAsync(int id);

    Task<int> SaveAlbumAsync(Album album);
    
    Task<IActionResult> DeleteAlbumByIdAsync(int id);

    Task<ActionResult<IEnumerable<Photo>>> GetAlbumPhotosAsync(int id);
}