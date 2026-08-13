using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication6.Models;

public class Photo
{
    [Key]
    public int Id { get; set; }
 
    [ForeignKey("Image")]
    public int ImageId { get; set; }
    
    [MaxLength(100)]
    public string? Title { get; set; }
    
    [MaxLength(500)]
    public string? Description { get; set; }
    
    public DateTime? CreatedAt { get; set; }
    
    // public string? Url { get; set; }
}