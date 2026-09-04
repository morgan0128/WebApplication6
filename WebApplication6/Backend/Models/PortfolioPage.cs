using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication6.Backend.Models;

public class PortfolioPage
{
    [Key]
    public int Id { get; set; }

    [MaxLength(20)]
    public string NavTitle { get; set; } = "";
    
    [MaxLength(80)]
    public string Title { get; set; } = "";

    public bool Published { get; set; } = false;

    [Range(-1, 4)]
    public int NavbarOrder { get; set; } = -1;
        
    public int AlbumId { get; set; }
    
    [ForeignKey("AlbumId")]
    [Required]
    public virtual Album Album { get; set; }

    public PageLayoutPreset LayoutPreset { get; set; } = PageLayoutPreset.Default;

}

// public enum PageLayoutPreset
// {
//     Default = 1,
//     Cozy = 2,
//     Spooky = 3
// }

public enum PageLayoutPreset
{
    Default = 0,
    Cozy = 1,
    Spooky = 2
}