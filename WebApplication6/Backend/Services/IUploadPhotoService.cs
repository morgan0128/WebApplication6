using Microsoft.AspNetCore.Mvc;
using WebApplication6.Backend.Models;

namespace WebApplication6.Backend.Services;

public interface IUploadPhotoService
{
    Task<bool> UploadPhoto(Album album, IFormFile file, PhotoSpecDto photoSpec);
}

// Specification for a photo based on user input
public record PhotoSpecDto(string? Name, string? Description, int? YearContentCreated);