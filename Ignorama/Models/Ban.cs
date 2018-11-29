using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ignorama.Models
{
    public class Ban
    {
        public long ID { get; set; }

        [Required]
        public string Reason { get; set; }

        [Required]
        public Post Post { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public User Moderator { get; set; }
    }
}