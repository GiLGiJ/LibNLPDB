using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NLPDB
{
    public partial class GrammarParser : Form
    {
        Dictionary<string, List<string>> dWordPOS = new Dictionary<string, List<string>>();

		public GrammarParser(ref DataStore dsrTemp)
        {
            InitializeComponent();

            CreateWordPOS(ref dsrTemp);
        }

		public void CreateWordPOS(ref DataStore dsrTemp)
        {
			System.IO.StreamWriter swWordPOS = new System.IO.StreamWriter(dsrTemp.ldrMain.First().libInput.InsertStringIntoFilename("-WordPOS"));

			foreach (int intSentenceID in dsrTemp.ldrMain.First().libWords.SentenceFirstPositions.Keys.OrderBy(a => a))
            {
				int intSentenceFirstPosition = dsrTemp.ldrMain.First().libWords.SentenceFirstPositions[intSentenceID];

				for (int intUWID = intSentenceFirstPosition; intUWID <= intSentenceFirstPosition + dsrTemp.ldrMain.First().libWords.SentenceLengths[intSentenceID] - 1; intUWID++)
                {
					string strWord = dsrTemp.ldrMain.First().libWords.GetPositionWord(intUWID);
					string strPOS = dsrTemp.ldrMain.First().libWords.GetPositionPOS(intUWID, ref dsrTemp.ldrMain.First().libPOS);

                    if (!dWordPOS.ContainsKey(strWord))
                    {
                        dWordPOS.Add(strWord, new List<string>());
                    }

                    if (!dWordPOS[strWord].Contains(strPOS))
                    {
                        dWordPOS[strWord].Add(strPOS);
                    }
                }
            }

			foreach (string strWord in dWordPOS.OrderBy(a=>a.Key).Select(a=>a.Key))
            {
                string strPOSs = "";

                foreach (string strPOS in dWordPOS[strWord])
                {
                    strPOSs += strPOS + " ";
                }

				swWordPOS.WriteLine(strWord + " ^ " + strPOSs.Trim());
            }

            swWordPOS.Close();
        }
    }
}
