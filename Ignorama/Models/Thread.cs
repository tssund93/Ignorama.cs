using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ignorama.Models
{
    public class Thread
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public bool Stickied { get; set; }
        public bool Locked { get; set; }
        public bool Deleted { get; set; }
        public User User { get; set; }
    }
}