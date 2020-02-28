using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LibNLPDB;

namespace NLPDB
{
    public partial class HandChunk : Form
    {
        //Data datMain;
        int intFirstIndex = -1;
        int intLastIndex = -1;
        string strHighlightedText = "";
        StreamWriter swHandChunks;
        string strFilename = "";
        //Dictionary<int, int[]> dI2P = new Dictionary<int, int[]>();
        
        //Initialization
        public HandChunk(ref DataStore dsrCurrent)
        {
            InitializeComponent();

            StringBuilder sbText = new StringBuilder();

            foreach (string sentence in dsrCurrent.ldrMain[0].libWords.SentenceList.Values)
            {
                sbText.AppendLine(sentence);
            }

            tbxWorking.Text = sbText.ToString();
            tbxWorking.Select(0, 0);

            strFilename = dsrCurrent.ldrMain[dsrCurrent.ldrMain.Count() - 1].libInput.InsertStringIntoFilename("-HandChunks");
            swHandChunks = new StreamWriter(strFilename,true);

            //try
            //{
            //    LoadWordPositionCharcterIndex(ref dsrCurrent);
            //}
            //catch
            //{
            //    CreateWordPositionCharacterIndexFile(ref dsrCurrent);
            //}
        }

        //Word Position -> Character Index Dictionary
        //private void WriteWordPositionCharacterIndex(ref DataStore dsrCurrent)
        //{
        //    StreamWriter swHandChunks = new StreamWriter(dsrCurrent.ldrMain[dsrCurrent.ldrMain.Count - 1].libInput.InsertStringIntoFilename("-WordPositionCharacterIndex.txt"), false);

        //    foreach (int intWordPosition in dI2P.Keys)
        //    {
        //        swHandChunks.WriteLine(intWordPosition.ToString() + " "
        //            + dI2P[intWordPosition][0].ToString() + " "
        //            + dI2P[intWordPosition][1].ToString());
        //    }

        //    swHandChunks.Close();
        //}

        //private void LoadWordPositionCharcterIndex(ref DataStore dsrCurrent)
        //{
        //    StreamReader srHandChunks = new StreamReader(dsrCurrent.ldrMain[dsrCurrent.ldrMain.Count - 1].libInput.InsertStringIntoFilename("-WordPositionCharacterIndex.txt"));
        //    string strLine = "";
        //    int intWordPosition = 0;
        //    int intFirstIndex = 0;
        //    int intLength = 0;

        //    dI2P.Clear();

        //    while (!srHandChunks.EndOfStream)
        //    {
        //        strLine = srHandChunks.ReadLine();
        //        intWordPosition = Convert.ToInt32(strLine.Split()[0]);
        //        intFirstIndex = Convert.ToInt32(strLine.Split()[1]);
        //        intLength = Convert.ToInt32(strLine.Split()[2]);

        //        dI2P.Add(intWordPosition, (int[])Array.CreateInstance(typeof(int), 2));
        //        dI2P[intWordPosition][0] = intFirstIndex;
        //        dI2P[intWordPosition][1] = intLength;
        //    }
        //}

        //private void CreateWordPositionCharacterIndexFile(ref DataStore dsrCurrent)
        //{
        //    string strWord = "";
        //    int intWordPosition = 0;  //Although not explicitly tested for, this should equate with the WordPosition data file

        //    for (int intCharacterIndex = 0; intCharacterIndex < tbxWorking.Text.Length; intCharacterIndex++)
        //    {
        //        switch (tbxWorking.Text[intCharacterIndex])
        //        {
        //            default:
        //                strWord += tbxWorking.Text[intCharacterIndex];
        //                break;
        //            case ' ':
        //                if (strWord.Trim() != "")
        //                {
        //                        intWordPosition++;
        //                        dI2P.Add(intWordPosition, (int[])Array.CreateInstance(typeof(int), 2));
        //                        dI2P[intWordPosition][0] = intCharacterIndex - strWord.Length;
        //                        dI2P[intWordPosition][1] =  strWord.Length;

        //                        strWord = "";
        //                }
        //                break;
        //        }
        //    }

        //    //Write Data File
        //    WriteWordPositionCharacterIndex(ref dsrCurrent);
        //}

        //private void UpdateWordPositionCharacterIndex(int intMessageLength, bool bAdd)
        //{
        //    var PositionFromIndex =
        //        from wp in dI2P.Keys
        //        where dI2P[wp][0] == intFirstIndex
        //        select wp;

        //    int intPositionFromIndex = PositionFromIndex.First();

        //    for (int intCurrentWordPosition = intPositionFromIndex; intCurrentWordPosition <= dI2P.Count; intCurrentWordPosition++)
        //    {
        //        if (intCurrentWordPosition > intPositionFromIndex)
        //        {
        //            if (bAdd)
        //            {
        //                dI2P[intCurrentWordPosition][0] += intMessageLength + 1;
        //            }
        //            else
        //            {
        //                dI2P[intCurrentWordPosition][0] -= intMessageLength + 1;
        //            }
        //        }

        //        if (bAdd)
        //        {
        //            dI2P[intCurrentWordPosition][1] += intMessageLength + 1;
        //        }
        //        else
        //        {
        //            dI2P[intCurrentWordPosition][1] -= intMessageLength + 1;
        //        }
        //    }
        //}

        //Boundaries
        
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

        //Tags
        private string CheckSpecialTags()
        {
            string strReturn = "";

            if (cbxName.Checked)
            {
                if (!rbnThing.Checked)
                {
                    rbnThing.Checked = true;
                }

                strReturn = "[T|[N|";
            }

            return strReturn;
        }

        private void InsertText(object objTextContainer, string strAddMessageTemp)
        {
            int intMessageLength = strAddMessageTemp.Length;

            //insert into TextBox
            ((TextBox)objTextContainer).Text = ((TextBox)objTextContainer).Text.Insert(intFirstIndex, strAddMessageTemp);
            intLastIndex += intMessageLength;
            ((TextBox)objTextContainer).Text = ((TextBox)objTextContainer).Text.Insert(intLastIndex + 1, "]");

            //UpdateWordPositionCharacterIndex(intMessageLength, true);

            //reset textbox selection
            ResetTextBoxSelection((TextBox)objTextContainer);
        }

        private void RemoveText(object objTextContainer, string strRemoveMessageTemp)
        {
            if (((TextBox)objTextContainer).Text.Substring(intFirstIndex, strRemoveMessageTemp.Length) == strRemoveMessageTemp)
            {
                int intMessageLength = strRemoveMessageTemp.Length;

                //remove from TextBox
                ((TextBox)objTextContainer).Text = ((TextBox)objTextContainer).Text.Remove(intFirstIndex, strRemoveMessageTemp.Length);
                intLastIndex -= intMessageLength;
                ((TextBox)objTextContainer).Text = ((TextBox)objTextContainer).Text.Remove(intLastIndex, 1);

                //UpdateWordPositionCharacterIndex(intMessageLength, false);

                //reset textbox selection
                ResetTextBoxSelection((TextBox)objTextContainer);
            }
        }

        //User Form Functions
        private void tbxWorking_MouseUp(object sender, MouseEventArgs e)
        {
            if (cbxTagOn.Checked)
            {
                string strMessage = "";
                string strCheckSpecialTags = "";

                FindBoundaries((TextBox)sender);

                //if (tabMain.SelectedTab.Name == "tabSynonyms")
                //{
                //    //Retrieve synonym data for the highlighted phrase and populate cboPhraseSynonyms
                //    //Dynamically generate comboboxes for each word in the phrase, 
                //    // place them in flpWordSynonyms and populate them with synonyms for each word
                //    //OPTIONAL: Set lblDetectedPhrase to detected phrase type (e.g. "Prepositional Phrase", "Adverbial Phrase")
                //}

                if (rbnThing.Checked)
                {
                    strMessage = "[T|";
                }
                else if (rbnExistence.Checked)
                {
                    strMessage = "[E|";
                }
                else if (rbnAction.Checked)
                {
                    strMessage = "[A|";
                }
                else if (rbnThingProperty.Checked)
                {
                    strMessage = "[TP|";
                }
                else if (rbnExistenceProperty.Checked)
                {
                    strMessage = "[EP|";
                }
                else if (rbnActionProperty.Checked)
                {
                    strMessage = "[AP|";
                }

                if (tbxCustomText.Text.Trim() != "")
                {
                    strMessage = tbxCustomText.Text.Trim();
                }

                //Check Special Tags
                strCheckSpecialTags = CheckSpecialTags();

                if (strCheckSpecialTags != "")
                {
                    strMessage = strCheckSpecialTags;
                }

                //Perform Tag Operation
                if (rbnTag.Checked)
                {
                    InsertText(sender, strMessage);
                    swHandChunks.WriteLine("+" + strHighlightedText);
                }
                else if (rbnUntag.Checked)
                {
                    RemoveText(sender, strMessage);
                    swHandChunks.WriteLine("-" + strHighlightedText);
                }

                lblHighlightedPhrase.Text = "Highlighted Phrase: " + strHighlightedText;
            }
        }

        private void btnCustomTextEnter_Click(object sender, EventArgs e)
        {
            //Is there highlighted text to replace?
            if (intLastIndex - intFirstIndex > -1 && tbxCustomText.Text.Trim() != "")
            {
                tbxWorking.Text = tbxWorking.Text.Remove(intFirstIndex, tbxWorking.SelectionLength);
                tbxWorking.Text = tbxWorking.Text.Insert(intFirstIndex, tbxCustomText.Text);
            }

            //Connect new custom text as a tag of the text currently selected in tbxWorking
        }

        //Form Closing - Write Hand Chunks File
        private void HandChunk_FormClosing(object sender, FormClosingEventArgs e)
        {
            swHandChunks.Close();
        }
    }
}
