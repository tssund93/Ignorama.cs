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
        
        public PermissionLevel RequiredPermissionLevel { get; set; }

        [Required]
        public bool Deleted { get; set; }
    }
}
