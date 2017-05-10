using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace iPodC
{
    public partial class FormSelectLyrics : Form
    {
        public FormSelectLyrics()
        {
            InitializeComponent();
        }
        public FormSelectLyrics(XmlNodeList nodes)
            : this() {
            DataTable dt = new DataTable();
            dt.Columns.Add("No.");
            dt.Columns.Add("Artist");
            dt.Columns.Add("Title");
            foreach (XmlNode node in nodes) {
                DataRow row = dt.NewRow();
                row[0] = node.Attributes["id"].Value;
                row[1] = node.Attributes["artist"].Value;
                row[2] = node.Attributes["title"].Value;
                dt.Rows.Add(row);
            }
            this.dataGridView1.DataSource = dt.DefaultView;
        }

        public DataGridView DataGrid {
            get {
                return this.dataGridView1;
            }
        }
    }
}