using WebApplication6.Backend.Data;
using WebApplication6.Backend.Models;

namespace WebApplication6.Backend.Repositories;

public class UntrackedFileRepository(ApplicationDbContext context) : IUntrackedFileRepository
{
    public async Task PostUntrackedAsync(UntrackedFile untrackedFile)
    {
        await context.UntrackedFiles.AddAsync(untrackedFile);
    }
}