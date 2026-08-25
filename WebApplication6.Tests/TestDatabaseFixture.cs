// using Microsoft.EntityFrameworkCore;
// using WebApplication6.Backend.Data;
// using WebApplication6.Backend.Models;
//
// namespace WebApplication6.Tests;
//
// public class TestDatabaseFixture
// {
//     private const string ConnectionString = "Host=localhost;Port=5432;Database=webapplication6test;Username=postgres;Password=morg";
//
//     private static readonly object _lock = new();
//     private static bool _databaseInitialized;
//
//     public TestDatabaseFixture()
//     {
//         lock (_lock)
//         {
//             if (!_databaseInitialized)
//             {
//                 using (var context = CreateContext())
//                 {
//                     context.Database.EnsureDeleted();
//                     context.Database.EnsureCreated();
//
//                     context.AddRange( TODO FIX
//                         new Image { FileName = "image1", ContentType = "image/png", FileSize = 617238, StorageFileName = "a7344381-fe36-4139-be80-5c61cb7fe8af.png", AltText = "alt text", Height = 771, Width = 646, Id = 1 },
//                         new Image { FileName = "image2", ContentType = "image/png", FileSize = 135767, StorageFileName = "63277fab-ef45-4e71-9eea-c5d8f9ac571b.png", AltText = "alt text", Height = 349, Width = 346, Id = 3 });
//                     context.SaveChanges();
//                 }
//
//                 _databaseInitialized = true;
//             }
//         }
//     }
//
//     public ApplicationDbContext CreateContext()
//         => new ApplicationDbContext(
//             new DbContextOptionsBuilder<ApplicationDbContext>()
//                 .UseNpgsql(ConnectionString)
//                 .Options);
// }