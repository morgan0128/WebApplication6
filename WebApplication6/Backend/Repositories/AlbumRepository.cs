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
            var nextOrder =
                (await context.AlbumPhotos
                     .Where(ap => ap.AlbumId == albumId)
                     .MaxAsync(ap => ap.Order, cancellationToken)) + 1;

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

    public async Task<IEnumerable<IAlbumRepository.PhotoDto>?> GetAlbumPhotosAsync(int id)
    {
        try
        {
            var album = await context.Albums
                .Include(a => a.Photos).ThenInclude(p => p.Image)
                .Where(a => a.Id == id)
                .SingleAsync();

            if (album.Photos.Count == 0) return null;

            var albumPhotos = await context.AlbumPhotos
                .Where(ap => ap.AlbumId == album.Id)
                .ToListAsync();

            if (albumPhotos.Count == 0) throw new Exception("Issue in AlbumPhotos table.");

            var photos = album.Photos
                .Join(
                    albumPhotos,
                    (p => p.Id),
                    (ap => ap.PhotoId),
                    (photo, albumPhoto) => new IAlbumRepository.PhotoDto(
                        photo.Id,
                        photo.Name,
                        photo.Description,
                        photo.YearContentCreated,
                        photo.Image,
                        albumPhoto.Order
                    )
                )
                .OrderBy(photos => photos.Order)
                .ToList();
            
            return photos;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> ReorderPhotoInAlbum(int albumId, int photoId, int newOrder, CancellationToken cancellationToken = default)
    {
        if (newOrder < 0) return false;

        await using var transaction =
            await context.Database.BeginTransactionAsync(
                System.Data.IsolationLevel.Serializable,
                cancellationToken);

        var links = await context.AlbumPhotos
            .Where(ap => ap.AlbumId == albumId)
            .OrderBy(ap => ap.Order)
            .ToListAsync(cancellationToken: cancellationToken);

        var photoToReorder = links.SingleOrDefault(ap => ap.PhotoId == photoId);
        if (photoToReorder is null) return false;

        var finalOrders = new Dictionary<AlbumPhoto, int>
        {
            [photoToReorder] = newOrder
        };

        foreach (var link in links)
        {
            if (link.PhotoId == photoId || link.Order < newOrder) continue;
            if (link.Order == int.MaxValue) return false;

            finalOrders[link] = link.Order + 1;
        }

        var highestRequiredOrder = Math.Max(
            links.Max(ap => (long)ap.Order),
            finalOrders.Values.Max(order => (long)order));

        if (highestRequiredOrder + finalOrders.Count > int.MaxValue) return false;

        var temporaryOrder = (int)highestRequiredOrder + 1;
        foreach (var link in finalOrders.Keys)
            link.Order = temporaryOrder++;

        await context.SaveChangesAsync(cancellationToken);

        foreach (var (link, finalOrder) in finalOrders)
            link.Order = finalOrder;

        await context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return true;
    }


}
