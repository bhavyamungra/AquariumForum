using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

public class ApplicationUser : IdentityUser
{
    [Required]
    public string Name { get; set; } // 🔥 Required Field

    public string Location { get; set; } // Optional

    public string ImageFilename { get; set; } // Optional Profile Picture

    [NotMapped] // 🔥 This prevents storing in the database
    public IFormFile ImageFile { get; set; }
}
