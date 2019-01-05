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
using System.Net.Http;
using System.Threading.Tasks;
using TylerRhodes.Akismet;

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

            collection.TryGetValue("Text", out StringValues text);

            if (Util.IsBanned(user, Util.GetCurrentIPString(Request), _context))
            {
                return RedirectToAction("Error", "Home");
            }
            var client = new HttpClient();
            var akismet = new AkismetClient("https://ignorama.azurewebsites.net/", Util.AkismetKey, client);

            var akismetComment = new AkismetComment()
            {
                Blog = "https://ignorama.azurewebsites.net/",
                UserIp = Util.GetCurrentIPString(Request),
                UserAgent = Request.Headers["User-Agent"].ToString(),
                Referrer = Request.Headers["Referer"].ToString(),
                Permalink = "https://ignorama.azurewebsites.net/Threads/View/" + threadID.ToString(),
                CommentType = "reply",
                Author = user?.UserName,
                AuthorEmail = null,
                AuthorUrl = null,
                Content = text,
            };

            var isSpam = await akismet.IsCommentSpam(akismetComment);

            if (!isSpam)
            {
                try
                {
                    var roles = Util.GetRoles(user, _userManager);
                    if (!thread.Locked || roles.Contains("Moderator"))
                    {
                        collection.TryGetValue("Anonymous", out StringValues anonymous);
                        collection.TryGetValue("Bump", out StringValues bump);
                        collection.TryGetValue("RevealOP", out StringValues revealOP);

                        var post = new Post
                        {
                            Thread = thread,
                            User = user,
                            IP = Util.GetCurrentIPString(Request),
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
            else
            {
                return new JsonResult(new
                {
                    error = "Cannot post reply: Spam detected.  If this was in error, please contact the administrator."
                });
            }
        }

        [HttpGet("/Posts/View/{postID}")]
        public IActionResult View(int postID)
        {
            if (_context.Posts.Find(postID) == null)
            {
                return new NotFoundResult();
            }

            var threadID = _context.Posts
                .Where(p => p.ID == postID)
                .Select(p => p.Thread.ID)
                .FirstOrDefault();
            return RedirectPermanent("~/Threads/View/" + threadID + "?post=" + postID);
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