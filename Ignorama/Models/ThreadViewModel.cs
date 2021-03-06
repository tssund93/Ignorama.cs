﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ignorama.Models
{
    public class ThreadViewModel
    {
        public string Title { get; set; }

        public bool IsOP { get; set; }

        public bool CanBump { get; set; }

        public long LastSeenPostID { get; set; }

        public bool Locked { get; set; }

        public User User { get; set; }

        public IList<string> Roles { get; set; }
    }
}