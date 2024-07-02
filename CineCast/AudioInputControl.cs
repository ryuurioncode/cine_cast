using CineCast.Properties;
using NAudio.CoreAudioApi;
using NAudio.Gui;
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
    public partial class AudioInputControl : UserControl
    {
        private bool Playing;
        public IAudioInput? input = null;
        InputProperties properties;
        public AudioInputControl()
        {
            InitializeComponent();
            Playing = false;
            button1.Text = String.Empty;
            label1.Text = String.Empty;
            button1.BackgroundImage = Resources.SoundMute;
            button1.BackgroundImageLayout = ImageLayout.Zoom;
        }

        public void Initialize(IAudioInput input, InputProperties properties)
        {
            this.properties = properties;
            this.input = input;
            input.Latency = properties.Latency;
            properties.Latency = input.Latency;
            textBox1.Text = input.Latency.ToString();
            textBox1.Enabled = input.LatencyChangable;
            button2.Enabled = input.LatencyChangable;
            switch (input.Type)
            {
                case AudioInputType.Device: label1.Text = $"Микрофон {input.Name}"; break;
                case AudioInputType.Playback: label1.Text = $"Динамик {input.Name}"; break;
                default: label1.Text = $"{input.Name}"; break;
            }
            soundControl.Volume = properties.Volume;
            soundControl.OnVolume += (sender, volume) =>
            {
                if (input is not null)
                {
                    var value = soundControl.Volume;
                    properties.Volume = value;
                    input.Volume = value;
                }
            };
            this.input.OnVolume += (sender, volume) =>
            {
                soundControl.Invoke(() =>
                {
                    soundControl.CurrentVolume = volume;
                });
            };
        }

        public void StartIfNeedOnInit()
        {
            if (properties.Enabled) Play();
            else Mute();
        }

        public void Mute()
        {
            Playing = false;
            properties.Enabled = false;
            soundControl.Enabled = false;
            input?.Stop();
            button1.BackgroundImage = Resources.SoundMute;
        }

        public void Play()
        {
            Playing = true;
            properties.Enabled = true;
            soundControl.Enabled = true;
            input?.Start();
            button1.BackgroundImage = Resources.SoundVolume;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (Playing) Mute();
            else Play();
        }

        private void AudioInputControl_Load(object sender, EventArgs e)
        {

        }

        public new void Dispose()
        {
            base.Dispose();
            input?.Dispose();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (input is not null)
            {
                input.Latency = int.Parse(textBox1.Text);
                properties.Latency = input.Latency;
                textBox1.Text = input.Latency.ToString();
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
