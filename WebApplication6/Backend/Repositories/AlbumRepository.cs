using System.Collections;
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
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> AddPhotoToAlbum(int albumId, int photoId)
    {
        var album = await context.Albums.FindAsync(albumId);
        if (album == null)
        {
            return false;
        }

        var photo = await context.Photos.FindAsync(photoId);
        if (photo == null)
        {
            return false;
        }

        album.Photos.Add(photo);
        try
        {
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }

    }

    public async Task<bool> DeleteAlbumByIdAsync(int id)
    {
        var album = await context.Albums.FindAsync(id);
        if (album == null) return false;


        context.Albums.Remove(album);

        try
        {
            await context.SaveChangesAsync();
            return true;
        }
        catch (AggregateException)
        {
            return false;
        }
    }

    public async Task<IAsyncEnumerable<IAlbumRepository.PhotoDto>?> GetAlbumPhotosAsyncEnumerable(int id)
    {
        try
        {
            var album = await context.Albums
                .Include(a => a.Photos).ThenInclude(p => p.Image)
                .Where(a => a.Id == id)
                .SingleAsync();

            var photos = album.Photos
                .Select(photo => new IAlbumRepository.PhotoDto(
                    photo.Id,
                    photo.Name, 
                    photo.Description,
                    photo.YearContentCreated, 
                    photo.Image
                ))
                .ToAsyncEnumerable();
            
            return photos;
        }
        catch (Exception)
        {
            return null;
        }
    }


}