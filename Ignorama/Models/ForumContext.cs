using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Ignorama.Models
{
    public class ForumContext : IdentityDbContext<User, IdentityRole<long>, long>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var uri = new Uri("postgres://travis:password@localhost:5432/ignorama");

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            {
                uri = new Uri(Environment.GetEnvironmentVariable("DATABASE_URL"));
            }

            var host = uri.Host;
            var port = uri.Port;
            var database = uri.Segments[1];
            var userPass = uri.UserInfo.Split(':');
            var username = userPass[0];
            var password = userPass[1];

            optionsBuilder.UseNpgsql($"Host={host};Port={port};Database={database};Username={username};Password={password};sslmode=Prefer;Trust Server Certificate=true");
        }

        public DbSet<Thread> Threads { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<HiddenThread> HiddenThreads { get; set; }
        public DbSet<FollowedThread> FollowedThreads { get; set; }
        public DbSet<SelectedTag> SelectedTags { get; set; }
        public DbSet<Ban> Bans { get; set; }
        public DbSet<BanReason> BanReasons { get; set; }
        public DbSet<Report> Reports { get; set; }
    }
}