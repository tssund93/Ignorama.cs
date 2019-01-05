using Ignorama.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Ignorama.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ForumContext _context;

        public RegisterModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ForumContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Username")]
            public string UserName { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new User { UserName = Input.UserName };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    var loggedInUser = await _context.Users.Where(u => u.UserName == Input.UserName).FirstOrDefaultAsync();

                    await _userManager.AddToRoleAsync(user, "User");

                    var ipFollowedThreads = _context.FollowedThreads
                        .Where(thread => thread.IP == Util.GetCurrentIPString(Request))
                        .Include(thread => thread.Thread);
                    foreach (FollowedThread followedThread in ipFollowedThreads)
                    {
                        await _context.AddAsync(new FollowedThread
                        {
                            User = loggedInUser,
                            Thread = followedThread.Thread,
                            LastSeenPostID = followedThread.LastSeenPostID
                        });
                    }

                    var ipHiddenThreads = _context.HiddenThreads
                        .Where(thread => thread.IP == Util.GetCurrentIPString(Request))
                        .Include(thread => thread.Thread);
                    foreach (HiddenThread hiddenThread in ipHiddenThreads)
                    {
                        await _context.AddAsync(new HiddenThread
                        {
                            User = loggedInUser,
                            Thread = hiddenThread.Thread
                        });
                    }

                    var ipSelectedTags = _context.SelectedTags
                        .Where(tag => tag.IP == Util.GetCurrentIPString(Request))
                        .Include(tag => tag.Tag);
                    foreach (SelectedTag selectedTag in ipSelectedTags)
                    {
                        await _context.AddAsync(new SelectedTag
                        {
                            User = loggedInUser,
                            Tag = selectedTag.Tag
                        });
                    }
                    await _context.SaveChangesAsync();

                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}