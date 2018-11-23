using Ignorama.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
                services.AddDefaultIdentity<User>()
                    .AddEntityFrameworkStores<ForumContext>()
                    .AddDefaultTokenProviders()
                    .AddRoles<IdentityRole<long>>()
                    .AddRoleManager<RoleManager<IdentityRole<long>>>()
                    .AddRoleStore<RoleStore<IdentityRole<long>, ForumContext, long>>(); ;
            });
        }
    }
}