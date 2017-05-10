using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace iPodC
{
    public partial class FormShowLyrics : Form
    {
        public FormShowLyrics()
        {
            InitializeComponent();
        }

        private void FormShowLyrics_Load(object sender, EventArgs e)
        {
            Color TranKey = Color.Black;
            this.TransparencyKey = TranKey;
            this.BackColor = TranKey;

            this.label1_lyrics1.BackColor = TranKey;

            this.label1_lyrics2.BackColor = TranKey;

            this.label_move.BackColor = TranKey;



            this.Top = Screen.AllScreens[0].Bounds.Height * 5 / 6;
            this.Left = Screen.AllScreens[0].Bounds.Width / 2 - this.Width / 2;
            this.TopMost = true;


            this.lyricsScrollTimer = new Timer();
            this.lyricsScrollTimer.Interval = Int32.MaxValue;
            this.lyricsScrollTimer.Tick += new System.EventHandler(refreshLyrics);
            this.lyricsScrollTimer.Enabled = true;
 
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {

            mouse_offset = new Point(-e.X, -e.Y);
        }

        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                Point mousepos = Control.MousePosition;

                mousepos.Offset(mouse_offset.X, mouse_offset.Y);

                Location = mousepos;

            }
        }

        public void setLabelColor(Color skinColor)
        {
            label_move.ForeColor = skinColor;

            label1_lyrics1.ForeColor = skinColor;
            label1_lyrics2.ForeColor = skinColor;

            labelFlash_1.BackColor = this.BackColor;
            labelFlash_2.BackColor = this.BackColor;

            diseColor = Color.Gray;
        }

        public void clearLyrcis()
        {

            labelLyricsBackGround_1 = "";
            labelLyricsBackGround_2 = "";

            labelLyricsFore_1 = " Loading lyrics...";
            labelLyricsFore_2 = " ";
            label1_lyrics1.Invalidate();
            label1_lyrics2.Invalidate();
        }



 

        private static int trans60(int para)
        {
            int rtn = 0;

            int part1 = (para / 100) * 60;

            int part2 = para % 100;
            //Debug.WriteLine("(nextTimeStamp / 60) * 60: nextTimeStamp" + nextTimeStamp + "  part1  " +part1);
            //int timeslice = ((nextTimeStamp / 60) * 60 + nextTimeStamp % 60) - ((currentTimeStamp / 60) * 60 + currentTimeStamp % 60);
            //Debug.WriteLine("nextTimeStamp % 60" + nextTimeStamp % 60);
            rtn = part1 + part2;
            //Debug.WriteLine("totoal" + rtn);
            return rtn;

        }

        public static string getTimeFromInt(string para, int add)
        {
            int par = Int32.Parse(para.Replace(":", ""));
            int rtn = trans60(par) + add;

            double m2 = rtn / 60;
            double s2 = rtn % 60;
            if (s2 <= 0) s2 = 0;
            string temp = Form1.addzero(m2) + ":" + Form1.addzero(s2);
            return temp;
        }

        private void refreshLyrics(object sender, EventArgs e)
        {
            Debug.WriteLine("refreshLyrics");
 
            if (CURRENT_FLASH_LYRICS_LABEL_NO == FLASH_LYRICS_LABEL_NO_1)
            {
                label1_lyrics1.Invalidate();

                if (labelFlash_1.BackColor == label1_lyrics1.ForeColor)
                {
                    labelFlash_1.BackColor = this.BackColor;
 
                }
                else
                {
                    labelFlash_1.BackColor = label1_lyrics1.ForeColor;
 
                }

            }
            else
            {
                label1_lyrics2.Invalidate();

                if (labelFlash_2.BackColor == label1_lyrics2.ForeColor)
                {
                    labelFlash_2.BackColor = this.BackColor; 
                }
                else
                {
                    labelFlash_2.BackColor = label1_lyrics2.ForeColor; 
                }
            }
        }
        private void loadLyrics()
        {

            LyricsParser lp = new LyricsParser();

            lp.readLrcFile(this.lyricsInfo);
        }



        private void FormShowLyrics_VisibleChanged(object sender, EventArgs e)
        {

            if (Visible == false)
            {

                this.clearLyrcis();
                lyricsScrollTimer.Interval = Int32.MaxValue;
                //this.lyricsScrollTimer.Enabled = false;

                //this.lyricsScrollTimer.Tick -= new System.EventHandler(refreshLyrics);

                //this.lyricsScrollTimer.Stop();
                //lyricsScrollTimer = null;
            }

        }

        internal void setLyrics(string curText)
        {
            if (lyricsInfo.lyricsIndex == 0)
            {
                string rtn = lyricsInfo.getFirstLrc(curText);

                if (rtn != null)
                {
                    labelLyricsFore_1 = " " + rtn;

                    labelLyricsBackGround_1 = "";

                    this.labelFlash_1.BackColor = this.BackColor;

                    CURRENT_FLASH_LYRICS_LABEL_NO = FLASH_LYRICS_LABEL_NO_1;
                    label1_lyrics1.Invalidate();
                    setLyricsDetail(curText);
                }

            }
            else
            {
                setLyricsDetail(curText);
            }

        }

        private void setLyricsDetail(string curText)
        {
            string text = lyricsInfo.FindLrc(curText);

            if (text != null)
            {
                text = " " + text;
                this.currentTimeStamp = Int32.Parse(curText.Replace(":", ""));

                this.nextTimeStamp = lyricsInfo.nextTimeStamp;


                int timeslice = trans60(nextTimeStamp) - trans60(currentTimeStamp);
                if (timeslice == 0)
                MessageBox.Show("trans60");

                timeslice = timeslice == 0 ? 100 : timeslice * 1000;
                if (lyricsInfo.lyricsIndex % 2 == 0)
                {

                    labelLyricsFore_1 = text;
                    labelLyricsBackGround_1 = "";


                    this.labelFlash_1.BackColor = this.BackColor;

                    CURRENT_FLASH_LYRICS_LABEL_NO = FLASH_LYRICS_LABEL_NO_2;
                    label1_lyrics1.Invalidate();
                    timeslice = timeslice / (this.labelLyricsFore_2.Length);
                
                }
                else
                {

                    labelLyricsFore_2 = text;
                    labelLyricsBackGround_2 = "";

                    this.labelFlash_2.BackColor = this.BackColor;
                    CURRENT_FLASH_LYRICS_LABEL_NO = FLASH_LYRICS_LABEL_NO_1;
                    label1_lyrics2.Invalidate();
                    timeslice = timeslice / (this.labelLyricsFore_1.Length );
                     
                
                }

                this.lyricsScrollTimer.Interval = timeslice < 10 ? 10 : timeslice;

                //int lyrcisLength = labelLyricsFore_1.Length == 0 ? 1 : labelLyricsFore_1.Length;

              

            }
        }

        private void label1_lyrics2_Paint(object sender, PaintEventArgs e)
        {
            // Create string to draw.
            String drawString = labelLyricsFore_2; //label1_lyrics.Text;
            // Create font and brush.
            Font drawFont = label1_lyrics1.Font;// new Font("Arial", 12);
            SolidBrush drawBrush = new SolidBrush(label1_lyrics1.ForeColor);

            SolidBrush drawBrush2 = new SolidBrush(diseColor);
            // Create point for upper-left corner of drawing.
            float x = 0.0F;
            float y = 0.0F;
            // Set format of string.
            //StringFormat drawFormat = new StringFormat();
            //drawFormat.FormatFlags = StringFormatFlags.;
            // Draw string to screen.

            //if (CURRENT_FLASH_LYRICS_LABEL_NO == FLASH_LYRICS_LABEL_NO_1)
            //{
            //    e.Graphics.DrawString(drawString, drawFont, drawBrush2, x, y);
            //}else
            if (drawString.Length > 0 && labelLyricsBackGround_2.Length < drawString.Length)
            {

                e.Graphics.DrawString(drawString, drawFont, drawBrush2, x, y);
                //if (CURRENT_FLASH_LYRICS_LABEL_NO == FLASH_LYRICS_LABEL_NO_2)
                //{
                    labelLyricsBackGround_2 = labelLyricsBackGround_2.Length == 0 ? drawString.Substring(0, 1) : drawString.Substring(0, labelLyricsBackGround_2.Length + 1);
                    e.Graphics.DrawString(labelLyricsBackGround_2, drawFont, drawBrush, x, y);
                //}
            }

            else
            {

                e.Graphics.DrawString(drawString, drawFont, drawBrush, x, y);
            }

            drawBrush.Dispose();
            drawBrush2.Dispose();

        }
        private void label1_lyrics_Paint(object sender, PaintEventArgs e)
        {

            // Create string to draw.
            String drawString = labelLyricsFore_1; //label1_lyrics.Text;
            // Create font and brush.
            Font drawFont = label1_lyrics1.Font;// new Font("Arial", 12);
            SolidBrush drawBrush = new SolidBrush(label1_lyrics1.ForeColor);


            SolidBrush drawBrush2 = new SolidBrush(diseColor);

            // Create point for upper-left corner of drawing.
            float x = 0.0F;
            float y = 0.0F;
            // Set format of string.
            //StringFormat drawFormat = new StringFormat();
            //drawFormat.FormatFlags = StringFormatFlags.;
            // Draw string to screen.


            //if (CURRENT_FLASH_LYRICS_LABEL_NO != FLASH_LYRICS_LABEL_NO_1)
            //{
            //    e.Graphics.DrawString(drawString, drawFont, drawBrush2, x, y);
            //}else
            if (drawString.Length > 0 && labelLyricsBackGround_1.Length < drawString.Length)
            {

                e.Graphics.DrawString(drawString, drawFont, drawBrush2, x, y);

                //if (CURRENT_FLASH_LYRICS_LABEL_NO == FLASH_LYRICS_LABEL_NO_1)
                //{
                    labelLyricsBackGround_1 = labelLyricsBackGround_1.Length == 0 ? drawString.Substring(0, 1) : drawString.Substring(0, labelLyricsBackGround_1.Length + 1);
                    e.Graphics.DrawString(labelLyricsBackGround_1, drawFont, drawBrush, x, y);
                //}
            }

            else
            {

                e.Graphics.DrawString(drawString, drawFont, drawBrush, x, y);
            }

            drawBrush.Dispose();
            drawBrush2.Dispose();
        }

        internal void startLoadLyrcisThread(string album, string artistname, string songname, string URL)
        {
            if (!this.Visible) Visible = true;
            pauseLyrics(true);
            this.lyricsInfo = new LyricsInfo(album, artistname, songname, URL);

            this.lyricsScrollTimer.Interval = Int32.MaxValue;
            labelFlash_1.BackColor = this.BackColor;
            labelFlash_2.BackColor = this.BackColor;
            this.clearLyrcis();
            this.currentTimeStamp = 0;

            this.nextTimeStamp = 0;

            CURRENT_FLASH_LYRICS_LABEL_NO = FLASH_LYRICS_LABEL_NO_1;
  
            System.Threading.Thread loadLyricsThread =
                         new System.Threading.Thread(new System.Threading.ThreadStart(loadLyrics));
            loadLyricsThread.Start();
        }


        internal void pauseLyrics(bool pause)
        {
            this.lyricsScrollTimer.Enabled = pause;
        }

    }
}