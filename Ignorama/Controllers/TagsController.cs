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
    public class TagsController : Controller
    {
        private readonly ForumContext _context;
        private readonly UserManager<User> _userManager;

        public TagsController(ForumContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult GetTags()
        {
            return new OkObjectResult(
                _context.Tags
                    .OrderBy(tag => tag.Name));
        }

        public async Task<IActionResult> GetSelectedTags()
        {
            var user = await _userManager.GetUserAsync(User);
            return new OkObjectResult(
                user != null
                    ? _context.SelectedTags
                        .Where(tag => tag.User == user)
                        .Select(tag => tag.Tag.ID)
                        .ToList()
                    : _context.SelectedTags
                        .Where(tag => tag.IP == Request.HttpContext.Connection.RemoteIpAddress.ToString())
                        .Select(tag => tag.Tag.ID)
                        .ToList());
        }

        public class TagIDModel
        {
            public int TagID { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> ToggleSelectedTag([FromBody] TagIDModel t)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var tag = _context.Tags.Find(t.TagID);

            if (tag != null)
            {
                var selectedTagRows = user != null
                    ? _context.SelectedTags.Where(st => st.User == user && st.Tag == tag)
                    : _context.SelectedTags.Where(
                        st => st.IP == Request.HttpContext.Connection.RemoteIpAddress.ToString() && st.Tag == tag);

                if (!selectedTagRows.Any())
                {
                    var selectedTag = new SelectedTag
                    {
                        Tag = tag,
                        User = user,
                        IP = user == null
                            ? Request.HttpContext.Connection.RemoteIpAddress.ToString()
                            : null
                    };

                    await _context.AddAsync(selectedTag);
                }
                else
                {
                    foreach (SelectedTag row in selectedTagRows)
                        _context.Remove(row);
                }
                await _context.SaveChangesAsync();

                return new OkObjectResult(t.TagID);
            }
            else return new BadRequestObjectResult(tag);
        }
    }
}