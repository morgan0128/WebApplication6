using System.ComponentModel.DataAnnotations;

namespace WebApplication6.Models;

public class Image
{
    [Key]
    public int Id { get; set; }

    public string FileName { get; set; } = "";

    public string ContentType { get; set; } = "";
        
    public long FileSize { get; set; }
    
    public string StoragePath { get; set; } = "";
    
    public string? AltText { get; set; }
}