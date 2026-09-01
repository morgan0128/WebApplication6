using Microsoft.AspNetCore.Mvc;
using WebApplication6.Backend.Models;
using WebApplication6.Backend.Repositories;

namespace WebApplication6.Backend.Controllers;

[ApiController]
[Route("api/Portfolio")]
public sealed class PortfolioPageController(IPortfolioPageRepository portfolioRepository) : ControllerBase
{

    [HttpGet("published")]
    public async Task<IEnumerable<PortfolioPage>> GetAllPublishedPortfolioPages()
    {
        var pps = await portfolioRepository.GetAllPublishedAsync();
        return pps;
    }

    [HttpGet("{id:int}")]
    public async Task<PortfolioPage?> GetPortfolioPageById(int id)
    {
        var pp = await portfolioRepository.GetPortfolioPageByIdAsync(id);
        return pp;
    }
    
    [HttpGet("by-album/{albumId:int}")]
    public async Task<PortfolioPage?> GetPortfolioPageByAlbumId(int albumId)
    {
        var pp = await portfolioRepository.GetPortfolioPageByAlbumAsync(albumId);
        return pp;
    }
    
    [HttpPost]
    public async Task<IActionResult> PostPortfolioPage(CreatePortfolioForAlbumRequest albumRequest)
    {
        var pp = new PortfolioPage();
        pp.AlbumId = albumRequest.albumId;
        pp.Title = albumRequest.Name[..80].TrimEnd();
        pp.NavTitle = albumRequest.Name[..20].TrimEnd();
        
        var ppId = await portfolioRepository.SavePortfolioPageAsync(pp);
        if (ppId == null) return Problem();
        
        return Ok();
    }

    [HttpGet("preview")]
    public async Task<IActionResult> PreviewLayout(PageLayoutPreset layout)
    {
        return Ok();
    }

    public sealed record CreatePortfolioForAlbumRequest(int albumId, string Name);

}