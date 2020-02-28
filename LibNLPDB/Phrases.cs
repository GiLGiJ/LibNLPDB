using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace LibNLPDB
{
    public class Phrases
    {
        string strPhrasesDirectory;
		//the information itself should control the structure of the program which manipulates the structure of definition for the information;ie. map this sentence into functions
		private Dictionary<string, int> dPhraseIDs = new Dictionary<string, int>();  //Phrase "Name" (eg.the text) useful as (or, to be used as) a Reference; each phrase's phrase id
		private Dictionary<string, int[]> dPhraseWordIDs = new Dictionary<string, int[]>();  //Parts-Wise Definition; each word's word id
		private Dictionary<string, int> dPhraseCounts = new Dictionary<string, int>();  //Count; the count of each phrase
		private Dictionary<string, List<int>> dPhraseFirstWordPositions = new Dictionary<string, List<int>>(); //each place in the overall text where "first word" was found
		private Dictionary<string, List<string>> dWordMetadata = new Dictionary<string, List<string>>(); //D<word, L<metadata>>
		Dictionary<string, List<string>> dPhraseParts = new Dictionary<string, List<string>> ();
		//private Dictionary<string, Dictionary<string, int>> dCommonLengthPhraseDoubles = new Dictionary<string, Dictionary<string, int>>();
		//private Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, string>>>> dPhraseStructure =
        //    new Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, string>>>>(); //D<SWID, D<FWID, D<PhraseLength, D<PhraseID, PhraseText>>>

		private string PhraseToStructureConverter(string strInput){
			//verbs use functional form of declaration and nouns use the variable form in c# code
			//if input is in free text form, output structure (eg. c# code (?and data files?))
			//..and vice-versa

			return null;
		}

		private List<string> GetWordMetadata(string strWord){
			return dWordMetadata [strWord];
		}

        public Dictionary<string, int> PhraseCounts
        {
            get
            {
                return dPhraseCounts;
            }
            set
            {
                dPhraseCounts = value;
            }
        }
        //public Dictionary<string, Dictionary<string, int>> Doubles
        //{
        //    get
        //    {
        //        return dCommonLengthPhraseDoubles;
        //    }
        //    set
        //    {
        //        dCommonLengthPhraseDoubles = value;
        //    }
        //}
        public Dictionary<string, List<int>> FWIDs
        {
            get
            {
                return dPhraseFirstWordPositions;
            }
            set
            {
                dPhraseFirstWordPositions = value;
            }
        }
        public Dictionary<string, int> PhraseIDs
        {
            get
            {
                return dPhraseIDs; 
            }
            set
            {
                dPhraseIDs = value; 
            }
        }
        public Dictionary<string, int[]> PhraseWordsIDs 
        {
            get
            { 
                return dPhraseWordIDs; 
            } 
            set 
            { 
                dPhraseWordIDs = value; 
            }
        }

        public Phrases() { }

        public bool LoadPhraseCounts(ref Input input)
        {
            bool bReturn = false;

            strPhrasesDirectory = input.DataPath + @"/" + input.Base + @"/Phrases";

            if (Directory.Exists(strPhrasesDirectory))
            {
                //Load Phrases
                foreach (string strFilenameTemp in Directory.EnumerateFiles(strPhrasesDirectory))
                {
                    if (strFilenameTemp.EndsWith("Counts.txt"))
                    {
                        LoadPhraseCountsFile(strFilenameTemp);
                    }
                }

                bReturn = true;
            }

            return bReturn;
        }

        public void LoadPhraseCountsFile(string strFilenameTemp)
        {
            try
            {
                StreamReader srPhrases = new StreamReader(strFilenameTemp);

                while (!srPhrases.EndOfStream)
                {
                    string strLine = srPhrases.ReadLine();
                    string strKey = strLine.Split('^')[0].Trim();

                    if (!dPhraseCounts.ContainsKey(strKey))
                    {
                        dPhraseCounts.Add(strKey, Convert.ToInt32(strLine.Split('^')[1].Trim()));
                    }
                }

                srPhrases.Close();
            }
            catch { }
        }

        public void LoadPhraseCountsFile(string strFilenameTemp, bool bClearDictionary)
        {
            if (bClearDictionary)
            {
                dPhraseCounts.Clear();
            }

            LoadPhraseCountsFile(strFilenameTemp);
        }

        //public bool LoadPhraseDoubles(ref Input input)
        //{
        //    bool bReturn = false;

        //    strPhrasesDirectory = input.DataPath + "/" + input.Base + "/Phrases";

        //    if (Directory.Exists(strPhrasesDirectory))
        //    {
        //        dCommonLengthPhraseDoubles.Clear();

        //        //Load PhraseDoubles
        //        foreach (string strFilenameTemp in Directory.EnumerateFiles(strPhrasesDirectory))
        //        {
        //            if (strFilenameTemp.Contains("PhraseDoubles.txt"))
        //            {
        //                StreamReader srPhraseDoubles = new StreamReader(strFilenameTemp);

        //                while (!srPhraseDoubles.EndOfStream)
        //                {
        //                    string strLine = srPhraseDoubles.ReadLine();
        //                    string strPhrase = strLine.Split('^')[0].Trim();
        //                    string strPhrase2 = strLine.Split('^')[1].Trim();

        //                    if (!dCommonLengthPhraseDoubles.ContainsKey(strPhrase))
        //                    {
        //                        dCommonLengthPhraseDoubles.Add(strPhrase, new Dictionary<string, int>());
        //                    }

        //                    if (!dCommonLengthPhraseDoubles[strPhrase].ContainsKey(strPhrase2))
        //                    {
        //                        dCommonLengthPhraseDoubles[strPhrase].Add(strPhrase2, Convert.ToInt32(strLine.Split('^')[2].Trim()));
        //                    }
        //                }

        //                srPhraseDoubles.Close();
        //            }
        //        }

        //        bReturn = true;
        //    }

        //    return bReturn;
        //}

        public bool LoadFirstWordIDs(ref Input input)
        {
            bool bReturn = false;

            strPhrasesDirectory = input.DataPath + "/" + input.Base + "/Phrases";

            if (Directory.Exists(strPhrasesDirectory))
            {
                //Load FirstWordIDs
                foreach (string strFilenameTemp in Directory.EnumerateFiles(strPhrasesDirectory))
                {
                    if (strFilenameTemp.Contains("FirstWordIDs.txt"))
                    {
                        try
                        {
                            StreamReader srFirstWordIDs = new StreamReader(strFilenameTemp);

                            while (!srFirstWordIDs.EndOfStream)
                            {
                                string strLine = srFirstWordIDs.ReadLine();
                                string strPhrase = strLine.Split('^')[0].Trim();

                                if (!dPhraseFirstWordPositions.Keys.Contains(strPhrase))
                                {
                                    dPhraseFirstWordPositions.Add(strPhrase, new List<int>());

                                    foreach (string strFirstWordID in strLine.Split('^')[1].Trim().Split())
                                    {
                                        dPhraseFirstWordPositions[strPhrase].Add(Convert.ToInt32(strFirstWordID));
                                    }
                                }
                            }

                            srFirstWordIDs.Close();
                        }
                        catch { }
                    }
                }

                bReturn = true;
            }

            return bReturn;
        }

        public bool LoadFirstWordIDs(ref Input input, int intPhraseLength)
        {
            bool bReturn = false;

            strPhrasesDirectory = input.DataPath + "/" + input.Base + "/Phrases";

            if (Directory.Exists(strPhrasesDirectory))
            {
                //Load FirstWordIDs
                foreach (string strFilenameTemp in Directory.EnumerateFiles(strPhrasesDirectory))
                {
                    if (strFilenameTemp.Contains(intPhraseLength.ToString() + "-FirstWordIDs.txt"))
                    {
                        StreamReader srFirstWordIDs = new StreamReader(strFilenameTemp);

                        while (!srFirstWordIDs.EndOfStream)
                        {
                            string strLine = srFirstWordIDs.ReadLine();
                            string strPhrase = strLine.Split('^')[0].Trim();

                            if (!dPhraseFirstWordPositions.Keys.Contains(strPhrase))
                            {
                                dPhraseFirstWordPositions.Add(strPhrase, new List<int>());

                                foreach (string strFirstWordID in strLine.Split('^')[1].Trim().Split())
                                {
                                    dPhraseFirstWordPositions[strPhrase].Add(Convert.ToInt32(strFirstWordID));
                                }
                            }
                        }

                        srFirstWordIDs.Close();
                    }
                }

                bReturn = true;
            }

            return bReturn;
        }

        public bool LoadPhraseIDs(ref Input input, int intPhraseLength) 
        {
            bool bReturn = false;

            strPhrasesDirectory = input.DataPath + "/" + input.Base + "/Phrases";

            if (Directory.Exists(strPhrasesDirectory))
            {
                //Load PhraseIDs
                foreach (string strFilenameTemp in Directory.EnumerateFiles(strPhrasesDirectory))
                {
                    if (strFilenameTemp.Contains(intPhraseLength.ToString() + "-PhraseIDs.txt"))
                    {
                        try
                        {
                            StreamReader srPhraseIDs = new StreamReader(strFilenameTemp);

                            while (!srPhraseIDs.EndOfStream)
                            {
                                string strLine = srPhraseIDs.ReadLine();
                                string strPhrase = strLine.Split('^')[0].Trim();
                                int intPhraseID = Convert.ToInt32(strLine.Split('^')[1].Trim());

                                if (!dPhraseIDs.Keys.Contains(strPhrase))
                                {
                                    dPhraseIDs.Add(strPhrase, intPhraseID);
                                }
                            }

                            srPhraseIDs.Close();
                        }
                        catch { }
                    }
                }

                bReturn = true;
            }

            return bReturn;
        }

        public bool LoadPhraseWordIDs(ref Input input, int intPhraseLength)
        {
            bool bReturn = false;

            strPhrasesDirectory = input.DataPath + "/" + input.Base + "/Phrases";

            if (Directory.Exists(strPhrasesDirectory))
            {
                //Load PhraseWordIDs
                foreach (string strFilenameTemp in Directory.EnumerateFiles(strPhrasesDirectory))
                {
                    if (strFilenameTemp.Contains(intPhraseLength.ToString() + "-PhraseWordIDs.txt"))
                    {
                        try
                        {
                            StreamReader srPhraseWordIDs = new StreamReader(strFilenameTemp);

                            while (!srPhraseWordIDs.EndOfStream)
                            {
                                string strLine = srPhraseWordIDs.ReadLine();
                                string strPhrase = strLine.Split('^')[0].Trim();
                                int[] intsPhraseWordIDs = (int[])Array.CreateInstance(typeof(int), strPhrase.Split().Count());
                                string strPhraseWordIDs = strLine.Split('^')[1].Trim();
                                int intPhraseWordIDsIndex = 0;

                                foreach (string strPhraseWordID in strPhraseWordIDs.Split())
                                {
                                    intsPhraseWordIDs[intPhraseWordIDsIndex] = Convert.ToInt32(strPhraseWordID.Trim());
                                    intPhraseWordIDsIndex++;
                                }

                                if (!dPhraseWordIDs.Keys.Contains(strPhrase))
                                {
                                    dPhraseWordIDs.Add(strPhrase, intsPhraseWordIDs);
                                }
                            }

                            srPhraseWordIDs.Close();
                        }
                        catch { }
                    }
                }

                bReturn = true;
            }

            return bReturn;
        }

		public bool WordPropertiesToClasses()
		{
			bool bReturn = false;

			//Load WordProperties Data
			string strWordPropertiesFilename = @"E:/Programming/Corpora/wordproperties.txt";
			StreamReader srWordProperties = new StreamReader(strWordPropertiesFilename);
			string strPhrase = "";

			while (!srWordProperties.EndOfStream) {
				try {
					string strLine = srWordProperties.ReadLine ();
					string strWord = strLine.Split ('^') [0].Trim ();
					string[] strsWordProperties = strLine.Split ('^') [1].Trim ().Split (',');
					int intPhraseWordIDsIndex = 0;

					strPhrase += strWord + " ";

					if (!dWordMetadata.ContainsKey (strWord)) {
						dWordMetadata.Add (strWord, new List<string> ());
					}

					foreach (string strWordProperty in strsWordProperties) {
                        foreach (string strWordPropertyWord in strWordProperty.Split(' '))
                        {
                            if (!dWordMetadata[strWord].Contains(strWordProperty.Trim()))
                            {
                                dWordMetadata[strWord].Add(strWordProperty.Trim());
                            }
                        }
					}
				} catch (Exception ex) {
				}
			}

			strPhrase = strPhrase.Trim ();
			CreatePhraseClasses (strPhrase);

			srWordProperties.Close();

			bReturn = true;

			return bReturn;
		}

		private void CreatePhraseClasses(string strPhrase){
			string strPhraseConcatenated = strPhrase.Replace (" ", "");
			StreamWriter swClasses = new StreamWriter("test-Classes.cs");// strPhraseConcatenated + ".cs");
			StringBuilder sbClasses = new StringBuilder ();

			foreach (string strWord in strPhrase.Trim().Split()) {
				string strWordProperCase = strWord [0].ToString ().ToUpper ();

				if (strWord.Length > 1) {
					strWordProperCase += strWord.Substring (1, strWord.Length - 1);
				}

				sbClasses.AppendLine ("public class " + strWordProperCase + "{");

				try {
					foreach (string strProperty in dWordMetadata[strWord]) {
						string strPropertyNoSpace = "";

						foreach (string strPropertyWord in strProperty.Trim().Split()) {
							string strPropertyProperCase = strPropertyWord [0].ToString ().ToUpper ();

							if (strProperty.Length > 1) {
								strPropertyProperCase += strPropertyWord.Substring (1, strPropertyWord.Length - 1);
							}

							strPropertyNoSpace += strPropertyProperCase.Trim ();
						}

						sbClasses.AppendLine ("/tpublic bool b" + strPropertyNoSpace + " = true;");
					}
				} catch (Exception ex) {
				}

				sbClasses.AppendLine ("}");
			}

			swClasses.Write (sbClasses.ToString ());
			swClasses.Close ();
		}

		public string GetPhrasePart(int intCounter, int intPhraseLength, string strPart, string[] strsPhrase, string strPhrase){
			try {
				for (int intBuilder2 = intCounter; intBuilder2 <= intPhraseLength; intBuilder2++) {
					strPart = strPart + " " + strsPhrase [intBuilder2 - 1];
				}
				strPart = strPart.Trim ();
			} catch {
			}

			if (!dPhraseParts.ContainsKey (strPhrase)) {
				dPhraseParts.Add (strPhrase, new List<string> ());
			}

			if (!dPhraseParts [strPhrase].Contains (strPart)) {
				dPhraseParts [strPhrase].Add (strPart);
			}
			//Console.WriteLine (strPart + "<<");

			return strPart;
		}

		public void CreatePhraseParts(ref Input input, Phrases phrases){
			string strPhrasesDirectory = input.DataPath + @"/" + input.Base + @"/Phrases";

			if (!Directory.Exists(strPhrasesDirectory) ||
				phrases.dPhraseIDs.Count == 0)
			{
				throw new Exception("Create Phrases First");
			}

			StreamWriter swPhraseParts = new StreamWriter (input.InsertStringIntoFilename(@"-PhraseParts"));
			
			for (int intPhraseLength = 1; intPhraseLength <= 100; intPhraseLength++) {
				foreach (string strPhrase in phrases.dPhraseCounts.OrderBy(a=>a.Value).
					Where(a=>a.Key.Split().Count() == intPhraseLength).
					Where(a=>phrases.dPhraseCounts[a.Key] > 1).Select(a=>a.Key)) {

					for (int intCounter = 1; intCounter <= intPhraseLength; intCounter++) {
						int intRemainer = intPhraseLength - intCounter;

						if (intRemainer >= 0) {
							string strPhrasePart = "";
							string[] strsPhrase = strPhrase.Split ();
							//Console.WriteLine (">>" + strPhrase + " ^ " + 
							//	intPhraseLength.ToString () + " ^ " + intCounter.ToString ());

							strPhrasePart = GetPhrasePart (intCounter, intPhraseLength, strPhrasePart, strsPhrase, strPhrase);

							//swPhraseParts.WriteLine (strPhrase + " ^ " + str1 + " ^ " + str2);
						}
					}
				}
			}
			Dictionary<string, List<string>> dPhrasePartsReversed = new Dictionary<string, List<string>> ();

			foreach (string strKey in dPhraseParts.Keys.OrderBy(a=>a)) {
				foreach (string strValue in dPhraseParts[strKey].OrderBy(a=>a)) {
					if (!dPhrasePartsReversed.ContainsKey (strValue)) {
						dPhrasePartsReversed.Add (strValue, new List<string> ());
					}

					dPhrasePartsReversed [strValue].Add (strKey);
				}
			}

			foreach (string strKey in dPhrasePartsReversed.Keys.OrderBy(a=>a)) {
				foreach (string strValue in dPhrasePartsReversed[strKey].OrderBy(a=>a)) {
					swPhraseParts.WriteLine (strKey + " ^ " + strValue);
					Console.WriteLine (strKey + "<>" + strValue);
				}
			}

			//Console.WriteLine (">>" + dPhraseParts.Count ().ToString () + "<<");

//			foreach (string strKey in dPhraseParts.Keys.OrderBy(a=>a)) {
//				foreach (string strValue in dPhraseParts[strKey].OrderBy(a=>a)) {
//					swPhraseParts.WriteLine (strKey + " ^ " + strValue);
//					Console.WriteLine (strKey + "<>" + strValue);
//				}
//			}

			swPhraseParts.Close ();
		}

		public void CreatePhrases(ref Input input, ref Words words)
        {
            bool bLongestCommonPhrase = false;
            int intPhraseLength = 1;
            int intPhraseID = 0;
            StringBuilder sbPhraseIDs = new StringBuilder();
            string strCurrentWord = "";
            
            strPhrasesDirectory = input.DataPath + @"/" + input.Base + @"/Phrases";
            
            if (!Directory.Exists(strPhrasesDirectory))
            {
                Directory.CreateDirectory(strPhrasesDirectory);
            }

            while (!bLongestCommonPhrase)
            {
                DateTime dtNow = DateTime.Now;
                string strLastPhrase = "";
                //var countKeys =
                //    from b in dPhraseCounts.Keys
                //    where b.Trim() != ""
                //    where dPhraseCounts[b] > 1
                //    orderby dPhraseCounts[b] descending
                //    orderby b
                //    select b.Trim();
                
                PhraseCounts.Clear();
                FWIDs.Clear();
                PhraseIDs.Clear();
                PhraseWordsIDs.Clear();

                bLongestCommonPhrase = true;

                for (int intPosition = 1; intPosition <= words.PositionWords.Count - intPhraseLength + 1; intPosition++)
                {
                    string strCurrentPhrase = "";
                    int[] intsPhraseWordIDs = (int[])Array.CreateInstance(typeof(int), intPhraseLength);
                    int intSWID = words.GetPositionWordID(intPosition);
                    
                    //build phrase
                    for (int intCounter = 1; intCounter <= intPhraseLength; intCounter++)
                    {
                        strCurrentWord = words.PositionWords[intPosition + intCounter - 1];
                        intsPhraseWordIDs[intCounter - 1] = words.WordIDs[strCurrentWord];
                        strCurrentPhrase += " " + strCurrentWord;
                    }

                    strCurrentPhrase = strCurrentPhrase.Trim();

                    if (strCurrentPhrase != "")
                    {
                        //PhraseIDs
                        if (!dPhraseIDs.ContainsKey(strCurrentPhrase))
                        {
                            intPhraseID++;
                            dPhraseIDs.Add(strCurrentPhrase, intPhraseID);
                            dPhraseWordIDs.Add(strCurrentPhrase, intsPhraseWordIDs);
                        }

                        //PhraseCounts
                        if (!dPhraseCounts.ContainsKey(strCurrentPhrase))
                        {
                            dPhraseCounts.Add(strCurrentPhrase, 1);
                        }
                        else
                        {
                            dPhraseCounts[strCurrentPhrase]++;
                            bLongestCommonPhrase = false;
                        }

                        //PhraseFirstWordIDs
                        if (!dPhraseFirstWordPositions.ContainsKey(strCurrentPhrase))
                        {
                            dPhraseFirstWordPositions.Add(strCurrentPhrase, new List<int>());
                        }

                        dPhraseFirstWordPositions[strCurrentPhrase].Add(intPosition);

                        ////PhraseStructure
                        //if (!dPhraseStructure.ContainsKey(intSWID)) //SWID
                        //{
                        //    dPhraseStructure.Add(intSWID, new Dictionary<int,Dictionary<int,Dictionary<int,string>>>());
                        //}

                        //if (!dPhraseStructure[intSWID].ContainsKey(intPosition)) //FWID
                        //{
                        //    dPhraseStructure[intSWID].Add(intPosition, new Dictionary<int,Dictionary<int,string>>());
                        //}

                        //if (!dPhraseStructure[intSWID][intPosition].ContainsKey(intPhraseLength)) //PhraseLength
                        //{
                        //    dPhraseStructure[intSWID][intPosition].Add(intPhraseLength, new Dictionary<int, string>());
                        //}

                        //if (!dPhraseStructure[intSWID][intPosition][intPhraseLength].ContainsKey(intPhraseID)) //PhraseID, PhraseText
                        //{
                        //    dPhraseStructure[intSWID][intPosition][intPhraseLength].Add(intPhraseID, strCurrentPhrase);
                        //}

                        //if (strLastPhrase != "")
                        //{
                        //    //CommonLengthPhraseDoubles
                        //    if (!dCommonLengthPhraseDoubles.ContainsKey(strLastPhrase))
                        //    {
                        //        dCommonLengthPhraseDoubles.Add(strLastPhrase, new Dictionary<string, int>());
                        //    }

                        //    if (!dCommonLengthPhraseDoubles[strLastPhrase].ContainsKey(strCurrentPhrase))
                        //    {
                        //        dCommonLengthPhraseDoubles[strLastPhrase].Add(strCurrentPhrase, 0);
                        //    }

                        //    dCommonLengthPhraseDoubles[strLastPhrase][strCurrentPhrase]++;

                        //}

                        strLastPhrase = strCurrentPhrase;
                    }
                }

                //int intCountKeys = countKeys.Count();

                //if (intCountKeys == 0)
                //{
                //    bLongestCommonPhrase = true;
                //}
                
                WritePhrases (ref input, intPhraseLength, null, null);

                intPhraseLength++;
            }

            //lblLongestPhrase.Text = "Longest Phrase: " + (intPhraseLength - 2).ToString();

//            //Since this was just created, reload the dictionary with all lengths
//            LoadPhraseCounts(ref input);
//            LoadFirstWordIDs(ref input);
//            //LoadPhraseDoubles(ref input);
//            LoadPhraseIDs(ref input);
//            LoadPhraseWordIDs(ref input);
			Console.WriteLine ("-----> " + dPhraseIDs.Count().ToString ());
            //ReducePhrases(ref input);

            ////Since this was just reduced, reload the dictionary with all lengths
            //LoadPhraseCounts(ref input);
            //LoadFirstWordIDs(ref input);
            ////LoadPhraseDoubles(ref input);
            //LoadPhraseIDs(ref input);
            //LoadPhraseWordIDs(ref input);

        }

        public void ReducePhrases(ref Input input)
        {
            if (!File.Exists(input.DataPath + @"/" + input.Base + @"/Phrases/" + input.Base + "-1-Reduced.txt"))
            {
                dPhraseCounts.Clear();
                dPhraseFirstWordPositions.Clear();
                dPhraseIDs.Clear();
                dPhraseWordIDs.Clear();

                LoadPhraseCounts(ref input);// File(input.DataPath + "/" + input.Base + "/Phrases/" + input.Base + "-" + intCurrentLength.ToString() + "-Counts.txt");
                LoadFirstWordIDs(ref input);

                for (int intCurrentPhraseLength = 1; intCurrentPhraseLength <= input.PhraseFilesCount(); intCurrentPhraseLength++)
                {
                    LoadPhraseIDs(ref input, intCurrentPhraseLength);
                    LoadPhraseWordIDs(ref input, intCurrentPhraseLength);
                }

                for (int intCurrentLength = input.PhraseFilesCount(); intCurrentLength > 1; intCurrentLength--)
                {
                    string strName = input.DataPath + @"/" + input.Base + @"/Phrases/" + input.Base + "-" + intCurrentLength.ToString() + "-Counts.txt";
                    string strNameBase = strName.Remove(0, (input.DataPath + @"/" + input.Base + @"/Phrases/" + input.Base + "-").Length).TrimEnd("-Counts.txt".ToCharArray());
                    List<string> lstrReduce = new List<string>();
                    int intNextPrevious = intCurrentLength - 1;
                    bool bContinue = true;

                    while (bContinue)
                    {
                        if (Regex.IsMatch(strNameBase, @"(?<length>[0-9]{1,})"))
                        {
                            Phrases phrasesNext = new Phrases();

                            lstrReduce = new List<string>();
                            bContinue = false;

                            foreach (string strPhrase in dPhraseCounts.Keys.Where(a => a.Trim().Split().Count() == intCurrentLength))
                            {
                                foreach (string strPhraseNext in PhraseCounts.Keys.Where(a => a.Trim().Split().Count() == intNextPrevious))
                                {
                                    if ((" " + strPhrase + " ").Contains(" " + strPhraseNext + " "))
                                    {
                                        if (PhraseCounts[strPhrase] == PhraseCounts[strPhraseNext])
                                        {
                                            bContinue = true;

                                            try
                                            {
                                                if (!lstrReduce.Contains(strPhraseNext))
                                                {
                                                    lstrReduce.Add(strPhraseNext);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                string strMessage = ex.Message;
                                            }
                                        }
                                    }
                                }
                            }

                            foreach (string strReduce in lstrReduce)
                            {
                                PhraseCounts.Remove(strReduce);
                                FWIDs.Remove(strReduce);
                                PhraseIDs.Remove(strReduce);
                                PhraseWordsIDs.Remove(strReduce);
                            }

                            WritePhrases(ref input, intNextPrevious, null, lstrReduce);
                        }

                        intNextPrevious--;

                        if (intNextPrevious == 0)
                        {
                            bContinue = false;
                        }
                    }
                }
            } //else already reduced
        }

        public void WritePhrases(ref Input inputWrite, int intPhraseLength, 
            StreamWriter swTimesWrite, List<string> lstrReduced)
        {
            StreamWriter swPhraseCounts = new StreamWriter(strPhrasesDirectory +
                        @"/" + inputWrite.Base + "-" + intPhraseLength.ToString() + "-Counts" + inputWrite.Ext);
            StreamWriter swFirstWordIDs = new StreamWriter(strPhrasesDirectory +
                @"/" + inputWrite.Base + "-" + intPhraseLength.ToString() + "-FirstWordIDs" +
                inputWrite.Ext);
            //StreamWriter swPhraseDoubles = new StreamWriter(strPhrasesDirectory +
            //    "/" + inputWrite.Base + "-" + intPhraseLength.ToString() + "-PhraseDoubles" +
            //    inputWrite.Ext);
            //Dictionary<string, int> dPhraseDoubles = new Dictionary<string, int>();
            StreamWriter swPhraseIDs = new StreamWriter(strPhrasesDirectory +
                @"/" + inputWrite.Base + "-" + intPhraseLength.ToString() + "-PhraseIDs" +
            inputWrite.Ext);
            StreamWriter swPhraseWordIDs = new StreamWriter(strPhrasesDirectory +
                @"/" + inputWrite.Base + "-" + intPhraseLength.ToString() + "-PhraseWordIDs" +
                inputWrite.Ext);
            StreamWriter swPhraseReduced = null;

            if (lstrReduced != null)
            {
                swPhraseReduced = new StreamWriter(strPhrasesDirectory +
                @"/" + inputWrite.Base + "-" + intPhraseLength.ToString() + "-Reduced" +
                inputWrite.Ext);
            }

            var phraseCountsKeys =
                from b in PhraseCounts
                where b.Value > 1
                select b.Key.Trim();

            if (phraseCountsKeys != null && phraseCountsKeys.Count() > 0)
            {
                foreach (string strPhrase in phraseCountsKeys)
                {
                    swPhraseCounts.WriteLine(strPhrase + " ^ " + (PhraseCounts[strPhrase]).ToString());
                }

                swPhraseCounts.Close();
                
                foreach (string strFirstWordIDs in FWIDs.Keys.Where(a => a.Trim().Split((char[])" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Count() == intPhraseLength))
                {
                    if (FWIDs[strFirstWordIDs].Count() > 1)
                    {
                        swFirstWordIDs.Write(strFirstWordIDs + " ^");

                        foreach (int intFirstWordID in FWIDs[strFirstWordIDs])
                        {
                            swFirstWordIDs.Write(" " + intFirstWordID.ToString());
                        }

                        swFirstWordIDs.WriteLine();
                    }
                }

                swFirstWordIDs.Close();

                foreach (string strPhraseIDs in PhraseIDs.Keys.Where(a => a.Trim().Split().Count() == intPhraseLength))
                {
                    if (FWIDs[strPhraseIDs].Count() > 1)
                    {
                        swPhraseIDs.Write(strPhraseIDs + " ^ ");
                        swPhraseIDs.WriteLine(PhraseIDs[strPhraseIDs]);
                    }
                }

                swPhraseIDs.Close();

                foreach (string strPhraseWordIDs in PhraseWordsIDs.Keys.Where(a => a.Trim().Split().Count() == intPhraseLength))
                {
                    if (FWIDs[strPhraseWordIDs].Count() > 1)
                    {
                        swPhraseWordIDs.Write(strPhraseWordIDs + " ^ ");

                        for (int intPhraseWordIDIndex = 0; intPhraseWordIDIndex < PhraseWordsIDs[strPhraseWordIDs].Length; intPhraseWordIDIndex++)
                        {
                            swPhraseWordIDs.Write((PhraseWordsIDs[strPhraseWordIDs][intPhraseWordIDIndex]).ToString() + " ");
                        }

                        swPhraseWordIDs.WriteLine();
                    }
                }

                swPhraseWordIDs.Close();

                if (lstrReduced != null)
                {
                    foreach (string strReduced in lstrReduced)
                    {
                        swPhraseReduced.WriteLine(strReduced);
                    }

                    swPhraseReduced.Close();
                }

                ////combine phrase doubles for easy output ordering
                //foreach (string strPhraseDoubles in dCommonLengthPhraseDoublesWrite.Keys)
                //{
                //    foreach (string strPhraseDoubles2 in dCommonLengthPhraseDoublesWrite[strPhraseDoubles].Keys)
                //    {
                //        if (dCommonLengthPhraseDoublesWrite[strPhraseDoubles][strPhraseDoubles2] > 1)
                //        {
                //            dPhraseDoubles.Add(strPhraseDoubles + " ^ " + strPhraseDoubles2,
                //                dCommonLengthPhraseDoublesWrite[strPhraseDoubles][strPhraseDoubles2]);
                //        }
                //    }
                //}

                ////write phrase doubles
                //foreach (string strPhraseDoubles in phraseDoubles)
                //{
                //    swPhraseDoubles.WriteLine(strPhraseDoubles + " ^ " +
                //        (dPhraseDoubles[strPhraseDoubles]).ToString());
                //}

                //swPhraseDoubles.Close();

            }

            if (swTimesWrite != null)
            {
                swTimesWrite.WriteLine((intPhraseLength).ToString() + " - " +
                    (DateTime.Now).ToString());
            }
        }

        public int GetHighestPhraseLength(ref Input input)
        {
            int intReturn = -1;

            strPhrasesDirectory = input.DataPath + @"/" + input.Base + @"/Phrases";

            foreach (string strFilenameTemp in Directory.EnumerateFiles(strPhrasesDirectory))
            {
                if (strFilenameTemp.EndsWith("-Counts.txt"))
                {
                    string strFilenameTemp2 = strFilenameTemp.Remove(strFilenameTemp.Length - 11);
                    strFilenameTemp2 = strFilenameTemp2.Remove(0, strPhrasesDirectory.Length + 1 + input.Base.Length + 1);

                    int intLengthTemp = Convert.ToInt32(strFilenameTemp2);

                    if (intLengthTemp > intReturn)
                    {
                        intReturn = intLengthTemp;
                    }
                }
            }

            return intReturn;
        }
    }

    public class PhraseID
    {
        public int intPhraseID = 0;
        public string strPhraseText = "";
        
        public PhraseID(int intPhraseIDTemp)
        {
            intPhraseID = intPhraseIDTemp;
        }
    }

    public class PhraseLength
    {
        public int intPhraseLength = 0;
        public List<PhraseID> lPhraseIDs = new List<PhraseID>();

        public PhraseLength(int intPhraseLengthTemp)
        {
            intPhraseLength = intPhraseLengthTemp;
        }

        public List<PhraseID> PhraseIDS(int intLowestPhraseID, int intHighestPhraseID)
        {
            List<PhraseID> lReturn = new List<PhraseID>();

            foreach (PhraseID piCurrent in lPhraseIDs.Where(a => a.intPhraseID >= intLowestPhraseID &&
                a.intPhraseID <= intHighestPhraseID))
            {
                lReturn.Add(piCurrent);
            }

            return lReturn;
        }
    }

    public class PhraseFWID
    {
        public int intFWID = 0;
        public List<PhraseLength> lPhraseLengths = new List<PhraseLength>();

        public PhraseFWID(int intFWIDTemp) 
        {
            intFWID = intFWIDTemp;
        }

        public List<PhraseLength> PhraseLengths(int intLowestPhraseLength, int intHighestPhraseLength)
        {
            List<PhraseLength> lReturn = new List<PhraseLength>();

            foreach (PhraseLength plCurrent in lPhraseLengths.Where(a => a.intPhraseLength >= intLowestPhraseLength &&
                a.intPhraseLength <= intHighestPhraseLength))
            {
                lReturn.Add(plCurrent);
            }

            return lReturn;
        }
    }

    public class PhraseComposition
    {
        Phrases phrCurrent;
        public Dictionary<int, int[]> dComposition = new Dictionary<int, int[]>(); //D<PhraseID, composed phrase> eg. D[12699] = "[64018][445]"
        public Dictionary<int, string> dPhraseText = new Dictionary<int, string>(); //D<PhraseID, composed phrase text>
        
        public PhraseComposition(ref Phrases phrTemp)
        {
            phrCurrent = phrTemp;
        }

        public void ComposeTwo(ref Input input, ref Words words)
        {
            string strCompositionFilename = input.InsertStringIntoFilename("-PhrasalComposition");

            if (!File.Exists(strCompositionFilename))
            {
                StreamWriter swPhrasalComposition = new StreamWriter(strCompositionFilename);
                //int intAlreadyInDictionary = 0;

                foreach (string strPhraseText in phrCurrent.PhraseWordsIDs.Keys.Where(a => a.Split().Count() > 1).OrderBy(a => a.Split().Count()))
                {
                    int[] intsPhraseVector = phrCurrent.PhraseWordsIDs[strPhraseText];
                    int[] intsComposed = (int[])Array.CreateInstance(typeof(int), intsPhraseVector.Length);
                    int intComposedIndex = 0;
                    int intCurrentPhraseID = phrCurrent.PhraseIDs[phrCurrent.PhraseWordsIDs.Where(a => a.Value == intsPhraseVector).First().Key];

                    for (int intVectorIndex = 0; intVectorIndex < intsPhraseVector.Length; intVectorIndex++)
                    {
                        List<int> lintPossiblePieces = new List<int>();
                        int intBestPossiblePiece = -1;

                        for (int intSubvectorSize = intsPhraseVector.Length - 1; intSubvectorSize > 0; intSubvectorSize--)
                        {
                            if (intSubvectorSize <= intsPhraseVector.Length - intVectorIndex)
                            {
                                string strSubvectorPhrase = "";
                                int[] intsSubVector = (int[])Array.CreateInstance(typeof(int), intSubvectorSize);
                                int intSubvectorPhraseID = -1;

                                for (int intSubvectorIndex = 0; intSubvectorIndex < intsSubVector.Length; intSubvectorIndex++)
                                {
                                    if (intVectorIndex + intSubvectorIndex < intsPhraseVector.Length)
                                    {
                                        intsSubVector[intSubvectorIndex] = intsPhraseVector[intVectorIndex + intSubvectorIndex];
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                foreach (int intSubvectorWordID in intsSubVector)
                                {
                                    strSubvectorPhrase += words.WordIDs.Where(a => a.Value == intSubvectorWordID).First().Key + " ";
                                }

                                strSubvectorPhrase = strSubvectorPhrase.Trim();

                                if (dComposition.ContainsValue(intsSubVector))
                                {
                                    intSubvectorPhraseID = dComposition.Where(a => a.Value == intsSubVector).First().Key;
                                    //intAlreadyInDictionary++;
                                }
                                else
                                {
                                    try
                                    {
                                        intSubvectorPhraseID = phrCurrent.PhraseIDs[strSubvectorPhrase];
                                    }
                                    catch (Exception ex)
                                    {
                                        intSubvectorPhraseID = -1;
                                    }
                                }

                                if (intSubvectorPhraseID > -1)
                                {
                                    if (intsSubVector.Intersect(intsPhraseVector).SequenceEqual(intsSubVector))
                                    {
                                        lintPossiblePieces.Add(intSubvectorPhraseID);
                                    }
                                }
                            }
                        }

                        foreach (int intPossiblePiecePhraseID in lintPossiblePieces)
                        {
                            if (intBestPossiblePiece > -1)
                            {
                                if (phrCurrent.PhraseCounts[phrCurrent.PhraseIDs.Where(a => a.Value == intPossiblePiecePhraseID).First().Key] >
                                  phrCurrent.PhraseCounts[phrCurrent.PhraseIDs.Where(a => a.Value == intBestPossiblePiece).First().Key])
                                {
                                    intBestPossiblePiece = intPossiblePiecePhraseID;
                                }
                            }
                            else
                            {
                                intBestPossiblePiece = intPossiblePiecePhraseID;
                            }
                        }

                        if (intBestPossiblePiece == -1)
                        {
                            string a = "";
                        }

                        intsComposed[intComposedIndex] = intBestPossiblePiece;
                        intComposedIndex++;
                    }

                    dComposition.Add(intCurrentPhraseID, intsComposed);
                    dPhraseText.Add(intCurrentPhraseID, strPhraseText);
                }

                //Write
                foreach (int intCurrentPhraseID in dComposition.Keys)
                {
                    swPhrasalComposition.Write(intCurrentPhraseID.ToString() + " ^");

                    foreach (int intCurrentValue in dComposition[intCurrentPhraseID])
                    {
                        swPhrasalComposition.Write(" " + intCurrentValue.ToString());
                    }

                    //swPhrasalComposition.Write(" ^" + 
                    swPhrasalComposition.WriteLine();
                }

                swPhrasalComposition.Close();
            }
            else
            {
                StreamReader srPhrasalComposition = new StreamReader(strCompositionFilename);
                Dictionary<int, int[]> dComposition = new Dictionary<int, int[]>();

                while (!srPhrasalComposition.EndOfStream)
                {
                    string strLine = srPhrasalComposition.ReadLine();
                    int intPhraseID = Convert.ToInt32(strLine.Split('^')[0]);
                    string[] strsPhrasalComposition = strLine.Remove(0, strLine.IndexOf('^') + 2).Split();
                    int[] intsPhrasalComposition = (int[])Array.CreateInstance(typeof(int), strsPhrasalComposition.Count());
                    int intPCIndex = -1;

                    try
                    {
                        foreach (string strPhrasalCompositionPart in strsPhrasalComposition)
                        {
                            intPCIndex++;

                            intsPhrasalComposition[intPCIndex] = Convert.ToInt32(strPhrasalCompositionPart);
                        }
                    }
                    catch { }

                    dComposition.Add(intPhraseID, intsPhrasalComposition);
                }

                srPhrasalComposition.Close();
            }
        }

        public void Compose(ref Input input, ref Words words)
        {
            string strCompositionFilename = input.InsertStringIntoFilename("-PhrasalComposition");

            if (!File.Exists(strCompositionFilename))
            {
                StreamWriter swPhrasalComposition = new StreamWriter(strCompositionFilename);
                //int intAlreadyInDictionary = 0;

                foreach (int[] intsPhraseVector in phrCurrent.PhraseWordsIDs.Values.Where(a => a.Length > 1).OrderBy(a => a.Length))
                {
                    int[] intsComposed = (int[])Array.CreateInstance(typeof(int), intsPhraseVector.Length);
                    int intComposedIndex = 0;
                    int intCurrentPhraseID = phrCurrent.PhraseIDs[phrCurrent.PhraseWordsIDs.Where(a => a.Value == intsPhraseVector).First().Key];

                    for (int intVectorIndex = 0; intVectorIndex < intsPhraseVector.Length; intVectorIndex++)
                    {
                        List<int> lintPossiblePieces = new List<int>();
                        int intBestPossiblePiece = -1;

                        for (int intSubvectorSize = intsPhraseVector.Length - 1; intSubvectorSize > 0; intSubvectorSize--)
                        {
                            if (intSubvectorSize <= intsPhraseVector.Length - intVectorIndex)
                            {
                                string strSubvectorPhrase = "";
                                int[] intsSubVector = (int[])Array.CreateInstance(typeof(int), intSubvectorSize);
                                int intSubvectorPhraseID = -1;

                                for (int intSubvectorIndex = 0; intSubvectorIndex < intsSubVector.Length; intSubvectorIndex++)
                                {
                                    if (intVectorIndex + intSubvectorIndex < intsPhraseVector.Length)
                                    {
                                        intsSubVector[intSubvectorIndex] = intsPhraseVector[intVectorIndex + intSubvectorIndex];
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                foreach (int intSubvectorWordID in intsSubVector)
                                {
                                    strSubvectorPhrase += words.WordIDs.Where(a => a.Value == intSubvectorWordID).First().Key + " ";
                                }

                                strSubvectorPhrase = strSubvectorPhrase.Trim();

                                if (dComposition.ContainsValue(intsSubVector))
                                {
                                    intSubvectorPhraseID = dComposition.Where(a => a.Value == intsSubVector).First().Key;
                                    //intAlreadyInDictionary++;
                                }
                                else
                                {
                                    try
                                    {
                                        intSubvectorPhraseID = phrCurrent.PhraseIDs[strSubvectorPhrase];
                                    }
                                    catch (Exception ex)
                                    {
                                        intSubvectorPhraseID = -1;
                                    }
                                }

                                if (intSubvectorPhraseID > -1)
                                {
                                    if (intsSubVector.Intersect(intsPhraseVector).SequenceEqual(intsSubVector))
                                    {
                                        lintPossiblePieces.Add(intSubvectorPhraseID);
                                    }
                                }
                            }
                        }

                        foreach (int intPossiblePiecePhraseID in lintPossiblePieces)
                        {
                            if (intBestPossiblePiece > -1)
                            {
                                if (phrCurrent.PhraseCounts[phrCurrent.PhraseIDs.Where(a => a.Value == intPossiblePiecePhraseID).First().Key] >
                                  phrCurrent.PhraseCounts[phrCurrent.PhraseIDs.Where(a => a.Value == intBestPossiblePiece).First().Key])
                                {
                                    intBestPossiblePiece = intPossiblePiecePhraseID;
                                }
                            }
                            else
                            {
                                intBestPossiblePiece = intPossiblePiecePhraseID;
                            }
                        }

                        if (intBestPossiblePiece == -1)
                        {
                            string a = "";
                        }

                        intsComposed[intComposedIndex] = intBestPossiblePiece;
                        intComposedIndex++;
                    }

                    dComposition.Add(intCurrentPhraseID, intsComposed);
                }

                //Write
                foreach (int intCurrentPhraseID in dComposition.Keys)
                {
                    swPhrasalComposition.Write(intCurrentPhraseID.ToString() + " ^");

                    foreach (int intCurrentValue in dComposition[intCurrentPhraseID])
                    {
                        swPhrasalComposition.Write(" " + intCurrentValue.ToString());
                    }

                    //swPhrasalComposition.Write(" ^" + 
                    swPhrasalComposition.WriteLine();
                }

                swPhrasalComposition.Close();
            }
            else
            {
                StreamReader srPhrasalComposition = new StreamReader(strCompositionFilename);
                Dictionary<int, int[]> dComposition = new Dictionary<int, int[]>();

                while (!srPhrasalComposition.EndOfStream)
                {
                    string strLine = srPhrasalComposition.ReadLine();
                    int intPhraseID = Convert.ToInt32(strLine.Split('^')[0]);
                    string[] strsPhrasalComposition = strLine.Remove(0, strLine.IndexOf('^') + 2).Split();
                    int[] intsPhrasalComposition = (int[])Array.CreateInstance(typeof(int), strsPhrasalComposition.Count());
                    int intPCIndex = -1;

                    try
                    {
                        foreach (string strPhrasalCompositionPart in strsPhrasalComposition)
                        {
                            intPCIndex++;

                            intsPhrasalComposition[intPCIndex] = Convert.ToInt32(strPhrasalCompositionPart);
                        }
                    }
                    catch { }

                    dComposition.Add(intPhraseID, intsPhrasalComposition);
                }

                srPhrasalComposition.Close();
            }
        }
    }
}