using Microsoft.AspNetCore.Mvc;
using WebApplication6.Backend.Models;

namespace WebApplication6.Backend.Repositories;

public interface IAlbumRepository
{
    Task<IEnumerable<Album>> GetAllAlbumsAsync();

    Task<IEnumerable<int>> GetAllAlbumsIdsAsync();
    
    Task<Album?> GetAlbumByIdAsync(int id);
    
    /// <returns>The Id of the saved album on success, or null on exception thrown or failure.</returns>
    Task<int?> SaveAlbumAsync(Album album);


    /// <returns>true on success, or false.</returns>
    Task<bool> AddPhotoToAlbum(int albumId, int photoId);
    
    /// <returns>true on success, false on not found, or null on found but either exception thrown or failure</returns>
    Task<bool?> DeleteAlbumByIdAsync(int id);

    /// <summary>
    /// Retrieves all photos for the queried Album
    /// </summary>
    /// <param name="id">The id of the Album row to query.</param>
    /// <returns>An ICollection with T as Photo on success, or null</returns>
    Task<ICollection<Photo>?> GetAlbumPhotosAsync(int id);
}