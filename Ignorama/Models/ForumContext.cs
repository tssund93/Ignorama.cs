using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ignorama.Models
{
    public class ForumContext : IdentityDbContext<User, IdentityRole<long>, long>
    {
        public ForumContext(DbContextOptions<ForumContext> options)
           : base(options)
        { }

        public DbSet<Thread> Threads { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PermissionLevel> PermissionLevels { get; set; }
    }
}