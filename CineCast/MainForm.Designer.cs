namespace CineCast
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            icecastControl1 = new IcecastControl();
            fileMp3CastControl1 = new FileMp3CastControl();
            audioInputsControl1 = new AudioInputsControl();
            tableLayoutPanel1 = new TableLayoutPanel();
            mixerControl = new MixerControl();
            trackInfoControl1 = new TrackInfoControl();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // icecastControl1
            // 
            icecastControl1.Dock = DockStyle.Fill;
            icecastControl1.Location = new Point(525, 6);
            icecastControl1.Margin = new Padding(6);
            icecastControl1.MinimumSize = new Size(334, 150);
            icecastControl1.Name = "icecastControl1";
            icecastControl1.Padding = new Padding(2);
            icecastControl1.Size = new Size(507, 245);
            icecastControl1.TabIndex = 6;
            icecastControl1.Load += icecastControl1_Load;
            // 
            // fileMp3CastControl1
            // 
            fileMp3CastControl1.Dock = DockStyle.Fill;
            fileMp3CastControl1.Location = new Point(525, 411);
            fileMp3CastControl1.Margin = new Padding(6);
            fileMp3CastControl1.MinimumSize = new Size(334, 158);
            fileMp3CastControl1.Name = "fileMp3CastControl1";
            fileMp3CastControl1.Size = new Size(507, 245);
            fileMp3CastControl1.TabIndex = 7;
            // 
            // audioInputsControl1
            // 
            audioInputsControl1.Dock = DockStyle.Fill;
            audioInputsControl1.Location = new Point(6, 6);
            audioInputsControl1.Margin = new Padding(6);
            audioInputsControl1.MinimumSize = new Size(334, 0);
            audioInputsControl1.Name = "audioInputsControl1";
            tableLayoutPanel1.SetRowSpan(audioInputsControl1, 4);
            audioInputsControl1.Size = new Size(507, 650);
            audioInputsControl1.TabIndex = 0;
            audioInputsControl1.Load += audioInputsControl1_Load;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(icecastControl1, 1, 0);
            tableLayoutPanel1.Controls.Add(audioInputsControl1, 0, 0);
            tableLayoutPanel1.Controls.Add(fileMp3CastControl1, 1, 3);
            tableLayoutPanel1.Controls.Add(mixerControl, 1, 1);
            tableLayoutPanel1.Controls.Add(trackInfoControl1, 1, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(2);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 94F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(1038, 662);
            tableLayoutPanel1.TabIndex = 8;
            tableLayoutPanel1.Paint += tableLayoutPanel1_Paint_1;
            // 
            // mixerControl
            // 
            mixerControl.Dock = DockStyle.Fill;
            mixerControl.Location = new Point(525, 263);
            mixerControl.Margin = new Padding(6);
            mixerControl.MaximumSize = new Size(0, 46);
            mixerControl.MinimumSize = new Size(220, 46);
            mixerControl.Name = "mixerControl";
            mixerControl.Size = new Size(507, 46);
            mixerControl.TabIndex = 8;
            // 
            // trackInfoControl1
            // 
            trackInfoControl1.Dock = DockStyle.Fill;
            trackInfoControl1.Location = new Point(523, 317);
            trackInfoControl1.Margin = new Padding(4, 6, 4, 6);
            trackInfoControl1.Name = "trackInfoControl1";
            trackInfoControl1.Size = new Size(511, 82);
            trackInfoControl1.TabIndex = 9;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(1038, 662);
            Controls.Add(tableLayoutPanel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(2);
            Name = "MainForm";
            Text = "CineCast";
            FormClosed += MainForm_FormClosed;
            Load += Form1_Load;
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private IcecastControl icecastControl1;
        private FileMp3CastControl fileMp3CastControl1;
        private AudioInputsControl audioInputsControl1;
        private TableLayoutPanel tableLayoutPanel1;
        private MixerControl mixerControl;
        private TrackInfoControl trackInfoControl1;
    }
}
