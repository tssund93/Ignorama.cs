using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ignorama.Models
{
    public class NewThreadViewModel
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public int TagID { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
    }
}