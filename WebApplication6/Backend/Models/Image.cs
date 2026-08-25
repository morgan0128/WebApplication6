using System.ComponentModel.DataAnnotations;

namespace WebApplication6.Backend.Models;

public class Image
{
    [Key]
    public int Id { get; init; }

    [Required]
    [MaxLength(100)]
    public required string FileName { get; init; } = "unnamed";

    [Required]
    [MaxLength(50)]
    public required string ContentType { get; init; }
    
    public long? FileSize { get; init; }
    
    [Required]
    [MaxLength(200)]
    public required string StorageFileName { get; init; }
    
    [Required]
    [MaxLength(500)]
    public required string Url { get; init; }

    [Required]
    [MaxLength(100)]
    public required string AltText { get; init; }
    
    [Required]
    public required int Width { get; init; }

    [Required]
    public required int Height { get; init; }
}