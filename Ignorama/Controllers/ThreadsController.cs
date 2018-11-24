using Ignorama.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

                var post = new Post
                {
                    Thread = thread,
                    User = user,
                    Text = model.Text,
                    Time = DateTime.UtcNow,
                    Deleted = false,
                    Bump = true,
                    RevealOP = true,
                    Anonymous = false,
                    IP = Request.HttpContext.Connection.RemoteIpAddress.ToString()
                };

                _context.Threads.Add(thread);
                _context.Posts.Add(post);
                await _context.SaveChangesAsync();

                return RedirectToAction("View", "Threads", new { threadID = thread.ID });
            }

            return RedirectToAction();
        }

        public IActionResult GetThreads()
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            var roles = user != null
                ? _userManager.GetRolesAsync(user).Result
                : new[] { "User" };

            var hiddenThreads = user != null
                ? _context.HiddenThreads
                    .Where(hiddenThread => hiddenThread.User == user)
                    .Select(hiddenThread => hiddenThread.Thread)
                    .ToList()
                : _context.HiddenThreads
                    .Where(hiddenThread => hiddenThread.IP == Request.HttpContext.Connection.RemoteIpAddress.ToString())
                    .Select(hiddenThread => hiddenThread.Thread)
                    .ToList();

            var followedThreads = user != null
                ? _context.FollowedThreads
                    .Where(followedThread => followedThread.User == user)
                    .Select(followedThread => followedThread)
                    .ToList()
                : _context.FollowedThreads
                    .Where(followedThread => followedThread.IP == Request.HttpContext.Connection.RemoteIpAddress.ToString())
                    .Select(followedThread => followedThread)
                    .ToList();

            var threads = _context.Threads
                    .OrderByDescending(thread => thread.Posts
                                        .Where(post => post.Bump == true)
                                        .OrderByDescending(post => post.Time)
                                        .FirstOrDefault().Time)
                    .Where(thread => thread.Deleted == false)
                    .Select(thread => new
                    {
                        thread.Title,
                        thread.ID,
                        thread.Locked,
                        thread.Stickied,
                        thread.Tag,
                        FirstPost = thread.Posts.First(),
                        LastPost = thread.Posts.Last(),
                        PostCount = thread.Posts.Count(),
                        OP = thread.Posts.First().User,
                        Hidden = hiddenThreads.Contains(thread),
                        Following = followedThreads.Select(ft => ft.Thread).Contains(thread),
                        LastSeenPostID = followedThreads.Where(ft => ft.Thread.ID == thread.ID) != null
                            ? followedThreads.Where(ft => ft.Thread.ID == thread.ID)
                                             .Select(ft => ft.LastSeenPost.ID)
                                             .FirstOrDefault()
                            : 0,
                        UserRoles = roles,
                    });

            return new OkObjectResult(threads);
        }

        public class ThreadIDModel
        {
            public int ThreadID { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> ToggleHidden([FromBody] ThreadIDModel t)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var thread = _context.Threads.Find(t.ThreadID);

            if (thread != null)
            {
                var hiddenThreadRows = user != null
                    ? _context.HiddenThreads.Where(ht => ht.User == user && ht.Thread == thread)
                    : _context.HiddenThreads.Where(
                        ht => ht.IP == Request.HttpContext.Connection.RemoteIpAddress.ToString() && ht.Thread == thread);

                if (!hiddenThreadRows.Any())
                {
                    var hiddenThread = new HiddenThread
                    {
                        Thread = thread,
                        User = user,
                        IP = user == null
                            ? Request.HttpContext.Connection.RemoteIpAddress.ToString()
                            : null
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

        public class ThreadIDLastSeenPostModel
        {
            public int ThreadID { get; set; }
            public int LastSeenPostID { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> Follow([FromBody] ThreadIDLastSeenPostModel t)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var thread = _context.Threads.Find(t.ThreadID);
            var lastSeenPost = _context.Posts.Find(t.LastSeenPostID);

            if (thread != null)
            {
                var followedThreadRows = (user != null
                    ? _context.FollowedThreads.Where(ft => ft.User == user && ft.Thread == thread)
                    : _context.FollowedThreads.Where(
                        ft => ft.IP == Request.HttpContext.Connection.RemoteIpAddress.ToString() && ft.Thread == thread))
                    .Include(ft => ft.LastSeenPost);

                if (!followedThreadRows.Any())
                {
                    var followedThread = new FollowedThread
                    {
                        Thread = thread,
                        User = user,
                        IP = user == null
                            ? Request.HttpContext.Connection.RemoteIpAddress.ToString()
                            : null,
                        LastSeenPost = lastSeenPost
                    };

                    await _context.AddAsync(followedThread);
                }
                else
                {
                    foreach (FollowedThread row in followedThreadRows)
                    {
                        if (row.LastSeenPost.ID < lastSeenPost.ID)
                        {
                            _context.Update(row);
                            row.LastSeenPost = lastSeenPost;
                        }
                    }
                }
                await _context.SaveChangesAsync();

                return new OkObjectResult(t.ThreadID);
            }
            else return new BadRequestObjectResult(thread);
        }

        [HttpPost]
        public async Task<IActionResult> Unfollow([FromBody] ThreadIDModel t)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var thread = _context.Threads.Find(t.ThreadID);

            if (thread != null)
            {
                var followedThreadRows = user != null
                    ? _context.FollowedThreads.Where(ft => ft.User == user && ft.Thread == thread)
                    : _context.FollowedThreads.Where(
                        ft => ft.IP == Request.HttpContext.Connection.RemoteIpAddress.ToString() && ft.Thread == thread);

                if (followedThreadRows.Any())
                {
                    foreach (FollowedThread row in followedThreadRows)
                        _context.Remove(row);
                };

                await _context.SaveChangesAsync();

                return new OkObjectResult(t.ThreadID);
            }
            else return new BadRequestObjectResult(thread);
        }

        [HttpGet("/Threads/View/{threadID}")]
        public IActionResult View(int threadID)
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            var followedThreadRows = (user != null
                    ? _context.FollowedThreads.Where(ft => ft.User == user && ft.Thread.ID == threadID)
                    : _context.FollowedThreads.Where(
                        ft => ft.IP == Request.HttpContext.Connection.RemoteIpAddress.ToString() && ft.Thread.ID == threadID))
                    .Include(ft => ft.LastSeenPost);

            var thread = _context.Threads
                .Where(t => t.ID == threadID)
                .Select(t => new ThreadViewModel
                {
                    Title = t.Title,
                    IsOP = t.Posts.FirstOrDefault().User.UserName == _userManager.GetUserName(User)
                        || t.Posts.FirstOrDefault().IP == Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    LastSeenPostID = followedThreadRows.Any() ? followedThreadRows.FirstOrDefault().LastSeenPost.ID : 0,
                })
                .FirstOrDefault();

            return View(thread);
        }

        [HttpGet("/Threads/GetPosts/{threadID}")]
        public IActionResult GetPosts(int threadID)
        {
            return new OkObjectResult(
                _context.Posts
                    .Where(post => post.Thread.ID == threadID)
                    .Include(post => post.User)
                    .OrderBy(post => post.Time));
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost("/Threads/ToggleStickied/{threadID}")]
        public async Task<IActionResult> ToggleStickied(int threadID)
        {
            var thread = await _context.Threads.FindAsync(threadID);

            if (thread != null)
            {
                _context.Update(thread);
                thread.Stickied = !thread.Stickied;
                await _context.SaveChangesAsync();
                return Ok();
            }

            return new BadRequestResult();
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost("/Threads/ToggleLocked/{threadID}")]
        public async Task<IActionResult> ToggleLocked(int threadID)
        {
            var thread = await _context.Threads.FindAsync(threadID);

            if (thread != null)
            {
                _context.Update(thread);
                thread.Locked = !thread.Locked;
                await _context.SaveChangesAsync();
                return Ok();
            }

            return new BadRequestResult();
        }
    }
}