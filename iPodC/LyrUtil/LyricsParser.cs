using System;
using System.Collections.Generic; 
using System.Collections;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using QianQianLrc;
using System.Windows.Forms;
using System.Xml;

namespace iPodC
{
    public class LyricsParser
    {  
        public long currentTime = 0;

        public String currentContent = null;
 

        public void readLrcFile(LyricsInfo lyinfo)
        {
            string path = lyinfo.songURL;
            string[] rtn = null;

            try
            {
                string finalPath = "";
                string path1 = path.Substring(0, path.LastIndexOf(".")) + ".lrc";
                bool isExistInCurrent = File.Exists(path1);
 
                if (isExistInCurrent)
                {
                    // current folder; same file name
                    finalPath = path1;
                }
                else
                {
                    // same folder, same title name
                    path1 = Path.GetDirectoryName(path) + "\\*" + lyinfo.title + "*.lrc";
                    isExistInCurrent = File.Exists(path1);
                    if (isExistInCurrent)
                    {
                        finalPath = path1;
                    } else if (!iPodC.Properties.Settings.Default.lyricsFolder.Equals(""))
                    {
                        // search lyrcis folder
                        string tempPath = iPodC.Properties.Settings.Default.lyricsFolder + "\\";

                        //string songname =;// Path.GetFileNameWithoutExtension(path);

                        //sameNamefileInLyrcisFolder                             
                        string[] files2 = Directory.GetFiles(tempPath, "*" +  Path.GetFileNameWithoutExtension(path) + "*.lrc");


                        string[] files = Directory.GetFiles(tempPath, "*" +  lyinfo.title + "*.lrc");
                         if (files2 != null && files2.Length > 0)
                        {
                            finalPath = files2[0];
                        } else if (files != null && files.Length > 0)
                        {
                            finalPath = files[0];
                        }
                        else
                        {
                            QianQianLrcer qqL = new QianQianLrcer(false);

                            //qqL.SelectSong += new EventHandler(q_SelectSong);

                            string singer = replaceAllSign(lyinfo.singer);
                            string title = replaceAllSign(lyinfo.title);
                            string rtnqql = qqL.DownloadLrc(singer, title);

                            if (!rtnqql.Equals(""))
                            {
                                finalPath = iPodC.Properties.Settings.Default.lyricsFolder
                                    + "\\" + lyinfo.singer + "-" + lyinfo.title + ".lrc";
                                File.WriteAllText(finalPath, rtnqql, Encoding.Default);

                            }
                            else
                            {
                                title = Path.GetFileNameWithoutExtension(path);
                                string temptitle = "";
                                if (title.IndexOf("／") > 0)
                                {
                                    temptitle = title.Split('／')[1];
                                    temptitle = replaceAllSign(temptitle);

                                }
                                else
                                {
                                    temptitle = replaceAllSign(title);
                                }

                                rtnqql = qqL.DownloadLrc("", temptitle);
                                if (!rtnqql.Equals(""))
                                {
                                    finalPath = iPodC.Properties.Settings.Default.lyricsFolder
                                        + "\\" + title + ".lrc";
                                    File.WriteAllText(finalPath, rtnqql, Encoding.Default);

                                }
                            }
                            
                            //finalPath = getWildCastLyricsFilePath(iPodC.Properties.Settings.Default.lyricsFolder,
                            //    songname, 2);

                        }
                    }
                }

                if (!finalPath.Equals(""))
                {
                    rtn = File.ReadAllLines(finalPath, Encoding.Default);
                    foreach (string eachline in rtn)
                    {
                        if (eachline.Length == 0 || eachline.IndexOf("[") < 0 || eachline.IndexOf("]") < 0)
                        {
                            continue;
                        }
                        else
                        {
                            try
                            {
                                regLrc(eachline, lyinfo);
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message);
                            }
                        }
                    }
                    if (lyinfo.infos.Count > 0)
                    {
                        string[] time = new string[lyinfo.infos.Count];
                        int index = 0;
                        foreach (string keylyrc in lyinfo.infos.Keys)
                        {
                            time[index++] = keylyrc;
                        }
                        Array.Sort(time);
                        foreach (string sortkey in time)
                        {
                            lyinfo.timeAxis.Add(sortkey);

                        }
                        lyinfo.lyricsIndex = 0;
                        
                    }

                    Form1.IsLyricsLoaded = Form1.LyricsLoaded;
                }
                else
                {

                    Form1.IsLyricsLoaded = Form1.LyricsNotLoaded;
                }

            }
            catch (Exception aaa)
            {

                Form1.IsLyricsLoaded = Form1.LyricsNotLoaded;
                Debug.WriteLine(aaa.Message); 
            }

        }

        private string replaceAllSign(string p)
        {
            return p.Replace(" ", "").Replace("-", "").Replace(".", "")
                            .Replace("0", "").Replace("1", "").Replace("2", "").Replace("3", "")
                            .Replace("4", "").Replace("5", "").Replace("6", "").Replace("7", "")
                            .Replace("8", "").Replace("9", "").Replace("(","").Replace(")","").ToLower();
        }

        private string getWildCastLyricsFilePath(string path, string songname, int length)
        {
            int namelength = songname.Length - (length - 1);
            string rtn = "";
            for (int index = 0; index < namelength; index++)
            {
                string sliceName = songname.Substring(index, length);

                string[] files = Directory.GetFiles(path, "*" + sliceName + "*.lrc");

                if (files.Length == 1)
                {
                    rtn = files[0];
                    break;
                    // found
                }
                else if (files == null || files.Length == 0)
                {
                    continue;
               
                } else if (files.Length > 1){
                    string locrtn = getWildCastLyricsFilePath(path, songname, length + 1);
                    if (!locrtn.Equals(""))
                    {
                        rtn = locrtn;
                        break;
                    }
                } else {
                    continue;
                }

            }
            return rtn;
        }

        void regLrc(string strInput, LyricsInfo lrcinfo)
        {
            //string sPattner="(?<t>\\[\\d[^[]+])(?<w>.*\r\n)";   
            //string sPattner = "(?<t>\\[\\d.*\\]+)(?<w>[^\\[]+\r\n)";
            //Regex reg = new Regex(sPattner);
            //foreach (Match mc in reg.Matches(strInput))
            //{
            //richTextBox1.AppendText(mc.Groups["t"].ToString() + "\r\n");
            //richTextBox1.AppendText(mc.Value + "\r\n");
            //LrcList.Add(new Lrc(mc.Groups["t"].ToString(), mc.Groups["w"].ToString()));
            //Debug.WriteLine(mc.Groups["t"].ToString());
            //Debug.WriteLine(mc.Value);
            //Debug.WriteLine(mc.Groups["t"].ToString() + "   " + mc.Groups["w"].ToString());

            //if (lrcinfo.infos == null) lrcinfo.infos = new Hashtable();
            //lrcinfo.infos.Add(mc.Value, mc.Value);

            //}

            // [03:56.80][03:21.13][01:45.36]
            string key = strInput.Substring(0, strInput.LastIndexOf("]") + 1);

            char keyChar = key.Substring(1, 1).ToCharArray()[0];

            if (keyChar < 47 || keyChar > 57) return;
            //if (key.IndexOf("ar:") >= 0 || key.IndexOf("ti:") >= 0 || key.IndexOf("al:") >= 0 || key.IndexOf("by:") >= 0)
            //{
            //    return;
            //}
            if (key.IndexOf("-") > 0) return;

            ArrayList keyList = new ArrayList();
            string value = strInput.Replace(key, "");// ok

            //if (value.Trim().Equals("")) return;


            while (key.Length > 0)
            {
                string keyEntry = key.Substring(1, key.IndexOf("]") - 1); 

                if (keyEntry.IndexOf(".") > 0)
                {
                    keyEntry = keyEntry.Substring(0, keyEntry.IndexOf("."));
                }
                if (keyEntry.Length < 5)
                {
                    string[] subkey = keyEntry.Split(':');
                    if (subkey[0].Length < 2) subkey[0] = "0" + subkey[0];
                    if (subkey[1].Length < 2) subkey[1] = "0" + subkey[1];
                    keyEntry = subkey[0] + ":" + subkey[1];
                }


                keyList.Add(keyEntry);

                key = key.Replace(key.Substring(0, key.IndexOf("]") + 1), "");
                

            }
             
            foreach (string keyStr in keyList)
            {

                string keyStr1 = keyStr;// FormShowLyrics.getTimeFromInt(keyStr, -1);
                if (!lrcinfo.infos.ContainsKey(keyStr1))
                {
                    lrcinfo.infos.Add(keyStr1, value);

                }
                else
                {
                    string value_org = lrcinfo.infos[keyStr1].ToString();

                    lrcinfo.infos.Remove(keyStr1);

                    lrcinfo.infos.Add(keyStr1, value_org + " " + value);

                }
           
            }

        }

        void q_SelectSong(object sender, EventArgs e)
        {
            XmlNodeList list = sender as XmlNodeList;
            if (list != null)
            {
                FormSelectLyrics song = new FormSelectLyrics(list);
                song.DataGrid.RowHeaderMouseDoubleClick += new DataGridViewCellMouseEventHandler(delegate(object _sender, DataGridViewCellMouseEventArgs _e)
                {
                    //int index = _e.RowIndex;
                    //if (index >= 0 & index < list.Count)
                    //{
                    //    q.CurrentSong = list[_e.RowIndex];
                    //}
                    //song.Close();
                });
                song.ShowDialog();
            }
        }
    }
}
