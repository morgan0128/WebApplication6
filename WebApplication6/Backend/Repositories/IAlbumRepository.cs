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
    
    // TODO: refactor  these Task<bool?> methods I've already decided they are weird. the other existing ones are in IImageRepository and IPhotoRepository
    /// <returns>true on success, false on not found, or null on found but either exception thrown or failure</returns>
    Task<bool?> DeleteAlbumByIdAsync(int id);

    /// <summary>
    /// Retrieves all photos for the queried Album
    /// </summary>
    /// <param name="id">The id of the Album row to query.</param>
    /// <returns>A task, fetching IAsyncEnumerable with T as Photo on success, or null</returns>
    Task<IAsyncEnumerable<AlbumRepository.PhotoDto>?> GetAlbumPhotosAsyncEnumerable(int id);
}