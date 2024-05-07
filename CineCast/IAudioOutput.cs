using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineCast
{
    internal interface IAudioOutput : IDisposable
    {
        public string Name { get; }
        public void Write(byte[] buffer, int offset, int length);
    }
}
