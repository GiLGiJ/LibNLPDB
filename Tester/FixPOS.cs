using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NLPDB
{
    public partial class FixPOS : Form
    {
        DataStore dsrMain;

        public FixPOS(ref DataStore dsrTemp)
        {
            InitializeComponent();

            dsrMain = dsrTemp;
        }

        public void btnNames_Click(object sender, EventArgs e)
        {
            if (dsrMain.ldrMain[0].libPOS != null &&
                dsrMain.ldrMain[0].libWords != null)
            {
                string strLocation = "";
				ofdlgPOSUpdateFile.Title = "Pick Names File";

                if (ofdlgPOSUpdateFile.ShowDialog() == DialogResult.OK)
                {
                    strLocation = ofdlgPOSUpdateFile.FileName;
                }

                if (strLocation != "")
                {
                    StreamReader srNames = new StreamReader(strLocation);

					//prepare data by changing all NNP to NN
					foreach (int intWordPosition in dsrMain.ldrMain[0].libPOS.GetPOSPositions("NNP")) {
						dsrMain.ldrMain [0].libPOS.POSs [intWordPosition] = "NN";
					}

					//update names with "NNP"
                    while (!srNames.EndOfStream)
                    {
                        string strLine = srNames.ReadLine().Split('^')[0].Trim();

                        foreach (int intWordPosition in dsrMain.ldrMain[0].libWords.PositionWords.Where(a=>a.Value == strLine).Select(a=>a.Key))
                        {
                            dsrMain.ldrMain[0].libPOS.POSs[intWordPosition] = "NNP";
                        }
                    }

                    dsrMain.ldrMain[0].libPOS.WritePOS(dsrMain.ldrMain[0].libInput.InsertStringIntoFilename("-POS"), dsrMain.ldrMain[0].libInput.InsertStringIntoArffFilename("-POS"), dsrMain.ldrMain[0].libInput.Base, ref dsrMain.ldrMain[0].libWords);
                }
            }
            else
            {
                MessageBox.Show("Open a Text File, Tokenize, and Create POS First");
            }
        }

        private void btnPOSWords_Click(object sender, EventArgs e)
        {
            //
            //TODO: get word lists ("POSWordsFix") data file and apply it here
            //
            if (dsrMain.ldrMain[0].libPOS != null &&
                dsrMain.ldrMain[0].libWords != null)
            {
                string strLocation = "";
                ofdlgPOSUpdateFile.Title = "Pick Names File";

                if (ofdlgPOSUpdateFile.ShowDialog() == DialogResult.OK)
                {
                    strLocation = ofdlgPOSUpdateFile.FileName;
                }

                if (strLocation != "")
                {
                    //TODO
                    StreamReader srNames = new StreamReader(strLocation);

                    //prepare data by changing all NNP to NN
                    foreach (int intWordPosition in dsrMain.ldrMain[0].libPOS.GetPOSPositions("NNP"))
                    {
                        dsrMain.ldrMain[0].libPOS.POSs[intWordPosition] = "NN";
                    }

                    //update names with "NNP"
                    while (!srNames.EndOfStream)
                    {
                        string strLine = srNames.ReadLine().Split('^')[0].Trim();

                        foreach (int intWordPosition in dsrMain.ldrMain[0].libWords.PositionWords.Where(a => a.Value == strLine).Select(a => a.Key))
                        {
                            dsrMain.ldrMain[0].libPOS.POSs[intWordPosition] = "NNP";
                        }
                    }

                    dsrMain.ldrMain[0].libPOS.WritePOS(dsrMain.ldrMain[0].libInput.InsertStringIntoFilename("-POS"), dsrMain.ldrMain[0].libInput.InsertStringIntoArffFilename("-POS"), dsrMain.ldrMain[0].libInput.Base, ref dsrMain.ldrMain[0].libWords);
                }
            }
            else
            {
                MessageBox.Show("Open a Text File, Tokenize, and Create POS First");
            }
        }
    }
}
