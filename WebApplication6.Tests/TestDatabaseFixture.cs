using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebApplication6.Backend.Data;
using WebApplication6.Backend.Models;

namespace WebApplication6.Tests;

public class TestDatabaseFixture
{
    private const string ConnectionString = "Host=localhost;Port=5432;Database=webapplication6test;Username=postgres;Password=morg"; // exposed; okay

    private static readonly object _lock = new();
    private static bool _databaseInitialized;

    public TestDatabaseFixture()
    {
        lock (_lock)
        {
            if (!_databaseInitialized)
            {
                using (var context = CreateContext())
                {
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();

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
                    var album2 = new Album
                    {
                        Id = 3002, Name = "Unnamed", Photos = new List<Photo>()
                    };
                    
                    
                    context.AddRange(
                        image1,
                        image2,
                        photo1,
                        photo2,
                        album1,
                        album2
                    );
                    context.SaveChanges();
                }

                _databaseInitialized = true;
            }
        }
    }

    public ApplicationDbContext CreateContext()
        => new ApplicationDbContext(
            new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql(ConnectionString)
                .Options);
}