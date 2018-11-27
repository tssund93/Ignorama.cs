using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ignorama.Models
{
    public class Tag
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public IdentityRole<long> ReadRole { get; set; }

        [Required]
        public IdentityRole<long> WriteRole { get; set; }

        [Required]
        public bool AlwaysVisible { get; set; }

        [Required]
        public bool Deleted { get; set; }
    }
}