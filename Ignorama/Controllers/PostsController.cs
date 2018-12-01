using Ignorama.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var thread = _context.Threads
                .Include(t => t.Posts)
                .ThenInclude(p => p.User)
                .Where(t => t.ID == threadID)
                .FirstOrDefault();

            var user = await _userManager.GetUserAsync(HttpContext.User);

            var canBump = Util.CanBump(
                thread.Posts.FirstOrDefault().User, thread.Posts.FirstOrDefault().IP, user, Util.GetCurrentIPString(Request));
            var isOP = Util.IsOP(
                thread.Posts.FirstOrDefault().User, thread.Posts.FirstOrDefault().IP, user, Util.GetCurrentIPString(Request));

            if (Util.IsBanned(user, Util.GetCurrentIPString(Request), _context))
            {
                return RedirectToAction("Error", "Home");
            }

            try
            {
                var roles = Util.GetRoles(user, _userManager);
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
                        Bump = bump == "on" && canBump ? true : false,
                        RevealOP = revealOP == "on" && isOP ? true : false,
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

        [Authorize(Roles = "Moderator")]
        [HttpPost("/Posts/Delete/{postID}")]
        public async Task<IActionResult> Delete(int postID)
        {
            var post = await _context.Posts.FindAsync(postID);

            if (post != null)
            {
                _context.Update(post);
                post.Deleted = true;
                await _context.SaveChangesAsync();
                return Ok();
            }

            return new BadRequestResult();
        }

        [Authorize(Roles = "Moderator")]
        [HttpPost("/Posts/Restore/{postID}")]
        public async Task<IActionResult> Restore(int postID)
        {
            var post = await _context.Posts.FindAsync(postID);

            if (post != null)
            {
                _context.Update(post);
                post.Deleted = false;
                await _context.SaveChangesAsync();
                return Ok();
            }

            return new BadRequestResult();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("/Posts/Purge/{postID}")]
        public async Task<IActionResult> Purge(int postID)
        {
            var post = await _context.Posts.FindAsync(postID);

            if (post != null)
            {
                _context.Remove(post);
                await _context.SaveChangesAsync();
                return Ok();
            }

            return new BadRequestResult();
        }
    }
}