using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using AquariumForum.Data;
using AquariumForum.Models;

public class HomeController : Controller
{
    private readonly AquariumForumContext _context;

    // Constructor to initialize the database context
    public HomeController(AquariumForumContext context)
    {
        _context = context;
    }

    // Home Page - Display all discussions sorted by CreateDate DESC
    public async Task<IActionResult> Index()
    {
        // Retrieve discussions from the database, including their comments
        // Ordered by the most recent discussion first
        var discussions = await _context.Discussions
            .Include(d => d.Comments) // Include comments to count them
            .OrderByDescending(d => d.CreateDate) // Sort by newest first
            .ToListAsync();

        return View(discussions);
    }

    // GetDiscussion - Show discussion details
    public async Task<IActionResult> GetDiscussion(int id)
    {
        // Find the discussion by ID, including its comments
        var discussion = await _context.Discussions
            .Include(d => d.Comments)
            .FirstOrDefaultAsync(d => d.DiscussionId == id); // Find the discussion matching the given ID

        // If no discussion is found, return a 404 
        if (discussion == null)
        {
            return NotFound();
        }

        return View(discussion);
    }
}
