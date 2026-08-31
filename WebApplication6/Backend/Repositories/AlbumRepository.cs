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

    public async Task<int> GetTotalNumberAlbums()
    {
        var amount = await context.Albums
            .CountAsync();

        return amount;
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

    // public async Task<bool> AddPhotoToAlbum(int albumId, int photoId)
    // {
    //     var album = await context.Albums.FindAsync(albumId);
    //     if (album == null)
    //     {
    //         return false;
    //     }
    //
    //     var photo = await context.Photos.FindAsync(photoId);
    //     if (photo == null)
    //     {
    //         return false;
    //     }
    //
    //     album.Photos.Add(photo);
    //     try
    //     {
    //         await context.SaveChangesAsync();
    //         return true;
    //     }
    //     catch (Exception)
    //     {
    //         return false;
    //     }
    //
    // }
    
    public async Task<bool> AddPhotoToAlbumAsync(int albumId, int photoId, CancellationToken cancellationToken = default)
    {
        const int maximumAttempts = 3;

        for (var attempt = 1; attempt <= maximumAttempts; attempt++)
        {
            var albumPhotos = context.AlbumPhotos.Where(ap => ap.AlbumId == albumId);
            var nextOrder = -1;
            if (!albumPhotos.Any())
            {
                nextOrder = 0;
            }
            else
            {
                nextOrder = (await albumPhotos.MaxAsync(ap => ap.Order, cancellationToken)) + 1;
            }


            var albumPhoto = new AlbumPhoto
            {
                AlbumId = albumId,
                PhotoId = photoId,
                Order = nextOrder,
                DisplaysName = true,
                DisplaysDescription = true,
                DisplaysYearContentCreated = true
            };

            context.AlbumPhotos.Add(albumPhoto);

            try
            {
                await context.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (DbUpdateException exception)
            {
                // do not track albumPhoto that violated db constraint
                context.Entry(albumPhoto).State = EntityState.Detached; 

                if (attempt == maximumAttempts)
                    throw;
            }
        }

        return false;
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

    public async Task<IEnumerable<IAlbumRepository.AlbumPhotoDto>> GetAlbumPhotosAsync(int id)
    {
        try
        {
            var album = await context.Albums
                .Include(a => a.Photos).ThenInclude(p => p.Image)
                .Where(a => a.Id == id)
                .SingleAsync();

            if (album.Photos.Count == 0) return new List<IAlbumRepository.AlbumPhotoDto>();

            var albumPhotos = await context.AlbumPhotos
                .Where(ap => ap.AlbumId == album.Id)
                .ToListAsync();

            if (albumPhotos.Count == 0) throw new Exception("Issue in AlbumPhotos table.");

            var photos = album.Photos
                .Join(
                    albumPhotos,
                    (p => p.Id),
                    (ap => ap.PhotoId),
                    (photo, albumPhoto) => new IAlbumRepository.AlbumPhotoDto(
                        photo.Id,
                        photo.Name,
                        photo.Description,
                        photo.YearContentCreated,
                        photo.Image,
                        albumPhoto.Order,
                        albumPhoto.DisplaysName,
                        albumPhoto.DisplaysDescription,
                        albumPhoto.DisplaysYearContentCreated
                    )
                )
                .OrderBy(photos => photos.Order)
                .ToList();
            
            return photos;
        }
        catch (Exception)
        {
            // return new List<IAlbumRepository.AlbumPhotoDto>();
            throw;
        }
    }

    public async Task<bool> ReorderPhotoInAlbum(int albumId, int photoId, int newOrder)
    {
        if (newOrder < 0) return false;

        var albumPhotos = await context.AlbumPhotos
            .Where(ap => ap.AlbumId == albumId)
            .OrderBy(ap => ap.Order)
            .ToListAsync();

        var toMove = albumPhotos.Find(ap => ap.PhotoId == photoId);
        if (toMove == null) return false;

        var atOrder = albumPhotos.Find(ap => ap.Order == newOrder);
        if (atOrder == null)
        {
            toMove.Order = newOrder;
            await context.SaveChangesAsync();
            return true;
        } else if (atOrder.PhotoId == toMove.PhotoId)
        {
            return true;
        }
        else
        {
            var index = albumPhotos.IndexOf(atOrder);
            while (index < albumPhotos.Count)
            {
                var moveMeForward = albumPhotos[index];
                moveMeForward.Order = moveMeForward.Order + 1;
                index++;
            }
            await context.SaveChangesAsync();

            toMove.Order = newOrder;
            await context.SaveChangesAsync();
            return true;
        }
        
    }

    public async Task<bool> ToggleDisplaysName(int albumId, int photoId)
    {
        var ap = await context.AlbumPhotos
            .FindAsync(albumId, photoId);

        if (ap == null) return false;
        
        ap.DisplaysName = !ap.DisplaysName;
        await context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> ToggleDisplaysDescription(int albumId, int photoId)
    {
        var ap = await context.AlbumPhotos
            .FindAsync(albumId, photoId);

        if (ap == null) return false;
        
        ap.DisplaysDescription = !ap.DisplaysDescription;
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ToggleDisplaysYearContenCreated(int albumId, int photoId)
    {
        var ap = await context.AlbumPhotos
            .FindAsync(albumId, photoId);

        if (ap == null) return false;
        
        ap.DisplaysYearContentCreated = !ap.DisplaysYearContentCreated;
        await context.SaveChangesAsync();
        return true;
    }
    

}
