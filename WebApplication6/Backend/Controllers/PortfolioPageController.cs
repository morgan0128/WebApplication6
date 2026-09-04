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

    [HttpGet("published/not-in-nav")]
    public async Task<IEnumerable<PortfolioPage>> GetPublishedNotInNav()
    {
        var pages = await portfolioRepository.GetPublishedNotInNavbar();
        return pages;
    }
    
    [HttpGet("published/in-nav/ordered")]
    public async Task<IEnumerable<PortfolioPage>> GetPublishedAndInNavOrdered()
    {
        var pages = await portfolioRepository.GetPublishedInNavbarOrdered();
        return pages;
    }

    [HttpPatch("publish/{id:int}")]
    public async Task<int?> PublishPortfolioPage(int id, int? navOrder)
    {
        var assignedNavOrder = await portfolioRepository.PublishPortfolioPageAsync(id, navOrder);
        return assignedNavOrder;
    }

    [HttpPatch("unpublish/{id:int}")]
    public async Task<IActionResult> UnpublishPortfolioPage(int id)
    {
        var unpublished = await portfolioRepository.UnpublishPortfolioPageAsync(id);
        return unpublished switch
        {
            false => Problem(),
            true => Ok()
        };
    }

    [HttpPatch("{id:int}/modify")]
    public async Task<IActionResult> ModifyPortfolioPage(int id, IPortfolioPageRepository.UpdatePortfolioPageDto model)
    {
        var modified = await portfolioRepository.UpdatePortfolioPageAsync(id, model);
        return modified switch
        {
            false => Problem(),
            true => Ok()
        };
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePortfolioPage(int id)
    {
        var deleted = await portfolioRepository.DeletePortfolioPageAsync(id);
        return deleted switch
        {
            false => Problem(),
            true => Ok()
        };
    }
    

    public sealed record CreatePortfolioForAlbumRequest(int albumId, string Name);

}