using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using NAudio.Mixer;
using static System.Windows.Forms.DataFormats;

namespace CineCast
{

    public class AudioInputMixer
    {
        public event EventHandler<float>? OnVolume;
        private readonly List<IAudioInput> inputs;
        private readonly MixingSampleProvider mixer;
        private readonly BufferedWaveProvider bufferedWaveProvider;
        private readonly MeteringSampleProvider meteringSampleProvider;
        private readonly VolumeSampleProvider volumeSampleProvider;
        private readonly WaveFormat format;
        public AudioInputMixer(WaveFormat? format = null, int DesiredLatency = 100)
        {
            this.format = format is null ? WaveFormat.CreateIeeeFloatWaveFormat(44100, 2) : format;
            mixer = new MixingSampleProvider(this.format);
            bufferedWaveProvider = new BufferedWaveProvider(this.format);
            bufferedWaveProvider.ReadFully = true;
            bufferedWaveProvider.DiscardOnBufferOverflow = true;
            OnVolume = null;
            meteringSampleProvider = new MeteringSampleProvider(mixer);
            meteringSampleProvider.SamplesPerNotification = this.format.ConvertLatencyToByteSize(10);
            meteringSampleProvider.StreamVolume += (sender, volume) =>
                OnVolume?.Invoke(this, volume.MaxSampleValues[0]);
            volumeSampleProvider = new VolumeSampleProvider(meteringSampleProvider);
            volumeSampleProvider.Volume = 1.0f;
            mixer.AddMixerInput(bufferedWaveProvider);
            inputs = new List<IAudioInput>();
        }
        public float Volume {  get => volumeSampleProvider.Volume; set => volumeSampleProvider.Volume = value; }
        public IWaveProvider ToWaveProvider()
            => volumeSampleProvider.ToWaveProvider();

        public bool Contains(IAudioInput audioInput)
        {
            lock (inputs) {
                return inputs.Where(x => x == audioInput).Any();
            }
        }

        public void AppendAudioInput(IAudioInput audioInput)
        {
            if (Contains(audioInput)) return;
            lock (inputs) { inputs.Append(audioInput); }
            mixer.AddMixerInput(audioInput.ToSampleProvider());
        }

        public void RemoveAudioInput(IAudioInput audioInput)
        {
            if (!Contains(audioInput)) return;
            lock (inputs) { inputs.Remove(audioInput); }
            mixer.RemoveMixerInput(audioInput.ToSampleProvider());
        }

        public void ClearAudioInputs()
        {
            lock (inputs) { inputs.RemoveAll((item) => true); }
            mixer.RemoveAllMixerInputs();
        }
        ~AudioInputMixer()
        {
        }
    }
}
