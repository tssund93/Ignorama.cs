using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ignorama.Models
{
    public class Post
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public Thread Thread { get; set; }

        [Required]
        public User User { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public DateTime Time { get; set; }

        [Required]
        public bool Deleted { get; set; }

        [Required]
        public bool DeletedTime { get; set; }
    }
}