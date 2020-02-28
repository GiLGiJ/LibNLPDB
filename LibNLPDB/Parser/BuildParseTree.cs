using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LibNLPDB.Parser
{
    public class BuildParseTree
    {
        private Input input;
        public Dictionary<int, Dictionary<int, string>> dParseTree;
        Parse parse;

        public BuildParseTree(ref Input inputTemp, ref Parse parseTemp)
        {
            input = inputTemp;
            parse = parseTemp;
            dParseTree = parse.ParsePartsData;
        }

        public Dictionary<int, TOP> Parse()
        {
            //dict<index, object>
            //dict<index, index of object this object is inside>
            //when adding each object, 
            Dictionary<int, TOP> dReturn = new Dictionary<int, TOP>();
            //StreamReader srParse = new StreamReader(input.InsertStringIntoFilename("-Parse"));
            TOP topCurrent = new TOP();
            string strLastParsePartLabel = "";
            int intLastParsePartLevel = -1;
            //int intOpenLevel = 0;
            //int intLastOpenIndex = -1;
            //int intLastCloseIndex = -1;
            
            //foreach (int intSentenceID in dParseTree.Keys.OrderBy(a=> a))
            //while (!srParse.EndOfStream)
            foreach (int intBranchID in parse.dBranches.Keys.OrderBy(a=>a))
            {
                foreach (int intSequenceIndex in parse.dBranches[intBranchID].Keys.OrderBy(a=> a))
                {
                    int intParsePartID = parse.dBranches[intBranchID][intSequenceIndex];
                    string strParsePartLabel = parse.dBranchLabels[intBranchID].Trim();
                    int intParsePartLevel = parse.dBranchLevels[intBranchID];
                    
                    switch (strParsePartLabel)
                    {
                        default:
                            if (intParsePartLevel == intLastParsePartLevel)
                            {
                                try
                                {
                                    dReturn.Last().Value.CurrentSentence().CurrentCC().strPOS = strParsePartLabel;
                                }
                                catch { }
                            }

                            break;
                        case "top":
                            topCurrent = new TOP();
                            dReturn.Add(dReturn.Count() + 1, topCurrent);
                            break;
                        //case "np":
                        //    if (strLastParsePartLabel == "top")
                        //    {
                        //        dReturn.Last().Value.CurrentSentence().CurrentCC().CurrentWord().strPOS = "np";
                        //    }
                        //    else
                        //    {
                        //        string str = "";
                        //    }

                        //    strLastParsePartLabel = strParsePartLabel;
                        //    break;
                    }


                    strLastParsePartLabel = strParsePartLabel;
                    intLastParsePartLevel = intParsePartLevel;

                    string strBreakHere = "";
                    //foreach (string strPart in intParsePart.ToString().Trim().Split((char[])" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                    //{
                    //    intPartCounter++;

                    //    if (intPartCounter == 1)
                    //    {
                    //        switch(strPart.ToLower())
                    //        {
                    //            default:
                    //                break;
                    //            case "s":
                    //                dReturn[dReturn.Count].NewSentence();
                    //                break;
                    //            case "np":
                    //                //((S)dReturn[dReturn.Count].Last()).
                    //                break;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        //check number for existence in dictionary
                    //        //-if exists, 
                    //    }
                    //}

                }
                //string strLine = srParse.ReadLine();
                //Dictionary<int, string> dLevelTypes = new Dictionary<int, string>(); //D<level, type>

                //foreach (Group grpCurrent in rgxParse.Matches(strLine.Split("^".ToCharArray())[1]))
                //{
                //    foreach (string strPart in grpCurrent.Value.Split())
                //    {
                //        if (strPart.StartsWith("("))
                //        {
                //            intOpenLevel++;

                //            if (dLevelTypes.ContainsKey(intOpenLevel))
                //            {
                //                dLevelTypes.Add(intOpenLevel, "");
                //            }

                //            dLevelTypes[intOpenLevel] = strPart.TrimStart("(".ToCharArray()).Trim();

                //            switch (strPart.TrimStart("(".ToCharArray()).Trim())
                //            {
                //                default:
                //                    dReturn[dReturn.Count()].CurrentSentence().CurrentCC().AddWord();
                //                    dReturn[dReturn.Count()].CurrentSentence().CurrentCC().CurrentWord().strPOS =
                //                        strPart.TrimStart("(".ToCharArray()).Trim();
                //                    break;
                //                case "TOP":
                //                    dReturn.Add(dReturn.Count() + 1, new TOP());
                //                    break;
                //                case "S":
                //                    dReturn[dReturn.Count()].NewSentence();
                //                    dReturn[dReturn.Count()].CurrentSentence().AddCC();
                //                    break;
                //                case "CC":
                //                    dReturn[dReturn.Count()].CurrentSentence().AddCC();
                //                    break;
                //            }
                //        }
                //        else if (strPart.EndsWith(")"))
                //        {
                //            if (strPart.Length > 1)
                //            {
                //                //word
                //                dReturn[dReturn.Count()].CurrentSentence().CurrentCC().CurrentWord().strWord = strPart.TrimEnd(")".ToCharArray()).Trim();
                //            }

                //            dLevelTypes.Remove(intOpenLevel);
                //            intOpenLevel--;
                //        }
                //    }
                    
                //}



                //get type from Parse data
                //switch on type
                //...
                //topCurrent = new TOP();
                //...

                
            }

            return dReturn;
        }
        

    }
}
