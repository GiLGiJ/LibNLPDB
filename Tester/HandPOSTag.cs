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
    public partial class HandPOSTag : Form
    {
        int intFirstIndex = -1;
        int intLastIndex = -1;
        string strHighlightedText = "";
        StreamWriter swHandPOSTag;
        string strHandTagFilename;
        string strFilename;
        DataStore dsrMain;
        
        public HandPOSTag(ref DataStore dsrCurrent, string strInputFilename, bool bPOSOrder = false)
        {
            InitializeComponent();
            
            Setup(ref dsrCurrent, strInputFilename, bPOSOrder);
        }

        public void Setup(ref DataStore dsrCurrent, string strInputFilename, bool bPOSOrder)
        { 
            StringBuilder sbText = new StringBuilder();

            dsrMain = dsrCurrent;
            strHandTagFilename = dsrMain.ldrMain[dsrMain.ldrMain.Count() - 1].libInput.InsertStringIntoFilename("-HandPOSTag");
            strFilename = strInputFilename;

            foreach (int intUWID in dsrMain.GetRecord(strInputFilename).libWords.PositionWords.Keys.OrderBy(a => a))
            {
                string strPOS = dsrMain.GetRecord(strInputFilename).libPOS.POSs[intUWID];

                if (strPOS != "")
                {
                    sbText.Append("[");
                    sbText.Append(strPOS);
                    sbText.Append("|");
                }

                sbText.Append(dsrMain.GetRecord(strInputFilename).libWords.GetPositionWord(intUWID));
                sbText.Append(" ");
            }

            tbxWorking.Text = sbText.ToString();
            tbxWorking.Select(0, 0);

            swHandPOSTag = new StreamWriter(strHandTagFilename, true);
        }

        private void ResetSelectionVariables(TextBox tbxWorking)
        {
            intFirstIndex = tbxWorking.SelectionStart;
            intLastIndex = intFirstIndex + tbxWorking.SelectionLength - 1;
            strHighlightedText = tbxWorking.SelectedText;
        }

        private void ResetTextBoxSelection(TextBox tbxWorking)
        {
            tbxWorking.SelectionStart = intFirstIndex;
            tbxWorking.SelectionLength = intLastIndex - intFirstIndex + 1;
            strHighlightedText = tbxWorking.SelectedText;
        }

        private void FindBoundaries(TextBox tbxWorking)
        {
            bool bFindingBoundary = true;

            ResetSelectionVariables((TextBox)tbxWorking);

            //reverse highlighting support
            if (intFirstIndex > intLastIndex)
            {
                int intTempIndex = intFirstIndex;

                intFirstIndex = intLastIndex;
                intLastIndex = intTempIndex;
            }

            //find word boundaries
            while (bFindingBoundary)
            {
                if (intFirstIndex > 0)
                {
                    intFirstIndex--;

                    if (((TextBox)tbxWorking).Text[intFirstIndex] == ' ' ||
                        ((TextBox)tbxWorking).Text[intFirstIndex] == '\n')
                    {
                        intFirstIndex++;

                        bFindingBoundary = false;
                    }
                }
                else
                {
                    bFindingBoundary = false;
                }
            }

            bFindingBoundary = true;

            while (bFindingBoundary)
            {
                if (intLastIndex < ((TextBox)tbxWorking).Text.Length - 1)
                {
                    intLastIndex++;

                    if (((TextBox)tbxWorking).Text[intLastIndex] == ' ' ||
                        ((TextBox)tbxWorking).Text[intLastIndex] == '\r')
                    {
                        intLastIndex--;

                        bFindingBoundary = false;
                    }
                }
                else
                {
                    bFindingBoundary = false;
                }
            }

            ResetTextBoxSelection((TextBox)tbxWorking);
        }

        private void UpdateText(object objTextContainer, string strPOS)
        {
            if (((TextBox)objTextContainer).Text.Substring(intFirstIndex, 1) == "[")
            {
                int intBar = ((TextBox)objTextContainer).Text.IndexOf("|", intFirstIndex);

                ((TextBox)objTextContainer).Text = ((TextBox)objTextContainer).Text.Remove(intFirstIndex, intBar - intFirstIndex + 1);
                intLastIndex -= intBar - intFirstIndex + 1;
            }

            ((TextBox)objTextContainer).Text = ((TextBox)objTextContainer).Text.Insert(intFirstIndex, "[" + strPOS + "|");
            intLastIndex += strPOS.Length + 2;

            //reset textbox selection
            ResetTextBoxSelection((TextBox)objTextContainer);
        }

        //User Form Functions
        private void tbxWorking_MouseUp(object sender, MouseEventArgs e)
        {
            string strPOS = "";
            int intUWID = 0;

            FindBoundaries((TextBox)sender);

            intUWID = tbxWorking.Text.Substring(0, intFirstIndex).Count(a => a == ' ') + 1;

            foreach (RadioButton rbnPOS in gbxPOSTags.Controls)
            {
                if (rbnPOS.Checked)
                {
                    strPOS = rbnPOS.Text;
                    break;
                }
            }

            UpdateText(sender, strPOS);

            dsrMain.GetRecord(strFilename).libPOS.POSs[intUWID] = strPOS;

            swHandPOSTag.WriteLine("+" + strHighlightedText);
        }

        private void HandPOSTag_FormClosing(object sender, FormClosingEventArgs e)
        {
            string strPOS = dsrMain.GetRecord(strFilename).libInput.InsertStringIntoFilename("-POS");
            string strArff = dsrMain.GetRecord(strFilename).libInput.InsertStringIntoArffFilename("-POS");

            swHandPOSTag.Close();

            //WritePOS writes both .arff and .txt files
            dsrMain.GetRecord(strFilename).libPOS.WritePOS(strPOS, strArff, dsrMain.GetRecord(strFilename).libInput.Base, ref dsrMain.GetRecord(strFilename).libWords);
            dsrMain.GetRecord(strFilename).libCombinedPOS.CreateCombinedPOS(ref dsrMain.GetRecord(strFilename).libInput, ref dsrMain.GetRecord(strFilename).libWords, ref dsrMain.GetRecord(strFilename).libPOS);
        }

        private void btnChangeToNameFromFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofdlgNP = new OpenFileDialog();
            string strNamesFilename = "";
            StreamReader srNames;
            string strCurrentName = "";

            while (ofdlgNP.ShowDialog() == DialogResult.OK)
            {
                strNamesFilename = ofdlgNP.FileName;
            }

            srNames = new StreamReader(strNamesFilename);

            while (!srNames.EndOfStream)
            {
                strCurrentName = srNames.ReadLine().Split()[0];

                foreach (int intCurrentPositionID in dsrMain.GetRecord(strFilename).libWords.GetWordPositions(strCurrentName))
                {
                    dsrMain.GetRecord(strFilename).libPOS.POSs[intCurrentPositionID] = "NP";
                }
            }
        }
    }
}
