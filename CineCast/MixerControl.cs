using NAudio.Mixer;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CineCast
{
    public partial class MixerControl : UserControl
    {
        public AudioInputMixer? audioInputMixer = null;
        private OutputMixerProperties properties;
        public MixerControl()
        {
            InitializeComponent();
        }

        public void Initialize(WaveFormat format, int latency, OutputMixerProperties properties)
        {
            this.properties = properties;
            audioInputMixer = new AudioInputMixer(format, latency);
            soundControl1.OnVolume += (sender, volume) =>
            {
                properties.Volume = volume;
                audioInputMixer.Volume = volume;
            };
            audioInputMixer.OnVolume += (sender, volume) =>
                soundControl1.Invoke(() => soundControl1.CurrentVolume = volume);
            soundControl1.Volume = properties.Volume;
        }

        private void MixerControl_Load(object sender, EventArgs e)
        {

        }

        private void soundControl1_Load(object sender, EventArgs e)
        {

        }
    }
}
