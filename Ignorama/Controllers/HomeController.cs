using Ignorama.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Ignorama.Controllers
{
    public class HomeController : Controller
    {
        private readonly ForumContext _context;

        public HomeController(ForumContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Tags.OrderBy(tag => tag.Name));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}