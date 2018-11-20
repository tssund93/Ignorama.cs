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

        [HttpGet]
        public IActionResult GetThreads()
        {
            return new OkObjectResult(
                _context.Threads
                    .Include(thread => thread.Posts)
                    .ThenInclude(post => post.User)
                    .Include(thread => thread.Tag)
                    .ToList());
        }
    }
}