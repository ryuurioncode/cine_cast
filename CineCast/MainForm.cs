using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Utils;
using NAudio.Wave.SampleProviders;

using IceCastLibrary;
using static System.Windows.Forms.DataFormats;
using NAudio.Mixer;
using System.IO;
using NAudio.Lame;
using System.Media;
using System.Numerics;
using System;
using Microsoft.VisualBasic.Devices;
using System.Windows.Forms;
using System.Diagnostics;
using System.Data;

namespace CineCast
{
    public partial class MainForm : Form
    {
        private readonly string cacheFileName = "cine.data";
        private readonly string cacheFileDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private readonly ProperiesFileCache cache;
        private readonly WaveFormat streamFormat;
        private readonly int Latency = 200;
        private readonly WaveOutCallbackEvent player;
        public MainForm()
        {
            InitializeComponent();
            cache = new ProperiesFileCache(Path.Combine(cacheFileDirectory, cacheFileName));
            streamFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);

            trackInfoControl1.Initialize(cache.properties.trackInfo, icecastControl1, fileMp3CastControl1);
            mixerControl.Initialize(streamFormat, Latency, cache.properties.outputMixerProperties);
            if(mixerControl.audioInputMixer is null) throw new NoNullAllowedException(nameof(mixerControl.audioInputMixer));
            fileMp3CastControl1.Initialize(streamFormat, cache.properties.mp3FileOutputProperties, trackInfoControl1.trackInfo);
            icecastControl1.Inititalize(streamFormat, cache.properties.icecastProperties, fileMp3CastControl1, trackInfoControl1.trackInfo);
            audioInputsControl1.Initialize(streamFormat, mixerControl.audioInputMixer, Latency, cache.properties.inputProperties);

            player = new WaveOutCallbackEvent();
            player.Init(mixerControl.audioInputMixer.ToWaveProvider());
            foreach (var buffer in player.buffers)
                buffer.DataAvailable += (sender, args) =>
                {
                    icecastControl1.Write(args.buffer, args.offset, args.length);
                    fileMp3CastControl1.Write(args.buffer, args.offset, args.length);
                };
            player.Play();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void audioInputsControl1_Load(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void icecastControl1_Load(object sender, EventArgs e)
        {

        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            cache.Save();
        }

        private void soundControl1_Load(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint_1(object sender, PaintEventArgs e)
        {

        }
    }
}
