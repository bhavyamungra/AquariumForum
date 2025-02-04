using System;

namespace AquariumForum.Models
{
    public class Comment
    {
        public int CommentId { get; set; } // Primary Key
        public string? Content { get; set; } // Content of the comment
        public DateTime CreateDate { get; set; } // Date the comment was created

        // Foreign key to Discussion
        public int DiscussionId { get; set; }

        // Navigation Property: Each Comment belongs to one Discussion
        public virtual Discussion Discussion { get; set; } = null!;

    }
}

