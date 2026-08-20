using System.ComponentModel.DataAnnotations;

namespace WebApplication6.Backend.Models;

public class Album
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string? Name { get; set; }
    
    public string? Description { get; set; }
    
    public ICollection<Photo> Photos { get; set; } = new List<Photo>();
    
    
}