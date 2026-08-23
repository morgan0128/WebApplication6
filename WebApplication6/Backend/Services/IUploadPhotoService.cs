using Microsoft.AspNetCore.Mvc;
using WebApplication6.Backend.Models;

namespace WebApplication6.Backend.Services;

public interface IUploadPhotoService
{
    Task<bool> UploadPhoto(Album album, IFormFile file, PhotoSpecDto photoSpec);
}

// Specification for a photo based on user input
public record PhotoSpecDto(string? Name, string? Description, int? YearContentCreated);

// public record UploadPhotoDto(IFormFile File, PhotoSpecDto PhotoSpec);

// public class UploadPhotoDto
// {
//      public IFormFile FormData { get; set; }
//      public PhotoSpecDto PhotoSpec { get; set; }
// }

     // public PhotoSpecDto(IFormFile imageFile, string? name, string? description, int? yearContentCreated)
     // {
     //     ImageFile = imageFile;
     //     Name = name;
     //     Description = description;
     //     YearContentCreated = yearContentCreated;
     // }

// public class CombinedPhotoSpecDto
public record CombinedPhotoSpecDto(IFormFile File, string? Name, string? Description, int? YearContentCreated);
// {
    // public IFormFile ImageFile { get; set; }
    // public string? Name { get; set; }
    // public string? Description { get; set; }
    // public int? YearContentCreated { get; set; }
    


// }

