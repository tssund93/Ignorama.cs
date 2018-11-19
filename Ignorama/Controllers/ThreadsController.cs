using Ignorama.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ignorama.Controllers
{
    public class ThreadsController : Controller
    {
        private readonly ForumContext _context;

        public ThreadsController(ForumContext context)
        {
            _context = context;
        }

        public IActionResult New()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetThreads()
        {
            return new OkObjectResult(
                _context.Threads
                    .Include(thread => thread.Posts)
                    .ToList());
        }
    }
}