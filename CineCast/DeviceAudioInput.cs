using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineCast
{
    internal class DeviceAudioInput : IAudioInput
    {
        public event EventHandler<float>? OnVolume;
        private int DesiredLatency = 100;
        private readonly WaveFormat format;
        private readonly MMDevice device;
        private readonly WasapiCapture captureDevice;
        private readonly BufferedWaveProvider bufferedWaveProvider;
        private readonly MediaFoundationResampler resampler;
        private readonly MeteringSampleProvider meteringSampleProvider;
        private readonly VolumeSampleProvider volumer;
        public DeviceAudioInput(WaveFormat format, MMDevice device, int DesiredLatency = 100)
        {
            this.DesiredLatency = DesiredLatency;
            this.format = format;
            this.device = device;
            captureDevice = new WasapiCapture(device, true, DesiredLatency);
            
            bufferedWaveProvider = new BufferedWaveProvider(captureDevice.WaveFormat);
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
        public string UniqueId { get => $"Device:{device.ID}"; }
        public float Volume {
            get => volumer.Volume;
            set {
                volumer.Volume = value;
            }
        }
        public bool LatencyChangable => false;
        public int Latency { get => DesiredLatency; set { } }
        public string Name { get => device.FriendlyName; }
        public AudioInputType Type { get => AudioInputType.Device; }
        public ISampleProvider ToSampleProvider() => volumer;
        public void Start() => captureDevice.StartRecording();
        public void Stop() => captureDevice.StopRecording();
        public static IEnumerable<IAudioInput> GetAvailable(WaveFormat format, int DesiredLatency = 100)
        {
            var result = new List<IAudioInput>();
            try
            {
                var enumerator = new MMDeviceEnumerator();
                var devices = enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);
                foreach(var device in devices)
                    result.Add(new DeviceAudioInput(format, device, DesiredLatency));
                enumerator.Dispose();
            }
            catch (Exception) { }
            return result;
        }

        public void Dispose()
        {
            device.Dispose();
            captureDevice.Dispose();
            resampler.Dispose();
        }
        ~DeviceAudioInput()
        {
        }
    }
}
