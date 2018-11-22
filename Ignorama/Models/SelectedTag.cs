using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ignorama.Models
{
    public class SelectedTag
    {
        public long ID { get; set; }

        public User User { get; set; }

        public string IP { get; set; }

        [Required]
        public Tag Tag { get; set; }
    }
}