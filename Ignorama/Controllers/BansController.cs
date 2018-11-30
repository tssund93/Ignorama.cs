using Ignorama.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ignorama.Controllers
{
    public class BansController : Controller
    {
        private readonly ForumContext _context;
        private readonly UserManager<User> _userManager;

        public BansController(ForumContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "Moderator")]
        [HttpGet("Bans/New/{postID}")]
        public IActionResult New(int postID)
        {
            var post = _context.Posts.Include(p => p.User).Where(p => p.ID == postID).FirstOrDefault();
            return View(new Ban
            {
                Post = post,
            });
        }

        [HttpPost("Bans/New/{postID}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(int postID, [Bind("Reason,EndTime")] Ban ban)
        {
            ban.Post = _context.Posts.Find(postID);
            ban.Moderator = await _userManager.GetUserAsync(User);

            _context.Add(ban);
            await _context.SaveChangesAsync();

            var users = _context.Posts
                .Where(p => p.IP == ban.Post.IP && p.User != null)
                .Select(p => p.User)
                .ToList();
            foreach (var user in users)
            {
                if (!await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    await _userManager.RemoveFromRoleAsync(user, "Moderator");
                }
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("Bans/View/{postID?}")]
        public IActionResult View(int? postID)
        {
            IQueryable<Ban> bans;
            if (postID != null)
            {
                var post = _context.Posts.Find(postID);
                bans = _context.Bans
                    .Where(b => (b.Post.User != null && post.User != null && b.Post.User.Id == post.User.Id)
                                || b.Post.IP == post.IP)
                    .Include(b => b.Post)
                    .ThenInclude(p => p.User)
                    .Include(b => b.Moderator)
                    .OrderByDescending(b => b.EndTime);
            }
            else
            {
                bans = _context.Bans
                    .Include(b => b.Post)
                    .ThenInclude(p => p.User)
                    .Include(b => b.Moderator)
                    .OrderByDescending(b => b.EndTime);
            }
            return View(bans);
        }
    }
}