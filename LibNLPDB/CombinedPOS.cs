using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LibNLPDB
{
    public class CombinedPOS
    {
        private Dictionary<string, string> dCombinedPOS = new Dictionary<string, string>();
        string strCombinedPOSFilename = "";

        public Dictionary<string, string> CombinedPOSs
        {
            get
            {
                return dCombinedPOS;
            }
            set
            {
                dCombinedPOS = value;
            }
        }

        public CombinedPOS() { }

        public bool LoadCombinedPOS(ref Input input)
        {
            strCombinedPOSFilename = input.InsertStringIntoFilename("-CombinedPOS");
            
            bool bReturn = false;

            dCombinedPOS.Clear();

            if (File.Exists(strCombinedPOSFilename))
            {
                StreamReader srPOS = new StreamReader(strCombinedPOSFilename);

                while (!srPOS.EndOfStream)
                {
                    string strLine = srPOS.ReadLine();

                    try
                    {
                        string strKey = strLine.Split('^')[0].Trim();
                        string strValue = strLine.Split('^')[1].Trim();

                        dCombinedPOS.Add(strKey, strValue);
                    }
                    catch { }
                }

                srPOS.Close();

                bReturn = true;
            }

            return bReturn;
        }

        public void CreateCombinedPOS(ref Input input, ref Words words, ref POS pos)
        {
            strCombinedPOSFilename = input.InsertStringIntoFilename("-CombinedPOS");

            dCombinedPOS.Clear();

            for (int intWordID = 1; intWordID <= words.WordIDs.Count(); intWordID++)
            {
                List<string> lstrTags = new List<string>();
                string strTags = "";

                foreach (int intWordPosition in words.GetWordIDPositions(intWordID))
                {
                    try
                    {
                        string strCurrentPOS = pos.POSs[intWordPosition];

                        if (!lstrTags.Contains(strCurrentPOS))
                        {
                            lstrTags.Add(strCurrentPOS);
                        }
                    }
                    catch { }
                }

                foreach (string strTag in lstrTags)
                {
                    strTags += strTag + " ";
                }

                dCombinedPOS.Add(words.GetWordIDWord(intWordID), strTags.Trim());
            }

            WriteCombinedPOS(ref input);
        }

        public void WriteCombinedPOS(ref Input input)
        {
            strCombinedPOSFilename = input.InsertStringIntoFilename("-CombinedPOS");

            //Write Data File
            StreamWriter swCombinedPOS = new StreamWriter(strCombinedPOSFilename);

            foreach (string strWord in dCombinedPOS.Keys.OrderBy(a => a))
            {
                swCombinedPOS.WriteLine(strWord + " ^ " + dCombinedPOS[strWord]);
            }

            swCombinedPOS.Close();
        }   
    }
}
