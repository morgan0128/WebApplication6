using Microsoft.AspNetCore.Mvc;
using WebApplication6.Backend.Models;

namespace WebApplication6.Backend.Repositories;

public interface IAlbumRepository
{
    Task<IEnumerable<Album>> GetAllAlbumsAsync();

    Task<IEnumerable<int>> GetAllAlbumsIdsAsync();

    Task<int> GetTotalNumberAlbums();
    
    Task<Album?> GetAlbumByIdAsync(int id);
    
    /// <returns>Id of the saved album on success, or null on exception thrown or failure.</returns>
    Task<int?> SaveAlbumAsync(Album album);


    // /// <returns>true on success, or false.</returns>
    // Task<bool> AddPhotoToAlbum(int albumId, int photoId);

    /// <summary>
    /// TODO
    /// Uses retry approach to setting an 'appended' Order, which is not ideal, but
    /// working on separate frontend refactoring and don't want to divert into
    /// writing interfaces + dependency injections for type of DB
    /// (as correct 'pessimistic' implementations may differ depending on DBMS)
    /// </summary>
    /// <param name="albumId"></param>
    /// <param name="photoId"></param>
    /// <param name="cancellationToken">(Optional)</param>
    /// <returns>true on success, or false</returns>
    Task<bool> AddPhotoToAlbumAsync(int albumId, int photoId, CancellationToken cancellationToken = default);
    
    /// <returns>true on success, or false on not found</returns>
    Task<bool> DeleteAlbumByIdAsync(int id);

    /// <summary>
    /// Retrieves all photos for the queried Album
    /// </summary>
    /// <param name="id">The Id of the Album row to query.</param>
    /// <returns>A (nullable) IEnumerable of AlbumPhotoDto with no guarantee that they have a correct or an explicit ordering</returns>
    Task<IEnumerable<AlbumPhotoDto>> GetAlbumPhotosAsync(int id); // TODO: Make Task<IAsyncEnumerable....> instead, once have more time to look into.

    Task<bool> ReorderPhotoInAlbum(int albumId, int photoId, int newOrder, CancellationToken cancellationToken = default);
    
    Task<bool> ToggleDisplaysName(int albumId, int photoId);

    public sealed record AlbumPhotoDto(int Id, string? Name, string? Description, int? YearContentCreated, Image Image, int? Order, bool displaysName = true, bool displaysDescription = true, bool displaysYearCC = true);

}