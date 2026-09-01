using Microsoft.AspNetCore.Mvc;
using WebApplication6.Backend.Models;

namespace WebApplication6.Backend.Repositories;

public interface IPortfolioPageRepository
{
    Task<IEnumerable<PortfolioPage>> GetAllPublishedAsync();
    
    Task<PortfolioPage?> GetPortfolioPageByIdAsync(int id);
    
    Task<PortfolioPage?> GetPortfolioPageByAlbumAsync(int albumId);
    
    /// <returns>Id of the saved portfolio page on success, or null on exception thrown or failure.</returns>
    Task<int?> SavePortfolioPageAsync(PortfolioPage portfolioPage);

    Task<bool> SetPortfolioPageLayoutPresetAsync(int ppId, PageLayoutPreset layout);
    
    Task<bool> ReorderPortfolioPageInNavAsync(int ppId, int newNavOrder);

    Task<bool> PublishPortfolioPageAsync(int ppId, int? navOrder);

    Task<bool> UpdatePortfolioPageAsync(int ppId, UpdatePortfolioPageDto model);
    
    /// <returns>true on success, or false on not found</returns>
    Task<bool> DeletePortfolioPageAsync(int id);

    public sealed record UpdatePortfolioPageDto(string? NavTitle, string? Title);
}