using Ignorama.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: HostingStartup(typeof(Ignorama.Areas.Identity.IdentityHostingStartup))]

namespace Ignorama.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<ForumContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("IgnoramaContextConnection")));

                services.AddDefaultIdentity<User>()
                    .AddEntityFrameworkStores<ForumContext>()
                    .AddDefaultTokenProviders();
            });
        }
    }
}