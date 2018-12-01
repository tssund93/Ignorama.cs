using Ignorama.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace Ignorama
{
    public static class Util
    {
        static public IList<string> GetRoles(User user, UserManager<User> userManager)
        {
            return user != null
                ? userManager.GetRolesAsync(user).Result
                : new string[] { "User" };
        }

        static public string GetCurrentIPString(HttpRequest request)
        {
            return request.HttpContext.Connection.RemoteIpAddress.ToString();
        }

        static public IQueryable<T> GetByUserOrIP<T>(
            User user, string ip, DbSet<T> table) where T : class, IUserIP
        {
            if (user != null)
                return table.Where(t => t.User == user);
            else
                return table.Where(t => t.IP == ip);
        }

        static public IQueryable<T> GetByUserAndIP<T>(
            User user, string ip, DbSet<T> table) where T : class, IUserIP
        {
            if (user != null)
                return table.Where(t => t.User == user ||
                            t.IP == ip);
            else
                return table.Where(t => t.IP == ip);
        }

        static public IQueryable<Tag> GetSelectedTags(User user, ForumContext context, HttpRequest request)
        {
            return GetByUserOrIP(user, GetCurrentIPString(request), context.SelectedTags)
                .Where(st => !st.Tag.Deleted && !st.Tag.AlwaysVisible)
                .Select(st => st.Tag);
        }

        static public IQueryable<HiddenThread> GetHiddenThreads(User user, ForumContext context, HttpRequest request)
        {
            return GetByUserOrIP(user, GetCurrentIPString(request), context.HiddenThreads);
        }

        static public IQueryable<FollowedThread> GetFollowedThreads(User user, ForumContext context, HttpRequest request)
        {
            return GetByUserOrIP(user, GetCurrentIPString(request), context.FollowedThreads);
        }

        static public long? GetLastSeenPost(User user, Thread thread, ForumContext context, HttpRequest request)
        {
            return GetFollowedThreadMatches(user, thread, context, request)?.FirstOrDefault()?.LastSeenPostID;
        }

        static public IQueryable<HiddenThread> GetHiddenThreadMatches(
            User user, Thread thread, ForumContext context, HttpRequest request)
        {
            return GetByUserOrIP(user, GetCurrentIPString(request), context.HiddenThreads)
                .Where(ht => ht.Thread == thread);
        }

        static public IQueryable<FollowedThread> GetFollowedThreadMatches(
            User user, Thread thread, ForumContext context, HttpRequest request)
        {
            return GetByUserOrIP(user, GetCurrentIPString(request), context.FollowedThreads)
                .Where(ft => ft.Thread == thread);
        }

        static public IQueryable<SelectedTag> GetSelectedTagMatches(
            User user, Tag tag, ForumContext context, HttpRequest request)
        {
            return GetByUserOrIP(user, GetCurrentIPString(request), context.SelectedTags)
                .Where(st => st.Tag == tag);
        }

        static public IQueryable<Tag> GetTags(ForumContext context)
        {
            return context.Tags.Where(tag => !tag.Deleted);
        }

        static public bool IsBanned(User user, string ip, ForumContext context)
        {
            return GetCurrentBans(user, ip, context).Count() > 0;
        }

        static public IQueryable<Ban> GetCurrentBans(User user, string ip, ForumContext context)
        {
            string shortIP = null;

            if (ip != null)
            {
                shortIP = ShortenIP(ip);
            }

            var posts = user != null
                ? context.Posts.Where(t => t.User == user ||
                                            t.IP.StartsWith(shortIP))
                : context.Posts.Where(t => t.IP.StartsWith(shortIP));

            var postIDs = posts.Select(p => p.ID);

            return context.Bans
                .Include(b => b.Post)
                .Where(b => postIDs.Contains(b.Post.ID) &&
                            b.EndTime > DateTime.UtcNow)
                .OrderByDescending(b => b.EndTime);
        }

        public static string ShortenIP(string ip)
        {
            Regex ipRegex = new Regex(@"(((.+?\.){3})|((.{4}:){4})).+",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);
            MatchCollection matches = ipRegex.Matches(ip);
            return matches.Count > 0 ? matches[0].Groups[1].Value : ip;
        }

        public static string ToReadableString(this TimeSpan span)
        {
            var timeLeft = "";
            if (span.Duration().Days > 365)
            {
                var years = span.Duration().Days / 365;
                timeLeft = string.Format("{0:0} year{1}", years, years == 1 ? String.Empty : "s");
            }
            else if (span.Duration().Days > 30)
            {
                var months = span.Duration().Days / 30;
                timeLeft = string.Format("{0:0} month{1}", months, months == 1 ? String.Empty : "s");
            }
            else if (span.Duration().Days > 7)
            {
                var weeks = span.Duration().Days / 7;
                timeLeft = string.Format("{0:0} week{1}", weeks, weeks == 1 ? String.Empty : "s");
            }
            else if (span.Duration().Days > 0)
            {
                timeLeft = string.Format("{0:0} day{1}", span.Days, span.Days == 1 ? String.Empty : "s");
            }
            else if (span.Duration().Hours > 0)
            {
                timeLeft = string.Format("{0:0} hour{1}", span.Hours, span.Hours == 1 ? String.Empty : "s");
            }
            if (string.IsNullOrEmpty(timeLeft)) timeLeft = "a few minutes";

            return timeLeft;
        }

        public static bool IsOP(User OP, string OPIP, User currentUser, string currentIP)
        {
            if (OP == null || currentUser == null)
            {
                return OPIP == currentIP;
            }
            else
            {
                return OP.UserName == currentUser.UserName
                        || (OP == null && OPIP == currentIP);
            }
        }

        public static bool CanBump(User OP, string OPIP, User currentUser, string currentIP)
        {
            if (OP == null || currentUser == null)
            {
                return OPIP != currentIP;
            }
            else
            {
                return !(OP.UserName == currentUser.UserName
                    || OPIP == currentIP);
            }
        }

        public static string GetStatusReason(int statusCode)
        {
            var key = ((HttpStatusCode)statusCode).ToString();
            return Regex.Replace(key, "(\\B[A-Z])", " $1");
        }
    }
}