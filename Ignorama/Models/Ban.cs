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

        public User User { get; set; }

        public string IP { get; set; }

        [Required]
        public string Reason { get; set; }

        [Required]
        public Post Post { get; set; }

        [Required]
        public DateTime EndTime { get; set; }
    }
}