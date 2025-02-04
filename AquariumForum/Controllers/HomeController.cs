//using System.Diagnostics;
//using AquariumForum.Models;
//using Microsoft.AspNetCore.Mvc;

//namespace AquariumForum.Controllers
//{
//    public class HomeController : Controller
//    {
//        private readonly ILogger<HomeController> _logger;

//        public HomeController(ILogger<HomeController> logger)
//        {
//            _logger = logger;
//        }

//        public IActionResult Index()
//        {
//            return View();
//        }

//        public IActionResult Privacy()
//        {
//            return View();
//        }

//        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//        public IActionResult Error()
//        {
//            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
//        }
//    }
//}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using AquariumForum.Data;
using AquariumForum.Models;

public class HomeController : Controller
{
    private readonly AquariumForumContext _context;

    public HomeController(AquariumForumContext context)
    {
        _context = context;
    }

    // ✅ Home Page - Display all discussions sorted by CreateDate DESC
    public async Task<IActionResult> Index()
    {
        var discussions = await _context.Discussions
            .Include(d => d.Comments) // Include comments to count them
            .OrderByDescending(d => d.CreateDate) // Sort by newest first
            .ToListAsync();

        return View(discussions);
    }

    // ✅ GetDiscussion - Show discussion details
    public async Task<IActionResult> GetDiscussion(int id)
    {
        var discussion = await _context.Discussions
            .Include(d => d.Comments)
            .FirstOrDefaultAsync(d => d.DiscussionId == id);

        if (discussion == null)
        {
            return NotFound();
        }

        return View(discussion);
    }
}
