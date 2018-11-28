﻿using Ignorama.Models;
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
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            var selectedTags = Util.GetSelectedTags(user, _context, Request);

            if (!selectedTags.Any())
            {
                selectedTags = Util.GetTags(_context)
                    .Where(tag => Util.GetRoles(user, _userManager).Contains(tag.WriteRole.Name));
            }

            var newThreadModel = new NewThreadViewModel
            {
                Tags = Util.GetTags(_context)
                    .Where(tag => selectedTags.Contains(tag))
                    .OrderBy(t => t.Name)
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
                    Tag = _context.Tags
                        .Include(tag => tag.WriteRole)
                        .Where(tag => tag.ID == model.TagID)
                        .FirstOrDefault()
                };

                var user = await _userManager.GetUserAsync(HttpContext.User);

                if (!Util.GetRoles(user, _userManager).Contains(thread.Tag.WriteRole.Name))
                {
                    return RedirectToAction("Error", "Home");
                }

                var post = new Post
                {
                    Thread = thread,
                    User = user,
                    Text = model.Text,
                    Time = DateTime.UtcNow,
                    Deleted = false,
                    Bump = true,
                    RevealOP = true,
                    Anonymous = model.Anonymous,
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
            var roles = Util.GetRoles(user, _userManager);

            var hiddenThreads = Util.GetHiddenThreads(user, _context, Request).ToList();
            var followedThreads = Util.GetFollowedThreads(user, _context, Request).ToList();

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
                        OP = thread.Posts.First().Anonymous ? null : thread.Posts.First().User,
                        Hidden = hiddenThreads.Select(ht => ht.Thread).Contains(thread),
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
                var hiddenThreadRows = Util.GetHiddenThreadMatches(user, thread, _context, Request);

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
            var roles = Util.GetRoles(user, _userManager);
            var thread = _context.Threads.Find(t.ThreadID);
            var lastSeenPost = _context.Posts.Find(t.LastSeenPostID);

            if (thread != null)
            {
                var followedThreadRows = Util.GetFollowedThreadMatches(user, thread, _context, Request);

                if (!followedThreadRows.Any())
                {
                    var followedThread = new FollowedThread
                    {
                        Thread = thread,
                        User = user,
                        IP = user == null
                            ? Request.HttpContext.Connection.RemoteIpAddress.ToString()
                            : null,
                        LastSeenPost = lastSeenPost,
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
                var followedThreadRows = Util.GetFollowedThreadMatches(user, thread, _context, Request);

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
            var roles = Util.GetRoles(user, _userManager);
            var followedThreadRows =
                Util.GetFollowedThreadMatches(user, _context.Threads.Find(threadID), _context, Request);

            var threadView = _context.Threads
                .Where(t => t.ID == threadID)
                .Select(t => new ThreadViewModel
                {
                    Title = t.Title,
                    IsOP = t.Posts.FirstOrDefault().User.UserName == _userManager.GetUserName(User)
                        || (t.Posts.FirstOrDefault().User == null
                            && t.Posts.FirstOrDefault().IP == Request.HttpContext.Connection.RemoteIpAddress.ToString()),
                    CanBump = !(t.Posts.FirstOrDefault().User.UserName == _userManager.GetUserName(User)
                        || t.Posts.FirstOrDefault().IP == Request.HttpContext.Connection.RemoteIpAddress.ToString()),
                    LastSeenPostID = followedThreadRows.Any() ? followedThreadRows.FirstOrDefault().LastSeenPost.ID : 0,
                    Locked = t.Locked,
                    User = user,
                    Roles = roles,
                })
                .FirstOrDefault();

            return View(threadView);
        }

        [HttpGet("/Threads/GetPosts/{threadID}")]
        public IActionResult GetPosts(int threadID)
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            var roles = Util.GetRoles(user, _userManager);

            var lastSeenPostID =
                Util.GetLastSeenPost(user, _context.Threads.Find(threadID), _context, Request)?.ID ?? 0;

            return new OkObjectResult(
                _context.Posts
                    .Where(post => post.Thread.ID == threadID)
                    .OrderBy(post => post.Time)
                    .Select(post => new
                    {
                        post.ID,
                        Highlighted = false,
                        post.Anonymous,
                        User = post.Anonymous && !roles.Contains("Moderator") ? null : post.User,
                        IP = roles.Contains("Moderator") ? post.IP : null,
                        post.RevealOP,
                        post.Bump,
                        post.Time,
                        post.Text,
                        Locked = post.Thread.Locked && !roles.Contains("Moderator"),
                        Seen = post.ID <= lastSeenPostID,
                        Roles = roles
                    }));
        }

        [Authorize(Roles = "Moderator")]
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

        [Authorize(Roles = "Moderator")]
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