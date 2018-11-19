using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ignorama.Models
{
    public class Thread
    {
        public int ID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public bool Stickied { get; set; }

        [Required]
        public bool Locked { get; set; }

        [Required]
        public bool Deleted { get; set; }

        [Required]
        public Tag Tag { get; set; }

        public IEnumerable<Post> Posts { get; set; }
    }
}