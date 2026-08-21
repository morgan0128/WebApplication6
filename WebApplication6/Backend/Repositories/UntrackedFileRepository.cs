using WebApplication6.Backend.Data;
using WebApplication6.Backend.Models;

namespace WebApplication6.Backend.Repositories;

public class UntrackedFileRepository(ApplicationDbContext context) : IUntrackedFileRepository
{
    public async Task<int?> SaveUntrackedAsync(UntrackedFile untrackedFile)
    {
        context.UntrackedFiles.Add(untrackedFile);

        try
        {
            await context.SaveChangesAsync();
            return untrackedFile.Id;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}