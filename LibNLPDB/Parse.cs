using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using OpenNLP.Tools.Parser;

namespace LibNLPDB
{
    public class Parse
    {
        private string mModelPath = "";

        private Dictionary<int, string> dParse = new Dictionary<int, string>();
        private Dictionary<int, Dictionary<int, string>> dParseParts = new Dictionary<int, Dictionary<int, string>>();
        public Dictionary<int, string> dLeaves = new Dictionary<int, string>();
        public Dictionary<int, Dictionary<int, int>> dBranches = new Dictionary<int, Dictionary<int, int>>();
        public Dictionary<int, string> dLeafLabels = new Dictionary<int, string>();
        public Dictionary<int, string> dBranchLabels = new Dictionary<int, string>();
        public Dictionary<int, int> dBranchLevels = new Dictionary<int, int>();
        public Dictionary<int, string> dBranchText = new Dictionary<int, string>();
        public Dictionary<int, int> dParsePartCount = new Dictionary<int, int>();
        
        //Parser.ParsePartsDS dsParseParts = new Parser.ParsePartsDS();

        public Dictionary<int, string> ParseData
        {
            get
            {
                return dParse;
            }
            set
            {
                dParse = value;
            }
        }

        public Dictionary<int, Dictionary<int, string>> ParsePartsData
        {
            get
            {
                return dParseParts;
            }
            set
            {
                dParseParts = value;
            }
        }

        public Parse(string strModelPath)
        {
            mModelPath = strModelPath;
        }

        public bool LoadParse(ref Input input)
        {
            string strParseParts = input.InsertStringIntoFilename("-Parse");
            bool bReturn = false;

            dParse.Clear();

            if (File.Exists(strParseParts))
            {
                StreamReader srParseParts = new StreamReader(strParseParts);

                while (!srParseParts.EndOfStream)
                {
                    string strLine = srParseParts.ReadLine();
                    int intSentenceID = Convert.ToInt32(strLine.Split('^')[0].Trim());
                    string strTag = strLine.Split('^')[1].Trim();

                    if (!dParse.ContainsKey(intSentenceID))
                    {
                        dParse.Add(intSentenceID, strTag);
                    }
                }

                srParseParts.Close();

                bReturn = true;
            }

            return bReturn;
        }

        public void CreateParseOpenNLP(string strModelPath, ref Input input, ref Words words, ref POS pOS)
        {
            StringBuilder output = new StringBuilder();
            EnglishTreebankParser mParser;// = EnglishTreebankParser;// (strModelPath);//, true, false, 20, 10); //, 4);
            StreamWriter swParse = new StreamWriter(input.InsertStringIntoFilename("-Parse"));
            int intCurrentPosition = 0;
            int intMaximumWordPositions = words.PositionWords.Count();

            mParser = new EnglishTreebankParser(strModelPath, true, false, 20, 10);

            dParse.Clear();

            foreach (int intSentenceID in words.SentenceList.Keys.OrderBy(a => a))
            {
                Dictionary<int, string> tokens = new Dictionary<int, string>();
                Dictionary<int, string> tags = new Dictionary<int, string>();
                int intTokenCounter = 0;
                string strCurrentSentence = words.SentenceList[intSentenceID];
                int intSentenceLength = words.SentenceLengths[intSentenceID];
                int intSentenceFirstPosition = words.SentenceFirstPositions[intSentenceID];

                if (intCurrentPosition >= intMaximumWordPositions)
                {
                    break;
                }

                if (intSentenceLength > 0)
                {
                    for (int intWordPosition = intSentenceFirstPosition; intWordPosition < intSentenceFirstPosition + intSentenceLength; intWordPosition++)
                    {
                        string strWord = words.PositionWords[intWordPosition];
                        string strPOS = "";

                        intTokenCounter++;
                        intCurrentPosition++;

                        strPOS = words.GetPositionPOS(intWordPosition, ref pOS);

                        tokens.Add(intTokenCounter, strWord);
                        tags.Add(intTokenCounter, strPOS);
                    }
                }

                if (tokens.Count > 0 && tokens.Count == tags.Count)
                {
                    string[] strsTokens = (string[])tokens.Values.ToArray();
                    string[] strsTags = (string[])tags.Values.ToArray();
                    try {
                        string strParseParts = mParser.DoParse(strCurrentSentence).Show();
                        dParse.Add(intSentenceID, strParseParts.Trim(" \r\n".ToCharArray()));
                    }
                    catch
                    {
                        dParse.Add(intSentenceID, "<Error: OpenNLP Parser DoParse(" + strCurrentSentence + ")>");
                    }
                }
                else if (tokens.Count != tags.Count)
                {
                    dParse.Add(intSentenceID, "<Token and Tag Counts Mismatch>");
                }
            }

            foreach (int intSentenceID in dParse.Keys.OrderBy(a => a))
            {
                swParse.WriteLine(intSentenceID.ToString() + " ^ " + dParse[intSentenceID]);
            }

            swParse.Close();
        }

        public void CreateParse(ref Input input, ref Words words, ref POS pos)
        {
            // nominal jj forms can be considered anywhere nn are considered
            Dictionary<string, Dictionary<string, float>> dPOSPhraseTagProbabilities =
                new Dictionary<string, Dictionary<string, float>>(); //D<POSPhrase, D<Semantic Tag, Probability>>

            foreach (int intSentenceID in words.SentenceList.Keys.OrderBy(a => a))
            {
                //First and Last Words.WordPositions in the Sentence, and the Size (in Words objects) of the Sentence (same as Sentences.SentenceLengths)
                int intFirstPosition = words.SentenceFirstPositions[intSentenceID];
                int intLastPosition = intFirstPosition + words.SentenceLengths[intSentenceID] - 1;
                int intSize = intLastPosition - intFirstPosition + 1;
                //Which words in the sentence have we evaluated at least once?
                bool[] accounting = (bool[])Array.CreateInstance(typeof(bool), intSize);
                
                for (int intWordPosition = intFirstPosition; intWordPosition <= intLastPosition; intWordPosition++)
                {
                    string strWord = words.PositionWords[intWordPosition];
                    string strPOS = words.GetPositionPOS(intWordPosition, ref pos);
                    int idx = intWordPosition - intFirstPosition; //ZERO based, for accounting[]

                    if (accounting[idx] == false)
                    {
                        //evaluate this word the first time
                        // pick the pos that mostly goes with the phrase probability one word forward and backward,
                        // and the pos that secondarily goes with the word probability
                        
                        if (intWordPosition < intLastPosition) //can we check the next word?
                        {   //check whether the next word has an initial valuation
                            if (accounting[idx + 1] == false)
                            {
                                //no new phrase info; use word probability only  
                            }
                            else 
                            {
                                //use phrase probability of (idx idx+1), then word

                            }
                        }
                        if (intWordPosition > intFirstPosition) //can we check the previous word?
                        {
                            //check whether the previous word has an initial valuation
                            if (accounting[idx - 1] == false)
                            {
                                //no new phrase info; use word probability only
                            }
                            else
                            {
                                //use phrase probability of (idx-1 idx), then word

                            }
                        }
                    }
                    else 
                    {
                        //evaluate this word a subsequent time
                    } 
                }
            }
        }

        public bool LoadParseParts(ref Input input)
        {
            string strParseParts = input.InsertStringIntoFilename("-ParseParts");
            bool bReturn = false;
            int intSentenceID = -1;
            StreamReader srParseParts;

            if (File.Exists(strParseParts))
            {
                srParseParts = new StreamReader(strParseParts);
                dParseParts.Clear();

                while (!srParseParts.EndOfStream)
                {
                    string strLine = srParseParts.ReadLine();
                    
                    if (Regex.IsMatch(strLine, @"Sentence [0-9]{1,}"))
                    {
                        intSentenceID = Convert.ToInt32(strLine.Split()[1].Trim('>'));
                    }
                    else if (strLine.Trim() != "")
                    {
                        int intIndex = Convert.ToInt32(strLine.Split('^')[0].Trim());
                        string strStructure = strLine.Split('^')[1].Trim();

                        if (!dParseParts.ContainsKey(intSentenceID))
                        {
                            dParseParts.Add(intSentenceID, new Dictionary<int, string>());
                        }

                        dParseParts[intSentenceID].Add(intIndex, strStructure);
                    }
                }

                srParseParts.Close();

                bReturn = true;
            }

            return bReturn;
        }

        public void CreateParseParts(ref Input input)
        {
            StreamWriter swParseParts = new StreamWriter(input.InsertStringIntoFilename("-ParseParts"));
            StringBuilder sbParseParts = new StringBuilder();
            Dictionary<int, int> dIndexTransformations = new Dictionary<int, int>(); //Transforms repeatable indexes to non-repeatable IDs
            int intNextGlobalIndex = 0;
            Dictionary<int, string> dParenthesisPhrases = new Dictionary<int, string>();
            Dictionary<int, List<int>> dPhraseKeys = new Dictionary<int, List<int>>();
            Rgxs r = new Rgxs();

            dParseParts.Clear();

            foreach (int intSentenceID in dParse.Keys.OrderBy(a => a))
            {
                Dictionary<int, Dictionary<int, int>> dParenthesisIndexes = new Dictionary<int, Dictionary<int, int>>(); //D<level #, D<open parenthesis endex, matching close index>>
                int intLevelCounter = 0;
                string strSentenceParse = dParse[intSentenceID].ToLower();
                bool bContinue = true;
                int intIndexCounter = -1;

                //Get Parenthesis Indexes
                foreach (char cParse in strSentenceParse)
                {
                    intIndexCounter++;

                    if (cParse == '(')
                    {
                        intLevelCounter++;

                        if (!dParenthesisIndexes.ContainsKey(intLevelCounter))
                        {
                            dParenthesisIndexes.Add(intLevelCounter, new Dictionary<int, int>());
                        }

                        dParenthesisIndexes[intLevelCounter].Add(intIndexCounter, -1);

                    }
                    else if (cParse == ')')
                    {
                        dParenthesisIndexes[intLevelCounter][dParenthesisIndexes[intLevelCounter].Max(a => a.Key)] = intIndexCounter;

                        intLevelCounter--;
                    }
                }

                //Fill dParenthesisPhrases
                for (int intPLevelCounter = dParenthesisIndexes.Count; intPLevelCounter > 0; intPLevelCounter--)
                {
                    foreach (int intIndex in dParenthesisIndexes[intPLevelCounter].Keys)
                    {
                        string strSubPhrase = strSentenceParse.Substring(intIndex + 1, (dParenthesisIndexes[intPLevelCounter][intIndex] - intIndex - 1));

                        if (r.rgxParenthesis.IsMatch(strSubPhrase))
                        {
                            foreach (Match mSubPhrase in r.rgxParenthesis.Matches(strSubPhrase))
                            {
                                strSubPhrase = strSubPhrase.Replace(mSubPhrase.Value, Convert.ToString(dParenthesisPhrases.Where(a => a.Value == mSubPhrase.Value.Trim('(').Trim(')')).First().Key));
                            }
                        }

                        if (!dParenthesisPhrases.ContainsValue(strSubPhrase))
                        {
                            dParenthesisPhrases.Add(intNextGlobalIndex + intIndex, strSubPhrase);
                        }
                    }
                }

                List<int> lintPhraseKeys = dParenthesisPhrases.Keys.Where(a => a >= intNextGlobalIndex).ToList<int>();
                dPhraseKeys.Add(intSentenceID, lintPhraseKeys);

                //Expand dParenthesisPhrases
                while (bContinue)
                {
                    //Dictionary<int, string> dParenthesisPhrasesWorking = new Dictionary<int, string>();

                    bContinue = false;

                    foreach (int intIndex in lintPhraseKeys)
                    {
                        string strSubPhrase = dParenthesisPhrases[intIndex];

                        if (r.rgxParenthesis.IsMatch(strSubPhrase))
                        {
                            foreach (Match mSubPhrase in r.rgxParenthesis.Matches(strSubPhrase))
                            {
                                strSubPhrase = strSubPhrase.Replace(mSubPhrase.Value, Convert.ToString(dParenthesisPhrases.Where(a => a.Value == mSubPhrase.Value.Trim('(').Trim(')')).First().Key));
                            }

                            if (!dParenthesisPhrases.ContainsKey(intIndex))
                            {
                                dParenthesisPhrases.Add(intIndex, strSubPhrase);
                            }
                            else
                            {
                                dParenthesisPhrases[intIndex] = strSubPhrase;
                            }
                        }
                    }

                    foreach (int intIndex in lintPhraseKeys)
                    {
                        string strCurrentSubPhrase = dParenthesisPhrases[intIndex];

                        if (r.rgxParenthesis.IsMatch(strCurrentSubPhrase))
                        {
                            bContinue = true;
                            break;

                        }
                    }
                }

                intNextGlobalIndex += strSentenceParse.Length;
            }

            //write dParenthesisPhrases
            foreach (int intSentenceID in dPhraseKeys.Keys.OrderBy(a => a))
            {
                bool bPOSWritten = false;
                bool bWordWritten = false;

                dParseParts.Add(intSentenceID, new Dictionary<int, string>());

                foreach (int intIndex in dPhraseKeys[intSentenceID])
                {
                    bPOSWritten = false;
                    bWordWritten = false;

                    sbParseParts.Append(intIndex.ToString() + " ^ " + intSentenceID.ToString());

                    foreach (string strParsePartsPartElement in dParenthesisPhrases[intIndex].Split())
                    {
                        if (strParsePartsPartElement.Trim() != "")
                        {
                            if (bPOSWritten == false)
                            {
                                sbParseParts.Append(" ^ " + strParsePartsPartElement.Trim());
                                bPOSWritten = true;
                            }
                            else if (bWordWritten == false) //not POS, so continue line analysis
                            {
                                if (!r.rgxNumbers.IsMatch(strParsePartsPartElement.Trim())) //must be a leaf node (eg. a word)
                                {
                                    sbParseParts.Append(" ^ " + strParsePartsPartElement.Trim());
                                }
                                else // must be the first number of the parse sequence
                                {
                                    sbParseParts.Append(" ^ " + " ^ " + strParsePartsPartElement); //skip the word column
                                }

                                bWordWritten = true;
                            }
                            else //only parse sequence numbers are left, so no more column skipping
                            {
                                sbParseParts.Append(" ^ " + strParsePartsPartElement.Trim());
                            }
                        }
                    }

                    if (!sbParseParts.ToString().EndsWith("\r\n"))
                    {
                        sbParseParts.AppendLine();
                    }

                    dParseParts[intSentenceID].Add(intIndex, dParenthesisPhrases[intIndex]);
                }

                if (!sbParseParts.ToString().EndsWith("\r\n"))
                {
                    sbParseParts.AppendLine();
                }
            }

            swParseParts.Write(sbParseParts.ToString());
            swParseParts.Close();
        }

        private int BranchLength(ref Dictionary<int, Dictionary<int, int>> dBranches, ref Dictionary<int,string> dLeaves, ref Dictionary<int, int> dCheckMeForLeaves, int intReturn = 0)
        {
            int intLargestLength = 0;

            foreach (int intCheckKey in dCheckMeForLeaves.Keys)
            {
                int intValue = dCheckMeForLeaves[intCheckKey];

                if (!dLeaves.ContainsKey(intValue)) //This is a branch (eg. structure)
                {
                    Dictionary<int, int> dNewCheckMeForLeaves;

                    intReturn++;

                    if (dBranches.ContainsKey(intValue))
                    {
                        dNewCheckMeForLeaves = dBranches[intValue];
                    }
                    else
                    {
                        dNewCheckMeForLeaves = new Dictionary<int, int>();
                    }

                    intReturn = BranchLength(ref dBranches, ref dLeaves, ref dNewCheckMeForLeaves, intReturn);

                    if (intReturn > intLargestLength)
                    {
                        intLargestLength = intReturn;
                    }
                }
            }

            return Math.Max(intReturn, intLargestLength);
        }

        public void CreateParsePartsTree()
        {
            Rgxs r = new Rgxs();

            dLeaves.Clear();
            dLeafLabels.Clear();
            dBranches.Clear();
            dBranchLabels.Clear();
            dBranchLevels.Clear();
            dBranchText.Clear();

            //dsParseParts.Clear();

            if (dParseParts.Count() > 0)
            {
                foreach (Dictionary<int,string> dParsePartPacket in dParseParts.Values)
                {
                    foreach(int intIndex in dParsePartPacket.Keys)
                    {
                        string[] strsPacket = dParsePartPacket[intIndex].Split();

                        if (dParsePartPacket[intIndex].StartsWith("cd ") || (!r.rgxNumbers.IsMatch(dParsePartPacket[intIndex]) && strsPacket.Length == 2))
                        {
                            //leaf node
                            dLeaves.Add(intIndex, strsPacket[1]);
                            dLeafLabels.Add(intIndex, strsPacket[0]);
                        }
                        else
                        {
                            //branch node
                            dBranches.Add(intIndex, new Dictionary<int, int>());
                            dBranchLabels.Add(intIndex, strsPacket[0]);

                            for (int intPacketIndex = 1; intPacketIndex < strsPacket.Length; intPacketIndex++)
                            {
                                try
                                {
                                    int intReferenceID = Convert.ToInt32(strsPacket[intPacketIndex]);

                                    dBranches[intIndex].Add(dBranches[intIndex].Count() + 1, intReferenceID);

                                    if (!dLeaves.ContainsKey(intReferenceID)) //don't count leaf nodes in the parsepart count
                                    {
                                        if (dParsePartCount.ContainsKey(intReferenceID))
                                        {
                                            dParsePartCount[intReferenceID]++;
                                        }
                                        else
                                        {
                                            dParsePartCount.Add(intReferenceID, 1);
                                        }
                                    }
                                }
                                catch { }
                            }
                        }
                    }
                }

                //calculate levels
                foreach (int intIndex in dBranches.Keys)
                {
                    Dictionary<int, int> dCheckTheseBranchesForLeafProximity = dBranches[intIndex];
                    int intLevelCounter = BranchLength(ref dBranches, ref dLeaves, ref dCheckTheseBranchesForLeafProximity) + 2;

                    dBranchLevels.Add(intIndex, intLevelCounter);
                    //THIS DOESN"T WORK YET dBranchText.Add(intIndex, FlattenBranch(intIndex));
                }

                //Create dataset rows
                foreach (int intLeafID in dLeaves.Keys)
                {
                   //dsParseParts.Leaves.AddLeavesRow(Guid.NewGuid(), intLeafID, dLeaves[intLeafID], dLeafLabels[intLeafID]);
                }

                foreach (int intBranchID in dBranches.Keys)
                {
                    foreach (int intSequenceID in dBranches[intBranchID].Keys)
                    {
                        //dsParseParts.Branches.AddBranchesRow(Guid.NewGuid(), intBranchID, dBranchLevels[intBranchID], intSequenceID, dBranches[intBranchID][intSequenceID], dBranchLabels[intBranchID]);
                    }
                }
            }
        }

        public void WriteParsePartsTree(string strParsePartsPartsTreeFilename, string strParsePartsPartsTreeTextFilename, string strParsePartsPartsCountFilename)
        {
            StreamWriter swParsePartsStructure = new StreamWriter(strParsePartsPartsTreeFilename);
            StreamWriter swParsePartsText = new StreamWriter(strParsePartsPartsTreeTextFilename);
            StreamWriter swParsePartsCount = new StreamWriter(strParsePartsPartsCountFilename);
            StringBuilder sbOutput = new StringBuilder();
            //foreach top branch
            // flatten each contained branch, inserting each leaf where appropriate

            foreach (int intParsePart in dParsePartCount.Keys.OrderByDescending(a=>dParsePartCount[a]))
            {
                swParsePartsCount.WriteLine(intParsePart.ToString() + " ^ " + dParsePartCount[intParsePart].ToString());
            }

            if (dLeaves.Count > 0 && dLeafLabels.Count > 0 && dBranches.Count > 0 && dBranchLabels.Count > 0 && dBranchLevels.Count > 0)
            {
                foreach(int intLeafID in dLeaves.Keys.OrderBy(a=>dLeaves[a]))
                {
                    swParsePartsStructure.WriteLine(intLeafID.ToString() + " ^ " + dLeaves[intLeafID] + " ^ " + dLeafLabels[intLeafID]);
                }

                foreach(int intBranchID in dBranches.Keys.Where(a => dBranchLabels[a].ToLower().Trim() == "top").OrderBy(a=>a))
                {
                    string strBranchIDsText = "";
                    string strBranchText = "";
                    FlattenBranch(intBranchID);

                    foreach (int intIndex in dBranches[intBranchID].Keys.OrderBy(a=>a))
                    {
                        strBranchIDsText += dBranches[intBranchID][intIndex].ToString() + " ";
                    }

                    swParsePartsStructure.WriteLine(intBranchID.ToString() + " ^ " + strBranchIDsText.Trim() + " ^ " + dBranchLabels[intBranchID] + " ^ " + dBranchLevels[intBranchID].ToString());
                    swParsePartsText.WriteLine(intBranchID.ToString() + " ^ " + strBranchText);
                }
            }

            swParsePartsStructure.Close();
            swParsePartsText.Close();
            swParsePartsCount.Close();
        }

        public void LoadParsePartsFlatTree(ref Input input)
        {
            StreamReader srPPTF = new StreamReader(input.InsertStringIntoFilename("-ParsePartsFlatTree"));
            dLeaves.Clear();
            dLeafLabels.Clear();
            dBranches.Clear();
            dBranchLabels.Clear();
            dBranchLevels.Clear();

            while (!srPPTF.EndOfStream)
            {
                string[] strsPPFT = srPPTF.ReadLine().Split('^');

                if (strsPPFT.Length == 3) //Leaves
                {
                    //intLeafID, strLeaf, strLeafLabel
                    dLeaves.Add(Convert.ToInt32(strsPPFT[0]), strsPPFT[1]);
                    dLeafLabels.Add(Convert.ToInt32(strsPPFT[0]), strsPPFT[2]);
                }
                else //Branches
                {
                    //intBranchID, strBranchText, strBranchLabel, intBranchLevel
                    int intBranchID = Convert.ToInt32(strsPPFT[0]);

                    if (!dBranches.ContainsKey(intBranchID))
                    {
                        dBranches.Add(intBranchID, new Dictionary<int, int>());
                    }

                    foreach (string strBranchItem in strsPPFT[1].Trim().Split())
                    {
                        dBranches[intBranchID].Add(dBranches[intBranchID].Count() + 1, Convert.ToInt32(strBranchItem.Trim()));
                    }

                    dBranchLabels.Add(intBranchID, strsPPFT[2]);
                    dBranchLevels.Add(intBranchID, Convert.ToInt32(strsPPFT[3]));
                }
            }
        }

        public string PPFTContains(string strPhrase)
        {
            Dictionary<int, int> dParsePartsIDs = new Dictionary<int, int>();
            string strReturn = "";
            var varReturn =
                from branch in dBranches
                where branch.Value == dParsePartsIDs
                select branch.Key.ToString();

            foreach (string strWord in strPhrase.Trim().Split())
            {
                try
                {
                    dParsePartsIDs.Add(dParsePartsIDs.Count + 1, dLeaves.Where(a => a.Value.Trim() == strWord).First().Key);
                }
                catch { }
            }

            if (varReturn.Count() > 0)
            {
                strReturn = varReturn.First();
            }

            return strReturn;
        }

        public Dictionary<int, Parser.Word> FlattenBranch(int intParsePartID)
        {
            Dictionary<int, Parser.Word> dReturn = new Dictionary<int, Parser.Word>();
            bool bDone = false;

            try {
                foreach (int intBranchSequenceID in dBranches[intParsePartID].Keys.OrderBy(a => a))
                {
                    int intSequencedBranchPart = dBranches[intParsePartID][intBranchSequenceID];
                    Parser.Word wReturn = new Parser.Word();

                    if (dLeaves.ContainsKey(intSequencedBranchPart))
                    {
                        bDone = true;
                        //switch on word tag to create Word-derived object

                        wReturn.strPOS = dLeafLabels[intSequencedBranchPart];
                        wReturn.strWord = dLeaves[intSequencedBranchPart];

                        //default settings; change freely with more information
                        if (wReturn.strPOS.Contains("nn"))
                        {
                            wReturn.semType = Parser.Word.SemanticType.Object;
                            wReturn.semThing = Parser.Word.SemanticThing.Object;
                            //wReturn = (Parser.NN)wReturn;
                        }
                        else if (wReturn.strPOS.Contains("jj"))
                        {
                            wReturn.semType = Parser.Word.SemanticType.Modifier;
                            wReturn.semModifier = Parser.Word.SemanticModifier.OtherModifier;
                            //wReturn = (Parser.JJ)wReturn;
                        }
                        else if (wReturn.strPOS.Contains("vb"))
                        {
                            wReturn.semType = Parser.Word.SemanticType.Verb;
                            wReturn.semAction = Parser.Word.SemanticAction.Action;
                            //wReturn = (Parser.VB)wReturn;
                        }
                        else if (wReturn.strPOS.Contains("dt"))
                        {
                            wReturn.semType = Parser.Word.SemanticType.Determiner;
                            wReturn.semModifier = Parser.Word.SemanticModifier.Determiner;
                            //wReturn = (Parser.DT)wReturn;
                        }
                        else if (wReturn.strPOS.Contains("cc"))
                        {
                            wReturn.semType = Parser.Word.SemanticType.Connector;
                            wReturn.semModifier = Parser.Word.SemanticModifier.Conjunction;
                            //wReturn = (Parser.CC)wReturn;
                        }
                        else if (wReturn.strPOS.Contains("pp"))
                        {
                            wReturn.semType = Parser.Word.SemanticType.Preposition;
                            wReturn.semModifier = Parser.Word.SemanticModifier.Preposition;
                            //wReturn = (Parser.PP)wReturn;
                        }

                        //is nn a subject?
                        //is jj a specific type of modifier? (time, place, personal)
                        //NEXT: synonyms, including both regular and phrasal
                        dReturn.Add(dReturn.Count() + 1, wReturn);
                    }
                    else //branch
                    {
                        //switch on parse type to create ?Phrase?-derived object
                        //strReturn += " " + FlattenBranch(intBranchPart);
                        wReturn.strWord = "<branch>";
                        try
                        {
                            wReturn.strPOS = dBranchLabels[intSequencedBranchPart];
                        }
                        catch
                        {
                            wReturn = null;
                        }
                        //if (wReturn.strPOS.Contains("top"))
                        //{

                        //}

                        if (wReturn != null)
                        {
                            dReturn.Add(dReturn.Count() + 1, wReturn);
                            dReturn.Add(dReturn.Count() + 1, FlattenBranch(intSequencedBranchPart).First().Value);
                        }
                    }


                }

                return dReturn;
            }
            catch { return null; }
        }

        public Dictionary<string,string> CompareParsePartsFlatTreeAndPhrasalComposition(ref Input input, ref Phrases phrases)
        {
            //parse - sane parts
            //phrase - not sane parts
            Dictionary<string, string> dParsablePhrases = new Dictionary<string, string>();

            LoadParsePartsFlatTree(ref input);
            
            //foreach phrase that is also a complete parse block, mark the phrase as complete
            foreach (string strPhrase in phrases.PhraseIDs.Keys)
            {
                string strPhraseTag = PPFTContains(strPhrase);

                if (strPhraseTag != "")
                {
                    dParsablePhrases.Add(strPhrase, strPhraseTag);
                }
            }

            return dParsablePhrases;
        }
    }
}
