using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AquariumForum.Data;
using AquariumForum.Models;

namespace AquariumForum.Controllers
{
    public class CommentsController : Controller
    {
        private readonly AquariumForumContext _context;

        public CommentsController(AquariumForumContext context)
        {
            _context = context;
        }

        //  Show Create Comment Form
        public IActionResult Create(int discussionId)
        {
            var comment = new Comment { DiscussionId = discussionId };
            return View(comment);
        }

        // ✅ Handle Comment Submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DiscussionId,Content")] Comment comment)
        {
            if (string.IsNullOrWhiteSpace(comment.Content))
            {
                TempData["Error"] = "Comment cannot be empty!";
                return RedirectToAction("GetDiscussion", "Home", new { id = comment.DiscussionId });
            }

            comment.CreateDate = DateTime.Now;
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            // ✅ Redirect back to discussion page after adding comment
            return RedirectToAction("GetDiscussion", "Home", new { id = comment.DiscussionId });
        }
    }
}
