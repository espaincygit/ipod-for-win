using System.Drawing;
namespace iPodC
{
    partial class FormShowLyrics
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormShowLyrics));
            this.label1_lyrics1 = new System.Windows.Forms.Label();
            this.label_move = new System.Windows.Forms.Label();
            this.label1_lyrics2 = new System.Windows.Forms.Label();
            this.labelFlash_1 = new System.Windows.Forms.Label();
            this.labelFlash_2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1_lyrics1
            // 
            this.label1_lyrics1.Font = new System.Drawing.Font("SimSun", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1_lyrics1.Location = new System.Drawing.Point(65, 0);
            this.label1_lyrics1.Name = "label1_lyrics1";
            this.label1_lyrics1.Size = new System.Drawing.Size(560, 23);
            this.label1_lyrics1.TabIndex = 0;
            this.label1_lyrics1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1_lyrics1.Paint += new System.Windows.Forms.PaintEventHandler(this.label1_lyrics_Paint);
            // 
            // label_move
            // 
            this.label_move.BackColor = System.Drawing.Color.Red;
            this.label_move.Font = new System.Drawing.Font("SimSun", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_move.Location = new System.Drawing.Point(1, 0);
            this.label_move.Name = "label_move";
            this.label_move.Size = new System.Drawing.Size(43, 46);
            this.label_move.TabIndex = 1;
            this.label_move.Text = "бя";
            this.label_move.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_move.MouseMove += new System.Windows.Forms.MouseEventHandler(this.label1_MouseMove);
            this.label_move.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label1_MouseDown);
            // 
            // label1_lyrics2
            // 
            this.label1_lyrics2.Font = new System.Drawing.Font("SimSun", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1_lyrics2.Location = new System.Drawing.Point(96, 23);
            this.label1_lyrics2.Name = "label1_lyrics2";
            this.label1_lyrics2.Size = new System.Drawing.Size(560, 23);
            this.label1_lyrics2.TabIndex = 2;
            this.label1_lyrics2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1_lyrics2.Paint += new System.Windows.Forms.PaintEventHandler(this.label1_lyrics2_Paint);
            // 
            // labelFlash_1
            // 
            this.labelFlash_1.Font = new System.Drawing.Font("SimSun", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelFlash_1.Location = new System.Drawing.Point(49, 0);
            this.labelFlash_1.Name = "labelFlash_1";
            this.labelFlash_1.Size = new System.Drawing.Size(10, 23);
            this.labelFlash_1.TabIndex = 3;
            this.labelFlash_1.Text = "...";
            this.labelFlash_1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelFlash_2
            // 
            this.labelFlash_2.Font = new System.Drawing.Font("SimSun", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelFlash_2.Location = new System.Drawing.Point(80, 23);
            this.labelFlash_2.Name = "labelFlash_2";
            this.labelFlash_2.Size = new System.Drawing.Size(10, 23);
            this.labelFlash_2.TabIndex = 4;
            this.labelFlash_2.Text = "...";
            this.labelFlash_2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FormShowLyrics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Red;
            this.ClientSize = new System.Drawing.Size(661, 46);
            this.Controls.Add(this.labelFlash_2);
            this.Controls.Add(this.labelFlash_1);
            this.Controls.Add(this.label1_lyrics2);
            this.Controls.Add(this.label_move);
            this.Controls.Add(this.label1_lyrics1);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormShowLyrics";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "iPod Lyrics";
            this.TransparencyKey = System.Drawing.Color.Red;
            this.Load += new System.EventHandler(this.FormShowLyrics_Load);
            this.VisibleChanged += new System.EventHandler(this.FormShowLyrics_VisibleChanged);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Label label_move;

        private Point mouse_offset;
        public System.Windows.Forms.Label label1_lyrics1;
        public System.Windows.Forms.Label label1_lyrics2; 

        private System.Windows.Forms.Timer lyricsScrollTimer;

        private static int CURRENT_FLASH_LYRICS_LABEL_NO;

        private static int FLASH_LYRICS_LABEL_NO_1 = 1;
        private static int FLASH_LYRICS_LABEL_NO_2 = 2;

        //private static int LyricsLabel2 = 2;

        public int currentTimeStamp=0000;
        public int nextTimeStamp=0000;

        public LyricsInfo lyricsInfo;
        public System.Windows.Forms.Label labelFlash_1;
        public System.Windows.Forms.Label labelFlash_2;

        //private static int FLASH_COUNT;


        public string labelLyricsFore_1 = "";
        public string labelLyricsFore_2 = "";

        public string labelLyricsBackGround_1 = "";
        public string labelLyricsBackGround_2 = "";

        private Color diseColor;
 
    }
}