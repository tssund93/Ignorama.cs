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
    }
}