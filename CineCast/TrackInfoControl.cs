using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CineCast
{
    public partial class TrackInfoControl : UserControl
    {
        public TrackInfo trackInfo = new TrackInfo();
        public TrackInfoControl()
        {
            InitializeComponent();
        }

        public void Initialize(TrackInfo trackInfo)
        {
            this.trackInfo = trackInfo;
            textBox1.Text = trackInfo.genre;
            textBox2.Text = trackInfo.title;
        }

        private void TrackInfoControl_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            trackInfo.genre = textBox1.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            trackInfo.title = textBox2.Text;
        }
    }
}
