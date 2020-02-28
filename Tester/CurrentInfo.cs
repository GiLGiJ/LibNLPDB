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
    public partial class CurrentInfo : Form
    {
        DataRecord drdCurrent;

        public CurrentInfo(ref DataRecord drdRecord)
        {
            InitializeComponent();

            drdCurrent = drdRecord;
        }

        private void CurrentInfo_Load(object sender, EventArgs e)
        {
            try
            {
                ChangeFilenames(drdCurrent.libInput.Base + drdCurrent.libInput.Ext);
            }
            catch { }
            
            try
            {
                ChangeWordIDs(drdCurrent.libWords.WordIDs.Count().ToString());
            }
            catch{}
            
            try
            {
                ChangeWordPositions(drdCurrent.libWords.PositionWords.Count().ToString());
            }
            catch{}
            
            try
            {
                ChangeHighestWordCount(drdCurrent.libWords.Counts.Max(a => a.Value).ToString());
            }
            catch{}

            try
            {
                ChangeSentenceCount(drdCurrent.libWords.SentenceList.Count().ToString());
            }
            catch { }

            try
            {
                ChangeLongestPhrase(drdCurrent.libPhrases.PhraseCounts.Max(a => a.Key.Split().Count()).ToString());
            }
            catch{}
            
            try
            {
                ChangePOSCount(drdCurrent.libPOS.POSs.Count().ToString());
            }
            catch{}
            
            try
            {
                ChangeMostFrequentPOSPhrase(drdCurrent.libPOSPhrases.POSPhrasesD.Max(a => a.Value).ToString());
            }
            catch{}
        }

        public void ChangeFilenames(string strChangeText)
        {
            lblCurrentFilename.Text = "Current Filename: " + strChangeText;
        }

        public void ChangeWordIDs(string strChangeText)
        {
            lblWordIDs.Text = "Word IDs: " + strChangeText;
        }

        public void ChangeWordPositions(string strChangeText)
        {
            lblWordPositions.Text = "Word Positions: " + strChangeText;
        }

        public void ChangeHighestWordCount(string strChangeText)
        {
            lblHighestWordCount.Text = "Highest Word Count: " + strChangeText;
        }

        public void ChangeSentenceCount(string strChangeText)
        {
            lblSentenceCount.Text = "Sentence Count: " + strChangeText;
        }

        public void ChangeLongestPhrase(string strChangeText)
        {
            lblLongestPhrase.Text = "Longest Phrase: " + strChangeText;
        }

        public void ChangePOSCount(string strChangeText)
        {
            lblPOSCount.Text = "POS Count: " + strChangeText;
        }

        public void ChangeMostFrequentPOSPhrase(string strChangeText)
        {
            lblMostFrequentPOSPhrase.Text = "Most Frequent POS Phrase Text: " + strChangeText;
        }
    }
}
