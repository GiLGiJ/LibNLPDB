using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NLPDB
{
    public partial class Classify : Form
    {
        string strFilename = "";
        Dictionary<string, List<string>> dClassifyMain = new Dictionary<string, List<string>>();

        public Classify()
        {
            InitializeComponent();
        }

        private void Classify_Load(object sender, EventArgs e)
        {
            StreamReader srClassifyWordList;
            string strCurrentPOSType = "Noun";
            string strCurrentClass = "NEW";

            lbxPOS.SelectedIndex = 0;

            if (ofdlgCurrent.ShowDialog() == DialogResult.OK)
            {
                strFilename = ofdlgCurrent.FileName;
            }

            if (strFilename.Trim() != "")
            {
                lblFilename.Text = "File: " + strFilename;
                srClassifyWordList = new StreamReader(strFilename);

                while (!srClassifyWordList.EndOfStream)
                {
                    string strLine = srClassifyWordList.ReadLine();

                    if (strLine.Trim() == "") //blank lines always reset the current class to "NEW"
                    {
                        strCurrentClass = "NEW";
                    }
                    else if (strLine.StartsWith("[") && strLine.EndsWith("]")) //square brackets that enclose a line signify the current class
                    {
                        strCurrentClass = strLine.TrimStart('[').TrimEnd(']');
                    }
                    else
                    {
                        dClassifyMain[strCurrentClass].Add(strLine.Split("^".ToCharArray())[0].Trim());
                    }

                    if (!dClassifyMain.ContainsKey(strCurrentClass))
                    {
                        dClassifyMain.Add(strCurrentClass, new List<string>());
                    }
                }

                srClassifyWordList.Close();

                lbxCategories.Items.Clear();

                foreach (string strCategory in dClassifyMain.Keys.OrderBy(a => a))
                {
                    lbxCategories.Items.Add(strCategory);

                    foreach (string strWord in dClassifyMain[strCategory])
                    {
                        lbxWords.Items.Add(strWord + " - " + strCategory);
                    }
                }
            }
        }
    }
}
