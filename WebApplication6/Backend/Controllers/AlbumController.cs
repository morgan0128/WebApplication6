using Microsoft.AspNetCore.Http.HttpResults;
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
        var albums = await albumRepository.GetAllAlbumsAsync();
        return Ok(albums);
    }
    
    
    [HttpGet("all-ids")]
    public async Task<ActionResult<IEnumerable<int>>> GetAllAlbumsIds()
    {
        var albumIds = await albumRepository.GetAllAlbumsIdsAsync();
        return Ok(albumIds);
    }
    
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Album>> GetAlbumById(int id)
    {
        var album = await albumRepository.GetAlbumByIdAsync(id);
        if (album == null)
        {
            return NotFound();
        }

        return Ok(album);
    }

    [HttpPost]
    public async Task<ActionResult<int>> PostAlbum(CreateAlbumItemRequest albumRequest)
    {
        var album = new Album
        {
            Name = albumRequest.Name,
            Description = albumRequest.Description
        };

        var id = await albumRepository.SaveAlbumAsync(album);
        if (id is null)
        {
            return Problem();
        }
        
        return id;
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAlbumById(int id)
    {
        var status = await albumRepository.DeleteAlbumByIdAsync(id);
        return status switch
        {
            true => Ok(),
            null => Problem(),
            false => NotFound()
        };
    }

    [HttpPost("{id:int}/upload")]
    public async Task<IActionResult> UploadPhotoToAlbum(int id, [FromForm] CombinedPhotoSpecDto combinedPhotoSpec)
    {
        var file = combinedPhotoSpec.File;
        
        var photoSpec = new PhotoSpecDto(combinedPhotoSpec.Name, combinedPhotoSpec.Description, combinedPhotoSpec.YearContentCreated);
        
        if (file.Length == 0)
        {
            return BadRequest("File upload fail");
        }
        
        var album = await albumRepository.GetAlbumByIdAsync(id);
        if (album == null)
        {
            return new ForbidResult();
        }
        
        var succeeded = await uploadPhotoService.UploadPhoto(album, file, photoSpec);
        if (!succeeded)
        {
            return Problem();
        }

        return Ok();
    }

    public sealed record CreateAlbumItemRequest(string? Name, string? Description);

}