using Ignorama.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ignorama.Controllers
{
    public class ThreadsController : Controller
    {
        private readonly ForumContext _context;
        private readonly UserManager<User> _userManager;

        public ThreadsController(ForumContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult New()
        {
            var newThreadModel = new NewThreadViewModel
            {
                Tags = _context.Tags.OrderBy(t => t.Name)
            };
            return View(newThreadModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(NewThreadViewModel model)
        {
            if (ModelState.IsValid)
            {
                var thread = new Thread
                {
                    Title = model.Title,
                    Stickied = false,
                    Locked = false,
                    Deleted = false,
                    Tag = _context.Tags.Find(model.TagID)
                };

                var user = await _userManager.GetUserAsync(HttpContext.User);
                if (user == null) return View("Error");

                var post = new Post
                {
                    Thread = thread,
                    User = user,
                    Text = model.Text,
                    Time = DateTime.UtcNow,
                    Deleted = false
                };

                _context.Threads.Add(thread);
                _context.Posts.Add(post);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult GetThreads()
        {
            var user = _userManager.GetUserAsync(HttpContext.User);
            var hiddenThreads = _context.HiddenThreads
                .Where(hiddenThread => hiddenThread.User == user.Result)
                .Select(hiddenThread => hiddenThread.Thread)
                .ToList();
            var followedThreads = _context.FollowedThreads
                .Where(followedThread => followedThread.User == user.Result)
                .Select(followedThread => followedThread.Thread)
                .ToList();
            return new OkObjectResult(
                _context.Threads
                    .OrderByDescending(thread => thread.Posts.OrderBy(post => post.Time).FirstOrDefault().Time)
                    .Where(thread => thread.Deleted == false)
                    .Select(thread => new
                    {
                        thread.Title,
                        thread.ID,
                        thread.Locked,
                        thread.Stickied,
                        TagName = thread.Tag.Name,
                        FirstPost = thread.Posts.First(),
                        LastPost = thread.Posts.Last(),
                        PostCount = thread.Posts.Count(),
                        OP = thread.Posts.First().User,
                        Hidden = hiddenThreads.Contains(thread),
                        Following = followedThreads.Contains(thread)
                    }));
        }

        public class ThreadIDModel
        {
            public int ThreadID { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> ToggleHidden([FromBody] ThreadIDModel t)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null) return new BadRequestObjectResult(user);

            var thread = _context.Threads.Find(t.ThreadID);

            if (thread != null)
            {
                var hiddenThreadRows =
                    _context.HiddenThreads.Where(ht => ht.User == user && ht.Thread == thread);
                if (!hiddenThreadRows.Any())
                {
                    var hiddenThread = new HiddenThread
                    {
                        Thread = thread,
                        User = user
                    };

                    await _context.AddAsync(hiddenThread);
                }
                else
                {
                    foreach (HiddenThread row in hiddenThreadRows)
                        _context.Remove(row);
                }
                await _context.SaveChangesAsync();

                return new OkObjectResult(t.ThreadID);
            }
            else return new BadRequestObjectResult(thread);
        }
    }
}