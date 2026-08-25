using System.ComponentModel.DataAnnotations;

namespace WebApplication6.Backend.Models;

public class Image
{
    [Key]
    public int Id { get; init; }

    [Required]
    public string FileName { get; init; } = "unnamed";

    [Required]
    public string ContentType { get; init; }
    
    public long? FileSize { get; init; }
    
    [Required]
    public string StorageFileName { get; init; }
    
    [Required]
    public string Url { get; init; }

    [Required]
    public string AltText { get; init; }
    
    [Required]
    public int Width { get; init; }

    [Required]
    public int Height { get; init; }
}