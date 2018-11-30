using Ignorama.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ignorama.Controllers
{
    public class HistoryController : Controller
    {
        private readonly ForumContext _context;
        private readonly UserManager<User> _userManager;

        public HistoryController(ForumContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "Moderator")]
        [HttpGet("/History/ByUser/{userID}")]
        public IActionResult ByUser(long userID)
        {
            return View("ByUser", _context.Users.Find(userID).UserName);
        }

        [Authorize(Roles = "Moderator")]
        [HttpGet("/History/ByIP/{ip}")]
        public IActionResult ByIP(string ip)
        {
            return View("ByIP", ip);
        }

        [Authorize(Roles = "Moderator")]
        [HttpGet("/History/GetUserPosts/{userID}")]
        public IActionResult GetUserPosts(int userID)
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            var roles = Util.GetRoles(user, _userManager);

            return new OkObjectResult(
                _context.Posts
                    .Where(post => post.User.Id == userID)
                    .OrderByDescending(post => post.Time)
                    .Select(post => new
                    {
                        post.ID,
                        Highlighted = false,
                        post.Anonymous,
                        User = post.User,
                        IP = post.IP,
                        post.RevealOP,
                        post.Bump,
                        post.Time,
                        post.Text,
                        Locked = true,
                        Seen = false,
                        Roles = roles,
                        AllBans = post.Bans,
                        UserIPBans = Util.GetCurrentBans(post.User, post.IP, _context),
                    }));
        }

        [Authorize(Roles = "Moderator")]
        [HttpGet("/History/GetIPPosts/{userIP}")]
        public IActionResult GetIPPosts(string userIP)
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            var roles = Util.GetRoles(user, _userManager);

            return new OkObjectResult(
                _context.Posts
                    .Where(post => post.IP == userIP)
                    .OrderByDescending(post => post.Time)
                    .Select(post => new
                    {
                        post.ID,
                        Highlighted = false,
                        post.Anonymous,
                        User = post.User,
                        IP = post.IP,
                        post.RevealOP,
                        post.Bump,
                        post.Time,
                        post.Text,
                        Locked = true,
                        Seen = false,
                        Roles = roles,
                        AllBans = post.Bans,
                        UserIPBans = Util.GetCurrentBans(post.User, post.IP, _context),
                    }));
        }
    }
}