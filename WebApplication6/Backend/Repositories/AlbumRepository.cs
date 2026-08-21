using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication6.Backend.Data;
using WebApplication6.Backend.Models;

namespace WebApplication6.Backend.Repositories;

public class AlbumRepository(ApplicationDbContext context) : IAlbumRepository
{
    public async Task<IEnumerable<Album>> GetAllAlbumsAsync()
    {
        var albums = await context.Albums
            .ToListAsync();
        
        return albums;
    }

    public async Task<IEnumerable<int>> GetAllAlbumsIdsAsync()
    {
        var albumIds = await context.Albums
            .AsNoTracking()
            .Select(a => a.Id)
            .ToListAsync();
        
        return albumIds;
    }

    public async Task<Album?> GetAlbumByIdAsync(int id)
    {
        var album = await context.Albums
            .FindAsync(id);
        
        return album;
    }

    public async Task<int?> SaveAlbumAsync(Album album)
    {
        context.Albums.Add(album);

        try
        {
            await context.SaveChangesAsync();
            return album.Id;
        }
        catch (Exception ex)
        {
            return null;
        }

    }

    public async Task<bool?> DeleteAlbumByIdAsync(int id)
    {
        var album = await context.Albums.FindAsync(id);
        if (album == null)
        {
            return false;
        }

        context.Albums.Remove(album);

        try
        {
            await context.SaveChangesAsync();
            
            return true;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<ICollection<Photo>?> GetAlbumPhotosAsync(int id)
    {
        try
        {
            var album = await context.Albums
                .Include(a => a.Photos)
                .Where(a => a.Id == id)
                .SingleAsync();

            var photos = album.Photos;
            
            return photos;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}