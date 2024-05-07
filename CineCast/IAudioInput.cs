using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineCast
{
    public interface IAudioInput : IDisposable
    {
        public event EventHandler<float>? OnVolume;
        public float Volume { get; set; }
        public string Name { get; }
        public string UniqueId { get; }
        public AudioInputType Type { get; }
        public ISampleProvider ToSampleProvider();
        public void Start();
        public void Stop();
        public bool Equals(IAudioInput other)
            => UniqueId == other.UniqueId;
        public bool LatencyChangable { get; }
        public int Latency { get; set; }
    }
}
