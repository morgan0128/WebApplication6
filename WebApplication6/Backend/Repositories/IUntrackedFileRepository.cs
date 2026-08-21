using WebApplication6.Backend.Models;

namespace WebApplication6.Backend.Repositories;

public interface IUntrackedFileRepository
{
    /// <returns>The Id of the saved untrackedFile on success, or null on exception thrown or failure.</returns>
    Task<int?> SaveUntrackedAsync(UntrackedFile untrackedFile);
}