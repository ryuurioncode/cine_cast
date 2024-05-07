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
    public partial class SoundControl : UserControl
    {
        private float SetVolume = 1.0f;
        private StringFormat textFormat = new StringFormat();
        private Rectangle rectSoundAndSet = new Rectangle();
        private Rectangle rectNoSoundAndSet = new Rectangle();
        private Rectangle rectSoundAndNoSet = new Rectangle();
        private Rectangle rectNoSoundAndNoSet = new Rectangle();
        private Rectangle controlRect = new Rectangle();
        private float MinVolume = 0.0f;
        private bool IsMouseDown = false;
        private Point MouseStartPoint = new Point();
        private float MouseStartVolume = 0.0f;
        private float SoundVolume = 0.0f;

        public Pen SoundAndSetPen = new Pen(Color.DarkGreen);
        public Pen NoSoundAndSetPen = new Pen(Color.LightGreen);
        public Pen SoundAndNoSetPen = new Pen(Color.DarkGray);
        public Pen NoSoundAndNoSetPen = new Pen(Color.LightGray);
        public Pen BoardPen = new Pen(Color.Black);
        public Pen TextPen = new Pen(Color.Black);
        public event EventHandler<float>? OnVolume = null;
        public float MaxVolume = 4;
        public float Volume
        {
            get => SetVolume;
            set
            {
                SetVolume = Math.Min(Math.Max(value, MinVolume), MaxVolume);
                this.Refresh();
                OnVolume?.Invoke(this, SetVolume);
            }
        }
        public float CurrentVolume
        {
            get => SoundVolume;
            set
            {
                SoundVolume = Math.Min(Math.Max(value, MinVolume), MaxVolume);
                this.Refresh();
            }
        }

        public SoundControl()
        {
            InitializeComponent();
            this.ResizeRedraw = true;
            this.DoubleBuffered = true;
            textFormat.Alignment = StringAlignment.Center;
            textFormat.LineAlignment = StringAlignment.Center;

        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            controlRect.Y = 0;
            controlRect.Height = this.Size.Height - 1;
            controlRect.X = 0;
            controlRect.Width = this.Size.Width - 1;

            var volumeSoundAndSet = Math.Min(SetVolume, SoundVolume);
            rectSoundAndSet.Y = 0;
            rectSoundAndSet.Height = this.Size.Height - 1;
            rectSoundAndSet.X = 0;
            rectSoundAndSet.Width = (int)(volumeSoundAndSet * (this.Size.Width - 1) / MaxVolume);

            var volumeNoSoundAndSet = Math.Max(volumeSoundAndSet, SetVolume) - volumeSoundAndSet;
            rectNoSoundAndSet.Y = 0;
            rectNoSoundAndSet.Height = this.Size.Height - 1;
            rectNoSoundAndSet.X = rectSoundAndSet.Right;
            rectNoSoundAndSet.Width = (int)(volumeNoSoundAndSet * (this.Size.Width - 1) / MaxVolume);

            var volumeSoundAndNoSet = Math.Max(SetVolume, SoundVolume) - SetVolume;
            rectSoundAndNoSet.Y = 0;
            rectSoundAndNoSet.Height = this.Size.Height - 1;
            rectSoundAndNoSet.X = rectNoSoundAndSet.Right;
            rectSoundAndNoSet.Width = (int)(volumeSoundAndNoSet * (this.Size.Width - 1) / MaxVolume);

            var volumeNoSoundAndNoSet = MaxVolume - Math.Max(SetVolume, SoundVolume);
            rectNoSoundAndNoSet.Y = 0;
            rectNoSoundAndNoSet.Height = this.Size.Height - 1;
            rectNoSoundAndNoSet.X = rectSoundAndNoSet.Right;
            rectNoSoundAndNoSet.Width = (int)(volumeNoSoundAndNoSet * (this.Size.Width + 1) / MaxVolume);

            e.Graphics.FillRectangle(SoundAndSetPen.Brush, rectSoundAndSet);
            e.Graphics.FillRectangle(NoSoundAndSetPen.Brush, rectNoSoundAndSet);
            e.Graphics.FillRectangle(SoundAndNoSetPen.Brush, rectSoundAndNoSet);
            e.Graphics.FillRectangle(NoSoundAndNoSetPen.Brush, rectNoSoundAndNoSet);
            e.Graphics.DrawRectangle(BoardPen, controlRect);
            var volumeText = Enabled ? (SetVolume).ToString("0.0 %") : (SetVolume).ToString("0.0 % (заблокировано)");
            e.Graphics.DrawString(volumeText, SystemFonts.DefaultFont, TextPen.Brush, controlRect, textFormat);

        }

        private void SoundControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (!Enabled) return;
            IsMouseDown = true;
            MouseStartPoint = e.Location;
            Volume = MaxVolume * (float)MouseStartPoint.X / (float)this.Size.Width;
            MouseStartVolume = Volume;
        }

        private void SoundControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (!Enabled) return;
            IsMouseDown = false;
        }

        private void SoundControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (!Enabled) return;
            if (IsMouseDown)
                Volume = MouseStartVolume - MaxVolume * (float)(MouseStartPoint.X - e.Location.X) / (float)this.Size.Width;
        }

        private void SoundControl_Load(object sender, EventArgs e)
        {

        }
    }
}
