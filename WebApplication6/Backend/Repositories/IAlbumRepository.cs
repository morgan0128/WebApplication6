using Microsoft.AspNetCore.Mvc;
using WebApplication6.Backend.Models;

namespace WebApplication6.Backend.Repositories;

public interface IAlbumRepository
{
    Task<IEnumerable<Album>> GetAllAlbumsAsync();

    Task<IEnumerable<int>> GetAllAlbumsIdsAsync();
    
    Task<Album?> GetAlbumByIdAsync(int id);
    
    /// <returns>Id of the saved album on success, or null on exception thrown or failure.</returns>
    Task<int?> SaveAlbumAsync(Album album);


    /// <returns>true on success, or false.</returns>
    Task<bool> AddPhotoToAlbum(int albumId, int photoId);
    
    /// <returns>true on success, or false on not found</returns>
    Task<bool> DeleteAlbumByIdAsync(int id);

    /// <summary>
    /// Retrieves all photos for the queried Album
    /// </summary>
    /// <param name="id">The Id of the Album row to query.</param>
    /// <returns>A task fetching IAsyncEnumerable with T as Photo on success, or null</returns>
    Task<IAsyncEnumerable<PhotoDto>?> GetAlbumPhotosAsyncEnumerable(int id);
    
    public sealed record PhotoDto(int Id, string? Name, string? Description, int? YearContentCreated, Image Image);
}