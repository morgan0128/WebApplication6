using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication6.Backend.Data;
using WebApplication6.Backend.Models;

namespace WebApplication6.Backend.Repositories;

public class PortfolioPageRepository(ApplicationDbContext context) : IPortfolioPageRepository
{
    public async Task<IEnumerable<PortfolioPage>> GetAllPublishedAsync()
    {
        var publishedPages = await context.PortfolioPages
            .Where(pp => pp.Published == true)
            .ToListAsync();

        return publishedPages;
    }

    public async Task<PortfolioPage?> GetPortfolioPageByIdAsync(int id)
    {
        var portfolio = await context.PortfolioPages
            .FindAsync(id);

        return portfolio;
    }

    public async Task<PortfolioPage?> GetPortfolioPageByAlbumAsync(int albumId)
    {
        var portfolio = await context.PortfolioPages
            .Where(pp => pp.AlbumId == albumId)
            .FirstAsync();
        
        return portfolio;
    }

    public async Task<int?> SavePortfolioPageAsync(PortfolioPage portfolioPage)
    {
        context.PortfolioPages.Add(portfolioPage);
        try
        {
            await context.SaveChangesAsync();
            return portfolioPage.Id;
        }
        catch (Exception)
        {
            return null;
        }
        
    }

    public async Task<bool> SetPortfolioPageLayoutPresetAsync(int ppId, PageLayoutPreset layout)
    {
        var pp = await context.PortfolioPages.FindAsync(ppId);
        if (pp == null) return false;

        pp.LayoutPreset = layout;

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ReorderPortfolioPageInNavAsync(int ppId, int newNavOrder)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> PublishPortfolioPageAsync(int ppId, int? navOrder)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdatePortfolioPageAsync(int ppId, IPortfolioPageRepository.UpdatePortfolioPageDto model)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeletePortfolioPageAsync(int id)
    {
        throw new NotImplementedException();
    }
}