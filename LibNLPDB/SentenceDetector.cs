using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LibNLPDB
{
    public partial class SentenceDetector : Form
    {
        int intFirstIndex, intLastIndex = -1;
        SortedList<int, string> slSentences = new SortedList<int, string>();
        int intSentenceIDShowing = -1;

        public SentenceDetector(SortedList<int, string> slSentencesTemp)
        {
            InitializeComponent();

            slSentences = slSentencesTemp;
            intSentenceIDShowing = slSentences.First().Key;
            
            UpdateOutput();
            
        }

        private void UpdateOutput()
        {
            tbxSentence.Text = intSentenceIDShowing.ToString() + "\t\t\t" +
                slSentences[intSentenceIDShowing] + "\r\n";
        }

        private void tbxSentence_Click(object sender, EventArgs e)
        {
            intFirstIndex = tbxSentence.SelectionStart;
            intLastIndex = intFirstIndex - 1;
            
            InsertText(sender, "^");
            //RemoveText(sender, "^");

            tbxOutput.Text = "";
            string[] strsSentences = tbxSentence.Text.Split((char[])"^".ToCharArray());

            for (int intSentenceCounter = 1; intSentenceCounter <= strsSentences.Count(); intSentenceCounter++)
            {
                tbxOutput.Text += intSentenceCounter.ToString() + "\t\t\t" +
                    strsSentences[intSentenceCounter - 1] + "\r\n\r\n";
            }
        }

        private void InsertText(object objTextContainer, string strAddMessageTemp)
        {
            int intMessageLength = strAddMessageTemp.Length;

            //insert into TextBox
            ((TextBox)objTextContainer).Text = ((TextBox)objTextContainer).Text.Insert(intFirstIndex, strAddMessageTemp);
            intLastIndex += intMessageLength;
        }

        private void RemoveText(object objTextContainer, string strRemoveMessageTemp)
        {
            if (((TextBox)objTextContainer).Text.Substring(intFirstIndex, strRemoveMessageTemp.Length) == strRemoveMessageTemp)
            {
                int intMessageLength = strRemoveMessageTemp.Length;

                //remove from TextBox
                ((TextBox)objTextContainer).Text = ((TextBox)objTextContainer).Text.Remove(intFirstIndex, strRemoveMessageTemp.Length);
                intLastIndex -= intMessageLength;
            }
        }

        private void SentenceDetector_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

    }
}
