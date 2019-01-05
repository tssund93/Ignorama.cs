using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Ignorama.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;
        private readonly IAmazonS3 _s3Client;

        public HomeController(
            ForumContext context,
            IHostingEnvironment hostingEnvironment,
            IConfiguration configuration,
            IAmazonS3 s3Client)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _s3Client = s3Client;
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
            var bucket = "ignorama";
            var maxFileMB = 10;
            var allowedTypes = new[] {
                "image/gif", "image/jpeg", "image/jpg", "image/pjpeg", "image/x-png", "image/png", "video/webm"
            };
            var allowedExts = new[]
            {
                ".gif", ".jpeg", ".jpg", ".png", ".webm"
            };

            if (upload.File.Length > maxFileMB * 1000000)
            {
                return new ContentResult
                {
                    Content = "<script>error = 'Could not upload file: File must be smaller than "
                        + maxFileMB + " MB.';</script>",
                    ContentType = "text/html",
                };
            }

            var path = "uploads/" + upload.FileName;
            var ext = Path.GetExtension(path);

            if (upload.File.Length > 0 &&
                allowedTypes.Contains(upload.File.ContentType.ToLower()) &&
                allowedExts.Contains(ext.ToLower()))
            {
                try
                {
                    var fileTransferUtility = new TransferUtility(_s3Client);
                    await fileTransferUtility.UploadAsync(upload.File.OpenReadStream(), bucket, path);

                    return new ContentResult
                    {
                        Content = $"<script>error = 'none'; fileUri = 'https://{bucket}.s3.amazonaws.com/{path}';</script>",
                        ContentType = "text/html",
                    };
                }
                catch (AmazonS3Exception e)
                {
                    return new ContentResult
                    {
                        Content = $"<script>error = 'Error encountered on server. Message:\"{e.Message}\" when writing an object';</script>",
                        ContentType = "text/html",
                    };
                }
                catch (Exception e)
                {
                    return new ContentResult
                    {
                        Content = $"<script>error = 'Unknown encountered on server. Message:\"{e.Message}\" when writing an object';</script>",
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