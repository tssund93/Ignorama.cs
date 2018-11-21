using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ignorama.Models
{
    public class User : IdentityUser<long>
    {
        public ICollection<Thread> Threads { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<HiddenThread> HiddenThreads { get; set; }
        public ICollection<FollowedThread> FollowedThreads { get; set; }
    }
}