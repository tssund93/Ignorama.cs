using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ignorama.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public ICollection<Thread> Threads { get; set; }
    }
}