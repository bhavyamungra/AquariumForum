using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using AquariumForum.Data;
using AquariumForum.Models;

namespace AquariumForum.Controllers
{
    public class DiscussionsController : Controller
    {
        private readonly AquariumForumContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DiscussionsController(AquariumForumContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Discussions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Discussions.ToListAsync());
        }

        // GET: Discussions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discussion = await _context.Discussions
                .Include(d => d.Comments) // Include comments in details view
                .FirstOrDefaultAsync(m => m.DiscussionId == id);

            if (discussion == null)
            {
                return NotFound();
            }

            return View(discussion);
        }

        // GET: Discussions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Discussions/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Title,Content,ImageFile")] Discussion discussion)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        discussion.CreateDate = DateTime.Now;

        //        // ✅ Handle Image Upload
        //        if (discussion.ImageFile != null)
        //        {
        //            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
        //            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(discussion.ImageFile.FileName);
        //            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

        //            using (var fileStream = new FileStream(filePath, FileMode.Create))
        //            {
        //                await discussion.ImageFile.CopyToAsync(fileStream);
        //            }

        //            discussion.ImageFilename = uniqueFileName; // ✅ Store filename in the database
        //        }

        //        _context.Add(discussion);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }

        //    return View(discussion);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content,ImageFile")] Discussion discussion)
        {
            if (ModelState.IsValid)
            {
                discussion.CreateDate = DateTime.Now;

                // ✅ Handle Image Upload
                if (discussion.ImageFile != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(discussion.ImageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await discussion.ImageFile.CopyToAsync(fileStream);
                    }

                    discussion.ImageFilename = uniqueFileName; // ✅ Store filename in the database
                }

                _context.Add(discussion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(discussion);
        }


        // GET: Discussions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discussion = await _context.Discussions.FindAsync(id);
            if (discussion == null)
            {
                return NotFound();
            }

            return View(discussion);
        }

        // POST: Discussions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DiscussionId,Title,Content,ImageFilename,ImageFile,CreateDate")] Discussion discussion)
        {
            if (id != discussion.DiscussionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingDiscussion = await _context.Discussions.AsNoTracking().FirstOrDefaultAsync(d => d.DiscussionId == id);

                    if (discussion.ImageFile != null)
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(discussion.ImageFile.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await discussion.ImageFile.CopyToAsync(fileStream);
                        }

                        // ✅ Delete Old Image if Exists
                        if (!string.IsNullOrEmpty(existingDiscussion?.ImageFilename))
                        {
                            string oldFilePath = Path.Combine(uploadsFolder, existingDiscussion.ImageFilename);
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        discussion.ImageFilename = uniqueFileName;
                    }
                    else
                    {
                        // ✅ Keep existing image if no new image is uploaded
                        discussion.ImageFilename = existingDiscussion?.ImageFilename;
                    }

                    _context.Update(discussion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiscussionExists(discussion.DiscussionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(discussion);
        }

        // GET: Discussions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discussion = await _context.Discussions
                .FirstOrDefaultAsync(m => m.DiscussionId == id);

            if (discussion == null)
            {
                return NotFound();
            }

            return View(discussion);
        }

        // POST: Discussions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var discussion = await _context.Discussions.FindAsync(id);
            if (discussion != null)
            {
                // ✅ Delete Image from Server
                if (!string.IsNullOrEmpty(discussion.ImageFilename))
                {
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", discussion.ImageFilename);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                _context.Discussions.Remove(discussion);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool DiscussionExists(int id)
        {
            return _context.Discussions.Any(e => e.DiscussionId == id);
        }
    }
}

