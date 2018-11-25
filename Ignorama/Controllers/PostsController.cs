using Ignorama.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ignorama.Controllers
{
    public class PostsController : Controller
    {
        private readonly ForumContext _context;
        private readonly UserManager<User> _userManager;

        public PostsController(ForumContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost("/Posts/New/{threadID}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(int threadID, IFormCollection collection)
        {
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var roles = user != null
                    ? _userManager.GetRolesAsync(user).Result
                    : new[] { "User" };
                var thread = _context.Threads.Find(threadID);
                if (!thread.Locked || roles.Contains("Moderator"))
                {
                    collection.TryGetValue("Text", out StringValues text);
                    collection.TryGetValue("Anonymous", out StringValues anonymous);
                    collection.TryGetValue("Bump", out StringValues bump);
                    collection.TryGetValue("RevealOP", out StringValues revealOP);

                    var post = new Post
                    {
                        Thread = thread,
                        User = user,
                        IP = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        Text = text,
                        Time = DateTime.UtcNow,
                        Deleted = false,
                        Anonymous = anonymous == "on" ? true : false,
                        Bump = bump == "on" ? true : false,
                        RevealOP = revealOP == "on" ? true : false,
                    };

                    await _context.AddAsync(post);
                    await _context.SaveChangesAsync();

                    return new OkObjectResult(collection);
                }
                else
                {
                    return new JsonResult(new { error = "Cannot post reply: thread locked." });
                }
            }
            catch
            {
                return new BadRequestObjectResult(collection);
            }
        }
    }
}