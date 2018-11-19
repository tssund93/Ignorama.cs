using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ignorama.Models
{
    public class User
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public PermissionLevel PermissionLevel { get; set; }

        public ICollection<Thread> Threads { get; set; }
    }
}