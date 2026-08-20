using Microsoft.AspNetCore.Mvc;
using WebApplication6.Backend.Models;

namespace WebApplication6.Backend.Repositories;

public interface IImageRepository
{
    Task<ActionResult<IEnumerable<Image>>> GetAllImagesAsync();

    Task<ActionResult<IEnumerable<int>>> GetAllImagesIdsAsync();
    
    Task<ActionResult<Image?>> GetImageByIdAsync(int id);
    
    Task<ActionResult<Image>> SaveImageAsync(Image image);
    
    Task<IActionResult> DeleteImageByIdAsync(int id);
    
}