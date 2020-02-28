using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LibNLPDB
{
    public class Words
    {
        private Dictionary<int, string> dPositionWords = new Dictionary<int, string>();
        private Dictionary<string, int> dWordIDs = new Dictionary<string, int>();
        private Dictionary<int, List<string>> dWordSynonyms = new Dictionary<int, List<string>> (); //D<synonym list id, synonym list>  example: D<1, L<"it", "this", "that">>
		private Dictionary<string, List<int>> dWordSynonymsIndex = new Dictionary<string, List<int>> (); //D<word, L<synonym list ids>> eg. D<each word in each list, L<each synonym list id of each list this word appears in>>
        private Dictionary<string, int> dCounts = new Dictionary<string, int>();

        private Sentences sentences = null;

        public Dictionary<int, string> PositionWords { get { return dPositionWords; } set { dPositionWords = value; } }
        public Dictionary<string, int> WordIDs { get { return dWordIDs; } set { dWordIDs = value; } }
        public Dictionary<string, int> Counts { get { return dCounts; } set { dCounts = value; } }
        public Sentences SentencesObject { get { return sentences; } set { sentences = value; } }
        public SortedList<int, string> SentenceList { get { return sentences.SentenceList; } set { sentences.SentenceList = value; } }
        public SortedList<int, int> SentenceLengths { get { return sentences.SentenceLengthList; } set { sentences.SentenceLengthList = value; } }
        public SortedList<int, int> SentenceFirstPositions { get { return sentences.SentenceFirstPositionList; } set { sentences.SentenceFirstPositionList = value; } }

        //public List<char> lchrRemove = new List<char>();
            
        public Words() { }

        public bool LoadWordPositions(ref Input input, ref Words words, ref POS pos)
        {
            string strWordPositions = input.InsertStringIntoFilename("-WordPositions");
            //string strSentenceBreaks = input.InsertStringIntoFilename("-SentenceBreaks");
            string strSentences = input.InsertStringIntoFilename("-Sentences");
            string strSentenceLengths = input.InsertStringIntoFilename("-SentenceLengths");
            string strSentenceFirstPositions = input.InsertStringIntoFilename("-SentenceFirstPositions");
            string strWordPositionsSentenceIDs = input.InsertStringIntoFilename("-WordPositionsSentenceIDs");
            bool bReturn = false;

            if (File.Exists(strWordPositions))
            {
                StreamReader srWordPositions = new StreamReader(strWordPositions);
                StreamReader srSentences = new StreamReader(strSentences);
                StreamReader srSentenceLengths = new StreamReader(strSentenceLengths);
                StreamReader srSentenceFirstPositions = new StreamReader(strSentenceFirstPositions);
                StreamReader srWordPositionsSentenceIDs;
                bool bWordPositionsSentenceIDsExists = false;

                try
                {
                    srWordPositionsSentenceIDs = new StreamReader(strWordPositionsSentenceIDs);
                    bWordPositionsSentenceIDsExists = true;
                }
                catch
                {
                    srWordPositionsSentenceIDs = null;
                    bWordPositionsSentenceIDsExists = false;
                }

                sentences = new Sentences(ref input, ref words, ref pos);

                dPositionWords.Clear();
                
                while (!srWordPositions.EndOfStream)
                {
                    string strLine = srWordPositions.ReadLine();

                    dPositionWords.Add(Convert.ToInt32(strLine.Split('^')[1].Trim()),
                        strLine.Split('^')[0].Trim());
                }

                while (!srSentences.EndOfStream)
                {
                    string strLine = srSentences.ReadLine();

                    sentences.SentenceList.Add(Convert.ToInt32(strLine.Split('^')[0].Trim()),
                        strLine.Split('^')[1].Trim());
                }

                while (!srSentenceLengths.EndOfStream)
                {
                    string strLine = srSentenceLengths.ReadLine();

                    sentences.SentenceLengthList.Add(Convert.ToInt32(strLine.Split('^')[0].Trim()),
                        Convert.ToInt32(strLine.Split('^')[1].Trim()));
                }

                while (!srSentenceFirstPositions.EndOfStream)
                {
                    string strLine = srSentenceFirstPositions.ReadLine();

                    sentences.SentenceFirstPositionList.Add(Convert.ToInt32(strLine.Split('^')[0].Trim()),
                        Convert.ToInt32(strLine.Split('^')[1].Trim()));
                }

                if (bWordPositionsSentenceIDsExists)
                {
                    while (!srWordPositionsSentenceIDs.EndOfStream)
                    {
                        string strLine = srWordPositionsSentenceIDs.ReadLine();

                        sentences.WordPositionsSentenceIDs.Add(Convert.ToInt32(strLine.Split('^')[0].Trim()),
                            Convert.ToInt32(strLine.Split('^')[1].Trim()));
                    }
                }

                srWordPositions.Close();
                srSentences.Close();
                srSentenceLengths.Close();
                srSentenceFirstPositions.Close();

                if (bWordPositionsSentenceIDsExists)
                {
                    srWordPositionsSentenceIDs.Close();
                }
                else
                {
                    srWordPositionsSentenceIDs = null;
                }

                bReturn = true;
            }

            if (sentences != null)
            {
                sentences.SplitSentences(input.InputText, ref words, ref pos, false, false); //to make Sentences.dStructures, which should be separated from Sentences.SplitSentences
            }

            return bReturn;
        }

        public bool CreateWordPositions(ref Input input, ref Words words, ref POS pos)
        {
            bool bReturn = false;

            string strWordPositions = input.InsertStringIntoFilename("-WordPositions");
            string strSentences = input.InsertStringIntoFilename("-Sentences");
            string strSentenceLengths = input.InsertStringIntoFilename("-SentenceLengths");
            string strWordPositionsSentenceIDs = input.InsertStringIntoFilename("-WordPositionsSentenceIDs");
            string strSentenceFirstPositions = input.InsertStringIntoFilename("-SentenceFirstPositions");
            StreamWriter swWordPositions = new StreamWriter(strWordPositions);
            StreamWriter swSentences = new StreamWriter(strSentences);
            StreamWriter swSentenceLengths = new StreamWriter(strSentenceLengths);
            StreamWriter swWordPositionsSentenceIDs = new StreamWriter(strWordPositionsSentenceIDs);
            StreamWriter swSentenceFirstPositions = new StreamWriter(strSentenceFirstPositions);
            StringBuilder sbWordPositions = new StringBuilder();
            StringBuilder sbSentences = new StringBuilder();
            StringBuilder sbSentenceLengths = new StringBuilder();
            StringBuilder sbWordPositionsSentenceIDs = new StringBuilder();
            StringBuilder sbSentenceFirstPositions = new StringBuilder();
            StreamReader srInputFile = new StreamReader(input.Filename);
            int intWordNumber = 0;

            sentences = new Sentences(ref input, ref words, ref pos);

            //foreach (char cRemove in @".?!;-,\)(][}{><@|#$%^&*_=+'" + '"' + '\t') //note: no : here
            //{
            //    lchrRemove.Add(cRemove);
            //}
            
            dPositionWords.Clear();

            //Sentences
            sentences.SplitSentences(srInputFile.ReadToEnd(), ref words, ref pos, false, false);
            
            //Map WordPositions
            foreach (int intSentenceID in sentences.SentenceList.Keys.OrderBy(a => a))
            {
                sbSentences.AppendLine(intSentenceID.ToString() + " ^ " + sentences.SentenceList[intSentenceID]);
                sbSentenceLengths.AppendLine(intSentenceID.ToString() + " ^ " + sentences.SentenceLengthList[intSentenceID]);
                sbSentenceFirstPositions.AppendLine(intSentenceID.ToString() + " ^ " + sentences.SentenceFirstPositionList[intSentenceID]);

                foreach (string strWord in sentences.SentenceList[intSentenceID].Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                {
                    intWordNumber++;
                    dPositionWords.Add(intWordNumber, strWord.ToLower());//.TrimEnd(":".ToCharArray()[0]).Trim());//Trim ending : punctuation (keeping : in verse references)
                    sbWordPositions.AppendLine(dPositionWords[intWordNumber] + " ^ " + intWordNumber.ToString());
                    sbWordPositionsSentenceIDs.AppendLine(intWordNumber.ToString() + " ^ " + intSentenceID.ToString());

                    try
                    {
                        //Add Word info to Sentences.dStructures
                        sentences.dStructures[intSentenceID][intWordNumber].Add(dPositionWords[intWordNumber], "");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }

            srInputFile.Close();

            //Write WordPositions
            swWordPositions.Write(sbWordPositions.ToString());
            swWordPositions.Close();

            //Write Sentences
            swSentences.Write(sbSentences.ToString());
            swSentences.Close();

            //Write SentenceLengths
            swSentenceLengths.Write(sbSentenceLengths.ToString());
            swSentenceLengths.Close();

            //Write SentenceFirstPositions
            swSentenceFirstPositions.Write(sbSentenceFirstPositions.ToString());
            swSentenceFirstPositions.Close();

            //Write WordPositionsSentenceIDs
            swWordPositionsSentenceIDs.Write(sbWordPositionsSentenceIDs.ToString());
            swWordPositionsSentenceIDs.Close();

            bReturn = true;

            return bReturn;
        }

        public bool LoadWordIDs(ref Input input)
        {
            string strWordIDs = input.InsertStringIntoFilename("-WordIDs");
            bool bReturn = false;

            if (File.Exists(strWordIDs))
            {
                StreamReader srWordIDs = new StreamReader(strWordIDs);

                dWordIDs.Clear();
                
                while (!srWordIDs.EndOfStream)
                {
                    string strLine = srWordIDs.ReadLine();

                    dWordIDs.Add(strLine.Split('^')[0].Trim(),
                        Convert.ToInt32(strLine.Split('^')[1].Trim()));
                }

                srWordIDs.Close();
                
                bReturn = true;
            }

            return bReturn;
        }

        public void CreateWordIDs(ref Input input)
        {
            string strWordIDs = input.InsertStringIntoFilename("-WordIDs");

            dWordIDs.Clear();

            StreamWriter swWordIDs = new StreamWriter(strWordIDs);
            StringBuilder sbWordIDs = new StringBuilder();
            
            //Map WordIDs
            foreach (int intWordPosition in dPositionWords.Keys)
            {
                if (!dWordIDs.Keys.Contains(dPositionWords[intWordPosition]))
                {
                    dWordIDs.Add(dPositionWords[intWordPosition], dWordIDs.Count() + 1);
                }
            }

            //Write WordIDs
            foreach (string strWord in dWordIDs.Keys.OrderBy(a => a))
            {
                sbWordIDs.AppendLine(strWord +
                    " ^ " + (dWordIDs[strWord]).ToString());
            }

            swWordIDs.Write(sbWordIDs.ToString());
            swWordIDs.Close();

            //lblWordIDs.Text = "Word IDs: " + dWordIDs.Count().ToString();
        }

		public void CreateSynonymSets(ref Input input)
		{
			string strSynonymsFilename = input.InsertStringIntoFilename ("-WordSynonyms");

			StreamWriter swSynonyms = new StreamWriter (strSynonymsFilename);

			dWordSynonyms.Clear ();

			//map synonyms
			//each word
			  //word forms
			  //adjectives are synonymous with some prepositional phrases
			  
		}

        public bool LoadCounts(ref Input input)
        {
            string strCounts = input.InsertStringIntoFilename("-Counts");
            bool bReturn = false;
            
            if (File.Exists(strCounts))
            {
                StreamReader srWordIDs = new StreamReader(strCounts);

                dCounts.Clear();

                while (!srWordIDs.EndOfStream)
                {
                    string strLine = srWordIDs.ReadLine();

                    dCounts.Add(strLine.Split('^')[0].Trim(), Convert.ToInt32(strLine.Split('^')[1].Trim()));
                }

                srWordIDs.Close();

                bReturn = true;
            }

            return bReturn;
        }

        public void CreateCounts(ref Input input)
        {
            string strCounts = input.InsertStringIntoFilename("-Counts");

            StreamWriter swCounts = new StreamWriter(strCounts);
            StringBuilder sbCounts = new StringBuilder();

            //Map Counts
            foreach (int intWordPosition in dPositionWords.Keys)
            {
                if (!dCounts.Keys.Contains(dPositionWords[intWordPosition]))
                {
                    dCounts.Add(dPositionWords[intWordPosition], 1);
                }
                else
                {
                    dCounts[dPositionWords[intWordPosition]]++;
                }
            }

            //Write Counts
            foreach (string strWord in dCounts.Keys.OrderByDescending(a => dCounts[a]))
            {
                sbCounts.AppendLine(strWord +
                    " ^ " + (dCounts[strWord]).ToString());
            }

            swCounts.Write(sbCounts.ToString());
            swCounts.Close();
        }

		public static void CombineCounts(string strDirectoryName)
		{
			Dictionary<string, int> dCombinedCounts = new Dictionary<string, int> ();
			StreamWriter swCounts = new StreamWriter (strDirectoryName + "/NLPData/Counts.txt");

			foreach (string strDirectory in Directory.EnumerateDirectories(strDirectoryName + "/NLPData")) {
				string strFileName = Directory.EnumerateFiles (strDirectory).Where (a => a.Contains ("-Counts")).First ();
				StreamReader srCounts = new StreamReader (strFileName);

				while (!srCounts.EndOfStream) {
					string strCountsLine = srCounts.ReadLine ();
					string strWord = strCountsLine.Split ('^')[0];
					int intCount = Convert.ToInt32 (strCountsLine.Split ('^') [1]);

					if (!dCombinedCounts.ContainsKey (strWord)) {
						dCombinedCounts.Add (strWord, 0);
					}

					dCombinedCounts [strWord] += intCount;
				}

				srCounts.Close ();
			}

			foreach (string strWord in dCombinedCounts.OrderByDescending(a=>a.Value).Select(a=>a.Key)) {
				swCounts.WriteLine (strWord + " ^ " + dCombinedCounts [strWord].ToString ());
			}

			swCounts.Close ();
		}

        //Position Helper Functions
        public string GetPositionPOS(int intPosition, ref POS pos)
        {
            try {
                return pos.POSs[intPosition];
            }
            catch
            {
                return "NA";
            }
        }

        public string GetPositionWord(int intPosition)
        {
            if (intPosition <= dPositionWords.Max(a => a.Key))
            {
                return dPositionWords[intPosition];
            }
            else
            {
                return "";
            }
        }

        public int GetPositionWordID(int intPosition)
        {
            if (intPosition < dWordIDs.Count)
            {
                return dWordIDs[dPositionWords[intPosition]];
            }
            else { return 0; }
        }

        public int GetPositionSentenceID(int intPosition)
        {
            var getPositionSentenceID =
                            from fpos in SentenceFirstPositions
                            where fpos.Value <= intPosition
                            orderby fpos.Key descending
                            select fpos.Key;
            return getPositionSentenceID.First();
        }

        //Word Helper Functions
        public List<int> GetWordPositions(string strWord)
        {
            List<int> lintReturn = new List<int>();

            foreach (int intPosition in dPositionWords.Keys.Where(a => dPositionWords[a] == strWord).OrderBy(a => a))
            {
                lintReturn.Add(intPosition);
            }

            return lintReturn;
        }

        public List<string> GetWordPOSs(string strWord, ref CombinedPOS combinedPOS)
        {
            List<string> lstrReturn = new List<string>();

            try
            {
                foreach (string strTag in combinedPOS.CombinedPOSs[strWord].Split())
                {
                    lstrReturn.Add(strTag);
                }
            }
            catch { }

            return lstrReturn.ToList();
        }

        public int GetWordWordID(string strWord)
        {
            return dWordIDs[strWord];
        }

        //WordID Helper Functions
        public List<int> GetWordIDPositions(int intID)
        {
            List<int> lintReturn = new List<int>();
            string strWord = GetWordIDWord(intID);

            foreach (int intPosition in dPositionWords.Where(a => a.Value == strWord).Select(a => a.Key))
            {
                lintReturn.Add(intPosition);
            }

            return lintReturn;
        }

        public List<string> GetWordIDPOSs(int intID, ref POS pos)
        {
            List<string> lstrReturn = new List<string>();
            string strWord = dWordIDs.Where(a => a.Value == intID).First().Key;

            foreach (int intPosition in dPositionWords.Where(a => a.Value == strWord).Select(a => a.Key).OrderBy(a=>a))
            {
                try {
                    string strPOS = pos.POSs[intPosition];

                    if (!lstrReturn.Contains(strPOS))
                    {
                        lstrReturn.Add(strPOS);
                    }
                }
                catch { }
            }

            return lstrReturn;
        }

        public string GetWordIDWord(int intID)
        {
            return dWordIDs.Where(a => a.Value == intID).First().Key;
        }

        //Sentence Helper Function
        public string[] GetSentenceWords(int intSentenceID)
        {
            int intFWID = SentenceFirstPositions[intSentenceID];
            int intSentenceLength = SentenceLengths[intSentenceID];
            string[] strsReturn = (string[]) Array.CreateInstance(typeof(string), intSentenceLength);
            
            for (int intFWIDCounter = intFWID; intFWIDCounter <
                intFWID + intSentenceLength; intFWIDCounter++)
            {
                strsReturn[intFWIDCounter - intFWID] = GetPositionWord(intFWIDCounter);
            }

            return strsReturn;
        }

		public List<int> GetWordSentenceIDs(string strWord){
			List<int> lReturn = new List<int> ();

			foreach (int intWordPosition in GetWordPositions(strWord)) {
				lReturn.Add (GetPositionSentenceID (intWordPosition));
			}

			return lReturn;
		}

		public List<StringBuilder> GetSurroundingText(string strWord, int intSentences){
			List<StringBuilder> lsbReturn = new List<StringBuilder> ();

			foreach (int intSentenceIDMain in GetWordSentenceIDs(strWord)) {
				int intHighestSentenceID = SentenceList.Count();
				int intSentenceIDFirst = intSentenceIDMain - intSentences;
				int intSentenceIDLast = intSentenceIDMain + intSentences;
				StringBuilder sbGroup = new StringBuilder ();

				if (intSentenceIDFirst < 1) {
					intSentenceIDFirst = 1;
				}

				if (intSentenceIDLast > intHighestSentenceID) {
					intSentenceIDLast = intHighestSentenceID;
				}

				for (int intSentenceID = intSentenceIDFirst; intSentenceID <= intSentenceIDLast; intSentenceID++) {
//					int intFirstPosition = SentenceFirstPositions [intSentenceID];
//					int intLastPosition = intFirstPosition + SentenceLengths[intSentenceID] - 1;
					//Console.WriteLine (intSentenceID.ToString () + ":");
					sbGroup.AppendLine (sentences.SentenceList [intSentenceID]);
//					for (int ctr = intFirstPosition; ctr <= intLastPosition; ctr++) {
//						sbGroup.Append (GetPositionWord (ctr));
//						sbGroup.Append (" ");
//					}
//					sbGroup.AppendLine ();
				}
				//Console.WriteLine (lsbReturn.Count ().ToString ());
				lsbReturn.Add (sbGroup);
			}

			return lsbReturn;
		}
    }
}
