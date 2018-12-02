using Ignorama.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ignorama.Controllers
{
    public class HomeController : Controller
    {
        private readonly ForumContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public HomeController(
            ForumContext context,
            IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View(new IndexViewModel
            {
                Tags = _context.Tags.OrderBy(tag => tag.Name),
                BanReasons = _context.BanReasons,
            });
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet("/Home/Error/{code}")]
        public IActionResult Error(int code)
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                StatusCode = code,
                StatusReason = Util.GetStatusReason(code),
            });
        }

        public class ImageUploadModel
        {
            public string FileName { get; set; }
            public IFormFile File { get; set; }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadFile(ImageUploadModel upload)
        {
            var allowedTypes = new[] {
                "image/gif", "image/jpeg", "image/jpg", "image/pjpeg", "image/x-png", "image/png", "video/webm"
            };
            var allowedExts = new[]
            {
                ".gif", ".jpeg", ".jpg", ".png", ".webm"
            };
            var ext = Path.GetExtension(upload.FileName);
            if (upload.File.Length > 0 &&
                allowedTypes.Contains(upload.File.ContentType.ToLower()) &&
                allowedExts.Contains(ext.ToLower()))
            {
                using (var stream = new FileStream(
                    Path.Combine(_hostingEnvironment.WebRootPath, "uploads/", upload.FileName), FileMode.Create))
                {
                    await upload.File.CopyToAsync(stream);

                    return new ContentResult
                    {
                        Content = "<script>error = 'none';</script>",
                        ContentType = "text/html",
                    };
                }
            }

            return new ContentResult
            {
                Content = "<script>error = 'Could not upload file: Unsupported file type.';</script>",
                ContentType = "text/html",
            };
        }

        public IActionResult Spam()
        {
            return View();
        }
    }
}