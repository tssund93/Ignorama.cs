using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ignorama.Models
{
    public interface IUserIP
    {
        User User { get; set; }
        string IP { get; set; }
    }
}