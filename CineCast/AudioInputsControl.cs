using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CineCast
{
    public partial class AudioInputsControl : UserControl
    {
        private WaveFormat? format = null;
        private AudioInputMixer? mixer = null;
        private int Latency = 100;
        private List<AudioInputControl> audioInputControls = new List<AudioInputControl>();
        private Dictionary<string, InputProperties> properties;
        public AudioInputsControl()
        {
            InitializeComponent();
        }

        public void Initialize(WaveFormat format, AudioInputMixer mixer, int Latency, Dictionary<string, InputProperties> properties)
        {
            this.properties = properties;
            this.Latency = Latency;
            this.format = format;
            this.mixer = mixer;
            UpdateSources();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (var control in audioInputControls)
                control.Mute();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void UpdateSources()
        {
            RemoveAudioSources();
            if (format is null) return;

            List<IAudioInput> inputs = new List<IAudioInput>();
            List<AudioInputControl> inputsResult = new List<AudioInputControl>();
            inputs.AddRange(DeviceAudioInput.GetAvailable(format, Latency));
            inputs.AddRange(PlaybackDeviceAudioInput.GetAvailable(format, Latency));
            foreach (var input in inputs)
            {
                var value = audioInputControls.Where(x => x.input?.Equals(input) ?? false).FirstOrDefault();
                if (value is not null)
                {
                    inputsResult.Add(value);
                }
                else
                {
                    var inputControl = new AudioInputControl();
                    if(!properties.TryGetValue(input.UniqueId, out var props))
                    {
                        props = new InputProperties();
                        props.Volume = 1;
                        props.UniqueId = input.UniqueId;
                        properties.Add(input.UniqueId, props);
                    }
                    inputControl.Initialize(input, props);
                    mixer?.AppendAudioInput(input);
                    inputsResult.Add(inputControl);
                }
            }
            foreach (var input in audioInputControls)
            {
                if (input.input is null) continue;
                if (!inputsResult.Where(x => x.input?.Equals(input.input) ?? false).Any())
                {
                    mixer?.RemoveAudioInput(input.input);
                    input.Dispose();
                }
            }
            audioInputControls = inputsResult;
            foreach (var input in audioInputControls)
                AppendAudioSource(input);
        }

        private void RemoveAudioSources()
        {
            foreach (var input in audioInputControls)
            {
                panel1.Controls.Remove(input);
            }
            panel1.Controls.Clear();
        }

        private void AppendAudioSource(AudioInputControl control)
        {
            var y = panel1.Controls.Count > 0
                ? panel1.Controls[panel1.Controls.Count - 1].Bottom
                : 0;
            control.Top = y;
            control.Width = panel1.Width - SystemInformation.VerticalScrollBarWidth;
            panel1.Controls.Add(control);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UpdateSources();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (var control in audioInputControls)
                control.Play();
        }

        private void AudioInputsControl_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            foreach (var control in panel1.Controls.Cast<AudioInputControl>())
            {
                control.Width = panel1.Width - SystemInformation.VerticalScrollBarWidth;
                control.Refresh();
            }
        }
    }
}
