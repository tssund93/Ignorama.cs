using Ignorama.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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
                EndTime = DateTime.UtcNow,
            });
        }

        [Authorize(Roles = "Moderator")]
        [HttpPost("/Bans/New/{postID}/{reasonID}")]
        public async Task<IActionResult> New(int postID, int reasonID)
        {
            var post = _context.Posts
                .Include(p => p.User)
                .Where(p => p.ID == postID)
                .FirstOrDefault();

            var banReason = _context.BanReasons.Find(reasonID);
            var banHours = banReason.BaseBanHours *
                Math.Pow(2, Util.GetAllBans(post.User, post.IP, _context).Count());

            if (banHours > int.MaxValue)
            {
                banHours = int.MaxValue;
            }

            return await New(postID, new Ban
            {
                Reason = banReason,
                EndTime = DateTime.UtcNow.AddHours(banHours),
            });
        }

        [Authorize(Roles = "Moderator")]
        [HttpPost("/Bans/New/{postID}")]
        public async Task<IActionResult> New(int postID, [Bind("Details,Reason,EndTime")] Ban ban)
        {
            ban.Post = _context.Posts
                .Include(p => p.User)
                .Where(p => p.ID == postID)
                .FirstOrDefault();
            ban.Moderator = await _userManager.GetUserAsync(User);

            if (!String.IsNullOrWhiteSpace(ban.Details) || ban.Reason != null)
            {
                _context.Add(ban);

                var reports = _context.Reports.Where(r => r.Post.ID == postID);

                foreach (var report in reports)
                {
                    _context.Update(report);
                    report.Active = false;
                }

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

            return View(ban);
        }

        [HttpGet("/Bans/View/{postID}")]
        public IActionResult View(int postID)
        {
            var post = _context.Posts
                .Include(p => p.User)
                .Include(p => p.Thread)
                .Where(p => p.ID == postID)
                .FirstOrDefault();

            var bans = _context.Bans
                .Where(b => (b.Post.User != null && post.User != null && b.Post.User.Id == post.User.Id)
                            || b.Post.IP.StartsWith(Util.ShortenIP(post.IP)))
                .Include(b => b.Post)
                .ThenInclude(p => p.User)
                .Include(b => b.Moderator)
                .Include(b => b.Reason)
                .OrderByDescending(b => b.EndTime);

            return View(new BansViewModel
            {
                Bans = bans,
                Post = post,
            });
        }

        public IActionResult GetReasons()
        {
            return new OkObjectResult(
                _context.BanReasons);
        }

        [HttpGet("/Rules")]
        public IActionResult Rules()
        {
            return View(_context.BanReasons);
        }

        [HttpPost("/Reports/New/{postID}/{reasonID}")]
        public async Task<IActionResult> Report(int postID, int reasonID)
        {
            var report = new Report
            {
                Post = _context.Posts
                    .Include(p => p.User)
                    .Where(p => p.ID == postID)
                    .FirstOrDefault(),
                User = await _userManager.GetUserAsync(User),
                IP = Util.GetCurrentIPString(Request),
                Reason = await _context.BanReasons.FindAsync(reasonID),
                Active = true,
            };

            _context.Add(report);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Moderator")]
        [HttpGet("/Reports")]
        public IActionResult Reports()
        {
            return View(_context.Reports
                .Where(r => r.Active)
                .Include(r => r.User)
                .Include(r => r.Post)
                .Include(r => r.Reason));
        }

        [Authorize(Roles = "Moderator")]
        [HttpGet("/Reports/Delete/{reportID}")]
        public async Task<IActionResult> DeleteReport(long reportID)
        {
            var report = await _context.Reports.FindAsync(reportID);
            _context.Update(report);
            report.Active = false;
            await _context.SaveChangesAsync();

            return RedirectToAction("Reports", "Bans");
        }
    }
}