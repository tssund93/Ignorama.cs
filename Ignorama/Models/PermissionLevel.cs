using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ignorama.Models
{
    public class PermissionLevel
    {
        public int ID { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        public int Level { get; set; }
    }
}
