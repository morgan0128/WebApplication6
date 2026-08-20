using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication6.Backend.Data;
using WebApplication6.Backend.Models;

namespace WebApplication6.Backend.Repositories;

public class AlbumRepository(ApplicationDbContext context) : IAlbumRepository
{
    public async Task<ActionResult<IEnumerable<Album>>> GetAllAlbumsAsync()
    {
        var albums = await context.Albums
            .ToListAsync();
        return albums;
    }

    public async Task<ActionResult<IEnumerable<int>>> GetAllAlbumsIdsAsync()
    {
        var albums = await context.Albums
            .AsNoTracking()
            .Select(a => a.Id)
            .ToListAsync();
        return albums;
    }

    public async Task<ActionResult<Album?>> GetAlbumByIdAsync(int id)
    {
        var album = await context.Albums
            .FindAsync(id);
        return album;
    }

    public async Task<int> SaveAlbumAsync(Album album)
    {
        context.Albums.Add(album);
        return await context.SaveChangesAsync();
    }

    public async Task<IActionResult> DeleteAlbumByIdAsync(int id)
    {
        var album = await context.Albums.FindAsync(id);
        if (album == null)
        {
            return new NotFoundResult();
        }

        context.Albums.Remove(album);
        await context.SaveChangesAsync();
        return new OkResult();
    }

    public async Task<ActionResult<IEnumerable<Photo>>> GetAlbumPhotosAsync(int id)
    {
        var photos = await context.Albums
            .Where(a => a.Id == id)
            .Include(a => a.Photos)
            .ToListAsync();
        
        return new ObjectResult(photos);
    }
}