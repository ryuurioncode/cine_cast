namespace CineCast
{
    partial class AudioInputControl
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
            button1 = new Button();
            label1 = new Label();
            soundControl = new SoundControl();
            tableLayoutPanel1 = new TableLayoutPanel();
            button2 = new Button();
            textBox1 = new TextBox();
            label2 = new Label();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Dock = DockStyle.Fill;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Location = new Point(3, 3);
            button1.Name = "button1";
            tableLayoutPanel1.SetRowSpan(button1, 2);
            button1.Size = new Size(39, 48);
            button1.TabIndex = 1;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label1
            // 
            label1.Dock = DockStyle.Fill;
            label1.Location = new Point(48, 0);
            label1.Name = "label1";
            label1.Size = new Size(230, 24);
            label1.TabIndex = 2;
            label1.Text = "label1";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // soundControl
            // 
            soundControl.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel1.SetColumnSpan(soundControl, 2);
            soundControl.CurrentVolume = 0F;
            soundControl.Dock = DockStyle.Fill;
            soundControl.Location = new Point(48, 28);
            soundControl.Margin = new Padding(3, 4, 3, 4);
            soundControl.MaximumSize = new Size(0, 20);
            soundControl.MinimumSize = new Size(200, 20);
            soundControl.Name = "soundControl";
            soundControl.Size = new Size(329, 20);
            soundControl.TabIndex = 4;
            soundControl.Volume = 1F;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 45F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 99F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
            tableLayoutPanel1.Controls.Add(button1, 0, 0);
            tableLayoutPanel1.Controls.Add(label1, 1, 0);
            tableLayoutPanel1.Controls.Add(soundControl, 1, 1);
            tableLayoutPanel1.Controls.Add(button2, 3, 1);
            tableLayoutPanel1.Controls.Add(textBox1, 3, 0);
            tableLayoutPanel1.Controls.Add(label2, 2, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.MaximumSize = new Size(0, 54);
            tableLayoutPanel1.MinimumSize = new Size(200, 54);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel1.Size = new Size(440, 54);
            tableLayoutPanel1.TabIndex = 5;
            tableLayoutPanel1.Paint += tableLayoutPanel1_Paint;
            // 
            // button2
            // 
            button2.Dock = DockStyle.Fill;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Location = new Point(383, 28);
            button2.Margin = new Padding(3, 4, 3, 4);
            button2.Name = "button2";
            button2.Size = new Size(54, 22);
            button2.TabIndex = 5;
            button2.Text = "✓";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // textBox1
            // 
            textBox1.Dock = DockStyle.Fill;
            textBox1.Location = new Point(383, 3);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(54, 23);
            textBox1.TabIndex = 6;
            textBox1.TextAlign = HorizontalAlignment.Center;
            textBox1.TextChanged += textBox1_TextChanged;
            textBox1.KeyPress += textBox1_KeyPress;
            // 
            // label2
            // 
            label2.Dock = DockStyle.Fill;
            label2.Location = new Point(284, 0);
            label2.Name = "label2";
            label2.Size = new Size(93, 24);
            label2.TabIndex = 7;
            label2.Text = "задержка (мс) :";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // AudioInputControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tableLayoutPanel1);
            Name = "AudioInputControl";
            Size = new Size(440, 54);
            Load += AudioInputControl_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Button button1;
        private Label label1;
        private SoundControl soundControl;
        private TableLayoutPanel tableLayoutPanel1;
        private Label label2;
        private Button button2;
        private TextBox textBox1;
    }
}
