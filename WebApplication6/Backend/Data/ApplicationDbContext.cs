using Microsoft.EntityFrameworkCore;
using WebApplication6.Backend.Models;

namespace WebApplication6.Backend.Data;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
    
    public DbSet<Image> Images => Set<Image>();
    
    public DbSet<Photo> Photos => Set<Photo>();
    
    public DbSet<Album> Albums => Set<Album>();

    public DbSet<UntrackedFile> UntrackedFiles => Set<UntrackedFile>();
}
