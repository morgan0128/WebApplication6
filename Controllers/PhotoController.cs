using Microsoft.AspNetCore.Mvc;
using WebApplication6.Data;

namespace WebApplication6.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class PhotoController(ApplicationDbContext dbContext, IWebHostEnvironment environment) : ControllerBase
{

    // [HttpGet]
    // public async Task<IActionResult> GetPhotos()
    // {
    //     // await EnsureDevelopmentDatabaseCreatedAsync();
    // }
    
    
}