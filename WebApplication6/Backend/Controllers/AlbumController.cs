using Microsoft.AspNetCore.Mvc;
using WebApplication6.Backend.Models;
using WebApplication6.Backend.Repositories;
using WebApplication6.Backend.Services;

namespace WebApplication6.Backend.Controllers;

[ApiController]
[Route("api/Album")]
public sealed class AlbumController(IAlbumRepository albumRepository, IUploadPhotoService uploadPhotoService) : ControllerBase
{
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<Album>>> GetAllAlbums()
    {
        return await albumRepository.GetAllAlbumsAsync();
    }
    
    
    [HttpGet("all-ids")]
    public async Task<ActionResult<IEnumerable<int>>> GetAllAlbumsIds()
    {
        return await albumRepository.GetAllAlbumsIdsAsync();
    }
    
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Album>> GetAlbumById(int id)
    {
        var album = await albumRepository.GetAlbumByIdAsync(id);
        if (album.Value == null)
        {
            return NotFound();
        }

        return Ok(album.Value);
    }

    [HttpPost]
    public async Task<int> PostAlbum(CreateAlbumItemRequest albumRequest)
    {
        var album = new Album
        {
            Name = albumRequest.Name,
            Description = albumRequest.Description
        };
        return await albumRepository.SaveAlbumAsync(album);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAlbumById(int id)
    {
        return await albumRepository.DeleteAlbumByIdAsync(id);
    }

    [HttpPost]
    public async Task<IActionResult> UploadPhotoToAlbum(int albumId, IFormFile file, PhotoSpecDto photoSpecification)
    {
        var albumResult = await albumRepository.GetAlbumByIdAsync(albumId);
        if (albumResult.Value == null)
        {
            return new ForbidResult();
        }

        var album = albumResult.Value;
        return await uploadPhotoService.UploadPhoto(album, file, photoSpecification);
    }

    public sealed record CreateAlbumItemRequest(string? Name, string? Description);

}