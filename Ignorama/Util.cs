using Ignorama.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ignorama
{
    public class Util
    {
        static public IList<string> GetRoles(User user, UserManager<User> userManager)
        {
            return user != null
                ? userManager.GetRolesAsync(user).Result
                : new string[] { };
        }

        static public List<Tag> GetSelectedTags(User user, ForumContext context, HttpRequest request)
        {
            return (user != null
                ? context.SelectedTags
                    .Where(st => st.User == user)
                : context.SelectedTags
                    .Where(st => st.IP == request.HttpContext.Connection.RemoteIpAddress.ToString()))
                .Select(st => st.Tag)
                .ToList();
        }

        static public List<HiddenThread> GetHiddenThreads(User user, ForumContext context, HttpRequest request)
        {
            return user != null
                ? context.HiddenThreads
                    .Where(hiddenThread => hiddenThread.User == user)
                    .ToList()
                : context.HiddenThreads
                    .Where(hiddenThread => hiddenThread.IP == request.HttpContext.Connection.RemoteIpAddress.ToString())
                    .ToList();
        }

        static public List<FollowedThread> GetFollowedThreads(User user, ForumContext context, HttpRequest request)
        {
            return (user != null
                ? context.FollowedThreads
                    .Where(followedThread => followedThread.User == user)
                : context.FollowedThreads
                    .Where(followedThread => followedThread.IP == request.HttpContext.Connection.RemoteIpAddress.ToString()))
                .Include(ft => ft.LastSeenPost)
                .ToList();
        }

        static public IQueryable<HiddenThread> GetHiddenThreadMatches(
            User user, Thread thread, ForumContext context, HttpRequest request)
        {
            return user != null
                ? context.HiddenThreads.Where(ht => ht.User == user && ht.Thread == thread)
                : context.HiddenThreads.Where(
                    ht => ht.IP == request.HttpContext.Connection.RemoteIpAddress.ToString() && ht.Thread == thread);
        }

        static public IQueryable<FollowedThread> GetFollowedThreadMatches(
            User user, Thread thread, ForumContext context, HttpRequest request)
        {
            return (user != null
                ? context.FollowedThreads.Where(ft => ft.User == user && ft.Thread == thread)
                : context.FollowedThreads.Where(
                    ft => ft.IP == request.HttpContext.Connection.RemoteIpAddress.ToString() && ft.Thread == thread))
                .Include(ft => ft.LastSeenPost);
        }
    }
}