using System.ComponentModel.DataAnnotations;

namespace WebApplication6.Backend.Models;

// Used to track saved files which are NOT found in database, where represents an instance of violating expected behavior and waste of file storage resources.
public class UntrackedFile
{
    [Key]
    public int Id { get; set; }

    public string? OccurredInClass { get; set; }

    public string? OccurredElaboration { get; set; } // To help identify code responsible; no explicit format. 

    public string? FileLocation { get; set; }

    public string? FileName { get; set; }
}