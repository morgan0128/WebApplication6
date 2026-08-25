using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication6.Backend.Models;
using WebApplication6.Backend.Repositories;
using Xunit;

namespace WebApplication6.Tests.Tests.Integration.Repositories;

// Tests against test database.
public class AlbumRepositoryTest(TestDatabaseFixture dbFixture) : IClassFixture<TestDatabaseFixture>
{
    public TestDatabaseFixture Fixture { get; } = dbFixture;
/*
 * TODO:
 *
    [Fact]
    public async Task GetAllAlbumsAsync_ReturnsExpectedValue()
    {
        var image1 = new Image
        {
            Id = 1, FileName = "image1", ContentType = "image/png", FileSize = 617238,
            StorageFileName = "a7344381-fe36-4139-be80-5c61cb7fe8af.png",
            Url = "uploads/a7344381-fe36-4139-be80-5c61cb7fe8af.png", AltText = "alt text",
            Height = 771, Width = 646
        };
        var image2 = new Image
        {
            Id = 3, FileName = "image2", ContentType = "image/png", FileSize = 135767,
            StorageFileName = "63277fab-ef45-4e71-9eea-c5d8f9ac571b.png",
            Url = "uploads/a7344381-fe36-4139-be80-5c61cb7fe8af.png", AltText = "alt text",
            Height = 349, Width = 346
        };

        var photo1 = new Photo
        {
            Id = 2, CreatedAt = new DateTime(DateOnly.MinValue, TimeOnly.MinValue), Name = "MyPhoto",
            Description = "I am a Photo!", Image = image2, ImageId = image2.Id, YearContentCreated = 2000
        };
        var photo2 = new Photo
        {
            Id = 64, CreatedAt = new DateTime(DateOnly.MaxValue, TimeOnly.MaxValue), Name = "Cat Picture",
            Description = "I am a cat!", Image = image1, ImageId = image1.Id, YearContentCreated = 2026
        };
        
        var album1 = new Album
        {
            Id = 1, Name = "MyPhotoAlbum", Description = "Here are all of my pictures", 
            Photos = new List<Photo>() { photo1, photo2 }
        };
        
        await using var context = Fixture.CreateContext();
        var repository = new AlbumRepository(context);

        var albums = await repository.GetAllAlbumsAsync();
        var retrievedAlbum = albums.ToList().Find(a => a.Id == 1);
        var albumsPhotos = await repository.GetAlbumPhotosAsyncEnumerable(1);
        if (albumsPhotos != null)
        {
            var retrievedPhotos =
                await albumsPhotos.ToListAsync(cancellationToken: TestContext.Current.CancellationToken);
        }
        


        Assert.NotNull(albums);
    }
*/
}