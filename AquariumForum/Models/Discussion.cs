using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace AquariumForum.Models
{
    public class Discussion
    {
        //Primary key for the Discussion entity.
        public int DiscussionId { get; set; }

        //Title of the discussion(Required).
        [Required]
        public string Title { get; set; } = string.Empty;

        //main content/body of the discussion(Required).
        [Required]
        public string Content { get; set; } = string.Empty;

        // The date and time when the discussion was created.
        // Defaults to the current UTC time.
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        /// Stores the filename of the uploaded image (if any).
        /// Only the filename is stored in the database, not the full path.
        public string? ImageFilename { get; set; } // Store only the filename

        /// List of comments associated with this discussion.
        /// This creates a one-to-many relationship with the Comment entity.
        public List<Comment> Comments { get; set; } = new List<Comment>();

        [NotMapped] // Prevents storing in the database
        public IFormFile? ImageFile { get; set; } // Handles file upload
    }
}
