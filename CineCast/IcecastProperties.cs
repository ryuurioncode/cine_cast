using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineCast
{
    public class IcecastProperties
    {
        public bool autoDump { get; set; } = false;
        public string? Host { get; set; }
        public string? Port { get; set; }
        public string? Mount { get; set; }
        public string? User { get; set; }
        public string? Password { get; set; }
    }
}
