using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ignorama.Models
{
    public class IndexViewModel
    {
        public IEnumerable<Tag> Tags { get; set; }

        public IEnumerable<BanReason> BanReasons { get; set; }
    }
}