using System.ComponentModel.DataAnnotations;

namespace WebApplication6.Backend.Models;

public class Album
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string? Name { get; set; }
    
    [StringLength(400)]
    public string? Description { get; set; }

    public ICollection<Photo> Photos { get; set; } = [];
    public ICollection<AlbumPhoto> AlbumPhotos { get; set; } = [];
    
}