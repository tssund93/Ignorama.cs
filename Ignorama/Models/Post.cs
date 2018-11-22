using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ignorama.Models
{
    public class Post
    {
        public int ID { get; set; }

        [Required]
        public Thread Thread { get; set; }

        public User User { get; set; }

        public string IP { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public DateTime Time { get; set; }

        [Required]
        public bool Deleted { get; set; }

        [Required]
        public bool Anonymous { get; set; }

        [Required]
        public bool Bump { get; set; }

        [Required]
        public bool RevealOP { get; set; }
    }
}