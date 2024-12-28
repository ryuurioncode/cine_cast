using CineCast.Properties;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace CineCast
{
    public partial class IcecastControl : UserControl
    {
        private FileMp3CastControl? fileDumper = null;
        private IcecastMp3AudioOutput? audioOutput;
        private TrackInfo? trackInfo = null;
        private bool Plaing;
        IcecastProperties properties = new IcecastProperties();
        public IcecastControl()
        {
            InitializeComponent();
            button1.Enabled = false;
            Plaing = false;
            textBox2.MaxLength = 6;
            pictureBox2.Image = Resources.EyeClosed;
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            textBox4.PasswordChar = '*';
            ValidateTarget();
        }

        public void Inititalize(WaveFormat format, IcecastProperties properties, FileMp3CastControl? fileDumper = null, TrackInfo? trackInfo = null)
        {
            this.trackInfo = trackInfo;
            this.fileDumper = fileDumper;
            this.properties = properties;
            audioOutput = new IcecastMp3AudioOutput(format);
            textBox1.Text = properties.Host;
            textBox1.PlaceholderText = "обязательное поле";
            textBox2.Text = properties.Port;
            textBox2.PlaceholderText = "обязательное поле";
            textBox5.Text = properties.Mount;
            textBox5.PlaceholderText = "обязательное поле";
            textBox3.Text = properties.User;
            textBox3.PlaceholderText = "обязательное поле";
            textBox4.Text = properties.Password;
            textBox4.PlaceholderText = "обязательное поле";
            checkBox1.Enabled = fileDumper is not null;
            checkBox1.Checked = fileDumper is not null;
            ValidateTarget();
        }

        public void Write(byte[] data, int offset, int length)
        {
            if (Plaing && audioOutput is not null)
            {
                audioOutput.Write(data, offset, length);
            }
        }

        private bool isTargetValid()
        {
            return !String.IsNullOrWhiteSpace(textBox1.Text)
                && !String.IsNullOrWhiteSpace(textBox2.Text)
                && !String.IsNullOrWhiteSpace(textBox3.Text)
                && !String.IsNullOrWhiteSpace(textBox4.Text)
                && !String.IsNullOrWhiteSpace(textBox5.Text)
                && !String.IsNullOrWhiteSpace(trackInfo?.title);
        }

        public void ValidateTarget()
        {
            if (!isTargetValid() || audioOutput is null)
                button1.Enabled = false;
            else button1.Enabled = true;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Start()
        {
            if (fileDumper is not null)
            {
                if (!fileDumper.Playing && checkBox1.Checked)
                {
                    if (!fileDumper.Valid)
                    {
                        MessageBox.Show($"Заполните окно записи в файл, сейчас она не возможна.");
                        return;
                    }
                    fileDumper.Play();
                }
            }
            if (!isTargetValid() || audioOutput is null)
            {
                button1.Enabled = false;
                return;
            }
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            try
            {
                audioOutput.Initialize(textBox1.Text,
                    int.Parse(textBox2.Text),
                    textBox5.Text,
                    textBox3.Text,
                    textBox4.Text,
                    trackInfo?.title,
                    trackInfo?.genre);
                pictureBox1.Image = Resources.PlayingGif;
                Plaing = true;
                button1.Text = "Отключиться";
            }
            catch (Exception e)
            {
                MessageBox.Show($"Не вышло подключиться к Icecast. Ниже будет преведена ошибка, вам очень повезет если она будет понятной!\n\n{e.Message}");
                Stop();
            }
            properties.Host = textBox1.Text;
            properties.Port = textBox2.Text;
            properties.Mount = textBox5.Text;
            properties.User = textBox3.Text;
            properties.Password = textBox4.Text;
        }

        private void Stop()
        {
            audioOutput?.Close();
            Plaing = false;
            pictureBox1.Image = Resources.PausedGif;
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            textBox5.Enabled = true;
            button1.Text = "Подключиться";

            if (fileDumper is not null)
            {
                if (fileDumper.Playing && checkBox1.Checked)
                {
                    fileDumper.Stop();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Plaing) Stop();
            else Start();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
            ValidateTarget();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            ValidateTarget();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            ValidateTarget();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            ValidateTarget();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            ValidateTarget();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            ValidateTarget();
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

            if (textBox4.PasswordChar == (char)0)
            {
                textBox4.PasswordChar = '*';
                pictureBox2.Image = Resources.EyeClosed;
            }
            else
            {
                textBox4.PasswordChar = (char)0;
                pictureBox2.Image = Resources.EyeOpened;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
        }
    }
}
