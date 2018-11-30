using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ignorama.Models
{
    public class BansViewModel
    {
        public IEnumerable<Ban> Bans { get; set; }
        public Post Post { get; set; }
    }
}