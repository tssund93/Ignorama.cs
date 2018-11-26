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

        static public IQueryable<T> GetByUserOrIP<T>(
            User user, DbSet<T> table, HttpRequest request) where T : class, IUserIP
        {
            if (user != null)
                return table.Where(t => t.User == user);
            else
                return table.Where(t => t.IP == request.HttpContext.Connection.RemoteIpAddress.ToString());
        }

        static public List<Tag> GetSelectedTags(User user, ForumContext context, HttpRequest request)
        {
            return GetByUserOrIP(user, context.SelectedTags, request)
                .Select(st => st.Tag)
                .ToList();
        }

        static public List<HiddenThread> GetHiddenThreads(User user, ForumContext context, HttpRequest request)
        {
            return GetByUserOrIP(user, context.HiddenThreads, request).ToList();
        }

        static public List<FollowedThread> GetFollowedThreads(User user, ForumContext context, HttpRequest request)
        {
            return GetByUserOrIP(user, context.FollowedThreads, request)
                .Include(ft => ft.LastSeenPost)
                .ToList();
        }

        static public IQueryable<HiddenThread> GetHiddenThreadMatches(
            User user, Thread thread, ForumContext context, HttpRequest request)
        {
            return GetByUserOrIP(user, context.HiddenThreads, request)
                .Where(ht => ht.Thread == thread);
        }

        static public IQueryable<FollowedThread> GetFollowedThreadMatches(
            User user, Thread thread, ForumContext context, HttpRequest request)
        {
            return GetByUserOrIP(user, context.FollowedThreads, request)
                .Where(ft => ft.Thread == thread)
                .Include(ft => ft.LastSeenPost);
        }
    }
}