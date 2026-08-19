using WebApplication6.Backend.Models;

namespace WebApplication6.Backend.Repositories;

public interface IUntrackedFileRepository
{
    Task PostUntrackedAsync(UntrackedFile untrackedFile);
}