using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Collections;
using System.Drawing.Drawing2D;
using System.IO;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using WMPLib;
using Microsoft.Win32;


namespace iPodC
{
    

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void InitOpenDialogDir()
        {
            string dir = Directory.GetCurrentDirectory();

            string cdcoverpath = iPodC.Properties.Settings.Default.cdCoverPath;

            if (cdcoverpath.Equals(""))
            {
                openFileDialog2.InitialDirectory = dir;
            }
            else
            {
                openFileDialog2.InitialDirectory = cdcoverpath;
            }
            openFileDialog1.InitialDirectory = dir;
        }

        private void InitCDCover()
        {
            string cdcoverpath = iPodC.Properties.Settings.Default.cdCoverPath;

            if (cdcoverpath != null && !cdcoverpath.Equals(""))
            {
                cdCoverList = new ArrayList();

                string[] extends = { "*.jpg", "*.jpeg", "*.bmp"};

                foreach (string ext in extends)
                {
                    string[] files = Directory.GetFiles(cdcoverpath, ext);

                    if (files != null && files.Length > 0)
                    {
                        cdCoverList.AddRange(files);

                    }
                }
                extends = (string[])cdCoverList.ToArray(typeof(string));
                sortListAscByModTime(extends, true);
                cdCoverList.Clear();
                cdCoverList.AddRange(extends);
                  
            }
        }

        private void SetCurrentSong()
        {
            if (Properties.Settings.Default.SongMark <=
                songlistView1.Items.Count - 1)
            {
                this.currentPlayingSongName = Properties.Settings.Default.SongMark;

                songlistView1.Items[currentPlayingSongName].Selected = true;
                songlistView1.Items[currentPlayingSongName].EnsureVisible();
            }
        }
        /*
        [DllImport("kernel32.dll")]

        private static extern bool SetProcessWorkingSetSize(

            IntPtr process,
            int minSize,
            int maxSize);


        private void CleanMemoryToSwap()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);

            }
        }
        */
        private void InitSkin()
        {
            this.Opacity = Properties.Settings.Default.Transparent;


            switch ((int)(Opacity * 100))
            {

                case 80:
                    toolStripMenuItem2.Checked = true;
                    break;

                case 70:

                    toolStripMenuItem3.Checked = true; 
                    break;

                case 60:
                    toolStripMenuItem4.Checked = true; 
                    
                    break;

                default:

                    toolStripMenuItem5.Checked = true;
                    break;

            }

            themeNo = Properties.Settings.Default.themeNo; 
        }





        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            mouse_offset = new Point(-e.X, -e.Y);
        }
         
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {

                Point mousepos = Control.MousePosition;

                mousepos.Offset(mouse_offset.X, mouse_offset.Y);

                Location = mousepos;

            }
        }

        private void addYourSongsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            //openFileDialog1.FilterIndex = 0;
            //openFileDialog1.RestoreDirectory = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((openFileDialog1.OpenFile()) != null)
                {
                    string[] filenames = openFileDialog1.FileNames;
                    sortListAscByModTime(filenames, true);

                    foreach (string filename in filenames)
                    {
                        AddSongListView(filename);
                    }

                }
            }

     
        }

        private static string GetFolderName(string origin_name)
        {
            string title = null;

           /* using (FileStream fs = File.OpenRead(origin_name))
            {
                ID3Tag tag = new ID3Tag();

                fs.Seek(-128, SeekOrigin.End);
                fs.Read(tag.Title, 0, tag.Title.Length);

                title = Encoding.Default.GetString(tag.Title);

                if (title.Equals(""))
                {

                //    string folderOrFile = origin_name.Substring(origin_name.LastIndexOf(@"\") + 1);
                 //   title = folderOrFile.Substring(0, folderOrFile.Length - 4);
                }

            }*/


            string folderOrFile = origin_name.Substring(origin_name.LastIndexOf(@"\") + 1);
            title = folderOrFile.Substring(0, folderOrFile.LastIndexOf("."));

            return title;


        }

        private void AddSongListView(string filename)
        {
            if (!File.Exists(filename)
                || (filename.ToLower().IndexOf(".mp3") < 0
                && filename.ToLower().IndexOf(".wma") < 0
                && filename.ToLower().IndexOf(".wav") < 0
                && filename.ToLower().IndexOf(".m4a") < 0
                && filename.ToLower().IndexOf(".cda") < 0
                && filename.ToLower().IndexOf(".mpg") < 0)) return;

            string name = GetFolderName(filename);

            if (!songListTable.Contains(name))
            {

                songListTable.Add(name, filename);
            }
            ListViewItem item1 = new ListViewItem(name);
            if (!songlistView1.Items.Contains(item1))
            {
                this.songlistView1.Items.Add(item1);
            }

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            //InitArcPlayPanel();
            
        }

        private void InitArcPlayPanel()
        {

            GraphicsPath myGraphicsPath = new GraphicsPath();

            myGraphicsPath.AddArc(new RectangleF(3, 3, panel_control.Width - 8, panel_control.Height - 8), 0, 360);
 
            this.panel_control.Region = new Region(myGraphicsPath);
        }

        private void LoadSongList()
        {
           // string commandsongpath = Environment.CommandLine[1].ToString();

           // MessageBox.Show(commandsongpath);
            /*
            if (commandsongpath.IndexOf("\" \"") > 0)
            {
                commandsongpath = commandsongpath.Substring(commandsongpath.IndexOf("' '"));
                MessageBox.Show(commandsongpath);
            }*/
            if (Properties.Settings.Default.SongList == null) return;
            songlistView1.Height = panel_SongDetail.Height ;
            foreach (string songpath in Properties.Settings.Default.SongList)
            {

                AddSongListView(songpath);

            }


        }


        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (Properties.Settings.Default.SkinStyle == SKIN09)
            {
                Form1_Paint_NEW(e);
            }
            else
            {
                Form1_Paint_OLD(e);
            }
        }

        private void Form1_Paint_NEW(PaintEventArgs e)
        {
            //  GraphicsPath myGraphicsPath = new GraphicsPath();

            if (linearBrush[0] == null)
            {
                InitRecAndGraph();
            }

            /*************************************/
            /**              Shadow             **/
            /*************************************/
            //drawShadow(e); 
   
            /*************************************/
            /**               Left              **/
            /*************************************/
            //e.Graphics.FillPath(linearBrush[0], myGraphicsPath[0]);

            //e.Graphics.FillPath(linearBrush[1], myGraphicsPath[1]);

            //e.Graphics.FillPath(linearBrush[2], myGraphicsPath[2]);

            //paintShell(skinColor, 4, 2, 0, 0, 10, height, e);
            //paintShell(skinColor, 5, 4, 10, 0, 20, height, e);
            //paintShell(skinColor, 5, 5, 30, 0, 20, height, e);
            
            // new shadow
            /*************************************/
            /**              Right              **/
            /*************************************/

            //e.Graphics.FillPath(linearBrush[3], myGraphicsPath[3]);

            //e.Graphics.FillPath(linearBrush[4], myGraphicsPath[4]);

            /*************************************/
            /**             Middle              **/
            /*************************************/

            //e.Graphics.FillPath(linearBrush[5], myGraphicsPath[5]);

            for (int index = 0; index < 6; index++)
            {
                e.Graphics.FillPath(linearBrush[index], myGraphicsPath[index]);
            }

            //paintShell(skinColor, 5, 4, width / 4, 0, width / 2, height, e);  

        }

        private void InitRecAndGraph()
        {
            float height = this.Height - 13;// 450;
            float width = this.Width;// 193;

            buildRecAndGraph(skinColor, 40, 200, 0, 0, 10, height, 0);
            buildRecAndGraph(skinColor, 70, 40, 10, 0, 20, height, 1);
            buildRecAndGraph(skinColor, 70, 70, 30, 0, 20, height, 2);
            buildRecAndGraph(skinColor, 200, 110, width - 10, 0, 10, height, 3);
            buildRecAndGraph(skinColor, 110, 30, width / 4 * 3, 0, width / 4 - 10, height, 4);
            buildRecAndGraph(skinColor, -30, 70, width / 4, 0, width / 2, height, 5);
        }

        private void paintShell(Color skinColor, int p, int p_3, float p_4, float p_5, float p_6, float p_7, PaintEventArgs e)
        {
            //float height = this.Height;// 450;
            //float width = this.Width;// 193;

            //int[] color = getRGBFromHSL(skinColor, p, p_3);
            int[] color = get6RGB(skinColor, p, p_3);
            RectangleF rec = new RectangleF(p_4, p_5, p_6, p_7);
            LinearGradientBrush linearBrush =
                 new LinearGradientBrush(rec,
                     Color.FromArgb(color[0], color[1], color[2]),
                     Color.FromArgb(color[3], color[4], color[5]),
                     180);

            GraphicsPath myGraphicsPath = new GraphicsPath();

            myGraphicsPath.AddRectangle(rec);

            e.Graphics.FillPath(linearBrush, myGraphicsPath);
            linearBrush.Dispose();
        }

        private GraphicsPath[] myGraphicsPath = new GraphicsPath[6];
        private LinearGradientBrush[] linearBrush = new LinearGradientBrush[6];

        private void buildRecAndGraph(Color skinColor, int p, int p_3, float p_4, float p_5, float p_6, float p_7, int index)
        {
            float height = this.Height;// 450;
            float width = this.Width;// 193;

            //int[] color = getRGBFromHSL(skinColor, p, p_3);
            int[] color = get6RGB(skinColor, p, p_3);
            RectangleF rec = new RectangleF(p_4, p_5, p_6, p_7);
            LinearGradientBrush _linearBrush =
                 new LinearGradientBrush(rec,
                     Color.FromArgb(color[0], color[1], color[2]),
                     Color.FromArgb(color[3], color[4], color[5]),
                     180);

            GraphicsPath _GraphicsPath = new GraphicsPath();

            _GraphicsPath.AddRectangle(rec);

            linearBrush[index] = _linearBrush;
            myGraphicsPath[index] = _GraphicsPath;

        }
 
        private int[] get6RGB(Color skinColor, int offset, int selectOffSet)
        {
            int[] returnv = new int[6];
            if (skinColor == Color.Black)
            {
                //offset = 110;
                //returnv[0] = offset;
                //returnv[1] = offset;
                //returnv[2] = offset;
                //selectOffSet = 80;
                //returnv[3] = selectOffSet;
                //returnv[4] = selectOffSet;
                //returnv[5] = selectOffSet;
                //int tmp = 0;
                //if (Properties.Settings.Default.SkinStyle == 1)
                //{
                //    tmp = 128;
                //    returnv[0] = tmp - offset < 0 ? 0 : tmp - offset;
                //    returnv[1] = tmp - offset < 0 ? 0 : tmp - offset;
                //    returnv[2] = tmp - offset < 0 ? 0 : tmp - offset;
                //    returnv[3] = tmp - selectOffSet < 0 ? 0 : tmp - selectOffSet;
                //    returnv[4] = tmp - selectOffSet < 0 ? 0 : tmp - selectOffSet;
                //    returnv[5] = tmp - selectOffSet < 0 ? 0 : tmp - selectOffSet;
                //}
                //else
                //{
                //    returnv[0] = tmp + offset;
                //    returnv[1] = tmp + offset;
                //    returnv[2] = tmp + offset;
                //    returnv[3] = tmp + selectOffSet;
                //    returnv[4] = tmp + selectOffSet;
                //    returnv[5] = tmp + selectOffSet;
                //}
            }
            else
            {
                 
                //returnv[0] = skinColor.R - offset < 0 ? 0 : skinColor.R - offset;
                //returnv[1] = skinColor.G - offset < 0 ? 0 : skinColor.G - offset;
                //returnv[2] = skinColor.B - offset < 0 ? 0 : skinColor.B - offset;
                //returnv[3] = skinColor.R - selectOffSet < 0 ? 0 : skinColor.R - selectOffSet;
                //returnv[4] = skinColor.G - selectOffSet < 0 ? 0 : skinColor.G - selectOffSet;
                //returnv[5] = skinColor.B - selectOffSet < 0 ? 0 : skinColor.B - selectOffSet;


                returnv[0] = getOffsetColor(skinColor.R, offset);
                returnv[1] = getOffsetColor(skinColor.G, offset);
                returnv[2] = getOffsetColor(skinColor.B, offset);
                returnv[3] = getOffsetColor(skinColor.R, selectOffSet);
                returnv[4] = getOffsetColor(skinColor.G, selectOffSet);
                returnv[5] = getOffsetColor(skinColor.B, selectOffSet);
            
            }
            return returnv;
        }

        private int[] getRGBFromHSL(Color skinColor, int offset, int selectOffSet)
        {
            int[] returnv = new int[6];
            if (skinColor == Color.Black)
            { 
            }
            else
            {
 
                //Color skinColor1 = RGBHSL.SetBrightness(skinColor, offset * 0.1);
                //returnv[0] = skinColor1.R;
                //returnv[1] = skinColor1.G;
                //returnv[2] = skinColor1.B;
                //skinColor1 = RGBHSL.SetBrightness(skinColor, selectOffSet * 0.1);
                //returnv[3] = skinColor1.R;
                //returnv[4] = skinColor1.G;
                //returnv[5] = skinColor1.B;
            }
            return returnv;
        }

        private int getOffsetColor(byte p, int offset)
        {
            return (p - offset < 0 ? 0 : (p - offset > 255 ? 255 : p - offset));
            //return (p - offset < 0 ? offset + p - offset : (p - offset > 255 ? 255 : p - offset));
        }

        private void Form1_Paint_OLD(PaintEventArgs e)
        {
            GraphicsPath myGraphicsPath = new GraphicsPath();

            //float radius = 50;
            //int offset = 0;// 2;
            float height = this.Height;// 450;
            float width = this.Width;// 193;
 


            /*************************************/
            /**              Shadow             **/
            /*************************************/
            //drawShadow(e);
            //int r = 0;
            //int g = 0;
            //int b = 0;

            int[] rgb = null;

            //int selectOffSet = 0;
            //int selectSkinColorR = 0;
            //int selectSkinColorG = 0;
            //int selectSkinColorB = 0;
            //byte black = 0;
            //if (skinColor.R == black && skinColor.G == black && skinColor.B == black)
            //{
                //offset = 30;
                //r = offset;
                //g = offset;
                //b = offset;
                //selectOffSet = 80;
                //selectSkinColorR = selectOffSet;
                //selectSkinColorG = selectOffSet;
                //selectSkinColorB = selectOffSet;

            //    rgb = get6RGB(skinColor, 30, 80);
            //}
            //else
            //{
                //offset = 110;
                //r = skinColor.R - offset < 0 ? 0 : skinColor.R - offset;
                //g = skinColor.G - offset < 0 ? 0 : skinColor.G - offset;
                //b = skinColor.B - offset < 0 ? 0 : skinColor.B - offset;
                //selectOffSet = 40;
                //selectSkinColorR = skinColor.R - selectOffSet < 0 ? 0 : skinColor.R - selectOffSet;
                //selectSkinColorG = skinColor.G - selectOffSet < 0 ? 0 : skinColor.G - selectOffSet;
                //selectSkinColorB = skinColor.B - selectOffSet < 0 ? 0 : skinColor.B - selectOffSet;

                rgb = get6RGB(skinColor, 110, 40);
            //}
            /*************************************/
            /**               Left              **/
            /*************************************/

            //LinearGradientBrush linearBrush =
            //    new LinearGradientBrush(new RectangleF(0, 0, width / 4, height),
            //        Color.FromArgb(selectSkinColorR, selectSkinColorG, selectSkinColorB),
            //        Color.FromArgb(r, g, b),
            //        180);

            LinearGradientBrush linearBrush =
                new LinearGradientBrush(new RectangleF(0, 0, width / 4, height),
                    Color.FromArgb(rgb[3], rgb[4], rgb[5]),
                    Color.FromArgb(rgb[0], rgb[1], rgb[2]),
                    180);

            myGraphicsPath = new GraphicsPath();

            myGraphicsPath.AddRectangle(new RectangleF(0, 0, width / 4, height - 13));

            e.Graphics.FillPath(linearBrush, myGraphicsPath);
            linearBrush.Dispose();
            // new shadow
            /*************************************/
            /**              Right              **/
            /*************************************/

            // old shadow
            //linearBrush =
            //    new LinearGradientBrush(new RectangleF(0, 0, width / 4, height),
            //        Color.FromArgb(r, g, b),
            //        Color.FromArgb(selectSkinColorR, selectSkinColorG, selectSkinColorB),
            //        180);

            linearBrush =
                new LinearGradientBrush(new RectangleF(0, 0, width / 4, height),
                    Color.FromArgb(rgb[0], rgb[1], rgb[2]),
                    Color.FromArgb(rgb[3], rgb[4], rgb[5]),
                    180);
            myGraphicsPath = new GraphicsPath();

            myGraphicsPath.AddRectangle(new RectangleF(width / 4 * 3, 0, width / 4, height - 13));

            e.Graphics.FillPath(linearBrush, myGraphicsPath);
            linearBrush.Dispose();


            /*************************************/
            /**             Middle              **/
            /*************************************/

            //offset = 45;
            //offset = 0; 
            //r = skinColor.R - offset < 0 ? 0 : skinColor.R - offset;
            //g = skinColor.G - offset < 0 ? 0 : skinColor.G - offset;
            //b = skinColor.B - offset < 0 ? 0 : skinColor.B - offset;

            //SolidBrush solidBrush = new SolidBrush(Color.FromArgb(selectSkinColorR, selectSkinColorG, selectSkinColorB));
            SolidBrush solidBrush = new SolidBrush(Color.FromArgb(rgb[3], rgb[4], rgb[5]));


            myGraphicsPath = new GraphicsPath();

            //myGraphicsPath.AddRectangle(new RectangleF(width / 4 - 5, 0, width / 2 + 10, height - 15));
            myGraphicsPath.AddRectangle(new RectangleF(width / 4, 0, width / 2, height - 13));

            e.Graphics.FillPath(solidBrush, myGraphicsPath);
            solidBrush.Dispose();
 
        }

        private void drawShadow(PaintEventArgs e)
        {
            float height = this.Height;// 450;
            float width = this.Width;// 193;
            float shadowHeight = 16;
            LinearGradientBrush linearBrush =
                new LinearGradientBrush(new RectangleF(0, height - shadowHeight, width / 2, shadowHeight + 10),
                    Color.FromArgb(Color.Black.R, Color.Black.G, Color.Black.B),
                    Color.FromArgb(skinColor.R, skinColor.G, skinColor.B),
                    95);

            GraphicsPath myGraphicsPath = new GraphicsPath();

            myGraphicsPath.AddRectangle(new RectangleF(0, height - shadowHeight, width / 2, shadowHeight));

            e.Graphics.FillPath(linearBrush, myGraphicsPath);
            linearBrush.Dispose();
            linearBrush =
               new LinearGradientBrush(new RectangleF(width / 2, height - shadowHeight, width / 2, shadowHeight + 10),
                   Color.FromArgb(Color.Black.R, Color.Black.G, Color.Black.B),
                   Color.FromArgb(skinColor.R, skinColor.G, skinColor.B),
                   85);

            myGraphicsPath = new GraphicsPath();

            myGraphicsPath.AddRectangle(new RectangleF(width / 2, height - shadowHeight, width / 2, shadowHeight));

            e.Graphics.FillPath(linearBrush, myGraphicsPath);
            linearBrush.Dispose();
        }

        private void shapeSongListPanel()
        {

            //panel_blackground
            GraphicsPath myGraphicsPath = new GraphicsPath();
            int offset = 3;
            PointF[] points = new PointF[8];
            points[0] = new PointF(offset, 0);
            points[1] = new PointF(0, offset);
            points[2] = new PointF(0, panel_blackground.Height - offset);
            points[3] = new PointF(offset, panel_blackground.Height);
            points[4] = new PointF(panel_blackground.Width - offset, panel_blackground.Height);
            points[5] = new PointF(panel_blackground.Width, panel_blackground.Height - offset);
            points[6] = new PointF(panel_blackground.Width, offset);
            points[7] = new PointF(panel_blackground.Width - offset, 0);
            myGraphicsPath.AddLines(points);
            // myGraphicsPath.AddLine(panel_blackground.Width - offset, 0, offset, 0);

            panel_blackground.Region = new Region(myGraphicsPath);
        }

        private void shapeMainRegion()
        {
            shapeOldMainRegion();
            //shapeNewMainRegion();
            
        }

        private void shapeOldMainRegion()
        {
            float height = this.Height;// 450;
            float width = this.Width;// 193;
            GraphicsPath myGraphicsPath = new GraphicsPath();
            //left up
            myGraphicsPath.AddLine(1, 1, 1, height - 15);

            PointF[] points = new PointF[2];
            //points[0] = new PointF(width / 3, height);
            //points[1] = new PointF(width / 3 * 2, height);
            //myGraphicsPath.AddLines(points);

            myGraphicsPath.AddLine(2, height - 14, 3, height - 13);
              
            //------------ shadow arc -------------
            //myGraphicsPath.AddArc(new RectangleF(1, height - radius, width, radius - 3), 160, -140);
            myGraphicsPath.AddLine(15, height - 11, width - 10, height - 11);
            //-------------------------------------

            myGraphicsPath.AddLine(width - 3, height - 13, width - 2, height - 14);
            myGraphicsPath.AddLine(width, height - 14, width, 1);
            //myGraphicsPath.AddArc(new RectangleF(0, 0, width, radius), 0, 180);

            //points[0] = new PointF(width, radius); 
            points[0] = new PointF(width - 2, 0);
            points[1] = new PointF(2, 0);
            //points[0] = new PointF(width / 4 * 3, radius - 5);
            //points[0] = new PointF(0, radius);


            myGraphicsPath.AddLines(points);


            //left down
            //myGraphicsPath.AddArc(new RectangleF(offset, height - radius, width, radius), 175, -170);
            //right down
            //myGraphicsPath.AddArc(new RectangleF(width - radius - offset, height - radius, radius, radius), 90, -90);
            //right up
            //myGraphicsPath.AddArc(new RectangleF(width - radius - offset, 0, radius, radius), 0, -90);
            //MessageBox.Show("1");

            //myGraphicsPath.AddRectangle(new RectangleF(0, 0, this.Width, this.Height));
            this.Region = new Region(myGraphicsPath);
        }

        private void shapeNewMainRegion()
        {
            float height = this.Height;// 450;
            float width = this.Width;// 193;
            GraphicsPath myGraphicsPath = new GraphicsPath();
            //left up
            myGraphicsPath.AddLine(1, 2, 1, height - 15);

            PointF[] points = new PointF[2];
            //points[0] = new PointF(width / 3, height);
            //points[1] = new PointF(width / 3 * 2, height);
            //myGraphicsPath.AddLines(points);

            //------------ shadow arc -------------
            //myGraphicsPath.AddArc(new RectangleF(1, height - radius, width, radius - 3), 160, -140);
            //myGraphicsPath.AddLine(40, height - 10, width - 40, height - 10);
            myGraphicsPath.AddLine(40, height - 15, width - 40, height - 15);
            //------------ right side --------------
            myGraphicsPath.AddLine(width, height - 15, width, 2);
            //myGraphicsPath.AddArc(new RectangleF(0, 0, width, radius), 0, 180);

            // top curve
            //points[0] = new PointF(width, radius); 
            points[0] = new PointF(width - 20, 2);
            points[1] = new PointF(20, 2);
            //points[0] = new PointF(width / 4 * 3, radius - 5);
            //points[0] = new PointF(0, radius);


            myGraphicsPath.AddLines(points);


            //left down
            //myGraphicsPath.AddArc(new RectangleF(offset, height - radius, width, radius), 175, -170);
            //right down
            //myGraphicsPath.AddArc(new RectangleF(width - radius - offset, height - radius, radius, radius), 90, -90);
            //right up
            //myGraphicsPath.AddArc(new RectangleF(width - radius - offset, 0, radius, radius), 0, -90);
            //MessageBox.Show("1");

            //myGraphicsPath.AddRectangle(new RectangleF(0, 0, this.Width, this.Height));
            this.Region = new Region(myGraphicsPath);
        }
        

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            SaveSongList();

            SaveTransparent();

            SavePlayMode();

            SaveCurrentSong();
 
            Properties.Settings.Default.isThemeApplied = isThemeApplied;

            Properties.Settings.Default.skinColor = skinColor;

            Properties.Settings.Default.panelColor = panelColor;

            Properties.Settings.Default.songListRandom = randomSongListTable;

            if (Properties.Settings.Default.songLibrary == null)
            {
                //printError("Song library is null");
            }
            else
            {
                //printError("Song library is filled: " + Properties.Settings.Default.songLibrary.Count);
            }
            Properties.Settings.Default.Save();
 

        }

        private void SaveCurrentSong()
        {
            Properties.Settings.Default.SongMark = currentPlayingSongName;
        }

        private void SavePlayMode()
        {
            if (this.orderToolStripMenuItem.Checked)
            {
                Properties.Settings.Default.IsRandom = 1;

                return;
            }
            if (this.randomToolStripMenuItem.Checked)
            {
                Properties.Settings.Default.IsRandom = 2;

                return;
            }
            if (reverseToolStripMenuItem.Checked){
                Properties.Settings.Default.IsRandom = 3;

                return;
            }

        }

        private void SaveTransparent()
        { 
            if (toolStripMenuItem2.Checked)
            {
                Properties.Settings.Default.Transparent = 0.80;

                return;
            }

            if (toolStripMenuItem3.Checked)
            {
                Properties.Settings.Default.Transparent = 0.70;

                return;
            }

            if (toolStripMenuItem4.Checked)
            {
                Properties.Settings.Default.Transparent = 0.60;

                return;
            }

            if (toolStripMenuItem5.Checked)
            {
                Properties.Settings.Default.Transparent = 1;

                return;
            }


        }

        private void SaveSongList()
        {

            if (Properties.Settings.Default.SongList == null)
            {

                Properties.Settings.Default.SongList = new StringCollection();
            }
            else
            {


                Properties.Settings.Default.SongList.Clear();
            }


            Properties.Settings.Default.themeNo = themeNo;

            foreach (ListViewItem songpath in this.songlistView1.Items)
            {

                Properties.Settings.Default.SongList.Add((string)this.songListTable[songpath.Text]);
            }

            Properties.Settings.Default.AlwaysOnTop = menuItem_alwaysontop.Checked;

        }

        private void button_stop_Paint(object sender, PaintEventArgs e)
        {

            InitButtonStopRegion(e);

        }

        
        private void SetButtonStopRegion()
        {

            GraphicsPath myGraphicsPath = new GraphicsPath();
            
            RectangleF rect = new RectangleF(2, 2, button_stop.Width - 8, button_stop.Height - 8);

            myGraphicsPath.AddEllipse(rect);

            this.button_stop.Region = new Region(myGraphicsPath);
        }

        LinearGradientBrush stopButtonBrush;

        private void InitStopButtonBrush()
        {
            RectangleF rect = new RectangleF(0, 0, button_stop.Width, button_stop.Height);
            byte black = 0;
            int r = 0;
            int g = 0;
            int b = 0;
            int selectSkinColorR = 0;
            int selectSkinColorG = 0;
            int selectSkinColorB = 0;
            int direction = 45;
            if (skinColor.R == black && skinColor.G == black && skinColor.B == black)
            {
                //int offset = 60;
                //r = offset;
                //g = offset;
                //b = offset;


                //int selectOffSet = 80;
                //selectSkinColorR = selectOffSet;
                //selectSkinColorG = selectOffSet;
                //selectSkinColorB = selectOffSet;
            }
            else
            {
                int offset = 70;
                int selectOffSet = 30;
                if (Properties.Settings.Default.SkinStyle == SKIN09)
                {
                    direction = 180;
                    offset = 60;
                    selectOffSet = -40;
                }

                r = getOffsetColor(skinColor.R, offset);
                g = getOffsetColor(skinColor.G, offset);
                b = getOffsetColor(skinColor.B, offset);


                selectSkinColorR = getOffsetColor(skinColor.R, selectOffSet);
                selectSkinColorG = getOffsetColor(skinColor.G, selectOffSet);
                selectSkinColorB = getOffsetColor(skinColor.B, selectOffSet);
            }
            stopButtonBrush =
                new LinearGradientBrush(rect,
                    Color.FromArgb(selectSkinColorR, selectSkinColorG, selectSkinColorB),
                    Color.FromArgb(r, g, b),
                    direction);

        }

        private void InitButtonStopRegion(PaintEventArgs e)
        {
            if (stopButtonBrush == null)
            {
                InitStopButtonBrush();
            }
            // Draw Inner Rim to screen.
            if (e != null && stopButtonBrush != null)
                e.Graphics.FillRegion(stopButtonBrush, button_stop.Region);

        }

        private void button_stop_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("method button_stop_Click");

            stopMediaplay();
        }

        private float buttonPathHeight = 8;

        private void button_play_Paint(object sender, PaintEventArgs e)
        {

            //GraphicsPath myGraphicsPath = new GraphicsPath();
 
            //float top = 5;
            ////float but = 15;
            //float xline = 4;

            ///** |\ || ||    32 23         **/
            ///** |/ || ||                  **/

            //PointF[] points = new PointF[3];
            //points[0] = new PointF(xline, top);
            //points[1] = new PointF(xline, top + buttonPathHeight);
            //points[2] = new PointF(12, 9);
            //myGraphicsPath.AddLines(points);
            //myGraphicsPath.AddRectangle(new RectangleF(xline + 12, top, 2, buttonPathHeight));
            //myGraphicsPath.AddRectangle(new RectangleF(xline + 16, top, 2, buttonPathHeight));
 
            //this.button_play.Region = new Region(myGraphicsPath);
        }

        private void Setbutton_playRegion()
        {

            GraphicsPath myGraphicsPath = new GraphicsPath();

            float top = 5;
           // float but = 15;
            float xline = 4;

            /** |\ || ||    32 23         **/
            /** |/ || ||                  **/

            PointF[] points = new PointF[3];
            points[0] = new PointF(xline, top);
            points[1] = new PointF(xline, top + buttonPathHeight);
            points[2] = new PointF(12, 9);
            myGraphicsPath.AddLines(points);
            myGraphicsPath.AddRectangle(new RectangleF(xline + 12, top + 1, 2, buttonPathHeight));
            myGraphicsPath.AddRectangle(new RectangleF(xline + 16, top + 1, 2, buttonPathHeight));

            this.button_play.Region = new Region(myGraphicsPath);
        }
        private void button_play_Click(object sender, EventArgs e)
        {

            Debug.WriteLine("method button_play_Click");

            PlaySong();

        
        }

        private void button_next_Click(object sender, EventArgs e)
        {

            if (isplaying)
            {
                PlayNextSong();
            }

        }

        private void button_previous_Click(object sender, EventArgs e)
        {

            Debug.WriteLine("method button_back_Click");

            if (isplaying)
            {
                PlayPreviousSong();
            }

        }

        private void button_next_Paint(object sender, PaintEventArgs e)
        {
             
        }

        private void Setbutton_nextRegion()
        {

            GraphicsPath myGraphicsPath = new GraphicsPath();
            PointF[] points = new PointF[12];
            float top = 5;
            float but = 12;
            float xline = 4;
            float middleHeight = 9.0f;
            float width = 18;
            float middleWidth = 11;

            /** |\|\||    32 23         **/
            /** |/|/||                  **/

            points[0] = new PointF(xline, top);
            points[1] = new PointF(xline, but);
            points[2] = new PointF(middleWidth, middleHeight);
            points[3] = new PointF(middleWidth, but);
            points[4] = new PointF(width, middleHeight);
            points[5] = new PointF(width, but + 1);
            points[6] = new PointF(width + 2, but);
            points[7] = new PointF(width + 2, top + 1);
            points[8] = new PointF(width, top + 1);
            points[9] = new PointF(width, middleHeight);
            points[10] = new PointF(middleWidth, top);
            points[11] = new PointF(middleWidth, middleHeight);

            myGraphicsPath.AddLines(points);

            //myGraphicsPath.AddRectangle(new RectangleF(4, 5, 25, 10));

            this.button_next.Region = new Region(myGraphicsPath);
        }
        private void button_previous_Paint(object sender, PaintEventArgs e)
        { 
        }

        private void Setbutton_previousRegion()
        {

            GraphicsPath myGraphicsPath = new GraphicsPath();
            PointF[] points = new PointF[12];
            float top = 5;
            float but = 13;
            float xline = 7;
            float middleHeight = 9;
            float width = 23;
            float middleWidth = 16;

            /** | |/|/|    32 23         **/
            /** | |\|\|                  **/

            points[0] = new PointF(xline, top + 1);
            points[1] = new PointF(xline, but);
            points[2] = new PointF(xline + 2, but);
            points[3] = new PointF(xline + 2, middleHeight);
            points[4] = new PointF(middleWidth, but);
            points[5] = new PointF(middleWidth, middleHeight);
            points[6] = new PointF(width, but);
            points[7] = new PointF(width, top);
            points[8] = new PointF(middleWidth, middleHeight);
            points[9] = new PointF(middleWidth, top);
            points[10] = new PointF(xline + 2, middleHeight);
            points[11] = new PointF(xline + 2, top);
            myGraphicsPath.AddLines(points);
            // myGraphicsPath.AddRectangle(new RectangleF(4, 6, 22, 12));

            this.button_previous.Region = new Region(myGraphicsPath);
        }

        private void songlistView1_DoubleClick(object sender, EventArgs e)
        {
            Debug.WriteLine("method songlistView1_DoubleClick");

            isplaying = false;

            this.PlaySong();
        }

        private void stopMediaplay()
        {

            this.label1.Text = "iPod";
            isplaying = false;
            this.buttonMenu.Select();
            this.axWindowsMediaPlayer1.close();
            this.labelProcessBlue.Width = 0;
            this.labelSongTimer1.Text = "00:00";
            this.formShowLyrics.Visible = false;

            if (songDetail_Ticker != null)
            {
                songDetail_Ticker.Interval = Int32.MaxValue;
                //songDetail_Ticker.Enabled = false;
                //this.songDetail_Ticker.Tick -= new System.EventHandler(refreshProgress);
                //songDetail_Ticker.Stop();
            }

        }



        private void setPlayPanel()
        {
            this.labelNumOfList.Text = (currentPlayingSongName + 1) + " of " + this.songlistView1.Items.Count;

            labelProcessBlue.Width = 0;
            //this.labelSongName.Text = axWindowsMediaPlayer1.currentMedia.name;// songlistView1.Items[currentPlayingSongName].Text;

            this.labelSongTimer1.Text = "00:00";
            //this.songDetail_Ticker = new Timer();

            //this.songDetail_Ticker.Enabled = true;

            this.songDetail_Ticker.Interval = 1000;
            //this.songDetail_Ticker.Tick += new System.EventHandler(refreshProgress);//refreshProgress  refreshPlayerPanel


            setPlayListShow(false);
 
            //loadLyrics();
        }





        private void setCDCover()
        {
            
            //currentAlbumArtPath = "";
            //System.Threading.Thread loadAlbumArtThread = new System.Threading.Thread(new System.Threading.ThreadStart(loadAlbumArtProc));
            //loadAlbumArtThread.Start();

        }

        private void loadAlbumArtProc()
        {

            string url = "";

            //DateTime orgDT = DateTime.Now;
            //DateTime dt = orgDT;
            //while (url.Equals("") && orgDT.AddMilliseconds(500) > dt)
            //{
                try
                {
                    //dt = DateTime.Now;

                    //if (dt.Millisecond % 10 == 0)
                    //{

                        IWMPMedia3 firstMedia = (IWMPMedia3)axWindowsMediaPlayer1.currentMedia;

                        IWMPMetadataPicture obj = (IWMPMetadataPicture)firstMedia.getItemInfoByType("WM/Picture", "", 0);

                        //"vnd.ms.wmhtml://localhost/WMPfabcb99d-645c-4e5f-ad20-291f675ceb37.jpg"                    
                        url = obj.URL;


                        if (url != null && !url.Equals(""))
                        {
                            int start = url.LastIndexOf("/") + 1;
                            int length = url.LastIndexOf(".") - start;
                            string tempPicName = url.Substring(start, length);

                            string tempPicPath = getFullIECachePath(tempPicName);

                            if (!tempPicPath.Equals(""))
                            {
                                string currentAlbumArtPath = tempPicPath;
                                loadAlbumArt(currentAlbumArtPath);
                                //break;
                            }
                        }
                    //}

                }
                catch (Exception aaa)
                {
                    Debug.WriteLine(aaa.Message);
                }
            //}
            if (url.Equals(""))
            {
                if (cdCoverList != null && cdCoverList.Count > 0)
                {
                    if (cdCoverCount > cdCoverList.Count - 1)
                    {
                        cdCoverCount = 0;
                    }

                    string currentAlbumArtPath = cdCoverList[cdCoverCount++].ToString();
                    loadAlbumArt(currentAlbumArtPath);
                } 

            }
      
            
        }

        private void loadAlbumArt(string filepath)
        {
            if (filepath != null && !filepath.Equals(""))
            {
                using (Stream s = new FileStream(filepath, FileMode.Open, FileAccess.Read))
                {
                    //currentAlbumArtPath = "";
                    pictureBox_Logo.BackgroundImage = Image.FromStream(s);
                    if (cdCoverShadow != null) cdCoverShadow.Dispose();
                    cdCoverShadow = rotateImage(pictureBox_Logo.BackgroundImage);
                    panel_SongDetail.Invalidate();
                    //pictureBox_shadow.BackgroundImage =bmp;// rotateImage((Bitmap)Image.FromStream(s), 0);
                    s.Close();
                }
            }
        }

        private string getFullIECachePath(string tempPicName)
        {
            string returnpath ="";
            try
            {
                object en = Environment.GetEnvironmentVariables();
                //DirectoryInfo dirinfo = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyComputer));
                //string internetpath = @"C:\Users\jean\AppData\Local\Microsoft\Windows\Temporary Internet Files\";
                //string internetpath = @"C:\TEMP\Temporary Internet Files\";// Environment.GetFolderPath(Environment.SpecialFolder.InternetCache); //Temporary Internet Files\

//                string internetpath = @"C:\Documents and Settings\yioc\Local Settings\Temporary Internet Files";
                string internetpath = Environment.GetFolderPath(Environment.SpecialFolder.InternetCache);

                if (internetpath.Equals(""))
                {
                    internetpath = @"C:\TEMP\Temporary Internet Files\";
                }


                //string[] folders = Directory.GetDirectories(internetpath); //Temporary Internet Files\Content.IE5

                returnpath = interateFolders(internetpath, tempPicName);

               
                 
            }
            catch (Exception file)
            {
                printError("GetCapacity_folders : " + file.Message);
            }
            return returnpath;
        }

        private string interateFolders(string path, string tempPicName)
        {
            string rtnPath = "";

            string[] subfiles = Directory.GetFiles(path, tempPicName + "*.jpg");

            foreach (string file in subfiles)
            {
                if (file != null && file != "")
                {
                    rtnPath = file;
                    break;
                }
            }
            if (rtnPath.Equals(""))
            {
                string[] folders = Directory.GetDirectories(path);

                foreach (string folder in folders)
                {
                    rtnPath = interateFolders(folder, tempPicName);
                    if (!rtnPath.Equals("")) break;
                    
                }
            }

            return rtnPath;
        }

        private void refreshPlayerPanel(object sender, EventArgs e)
        {
            //labelProcess.Invalidate();
            //panel_SongDetail.Invalidate();
            //panel_blackground.Invalidate();
            panel_SongDetail.Invalidate();
        }

        public static string addzero(double m)
        {
            int t = (int)Math.Floor(m);

            return t.ToString().Length < 2 ? "0" + t.ToString() : t.ToString();
        }

        private void PlaySong()
        {
            Debug.WriteLine("method PlaySong");
            // this.nextsong_Ticker.Stop();

            this.buttonMenu.Select();
            if (songlistView1.Items.Count == 0) return;
            if (!isplaying)
            {
                bool isselect = false;
                int num = 0;

                //foreach (ListViewItem songinfo in this.songlistView1.Items)
                //{
                //    songinfo.ForeColor = Color.MidnightBlue;
                //}

                foreach (ListViewItem songinfo in this.songlistView1.SelectedItems)
                {
                    //if (songinfo.Selected)
                    //{
                    //songinfo.ForeColor = Color.RoyalBlue;
                   
                    num = songlistView1.Items.IndexOf(songinfo);//
                    isselect = true;

                }

                if (!isselect && songlistView1.Items.Count > 0)
                {
                    if (orderToolStripMenuItem.Checked)
                    {
                        num = 0;
                        
                    }
                    else
                    {
    
                        num = getRandomSong();

                    }
                }

                if (!randomSongListTable.Contains(songlistView1.Items[num].Text))
                    randomSongListTable.Add(songlistView1.Items[num].Text);
                this.label1.Text = "Now Playing";
                songlistView1.Items[num].Selected = true;
                songlistView1.Items[num].EnsureVisible();
                //songinfo.EnsureVisible();
                //songinfo.ForeColor = Color.RoyalBlue;
                this.axWindowsMediaPlayer1.URL = songListTable[songlistView1.Items[num].Text].ToString();
                this.currentPlayingSongName = num;
                // this.axWindowsMediaPlayer1.Ctlcontrols.play();
                this.isplaying = true;
                this.isPause = false;
                isselect = true;


                this.setPlayPanel();
                    
                return;

            }
            else
            {
                if (isPause)
                {

                    this.axWindowsMediaPlayer1.Ctlcontrols.play();


                    isPause = false;
                    formShowLyrics.pauseLyrics(!isPause);
                    return;
                }
                else
                {
                    this.axWindowsMediaPlayer1.Ctlcontrols.pause();

                    isPause = true;
                    formShowLyrics.pauseLyrics(!isPause);
                    return;

                }
            }



        }

        private int getRandomSong()
        {

           int num = new Random().Next(songlistView1.Items.Count - 1);

            //this.songListTable[songlistView1.Items[num].Text].ToString();

            string songname = songlistView1.Items[num].Text;
            //check the selected song
            while (this.randomSongListTable.Contains(songname))
            {
                if (this.randomSongListTable.Count >= Math.Floor(this.songlistView1.Items.Count * 0.8))
                {
                    int numberOfSong = 0;
                    foreach (ListViewItem lvi in songlistView1.Items)
                    {
                        if (!this.randomSongListTable.Contains(lvi.Text))
                        {
                            return returnNumber(numberOfSong);
                          
                        }
                        numberOfSong++;

                    }
                    if (this.songlistView1.Items.Count == numberOfSong)
                    {
                        randomSongListTable.Clear();
                        return returnNumber(0);

                    }

                }
                else
                {
                    num = new Random().Next(songlistView1.Items.Count - 1);

                    songname = songlistView1.Items[num].Text;
                   
                }
            }

            return returnNumber(num);
        }

        private int returnNumber(int num)
        {
            this.randomSongListTable.Add(songlistView1.Items[num].Text);

            return num;
        }

        /*
      switch (e.newState)
    {
        case 0:    // Undefined
            currentStateLabel.Text = "Undefined";
            break;

        case 1:    // Stopped
            currentStateLabel.Text = "Stopped";
            break;

        case 2:    // Paused
            currentStateLabel.Text = "Paused";
            break;

        case 3:    // Playing
            currentStateLabel.Text = "Playing";
            break;

        case 4:    // ScanForward
            currentStateLabel.Text = "ScanForward";
            break;

        case 5:    // ScanReverse
            currentStateLabel.Text = "ScanReverse";
            break;

        case 6:    // Buffering
            currentStateLabel.Text = "Buffering";
            break;

        case 7:    // Waiting
            currentStateLabel.Text = "Waiting";
            break;

        case 8:    // MediaEnded
            currentStateLabel.Text = "MediaEnded";
            break;

        case 9:    // Transitioning
            currentStateLabel.Text = "Transitioning";
            break;

        case 10:   // Ready
            currentStateLabel.Text = "Ready";
            break;

        case 11:   // Reconnecting
            currentStateLabel.Text = "Reconnecting";
            break;

        case 12:   // Last
            currentStateLabel.Text = "Last";
            break;

        default:
            currentStateLabel.Text = ("Unknown State: " + e.newState.ToString());
            break;
    }

         */
        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            Debug.WriteLine("method axWindowsMediaPlayer1_PlayStateChange" + e.newState);
            
            // if stopped, enter
            // if started, enter

            switch (e.newState)
            {
        
                case 3:    // Playing
                    //currentStateLabel.Text = "Playing";
                    if (isplaying && !isPause)
                    {
                        string songname = axWindowsMediaPlayer1.currentMedia.getItemInfo("Title");
                        string artistname = axWindowsMediaPlayer1.currentMedia.getItemInfo("Author");
                        string album = axWindowsMediaPlayer1.currentMedia.getItemInfo("WM/AlbumTitle");

                        IsLyricsLoaded = LyricsNotLoaded;
                        if (Properties.Settings.Default.ISSHOWLYRICS)
                        {

                            formShowLyrics.startLoadLyrcisThread(album, artistname, songname, axWindowsMediaPlayer1.URL);
                        }
                        songname = limitSongNameLength(songname);
                        artistname = limitSongNameLength(artistname);
                        album = limitSongNameLength(album);

                        this.labelSongName.Text = artistname + "\n" + songname + "\n" + album;// artistname;


                        loadAlbumArtProc();

                        //this.labelSongName.Text = lyrics;



                    } 
                    break;
             
                 
                case 8:    // MediaEnded
                    //currentStateLabel.Text = "MediaEnded";
                    if (isplaying && !isPause)
                    {
                        this.nextsong_Ticker = new Timer();

                        this.nextsong_Ticker.Enabled = true;

                        this.nextsong_Ticker.Interval = 100;
                        this.nextsong_Ticker.Tick += new System.EventHandler(this.PlayNextSongTick);
                        
                    }
                    break;
 
                default:
                    //currentStateLabel.Text = ("Unknown State: " + e.newState.ToString());
                   
                    break;
            }

            //if ((e.newState == (int)WMPLib.WMPPlayState.wmppsMediaEnded))
            //{
                //if (isplaying && !isPause)
                //{
                    //isplaying = false;
                    //this.axwindowsmediaplayer1.ctlcontrols.play();
                  
                    //this.nextsong_Ticker.Start();
                    //if (this.formShowLyrics != null)
                    //{
                    //    formShowLyrics.Visible = false;
                    //    IsLyricsLoaded = LyricsNotLoaded;
                    //}
                    //Debug.WriteLine("wmppsMediaEnded");
                //}
            //}
            //if ((e.newState == (int)WMPLib.WMPPlayState.wmppsPlaying))
            //{
            //    Debug.WriteLine("wmppsPlaying");

               
            //}
 
       
            
        }


        private void axWindowsMediaPlayer1_CurrentMediaItemAvailable(object sender,
            AxWMPLib._WMPOCXEvents_CurrentMediaItemAvailableEvent e)
        {

            Debug.WriteLine("axWindowsMediaPlayer1_CurrentMediaItemAvailable");

          
        }


        private void PlayPreviousSong()
        {
            string name = "";

            int num = 0;
            if (randomToolStripMenuItem.Checked)
            {
                num = getRandomSong();

            }
            else
            {
                num = --currentPlayingSongName;
                if (num >= 0)
                {
                }
                else
                {
                    num = songlistView1.Items.Count - 1;
                }
            
            }

            if (!randomSongListTable.Contains(songlistView1.Items[num].Text))
                randomSongListTable.Add(songlistView1.Items[num].Text);
            name = songlistView1.Items[num].Text;
            songlistView1.Items[num].EnsureVisible();
            songlistView1.Items[num].Selected = true;
            currentPlayingSongName = num;
            this.axWindowsMediaPlayer1.close();
            this.axWindowsMediaPlayer1.URL = songListTable[name].ToString();

            this.buttonMenu.Select();
            
            setPlayPanel();
            
            isplaying = true;

            isPause = false;
           

        }



        private void songlistView1_KeyUp(object sender, KeyEventArgs e)
        {
            //Debug.WriteLine("songlistView1_KeyUp:" + e.KeyCode);
            switch (e.KeyCode)
            {
                case Keys.Delete:
                    deleteSongName(false);

                    break;


                case Keys.MediaNextTrack:
                    if (isplaying)
                        PlayNextSong();
                    break;

                case Keys.MediaPlayPause:

                    PlaySong();
                    break;

                case Keys.MediaPreviousTrack:
                    if (isplaying)
                        PlayPreviousSong();
                    break;

                case Keys.MediaStop:

                    stopMediaplay();
                    break;
                case Keys.D:
                    if (e.Control)
                    {
                        deleteSongName(true);
                    }
                    break;
                case Keys.F5:
                    PlaySong();
                    break;
                case Keys.A:
                    if (e.Control) PlaySong();
                    break;
                case Keys.NumPad2:
                    PlaySong();
                    break;
                case Keys.S:
                    if (e.Control) stopMediaplay();
                    break;
                case Keys.F6:
                    stopMediaplay();
                    break;
                case Keys.NumPad5:
                    stopMediaplay();
                    break;
                case Keys.F7:
                    if (isplaying)
                        PlayPreviousSong();
                    break;

                case Keys.Z:
                    if (e.Control && isplaying)
                        PlayPreviousSong();
                    break;
                case Keys.NumPad4:
                    if (isplaying)
                        PlayPreviousSong();
                    break;

                case Keys.F8:
                    if (isplaying)
                        PlayNextSong();
                    break;

                case Keys.X:
                    if (e.Control && isplaying)
                        PlayNextSong();
                    break;
                case Keys.NumPad6:
                    if (isplaying)
                        PlayNextSong();
                    break;
                case Keys.Left:
                    if (e.Control)
                    {
                        setFastForward(-5);
                    }
                    break;
                case Keys.Right:
                    if (e.Control)
                    {

                        setFastForward(5);
                    }
                    break;
                case Keys.Down:
                    if (e.Control && isplaying)
                    {
                        axWindowsMediaPlayer1.settings.volume -= 5;
                    }
                    break;
                case Keys.F9:
                    if (isplaying)
                    {
                        axWindowsMediaPlayer1.settings.volume -= 5; 
                    }
                    break;
                case Keys.Up:
                    if (e.Control && isplaying)
                    {
                        axWindowsMediaPlayer1.settings.volume += 5;
                    }
                    break;
                case Keys.F10:

                    if (isplaying)
                    {
                        axWindowsMediaPlayer1.settings.volume += 5; 

                    }
                    break;
                case Keys.F11:

                    if (isplaying)
                    {
                        if (!axWindowsMediaPlayer1.settings.mute)
                        {
                            axWindowsMediaPlayer1.settings.mute = true;
                        }
                        else
                        {
                            axWindowsMediaPlayer1.settings.mute = false;
                        }

                    }
                    break;

                default:
                    break;
            }
        }

        private void setFastForward(int p)
        {
            
            axWindowsMediaPlayer1.Ctlcontrols.currentPosition += p;
            this.labelProcessBlue.Width = (int)(axWindowsMediaPlayer1.Ctlcontrols.currentPosition
                / axWindowsMediaPlayer1.currentMedia.duration * labelProcess.Width);

                //axWindowsMediaPlayer1.currentMedia.duration * e.X / labelProcess.Width;
             
        }

        private void deleteSongName(bool isPhysicalDelete)
        {

            foreach (ListViewItem lvi in this.songlistView1.SelectedItems)
            {
                this.randomSongListTable.Remove(songlistView1.Items.IndexOf(lvi));
                string filepath = songListTable[lvi.Text].ToString();
                if (isPhysicalDelete && File.Exists(filepath))
                {
                    File.Delete(filepath);
                }
                this.songListTable.Remove(lvi.Text);
                songlistView1.Items.Remove(lvi);
            }
        }

        private void PlayNextSong()
        {
            string name = "";

            int num = ++currentPlayingSongName;
            if (randomToolStripMenuItem.Checked)
            {
                num = getRandomSong();

            }
            else
            {
                if ((songlistView1.Items.Count - 1) < num)
                {
                    num = 0;
                }
            }
            if (!randomSongListTable.Contains(songlistView1.Items[num].Text))
                randomSongListTable.Add(songlistView1.Items[num].Text);
            name = songlistView1.Items[num].Text;
            songlistView1.Items[num].Selected = true;
            songlistView1.Items[num].EnsureVisible();
            currentPlayingSongName = num;

            this.axWindowsMediaPlayer1.close();
            this.axWindowsMediaPlayer1.URL = songListTable[name].ToString();

            this.buttonMenu.Select();
            
            isplaying = true;
            isPause = false;

            setPlayPanel();

        }



        // if stopped, enter
        // if started, enter
        private void PlayNextSongTick(object sender, EventArgs e)
        {
            this.nextsong_Ticker.Stop();

            //
            if (reverseToolStripMenuItem.Checked)
            {
                this.PlayPreviousSong();
            }
            else
            {
                PlayNextSong();
            }

        }

        private void minutesLaterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShutdownTick(5);

            this.clearCheckedMenuItem(sleepTimerToolStripMenuItem);

            this.minutesLaterToolStripMenuItem.Checked = true;
            
        }

        private void ShutdownTick(int p)
        {
            if (shutdownTimer != null)
            {
                this.shutdownTimer.Enabled = false;

                this.shutdownTimer = null;
            }

            this.shutdownTimer = new Timer();

            shutdownTimer.Enabled = true;

            shutdownTimer.Interval = p * 60 * 1000;

            //shutdownTimer.Interval =  10;

            this.shutdownTimer.Tick += new System.EventHandler(this.ShutdownComputer);
        }

        private void minutesLaterToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            ShutdownTick(15);

            this.clearCheckedMenuItem(sleepTimerToolStripMenuItem);

            this.minutesLaterToolStripMenuItem1.Checked = true;
           
        }

        private void minutesLaterToolStripMenuItem2_Click(object sender, EventArgs e)
        {

            ShutdownTick(30);
            this.clearCheckedMenuItem(sleepTimerToolStripMenuItem);

            this.minutesLaterToolStripMenuItem2.Checked = true;
           
        }

        private void oneHourLaterToolStripMenuItem_Click(object sender, EventArgs e)
        {

            ShutdownTick(60);
            this.clearCheckedMenuItem(sleepTimerToolStripMenuItem);

            this.oneHourLaterToolStripMenuItem.Checked = true;
           
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.Opacity = 0.80;

            this.clearCheckedMenuItem(opacityToolStripMenuItem);

            this.toolStripMenuItem2.Checked = true;
        }

        private void clearCheckedMenuItem(ToolStripMenuItem menu)
        {
            isThemeApplied = true;
            foreach (ToolStripMenuItem ddi in menu.DropDownItems)
            {

                ddi.Checked = false;
            }

        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            this.Opacity = 0.70;

            this.clearCheckedMenuItem(opacityToolStripMenuItem);

            this.toolStripMenuItem3.Checked = true;
       
        }

        private void normalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Opacity = 0.6;

            this.clearCheckedMenuItem(opacityToolStripMenuItem);

            this.toolStripMenuItem4.Checked = true;
       
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {

            this.Opacity = 1;

            this.clearCheckedMenuItem(opacityToolStripMenuItem);

            this.toolStripMenuItem5.Checked = true;
        }


        public enum ShutDown
        {
            LogOff = 0,
            Shutdown = 1,
            Reboot = 2,
            ForcedLogOff = 4,
            ForcedShutdown = 5,
            ForcedReboot = 6,
            PowerOff = 8,
            ForcedPowerOff = 12
        }
        private void ShutdownComputer(object sender, EventArgs e)
        {
            if (shutdownTimer != null)
            {
                shutdownTimer.Enabled = false;
                shutdownTimer.Stop();
                shutdownTimer = null;
            }

            ManagementClass W32_OS = new ManagementClass("Win32_OperatingSystem");
            ManagementBaseObject inParams;//, outParams;
           // int result;
            W32_OS.Scope.Options.EnablePrivileges = true;
            foreach (ManagementObject obj in W32_OS.GetInstances())
            {
                inParams = obj.GetMethodParameters("Win32Shutdown");
                inParams["Flags"] = ShutDown.ForcedShutdown;
                inParams["Reserved"] = 0;
                //outParams = obj.InvokeMethod("Win32Shutdown", inParams, null);

               obj.InvokeMethod("Win32Shutdown", inParams, null);
                
                //result = Convert.ToInt32(outParams["returnValue"]);
                //if (result != 0) throw new Win32Exception(result);
            }
            this.Close();
            
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (shutdownTimer != null)
            {

                this.shutdownTimer.Enabled = false;

                shutdownTimer = null;
            }

            this.clearCheckedMenuItem(sleepTimerToolStripMenuItem);

            this.cancelToolStripMenuItem.Checked = true;
           

        }

        private void exitAltF4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void shutDownComputerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShutdownComputer(null, null);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

            setTopMost(!menuItem_alwaysontop.Checked);
        }

        private void setTopMost(bool istop)
        {
            menuItem_alwaysontop.Checked = istop;

            this.TopMost = istop;
        }

        private void SetPlayMode(int random)
        {
            switch (random)
            {//1 order 2 random 3 reverse
                case 1:
                    this.randomToolStripMenuItem.Checked = false;
                    this.orderToolStripMenuItem.Checked = true;
                    this.reverseToolStripMenuItem.Checked = false;
                    break;
                case 2:

                    this.randomToolStripMenuItem.Checked = true;

                    this.orderToolStripMenuItem.Checked = false;

                    this.reverseToolStripMenuItem.Checked = false;
                    break;
                case 3:
                    this.reverseToolStripMenuItem.Checked = true;
                    this.randomToolStripMenuItem.Checked = false;

                    this.orderToolStripMenuItem.Checked = false;

                    break;
            }
        }

        private void songlistView1_MouseMove(object sender, MouseEventArgs e)
        {
            ListViewItem lvi = this.songlistView1.GetItemAt(e.X, e.Y);


            if (lvi != null)
            {
                this.toolTip1.SetToolTip(this.songlistView1, lvi.Text);
            }
        }

        private void orderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetPlayMode(1);
        }

        private void randomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetPlayMode(2);
        }
         
        private bool isPlayListShown = false;
        private void buttonMenu_Click(object sender, EventArgs e)
        {
            setPlayListShow(isPlayListShown);
        }

        private void setPlayListShow(bool isplaylistshown){

            if (!isplaying && !isplaylistshown)
            {
                return;
            }

            if (isplaylistshown)
            {
                isPlayListShown = false;
                // song name list
                this.songlistView1.Show();
                // song cd cover
                this.panel_SongDetail.Hide();
            }
            else
            {

                isPlayListShown = true;
                this.panel_SongDetail.Show();
                this.songlistView1.Hide();
            }
        }

        private void SetbuttonMenuRegion()
        {

            GraphicsPath myGraphicsPath = new GraphicsPath();


            myGraphicsPath.AddRectangle(new RectangleF(6, 6, 38, 13));


            this.buttonMenu.Region = new Region(myGraphicsPath);
        }
        private void buttonMenu_Paint(object sender, PaintEventArgs e)
        {
            //SetbuttonMenuRegion
        }

        private void toolStripMenuItem_mini_Click(object sender, EventArgs e)
        {
            MinimizeWindow();
        }

  

        private void MinimizeWindow()
        {

            this.WindowState = FormWindowState.Minimized;
            //this.Hide();
        }


        private void MaxmizeWindow()
        {
            //this.Show();
            this.WindowState = FormWindowState.Normal;
         
        }

        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MinimizeWindow();
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            Debug.WriteLine("Form1_DragDrop invoked");
            DragDropInvoke(sender, e);
        }

        private void DragDropInvoke(object sender, DragEventArgs e)
        {
            Debug.WriteLine("DragDropInvoke invoked");
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (files.Length == 1 && Directory.Exists(files[0]))
            {
                FormAddLyrics fal = new FormAddLyrics();
                fal.lyricsFolder = files[0];
                fal.ShowDialog();
            }
            else
            {
                sortListAscByModTime(files, true);

                foreach (string filename in files)
                {
                    AddSongListView(filename);
                }
            }
           
        } 

        private void sortListAscByModTime(string[] files, bool isreverse)
        {
            string[] fileinfos = new string[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                fileinfos[i] = Directory.GetLastWriteTime(files[i]).ToString("yyyy-MM-dd HH:ss:mm");
                Debug.WriteLine(fileinfos[i] + "  " + files[i]);
            }
            if (isreverse == false)
            {
                Array.Sort(fileinfos, files);
            }
            else
            {
                Array.Sort(fileinfos, files);
                Array.Reverse(files);
                
            }
        }

       

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            Debug.WriteLine("Form1_DragEnter invoked");

            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                e.Effect = DragDropEffects.All;
            }
        }

        private void reverseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetPlayMode(3);
        }

        private void labelProcess_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.labelProcessBlue.Width = e.X;
                axWindowsMediaPlayer1.Ctlcontrols.currentPosition = axWindowsMediaPlayer1.currentMedia.duration * e.X / labelProcess.Width;
                
            }
        }

        private void labelProcessBlue_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.labelProcessBlue.Width = e.X;
                axWindowsMediaPlayer1.Ctlcontrols.currentPosition = axWindowsMediaPlayer1.currentMedia.duration * e.X / labelProcess.Width;

            }
        }
        private bool isThemeApplied;
        private Color skinColor;
        private Color panelColor;
        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                if (colorDialog1.Color == Color.Black)
                {
                    skinColor = Color.Gray;
                }
                else
                {
                    skinColor = colorDialog1.Color;
                }

                //SetSkinColor(colorDialog1.Color);
                isThemeApplied = false;
                InitRecAndGraph();
                InitStopButtonBrush();

                this.formShowLyrics.setLabelColor(skinColor);

                //setControlPanelColor();
                Refresh();
                //Invalidate();
            }
        }

        private void setControlPanelColor(Color panelColor)
        {
            //byte limit = 128;
            //byte limit2 = 255;
            //byte limit3 = 192;
            //byte limit4 = 0;
            //if ((skinColor.G == limit && skinColor.B == limit && skinColor.R == limit)
            //    || (skinColor.G == limit2 && skinColor.B == limit2 && skinColor.R == limit2)
            //    || (skinColor.G == limit3 && skinColor.B == limit3 && skinColor.R == limit3)
            //    || (skinColor.G == limit4 && skinColor.B == limit4 && skinColor.R == limit4))
            if (panelColor == Color.Black)
            {
                panel_control.BackColor = Color.Black;
                buttonMenu.BackColor = Color.Black;
                buttonMenu.ForeColor = Color.White;
                button_play.BackColor = Color.White;
                button_previous.BackColor = Color.White;
                button_next.BackColor = Color.White; 
                /*
                button_play.BackgroundImage = iPodC.Properties.Resources.playBlack;
                button_previous.BackgroundImage = iPodC.Properties.Resources.previousBlack;
                button_next.BackgroundImage = iPodC.Properties.Resources.nextBlack;
                buttonMenu.BackgroundImage = iPodC.Properties.Resources.menuBlack;
                 * */
            }
            else
            {
                panel_control.BackColor = Color.White;
                buttonMenu.BackColor = Color.White;
                buttonMenu.ForeColor = Color.Gray;
				
				button_play.BackColor = Color.Gray;
				button_previous.BackColor = Color.Gray;
				button_next.BackColor = Color.Gray;
                /*
                button_play.BackgroundImage = iPodC.Properties.Resources.playWhite;
                button_previous.BackgroundImage = iPodC.Properties.Resources.previousWhite;
                button_next.BackgroundImage = iPodC.Properties.Resources.nextWhite;
                buttonMenu.BackgroundImage = iPodC.Properties.Resources.menuWhite;
                 * */
            }
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            removeSongListView();
        }

        private void removeSongListView()
        {
            songlistView1.BeginUpdate();
            foreach (ListViewItem lvi in this.songlistView1.Items)
            {
                this.randomSongListTable.Remove(songlistView1.Items.IndexOf(lvi));

                this.songListTable.Remove(lvi.Text);
                songlistView1.Items.Remove(lvi);
            }
            songlistView1.EndUpdate();
        }

        //paintTitleBar
        private void label1_Paint(object sender, PaintEventArgs e)
        {
            paintTitleBar(e);
        }

        private void paintTitleBar(PaintEventArgs e)
        {
            Debug.WriteLine("paintTitleBar");
            GraphicsPath myGraphicsPath = new GraphicsPath();

            PointF[] points = new PointF[3];

            points[0] = new PointF(this.label1.Width - 30, 4);
            points[1] = new PointF(this.label1.Width - 30, 11);
            points[2] = new PointF(this.label1.Width - 24, 7);
 
            myGraphicsPath.AddLines(points);

            SolidBrush sb = new SolidBrush(Color.White);
            
            //tringle
            e.Graphics.FillPath(sb, myGraphicsPath);

            //battery
            e.Graphics.FillRectangle(sb, new RectangleF(label1.Width - 20, 4, 16, 8));

            sb = new SolidBrush(Color.Black);

            e.Graphics.FillRectangle(sb, new RectangleF(label1.Width - 19, 5, 14, 6));

            sb = new SolidBrush(Color.White);

            e.Graphics.FillRectangle(sb, new RectangleF(label1.Width - 18, 6, 8, 4));

            e.Graphics.FillRectangle(sb, new RectangleF(label1.Width - 4, 6, 2, 4));
            sb.Dispose();


            // make a polish 
           

            paintPolish(e, label1.Height);

        }

        private void paintPolish(PaintEventArgs e, int Height)
        {


            Debug.WriteLine("paintPolish");

            Rectangle myRectangle = new Rectangle(label1.Width / 2 - 5, 0, label1.Width + 350, Height);
            //shadow.Dispose();
            LinearGradientBrush myLinearGradientBrush =
                new LinearGradientBrush(myRectangle, Color.Transparent, Color.White, LinearGradientMode.Horizontal);

            myRectangle = new Rectangle(label1.Width / 2, 0, 49, Height);

            e.Graphics.FillRectangle(myLinearGradientBrush, myRectangle);
            myLinearGradientBrush.Dispose();
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            addCDCover();
        }

        private void addCDCover()
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                if ((openFileDialog2.OpenFile()) != null)
                {
                    string cdcoverpath = openFileDialog2.FileName;

                    iPodC.Properties.Settings.Default.cdCoverPath = cdcoverpath.Substring(0, cdcoverpath.LastIndexOf(@"\")); ;

                    cdCoverList = new ArrayList();
                    //cdCoverList.Add(openFileDialog2.FileNames);
                    string[] filenames = openFileDialog2.FileNames;
                    sortListAscByModTime(filenames, true);
                    cdCoverList.AddRange(filenames);
                    //foreach (string filename in filenames)
                    //{
                    //    cdCoverList.Add(filename); 
                    //}

                }
            } 
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            showLyricsToolStripMenuItem.Checked = Properties.Settings.Default.ISSHOWLYRICS;

            string lyricsFolder = Properties.Settings.Default.lyricsFolder;
            if (!lyricsFolder.Equals("") && Directory.Exists(lyricsFolder))
            {
                ToolStripMenuItem songCollection = new ToolStripMenuItem();
                //songCollection.Name = "songCollection";
                songCollection.Size = new System.Drawing.Size(166, 22);
                songCollection.Text = lyricsFolder; 
                currentLyricsToolStripMenuItem.DropDownItems.Add(songCollection);
            }

            SetStyle(ControlStyles.SupportsTransparentBackColor, true);   
            isThemeApplied = Properties.Settings.Default.isThemeApplied;

            skinColor = Properties.Settings.Default.skinColor;
            panelColor = Properties.Settings.Default.panelColor;
            setControlPanelColor(panelColor);
            if (panelColor == Color.Black)
            {
                blackToolStripMenuItem1.Checked = true;
                whiteToolStripMenuItem.Checked = false;
            }
            else
            {
                blackToolStripMenuItem1.Checked = false;
                whiteToolStripMenuItem.Checked = true;
            }


            if (Properties.Settings.Default.songListRandom == null)
            {
                randomSongListTable = new ArrayList();
            }
            else
            {
                randomSongListTable = Properties.Settings.Default.songListRandom;
            }
            LoadSongList();

            InitSkin();

            setTopMost(Properties.Settings.Default.AlwaysOnTop);

            SetPlayMode(Properties.Settings.Default.IsRandom);
 
            SetCurrentSong();

            InitCDCover();
          

            InitOpenDialogDir();
            InitArcPlayPanel();
            SetSkinMode(Properties.Settings.Default.SkinStyle);

            InitAllRegion();
            InitSongListCollection();

            this.buttonMenu.Select();

            cdCoverShadow = rotateImage(pictureBox_Logo.BackgroundImage);
            panel_SongDetail.Invalidate();
            //CleanMemoryToSwap();

            this.songDetail_Ticker = new Timer();

            this.songDetail_Ticker.Enabled = true;

            this.songDetail_Ticker.Interval = Int32.MaxValue;
            this.songDetail_Ticker.Tick += new System.EventHandler(refreshProgress);//refreshProgress  refreshPlayerPanel


            this.formShowLyrics = new FormShowLyrics();
            this.formShowLyrics.setLabelColor(skinColor);
        }

        private void InitAllRegion()
        {

            /******************************/
            /**       shape region       **/
            /******************************/
            shapeMainRegion();
            /*************************************/
            /**             Panel Black         **/
            /*************************************/
            shapeSongListPanel();

            SetbuttonMenuRegion();
            SetButtonStopRegion();
            Setbutton_previousRegion();
            Setbutton_nextRegion();
            Setbutton_playRegion();

        }

        private void InitSongListCollection()
        {
            if (Properties.Settings.Default.songLibrary == null)
            {
                Properties.Settings.Default.songLibrary = new StringCollection();
                //printError("Song library is empty");
            }
            else
            {
                ArrayList deleteList = new ArrayList();
                isScLibExist();
                foreach (string keyname in Properties.Settings.Default.songLibrary)
                {
                    if (!File.Exists(songFolder + "\\" + keyname + ".xml"))
                    {
                        deleteList.Add(keyname);
                        continue;
                    }
                    else
                    {

                        AddSongListToSongLib(keyname);
                    }
                }

                foreach (string delname in deleteList)
                {
                    Properties.Settings.Default.songLibrary.Remove(delname);
                }
                //printError("Song library is filled");

                string[] existedSongList = Directory.GetFiles(songFolder, "*.xml");
 
                foreach (string existedsong in existedSongList)
                {
                    string songlistName = Path.GetFileNameWithoutExtension(existedsong);

                    if (Properties.Settings.Default.songLibrary.Contains(songlistName))
                    {
                        continue;
                    }
                    else
                    {
                        AddSongListToSongLib(songlistName);
                    }

                }
                foreach (ToolStripMenuItem smi in toolStripMenuItem_songlist.DropDownItems)
                {
                    if (smi.Text.Equals(Properties.Settings.Default.currentSongListName))
                    {
                        smi.Checked = true;
                    }
                    else
                    {
                        smi.Checked = false;
                    }
                }

            }
        }

        private void AddSongListToSongLib(string keyname)
        {
            ToolStripMenuItem songCollection = new ToolStripMenuItem();
            //songCollection.Name = "songCollection";
            songCollection.Size = new System.Drawing.Size(166, 22);
            songCollection.Text = keyname;
            songCollection.Click += new System.EventHandler(this.songCollection_Click);
            toolStripMenuItem_songlist.DropDownItems.Add(songCollection);

        }

        private void labelProcess_Paint(object sender, PaintEventArgs e)
        {
            //if (isplaying)
            //{
            //    refreshProgress();
            //}
        }

        private void refreshProgress(object sender, EventArgs e)
        {
            //this.printError("refreshProgress");
            Debug.WriteLine("refreshProgress");

            if (IsLyricsLoaded == LyricsLoaded
                && Properties.Settings.Default.ISSHOWLYRICS
                && isplaying)
            {
                // lyrics max length is 23
                formShowLyrics.setLyrics(this.labelSongTimer1.Text);

            }

            this.labelProcessBlue.Width =
                (int)Math.Ceiling(axWindowsMediaPlayer1.Ctlcontrols.currentPosition
                    / axWindowsMediaPlayer1.currentMedia.duration * labelProcess.Width);

            // timer number caused the panel song detail refreshed 2 times
            double d2 = axWindowsMediaPlayer1.Ctlcontrols.currentPosition;

            double m2 = d2 / 60;
            double s2 = d2 % 60;
            string temp = addzero(m2) + ":" + addzero(s2);
             
            this.labelSongTimer1.Text = temp;
             
            double d1 = axWindowsMediaPlayer1.currentMedia.duration - d2;
            double m1 = d1 / 60;
            double s1 = d1 % 60;
            this.labelSongTimer2.Text = "-" + addzero(m1) + ":" + addzero(s1);
              
        }


        private void toolStripMenuItem_saveList_Click(object sender, EventArgs e)
        { 
            ToolStripTextBox toolStripTextBox = new ToolStripTextBox();
            toolStripTextBox.Size = new System.Drawing.Size(150, 22);
            toolStripTextBox.Text = "New Song List";
            //toolStripTextBox.LostFocus += new System.EventHandler(this.songCollectionLeave_Click);
            toolStripTextBox.KeyUp += new KeyEventHandler(this.songCollectionEdit);
            toolStripTextBox.LostFocus += new EventHandler(this.songCollectionEdit_LostFocus);
 
            toolStripMenuItem_songlist.DropDownItems.Add(toolStripTextBox);
            contextMenuStrip1.Show();
            contextMenuStrip1.Enabled = false;
            toolStripMenuItem_songlist.ShowDropDown();
 
            toolStripTextBox.Focus(); 

        }

        private void songCollectionEdit(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    songCollectionLeave_Click(null, null);
            //}

        }

        private void songCollectionEdit_LostFocus(object sender, EventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            songCollectionLeave_Click(null, null);
            //}

        }

        private void songCollection_Click(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem smi in toolStripMenuItem_songlist.DropDownItems)
            {
                smi.Checked = false;
            }
            ToolStripMenuItem sender1 = (ToolStripMenuItem)sender;
            sender1.Checked = true;
            if (File.Exists(songFolder + "\\" + sender1.Text + ".xml"))
            {
                removeSongListView();
                XmlTextReader settingsReader = new XmlTextReader(songFolder + "\\" + sender1.Text + ".xml"); 
                while (settingsReader.Read())
                {
                    if (settingsReader.NodeType == XmlNodeType.Element)
                    {

                        if (settingsReader.IsStartElement() && settingsReader.Name == "SonInfo")
                        {
                            settingsReader.MoveToContent();
                            AddSongListView(settingsReader.ReadString());
 
                        }
                    }
                }
                Properties.Settings.Default.currentSongListName = sender1.Text;
            }
        }

        private static string songFolder = Directory.GetCurrentDirectory() + "\\scLib";

        private void songCollectionLeave_Click(object sender, EventArgs e)
        {


            contextMenuStrip1.Enabled = true;

            ToolStripTextBox toolStripTextBox = (ToolStripTextBox)toolStripMenuItem_songlist.DropDownItems[toolStripMenuItem_songlist.DropDownItems.Count - 1];

            if (toolStripTextBox.Text.Equals(""))
            {
                toolStripTextBox.Dispose();
                MessageBox.Show("Library name is empty, can not save!");
            }
            else
            {
                ToolStripMenuItem songCollection = new ToolStripMenuItem();
                //songCollection.Name = "songCollection";
                songCollection.Size = new System.Drawing.Size(166, 22);
                songCollection.Text = toolStripTextBox.Text;
                songCollection.Click += new System.EventHandler(this.songCollection_Click);

                toolStripTextBox.Dispose();

                foreach (ToolStripMenuItem tsm in toolStripMenuItem_songlist.DropDownItems)
                {
                    tsm.Checked = false;
                }
                toolStripMenuItem_songlist.DropDownItems.Add(songCollection);
                songCollection.Checked = true;

                SaveSongListByName(songCollection.Text);
            }

        }

        private void SaveSongListByName(string songCollection)
        {

            XmlTextWriter settingWriter = null;
            try
            {
                isScLibExist();
                settingWriter = new XmlTextWriter(songFolder + "\\" + songCollection + ".xml", null);
                settingWriter.Formatting = Formatting.Indented;
                settingWriter.Indentation = 6;
                settingWriter.Namespaces = false;
                settingWriter.WriteStartDocument();
                settingWriter.WriteStartElement("", "FileList", "");
                foreach (ListViewItem songpath in this.songlistView1.Items)
                {
                    settingWriter.WriteStartElement("", "SonInfo", "");

                    settingWriter.WriteString((string)this.songListTable[songpath.Text]);

                    settingWriter.WriteEndElement();
                    //Properties.SettingsHeavy.Default.songMap.Add(songCollection.Text, (string)this.songListTable[songpath.Text]);

                }
                settingWriter.WriteEndElement();
                settingWriter.Flush();
                if (!Properties.Settings.Default.songLibrary.Contains(songCollection))
                {
                    Properties.Settings.Default.songLibrary.Add(songCollection);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: {0}", ex.ToString());
            }
            finally
            {
                if (settingWriter != null)
                {
                    settingWriter.Close();
                }
            }

        }

        private void isScLibExist()
        {
            if (!Directory.Exists(songFolder))
            {
                Directory.CreateDirectory(songFolder);
            }
        }

        protected override CreateParams CreateParams
        {
            [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
            get
            {
                CreateParams parameters = base.CreateParams;

                if (DropShadowSupported)
                {
                    parameters.ClassStyle = (parameters.ClassStyle | CS_DROPSHADOW);
                }
                return parameters;
            }
        }

        public static bool DropShadowSupported
        {
            get { return IsWindowsXPOrAbove; }
        }

        public static bool IsWindowsXPOrAbove
        {
            get
            {
                OperatingSystem system = Environment.OSVersion;
                bool runningNT = system.Platform == PlatformID.Win32NT;

                return runningNT && system.Version.CompareTo(new Version(5, 1, 0, 0)) >= 0;
            }
        }

        private void contextMenuStrip1_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            if (!contextMenuStrip1.Enabled)
            {
                contextMenuStrip1.Enabled = true;
            }
        }

        private void printError(string msg)
        {
            TextWriter tw = new StreamWriter("iPod.log", true);

            // write a line of text to the file
            tw.WriteLine("- " + msg + " " + DateTime.Now.ToLocalTime() + "\n");

            // close the stream
            tw.Close();
            //richTextBox_Log.AppendText("- " + msg + " " + DateTime.Now.ToLocalTime() + "\n");

        }

        private void panel_SongDetail_Paint(object sender, PaintEventArgs e)
        {
           
            Debug.WriteLine("panel_SongDetail_Paint");
           
            //if (panel_SongDetail_Paint_TIMESTAMP.Equals(DateTime.Now.ToString()))
            //{
            //    return;
            //}
            //else
            //{
            //    panel_SongDetail_Paint_TIMESTAMP = DateTime.Now.ToString();
            //}
            
            // draw song name background with grep -> black
            RectangleF rec = new RectangleF(0, 0, panel_SongDetail.Width, labelSongName.Height);
            LinearGradientBrush linearBrush =
                new LinearGradientBrush(rec,
                    Color.FromArgb(64, 64, 64),
                    Color.FromArgb(Color.Black.R, Color.Black.G, Color.Black.B),
                    90);

            GraphicsPath myGraphicsPath = new GraphicsPath();

            myGraphicsPath.AddRectangle(rec);

            e.Graphics.FillPath(linearBrush, myGraphicsPath); 
            linearBrush.Dispose();

            // make a polish 
            //Rectangle myRectangle = new Rectangle(0, 10, panel_SongDetail.Width + 400, 30);
            ////shadow.Dispose();
            //LinearGradientBrush myLinearGradientBrush =
            //    new LinearGradientBrush(myRectangle, Color.Transparent, Color.White, LinearGradientMode.Horizontal);

            //myRectangle = new Rectangle(0, 0, panel_SongDetail.Width - 30, 30);

            //e.Graphics.FillRectangle(myLinearGradientBrush, myRectangle);
            //myLinearGradientBrush.Dispose();
 
           // this.pictureBox_Logo.BackgroundImage = shadow;

            if (cdCoverShadow != null)
            {
                int y = labelSongName.Height + pictureBox_Logo.Height;
                Rectangle myRectangle = new Rectangle(0, y, panel_SongDetail.Width, panel_SongDetail.Width);
                e.Graphics.DrawImage(cdCoverShadow, myRectangle);

                //myRectangle = new Rectangle(0, 140, panel_SongDetail.Width, 85);
                myRectangle = new Rectangle(0, y - 20, panel_SongDetail.Width, panel_SongDetail.Width / 2);
                //shadow.Dispose();
                LinearGradientBrush myLinearGradientBrush =
                    new LinearGradientBrush(myRectangle, Color.Transparent, Color.Black, LinearGradientMode.Vertical);
                e.Graphics.FillRectangle(myLinearGradientBrush, myRectangle);
                myLinearGradientBrush.Dispose();


                // make a polish 
 

                paintPolish(e, panel_SongDetail.Height);


            }


           // e.Graphics.FillRectangle(Brushes.White, 0, 184, this.panel_SongDetail.Width, this.panel_SongDetail.Width);

            //refreshProgress();
        }

        private Bitmap rotateImage(Image b)
        {
            //create a new empty bitmap to hold rotated image
            //Bitmap returnBitmap = new Bitmap(b.Width, b.Height);
            Bitmap bmp = new Bitmap(b);
            bmp.RotateFlip(RotateFlipType.Rotate180FlipX);
            return bmp;
            //make a graphics object from the empty bitmap
           // Graphics g = Graphics.FromImage(returnBitmap);
           // g.Clear(Color.Black);
            //move rotation point to center of image
           // g.TranslateTransform((float)b.Width / 2, (float)b.Height / 2);
            //rotate 
          //  g.RotateTransform(angle);
           
            //move image back
          //  g.TranslateTransform(-(float)b.Width / 2, -(float)b.Height / 2);
            //draw passed in image onto graphics object
          //  g.DrawImage(b, new Point(0, 0));
            
        }

        private void toolStripMenuItem_updList_Click(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem item in toolStripMenuItem_songlist.DropDownItems)
            {
                if (item.Checked)
                {
                    //toolStripMenuItem_songlist.DropDownItems.Add(songCollection);

                    SaveSongListByName(item.Text);
                    break;
                }
            }
        }

        private void style08ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.SkinStyle = SKIN08;
            SetSkinMode(SKIN08);
            InitRecAndGraph();
            InitStopButtonBrush();
            this.Invalidate();
        }

        private void style09ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Properties.Settings.Default.SkinStyle = SKIN09;
            SetSkinMode(SKIN09);
            InitRecAndGraph();
            InitStopButtonBrush();
            this.Invalidate();
        }
        private void SetSkinMode(int random)
        {
            switch (random)
            {//1 order 2 random 3 reverse
                case 0:
                    style08ToolStripMenuItem.Checked = true;
                    style09ToolStripMenuItem.Checked = false;
                    break;
                case 1:

                    style08ToolStripMenuItem.Checked = false;
                    style09ToolStripMenuItem.Checked = true;
                    break;
                default:
                    style08ToolStripMenuItem.Checked = true;
                    style09ToolStripMenuItem.Checked = false;

                    break;
            }
        }

        private void panel_blackground_Paint(object sender, PaintEventArgs e)
        {
            RectangleF rec = new RectangleF(0, 0, panel_blackground.Width / 4 * 3 + 8, panel_blackground.Height);
            LinearGradientBrush linearBrush = 
                new LinearGradientBrush(rec,
                    Color.FromArgb(Color.Gray.R, Color.Gray.G, Color.Gray.B),
                    Color.FromArgb(Color.Black.R, Color.Black.G, Color.Black.B),
                    180);

            GraphicsPath myGraphicsPath = new GraphicsPath();

            myGraphicsPath.AddRectangle(rec);

            e.Graphics.FillPath(linearBrush, myGraphicsPath);
            linearBrush.Dispose();
        }

        private void pictureBox_Logo_Paint(object sender, PaintEventArgs e)
        {
        
            if (cdCoverShadow != null)
            {
                // make a polish on CD cover
                Debug.WriteLine("pictureBox_Logo_Paint");
                paintPolish(e, pictureBox_Logo.Height);
            }
        }

        private void whiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelColor = Color.White;
            setControlPanelColor(panelColor);
            blackToolStripMenuItem1.Checked = false;
            whiteToolStripMenuItem.Checked = true;
        }


        private void blackToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            panelColor = Color.Black;
            setControlPanelColor(panelColor);
            whiteToolStripMenuItem.Checked = false;
            blackToolStripMenuItem1.Checked = true;

        }



        private void panel_control_MouseDown(object sender, MouseEventArgs e)
        {

            org_Touch_Pos = new Point(e.X, e.Y);

        }

        private void panel_control_MouseUp(object sender, MouseEventArgs e)
        {


            if (org_Touch_Pos.X <= e.X && org_Touch_Pos.Y <= e.Y)
            {
                // next
                if (isplaying)
                {
                    PlayNextSong();
                }

            }
            else if (org_Touch_Pos.X >= e.X && org_Touch_Pos.Y >= e.Y)
            {
                // previous
                if (isplaying)
                {
                    PlayPreviousSong();
                }

            }

        }

        private void labelSongTimer1_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.labelSongTimer1, labelSongTimer1.Text);
        }

        private void labelSongTimer2_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.labelSongTimer2, labelSongTimer2.Text);
        }

        private void labelNumOfList_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.labelNumOfList, labelNumOfList.Text);
        }

        private void labelSongName_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.labelSongName, labelSongName.Text);
        }

        private void showLyricsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.ISSHOWLYRICS)
            {
                Properties.Settings.Default.ISSHOWLYRICS = false;
                formShowLyrics.Visible = false;
            }
            else
            {
                Properties.Settings.Default.ISSHOWLYRICS = true;
                formShowLyrics.Visible = true;
            }
            showLyricsToolStripMenuItem.Checked = Properties.Settings.Default.ISSHOWLYRICS;
        }


        private string limitSongNameLength(string songname)
        {
            int songlength = 28;
            if (System.Text.Encoding.Default.GetByteCount(songname) > songname.Length)
            {
                // kan ji
                if (songname.Length > (songlength / 2))
                {
                    songname = songname.Substring(0, songlength / 2) + "...";
                }
            }
            else
            {
                if (songname.Length > songlength)
                {
                    songname = songname.Substring(0, songlength) + "...";
                }
            }
            return songname;
        }

        private void reloadLyricsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

    }

    /*
    class ID3Tag
    {
        public byte[] TAGID = new byte[3]; // 3, 必衼E荰AG
        public byte[] Title = new byte[30]; // 30, 歌曲的眮E丒
        public byte[] Artist = new byte[30]; // 30, 歌曲的艺术家
        public byte[] Album = new byte[30]; // 30, 专辑名称
        public byte[] Year = new byte[4]; // 4, 出版膩E丒
        public byte[] Comment = new byte[30]; // 30, 评论
        public byte[] Genre = new byte[1]; // 1, 种类眮E?
    }
    */
}