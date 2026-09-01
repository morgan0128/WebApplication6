using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication6.Backend.Models;
using WebApplication6.Backend.Repositories;
using Xunit;

namespace WebApplication6.Tests.Tests.Integration.Repositories;

// Tests against test database.
public class AlbumRepositoryModifyDataTests(TestDatabaseFixture dbFixture) : IClassFixture<TestDatabaseFixture>
{
    public TestDatabaseFixture Fixture { get; } = dbFixture;
    
    [Theory]
    [InlineData(1, 4, -1)]
    [InlineData(1, 4, -99)]
    [InlineData(1, 3, -7)]
    [InlineData(1, 64, -9999)]
    public async Task ReorderPhotoInAlbum_InvalidOrder_MaintainsPracticalOrderButNormalizedAndReturnsTrue(int albumId, int photoId, int newOrder)
    {
        await using var context = Fixture.CreateContext();
        await context.Database.BeginTransactionAsync(TestContext.Current.CancellationToken);
        var repository = new AlbumRepository(context);


        
        var task = await repository.ReorderPhotoInAlbum(albumId, photoId, newOrder);
        context.ChangeTracker.Clear();
        
        var albumPhotos = await context.AlbumPhotos
            .Where(ap => ap.AlbumId == albumId)
            .OrderBy(ap => ap.Order)
            .ToListAsync(cancellationToken: TestContext.Current.CancellationToken);
        
        Assert.Collection(albumPhotos,
            element1Inspector =>
            {
                Assert.Equal(4, element1Inspector.PhotoId);
                Assert.Equal(0, element1Inspector.Order);
            },
            element2Inspector =>
            {
                Assert.Equal(2, element2Inspector.PhotoId);
                Assert.Equal(1, element2Inspector.Order);
            },
            element3Inspector =>
            {
                Assert.Equal(3, element3Inspector.PhotoId);
                Assert.Equal(2, element3Inspector.Order);
            },
            element4Inspector =>
            {
                Assert.Equal(64, element4Inspector.PhotoId);
                Assert.Equal(3, element4Inspector.Order);
            });
        
        Assert.True(task);
    }
    
    [Fact]
    public async Task ReorderPhotoInAlbum_AssignOrderInBetweenGap_ReturnsExpectedAndNormalized()
    {
        const int albumId = 1;
        const int photoId = 4;
        const int newOrder = 3;
        
        await using var context = Fixture.CreateContext();
        await context.Database.BeginTransactionAsync(TestContext.Current.CancellationToken);
        var repository = new AlbumRepository(context);
        
        var task = await repository.ReorderPhotoInAlbum(albumId, photoId, newOrder);
        context.ChangeTracker.Clear();
        
        var albumPhotos = await context.AlbumPhotos
            .Where(ap => ap.AlbumId == albumId)
            .OrderBy(ap => ap.Order)
            .ToListAsync(cancellationToken: TestContext.Current.CancellationToken);
        
        Assert.Collection(albumPhotos,
            element1Inspector =>
            {
                Assert.Equal(2, element1Inspector.PhotoId);
                Assert.Equal(0, element1Inspector.Order);
            },
            element2Inspector =>
            {
                Assert.Equal(3, element2Inspector.PhotoId);
                Assert.Equal(1, element2Inspector.Order);
            },
            element3Inspector =>
            {
                Assert.Equal(4, element3Inspector.PhotoId);
                Assert.Equal(2, element3Inspector.Order);
            },
            element4Inspector =>
            {
                Assert.Equal(64, element4Inspector.PhotoId);
                Assert.Equal(3, element4Inspector.Order);
            });
        
        Assert.True(task);
    }
    
    
    

}