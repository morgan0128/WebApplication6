using Microsoft.EntityFrameworkCore;
using WebApplication6.Models;

namespace WebApplication6.Data;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
    
    public DbSet<Image> Images => Set<Image>();
    
    public DbSet<Photo> Photos => Set<Photo>();

    public DbSet<UntrackedFile> UntrackedFiles => Set<UntrackedFile>();

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<TodoItem>(entity =>
    //     {
    //         entity.ToTable("todo_items");
    //         entity.HasKey(todo => todo.Id);
    //
    //         entity.Property(todo => todo.Id)
    //             .HasColumnName("id");
    //
    //         entity.Property(todo => todo.Title)
    //             .HasColumnName("title")
    //             .HasMaxLength(200)
    //             .IsRequired();
    //
    //         entity.Property(todo => todo.IsComplete)
    //             .HasColumnName("is_complete");
    //
    //         entity.Property(todo => todo.CreatedAt)
    //             .HasColumnName("created_at")
    //             .HasDefaultValueSql("CURRENT_TIMESTAMP");
    //     });

    // modelBuilder.Entity<Image>(entity =>
    // {
    //     entity.ToTable("images");
    //     entity.HasKey(image => image.Id);
    // });
    // }
}
