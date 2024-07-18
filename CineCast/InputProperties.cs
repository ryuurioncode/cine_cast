using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineCast
{
    public class InputProperties
    {
        public string UniqueId { get; set; } = default!;
        public float Volume { get; set; }
        public bool Enabled { get; set; } = false;
        public int Latency { get; set; } = 0;
        public bool Hidden { get; set; } = false;
    }
}
