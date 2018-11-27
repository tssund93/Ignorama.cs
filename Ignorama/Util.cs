﻿using Ignorama.Models;
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

        static public IQueryable<Tag> GetSelectedTags(User user, ForumContext context, HttpRequest request)
        {
            return GetByUserOrIP(user, context.SelectedTags, request)
                .Where(st => !st.Tag.Deleted)
                .Select(st => st.Tag);
        }

        static public IQueryable<HiddenThread> GetHiddenThreads(User user, ForumContext context, HttpRequest request)
        {
            return GetByUserOrIP(user, context.HiddenThreads, request);
        }

        static public IQueryable<FollowedThread> GetFollowedThreads(User user, ForumContext context, HttpRequest request)
        {
            return GetByUserOrIP(user, context.FollowedThreads, request)
                .Include(ft => ft.LastSeenPost);
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

        static public IQueryable<SelectedTag> GetSelectedTagMatches(
            User user, Tag tag, ForumContext context, HttpRequest request)
        {
            return GetByUserOrIP(user, context.SelectedTags, request)
                .Where(st => st.Tag == tag && !st.Tag.Deleted);
        }

        static public IQueryable<Tag> GetTags(ForumContext context)
        {
            return context.Tags.Where(tag => !tag.Deleted);
        }
    }
}