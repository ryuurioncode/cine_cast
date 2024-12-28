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
using static System.Windows.Forms.DataFormats;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CineCast
{
    public partial class FileMp3CastControl : UserControl
    {
        private WaveFormat format;
        public bool Playing;
        private string FilePrefix;
        private FileMp3AudioOutput? audioOutput;
        private TrackInfo? trackInfo = null;
        private Mp3FileOutputProperties properties = new Mp3FileOutputProperties();
        public FileMp3CastControl()
        {
            InitializeComponent();
            audioOutput = null;
            textBox2.WordWrap = false;
            button2.Enabled = false;
            Playing = false;
        }

        public void Initialize(WaveFormat format, Mp3FileOutputProperties properties, TrackInfo? trackInfo = null)
        {
            this.trackInfo = trackInfo;
            this.properties = properties;
            this.format = format;
            if (String.IsNullOrWhiteSpace(properties.Directory))
                properties.Directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            else textBox1.Text = String.Empty;
            textBox2.Text = Path.Combine(properties.Directory, textBox1.Text);
        }

        public void Write(byte[] data, int offset, int length)
        {
            if (Playing) audioOutput?.Write(data, offset, length);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                properties.Directory = folderBrowserDialog1.SelectedPath;
                textBox2.Text = Path.Combine(properties.Directory, textBox1.Text);
            }
        }

        public void Play()
        {
            Stop();
            textBox1.Enabled = false;
            button1.Enabled = false;
            button2.Text = "Остановить запись";
            audioOutput = new FileMp3AudioOutput(format, textBox2.Text, trackInfo?.title, trackInfo?.genre);
            Playing = true;
        }

        public void Stop()
        {
            audioOutput?.Stop();
            audioOutput?.Dispose();
            audioOutput = null;
            Playing = false;
            textBox1.Enabled = true;
            button1.Enabled = true;
            button2.Text = "Начать запись";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Playing) Stop();
            else Play();
        }

        public bool Valid => !(String.IsNullOrWhiteSpace(FilePrefix) || String.IsNullOrWhiteSpace(textBox1.Text));

        public void UpdateName(string name)
        {
            textBox1.Text = name;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (Valid) button2.Enabled = true;
            else button2.Enabled = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            properties.Name = textBox1.Text;
            FilePrefix = Path.Combine(properties.Directory ?? String.Empty, textBox1.Text);
            textBox2.Text = FilePrefix;
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        public new void Dispose()
        {
            base.Dispose();
            audioOutput?.Dispose();
        }
    }
}
