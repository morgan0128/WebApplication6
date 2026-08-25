using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication6.Backend.Data;
using WebApplication6.Backend.Models;
using WebApplication6.Backend.Repositories;
using WebApplication6.Backend.Services;
using Image = WebApplication6.Backend.Models.Image;

namespace WebApplication6.Backend.Controllers;

[ApiController]
[Route("api/Image")]
public sealed class ImageController(IImageRepository imageRepository) : ControllerBase
{
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<Image>>> GetAllImages()
    {
        var images = await imageRepository.GetAllImagesAsync();
        return Ok(images);
    }
    
    
    [HttpGet("all-ids")]
    public async Task<ActionResult<IEnumerable<int>>> GetAllImagesIds()
    {
        var imageIds = await imageRepository.GetAllImagesIdsAsync();
        return Ok(imageIds);
    }
    
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Image>> GetImageById(int id)
    {
        var image = await imageRepository.GetImageByIdAsync(id);
        if (image == null)
        {
            return NotFound();
        }

        return Ok(image);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteImageById(int id)
    {
        var status = await imageRepository.DeleteImageByIdAsync(id);
        return status switch
        {
            true => Ok(),
            false => Problem()
        };
    }
    
    
    private static ImageItemDto ImageToDto(Image image)
    {
        return new ImageItemDto(image.Id, image.FileName, image.ContentType, image.FileSize, image.StorageFileName, image.AltText, image.Width, image.Height);
    }
    
    public sealed record ImageItemDto(int Id, string FileName, string ContentType, long? FileSize, string StorageFileName, string AltText, int Width, int Height);

}