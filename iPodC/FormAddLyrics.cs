using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace iPodC
{
    public partial class FormAddLyrics : Form
    {
        public FormAddLyrics()
        {
            InitializeComponent();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            iPodC.Properties.Settings.Default.lyricsFolder = this.lyricsFolder;
            this.Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormAddLyrics_Load(object sender, EventArgs e)
        {
            this.label_lyrics.Text = this.lyricsFolder;
            this.Top = Screen.AllScreens[0].Bounds.Height / 2 - this.Height / 2;
            this.Left = Screen.AllScreens[0].Bounds.Width / 2 - this.Width / 2;
            this.TopMost = true;
        }
    }
}