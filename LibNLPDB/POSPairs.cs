using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LibNLPDB
{
    public class POSPairs
    {
        public void CreatePOSPairs(ref Input libInput, ref POS libPOS, ref Words libWords)
        {
            Directory.CreateDirectory(libInput.GetPOSPairsDirectory());

            StreamWriter swPOSPairCounts = new StreamWriter(libInput.GetPOSPairsFilename("Counts"));
            StringBuilder sbPOSPairCounts = new StringBuilder();

            foreach (string strPOS1 in libPOS.lstrTags.OrderBy(a => a))
            {
                foreach (string strPOS2 in libPOS.lstrTags.OrderBy(a => a))
                {
                    try
                    {
                        List<int> lPOSPair = libPOS.GetPOSPairPositions(strPOS1, strPOS2);
                        Dictionary<string, int> dPOSPairCount = new Dictionary<string, int>();

                        if (lPOSPair.Count() > 0) //don't create files for nonexistent POS combinations
                        {
                            string strPOSPairFilename = libInput.GetPOSPairsFilename(strPOS1 + "-" + strPOS2);
                            StreamWriter swPOSPair = new StreamWriter(strPOSPairFilename);
                            StringBuilder sbPOSPair = new StringBuilder();

                            foreach (int intFirstUWID in lPOSPair)
                            {
                                sbPOSPair.Append(libWords.GetPositionWord(intFirstUWID)
                                    + " " + libWords.GetPositionWord(intFirstUWID + 1));
                                sbPOSPair.Append(" ^ ");
                                sbPOSPair.AppendLine(intFirstUWID.ToString());
                            }

                            swPOSPair.Write(sbPOSPair.ToString());
                            swPOSPair.Close();

                            foreach (string strPOSPairLine in sbPOSPair.ToString().Split('\n'))
                            {
                                string strPOSPairWords = strPOSPairLine.Split('^')[0].Trim();

                                if (!dPOSPairCount.ContainsKey(strPOSPairWords))
                                {
                                    dPOSPairCount.Add(strPOSPairWords, 1);
                                }
                                else
                                {
                                    dPOSPairCount[strPOSPairWords]++;
                                }
                            }

                            sbPOSPairCounts.AppendLine("[" + strPOS1 + "] [" + strPOS2 + "]");

                            foreach (string strPOSPairCount in dPOSPairCount.Keys.OrderBy(a => a))
                            {
                                if (strPOSPairCount.Trim() != "")
                                {
                                    sbPOSPairCounts.Append(strPOSPairCount);
                                    sbPOSPairCounts.Append(" ^ ");
                                    sbPOSPairCounts.AppendLine(dPOSPairCount[strPOSPairCount].ToString());
                                }
                            }

                            sbPOSPairCounts.AppendLine();
                        }
                    }
                    catch { }
                }
            }

            swPOSPairCounts.Write(sbPOSPairCounts.ToString());
            swPOSPairCounts.Close();
        }

        public void LoadPOSPairs(string strPOSPairsDirectoryName)
        {
            foreach (string strFilename in Directory.EnumerateFiles(strPOSPairsDirectoryName))
            {
                if (strFilename.Contains("Counts")) { break; } //The Counts file has a different format

                StreamReader srPOSPair = new StreamReader(strFilename);

                while (!srPOSPair.EndOfStream)
                {
                    string strPOSPairLine = srPOSPair.ReadLine();
                    //NO REASON TO LOAD YET
                    //COMPLETE CODE WHEN IT'S NEEDED, SILLY
                }
            }
        }
    }
}
