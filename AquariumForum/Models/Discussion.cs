using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace AquariumForum.Models
{
    public class Discussion
    {
        public int DiscussionId { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        public string? ImageFilename { get; set; } // Store only the filename

        public List<Comment> Comments { get; set; } = new List<Comment>();

        [NotMapped] // Prevents storing in the database
        public IFormFile? ImageFile { get; set; } // Handles file upload
    }
}
