using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ignorama.Models
{
    public class Report : IUserIP
    {
        public long ID { get; set; }

        [Required]
        public Post Post { get; set; }

        public User User { get; set; }

        public string IP { get; set; }

        [Required]
        public BanReason Reason { get; set; }

        [Required]
        public bool Active { get; set; }
    }
}