namespace CineCast
{
    partial class MixerControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            groupBox1 = new GroupBox();
            soundControl1 = new SoundControl();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(soundControl1);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(0, 0);
            groupBox1.MaximumSize = new Size(0, 45);
            groupBox1.MinimumSize = new Size(0, 45);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(220, 45);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Mixer";
            // 
            // soundControl1
            // 
            soundControl1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            soundControl1.CurrentVolume = 0F;
            soundControl1.Dock = DockStyle.Fill;
            soundControl1.Location = new Point(3, 19);
            soundControl1.MaximumSize = new Size(0, 20);
            soundControl1.MinimumSize = new Size(200, 20);
            soundControl1.Name = "soundControl1";
            soundControl1.Size = new Size(214, 20);
            soundControl1.TabIndex = 0;
            soundControl1.Volume = 1F;
            soundControl1.Load += soundControl1_Load;
            // 
            // MixerControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(groupBox1);
            MaximumSize = new Size(0, 45);
            MinimumSize = new Size(220, 45);
            Name = "MixerControl";
            Size = new Size(220, 45);
            Load += MixerControl_Load;
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private SoundControl soundControl1;
    }
}
