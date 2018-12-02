using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ignorama.Models
{
    public class BanReason
    {
        public int ID { get; set; }

        public string Text { get; set; }

        public int BaseBanHours { get; set; }

        public string RuleDescription { get; set; }
    }
}