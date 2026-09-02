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
        // TODO not yet implemented
        return Ok();
    }

    [HttpPatch("{id:int}/modify/layout-preset")]
    public async Task<IActionResult> UpdateLayoutPreset(int id, int layout)
    {
        if (!Enum.IsDefined((PageLayoutPreset)layout)) return Problem($"Failed. {layout} is not a valid PageLayoutPreset.");
        
        var applyChanges = await portfolioRepository.SetPortfolioPageLayoutPresetAsync(id, (PageLayoutPreset)layout);
        return applyChanges ? Ok() : Problem();
    }

    [HttpPatch("{id:int}/modify/nav-order")]
    public async Task<IActionResult> AssignNavOrder(int id, int navOrder)
    {
        var reordered = await portfolioRepository.ReorderPortfolioPageInNavAsync(id, navOrder);

        return reordered ? Ok() : Problem();
    }
    
    

    public sealed record CreatePortfolioForAlbumRequest(int albumId, string Name);

}