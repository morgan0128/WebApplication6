using Microsoft.AspNetCore.Mvc;
using WebApplication6.Backend.Models;
using WebApplication6.Backend.Repositories;

namespace WebApplication6.Backend.Controllers;

[ApiController]
[Route("api/Album")]
public sealed class AlbumController(IAlbumRepository repository) : ControllerBase
{
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<Album>>> GetAllAlbums()
    {
        return await repository.GetAllAlbumsAsync();
    }
    
    
    [HttpGet("all-ids")]
    public async Task<ActionResult<IEnumerable<int>>> GetAllAlbumsIds()
    {
        return await repository.GetAllAlbumsIdsAsync();
    }
    
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Album>> GetAlbumById(int id)
    {
        var album = await repository.GetAlbumByIdAsync(id);
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
        return await repository.PostAlbumAsync(album);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAlbumById(int id)
    {
        return await repository.DeleteAlbumByIdAsync(id);
    }

    public sealed record CreateAlbumItemRequest(string? Name, string? Description);

}