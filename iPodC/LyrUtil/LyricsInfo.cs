using System;
using System.Collections.Generic; 
using System.Text;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
namespace iPodC
{
    public class LyricsInfo
    {
        public string title;

        public string singer;

        public string album;

        public string songURL;

        public Hashtable infos = new Hashtable();

        public ArrayList timeAxis = new ArrayList();

        public int lyricsIndex = 0;

        public int nextTimeStamp;
        public LyricsInfo()
        {

        }
        public LyricsInfo(string album, string artistname, string songname, string URL)
            : this()
        {
            this.album = album;
            this.singer = artistname;
            this.title = songname;
            this.songURL = URL;
        }

        public string getFirstLrc(string time)
        {
            string rtn = null;

            string key = this.timeAxis[0].ToString();

            if (key.Equals(time))
            {
                rtn = this.infos[key].ToString();
            }
            return rtn;
        }

        public string FindLrc(string time)
        {
            string rtn = null;
            // 02:03
            //if (lyricsIndex == 0)
            //{
                //string key = timeAxis[lyricsIndex].ToString();

                //if (infos.ContainsKey(key))
                //{
                //rtn = infos[key].ToString();
                //}

                //nextTimeStamp = Int32.Parse(timeAxis[++lyricsIndex].ToString().Replace(":", "")); 

            //}
            //else
            //{
                //if (infos.ContainsKey(time))
                //{
                //Debug.WriteLine(infos[time].ToString());
                //if (infos.ContainsKey(time))
                //{

                int nextIndex = timeAxis.IndexOf(time) + 1;
                if (nextIndex != 0 && timeAxis.Count > nextIndex)
                {
                    string key = this.timeAxis[nextIndex].ToString();
                    //if (infos.ContainsKey(key))
                    //{
                    nextTimeStamp = Int32.Parse(key.Replace(":", ""));
                    lyricsIndex = nextIndex;
                    rtn = infos[key].ToString();

                    //}

                }
                //}


                //}
            //}
            

            return rtn;
        }


        internal string FindLrcNext(string time)
        {
            string rtn = "";
            int nextIndex = lyricsIndex + 1;
            if (timeAxis.Count > nextIndex)
            {
                string key = this.timeAxis[nextIndex].ToString();
                if (infos.Contains(key) && key.Equals(time))
                {

                    lyricsIndex++; 
                    rtn = infos[key].ToString();

                }
            }

            return rtn;
        }
    }
}
