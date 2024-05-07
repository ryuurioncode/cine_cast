using NAudio.CoreAudioApi;
using NAudio.Wave.SampleProviders;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using static System.Formats.Asn1.AsnWriter;

namespace CineCast
{
    internal class PlaybackDeviceAudioInput : IAudioInput
    {
        public event EventHandler<float>? OnVolume;
        private readonly WaveFormat format;
        private readonly MMDevice device;
        private readonly WasapiLoopbackCapture captureDevice;
        private readonly BufferedWaveProvider bufferedWaveProvider;
        private readonly MediaFoundationResampler resampler;
        private readonly MeteringSampleProvider meteringSampleProvider;
        private readonly VolumeSampleProvider volumer;
        private int maxLatency = 40000;
        private int currentLatencyMillis = 0;
        public PlaybackDeviceAudioInput(WaveFormat format, MMDevice device, int DesiredLatency = 100)
        {
            this.currentLatencyMillis = DesiredLatency;
            this.format = format;
            this.device = device;
            captureDevice = new WasapiLoopbackCapture(device);

            bufferedWaveProvider = new BufferedWaveProvider(captureDevice.WaveFormat);
            bufferedWaveProvider.BufferDuration = new TimeSpan(0, 0, 0, 0, maxLatency*2);
            bufferedWaveProvider.ReadFully = true;
            bufferedWaveProvider.DiscardOnBufferOverflow = true;
            captureDevice.DataAvailable += (sender, waveInEventArgs) =>
                bufferedWaveProvider.AddSamples(waveInEventArgs.Buffer, 0, waveInEventArgs.BytesRecorded);
            resampler = new MediaFoundationResampler(bufferedWaveProvider, format);

            meteringSampleProvider = new MeteringSampleProvider(resampler.ToSampleProvider());
            meteringSampleProvider.SamplesPerNotification = format.ConvertLatencyToByteSize(10);
            volumer = new VolumeSampleProvider(meteringSampleProvider);
            OnVolume = null;
            meteringSampleProvider.StreamVolume += (sender, volume) =>
                OnVolume?.Invoke(this, volume.MaxSampleValues[0]);
        }
        public string UniqueId { get => $"PLayback:{device.ID}"; }
        public float Volume
        {
            get => volumer.Volume;
            set
            {
                volumer.Volume = value;
            }
        }
        public bool LatencyChangable => true;
        public int Latency { get => currentLatencyMillis;
            set
            {
                var val = Math.Min(maxLatency, value);
                if (currentLatencyMillis < val)
                    Back(val - currentLatencyMillis);
                else
                    Forward(currentLatencyMillis - val);
                currentLatencyMillis = val;
            }
        }
        public string Name { get => device.FriendlyName; }
        public AudioInputType Type { get => AudioInputType.Playback; }
        public ISampleProvider ToSampleProvider() => volumer;
        public void Forward(int millis)
        {
            if (millis <= 0) return;
            var len = captureDevice.WaveFormat.ConvertLatencyToByteSize(millis);
            byte[] buffer = new byte[len];
            bufferedWaveProvider.Read(buffer, 0, len);
        }
        public void Back(int millis)
        {
            if (millis <= 0) return;
            var len = captureDevice.WaveFormat.ConvertLatencyToByteSize(millis);
            byte[] buffer = new byte[len];
            Array.Clear(buffer, 0, len);
            bufferedWaveProvider.AddSamples(buffer, 0, len);
        }
        public void Start()
        {
            bufferedWaveProvider.ClearBuffer();
            Back(currentLatencyMillis);
            captureDevice.StartRecording();
        }
        public void Stop() {
            captureDevice.StopRecording();
        }
        public static IEnumerable<IAudioInput> GetAvailable(WaveFormat format, int DesiredLatency = 100)
        {
            var result = new List<IAudioInput>();
            try
            {
                var enumerator = new MMDeviceEnumerator();
                var devices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
                foreach (var device in devices)
                    result.Add(new PlaybackDeviceAudioInput(format, device, DesiredLatency));
                enumerator.Dispose();
            } catch (Exception) { }
            return result;
        }
        public void Dispose()
        {
            device.Dispose();
            captureDevice.Dispose();
            resampler.Dispose();
        }
        ~PlaybackDeviceAudioInput()
        {
            device.Dispose();
            captureDevice.Dispose();
            resampler.Dispose();
        }
    }
}
