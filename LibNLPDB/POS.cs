using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using OpenNLP.Tools.PosTagger;

namespace LibNLPDB
{
    public class POS
    {
        // transformation rules from Mark Watson's FastTag v2
        //
        ////  rule 1: DT, {VBD | VBP} --> DT, NN		
        //if (i > 0 && string.Equals(pTags[i - 1], "DT"))
        //{
        //    if (string.Equals(pTag, "VBD") || string.Equals(pTag, "VBP") || string.Equals(pTag, "VB"))
        //    {
        //        pTag = "NN";
        //    }
        //}
        //// rule 2: convert a noun to a number (CD) if "." appears in the word
        //if (pTag.StartsWith("N"))
        //{
        //    Single s;
        //    if (word.IndexOf(".", StringComparison.CurrentCultureIgnoreCase) > -1 || Single.TryParse(word, out s))
        //    {
        //        pTag = "CD";
        //    }
        //}
        //// rule 3: convert a noun to a past participle if words.get(i) ends with "ed"
        //if (pTag.StartsWith("N") && word.EndsWith("ed"))
        //{
        //    pTag = "VBN";
        //}
        //// rule 4: convert any type to adverb if it ends in "ly";
        //if (word.EndsWith("ly"))
        //{
        //    pTag = "RB";
        //}
        //// rule 5: convert a common noun (NN or NNS) to a adjective if it ends with "al"
        //if (pTag.StartsWith("NN") && word.EndsWith("al"))
        //{
        //    pTag = "JJ";
        //}
        //// rule 6: convert a noun to a verb if the preceeding word is "would"; counter-example: Would I want to..
        //if (i > 0 && pTag.StartsWith("NN") && string.Equals(words[i - 1], "would"))
        //{
        //    pTag = "VB";
        //}
        //// rule 7: if a word has been categorized as a common noun and it ends with "s",
        ////         then set its type to plural common noun (NNS)
        //if (string.Equals(pTag, "NN") && word.EndsWith("s"))
        //{
        //    pTag = "NNS";
        //}
        //// rule 8: convert a common noun to a present participle verb (i.e., a gerand)
        //if (pTag.StartsWith("NN") && word.EndsWith("ing"))
        //{
        //    pTag = "VBG";
        //}

        private Dictionary<int, string> dPOS = new Dictionary<int, string>(); //D<Word Position, Tag>
        private EnglishMaximumEntropyPosTagger mPosTagger; //Requires SharpEntropy

        public List<string> lstrTags = new List<string>();

        public Dictionary<int, string> POSs
        {
            get
            {
                return dPOS;
            }
            set
            {
                dPOS = value;
            }
        }
        
        public POS(string strModelPath, ref Input mainInput, ref Words mainWords)
        {
            if (!LoadPOS(ref mainInput))
            {
                mPosTagger = new EnglishMaximumEntropyPosTagger(strModelPath + "EnglishPOS.nbin", strModelPath + "Parser/tagdict");
            
                //Create POS File
                CreatePOS(ref mainInput, ref mainWords);

                //Create Noun Files
                //CreateNounFiles(ref mainInput, ref mainWords);
            }

            lstrTags.Clear();

            foreach (string strTag in "DT NN IN NNP VBD CC PRP$ CD VBG VBN JJ VB EX NNS PRP WDT RB VBP WP$ VBZ TO JJR MD WRB PDT RP WP SYM RBR FW JJS UH RBS NNPS POS LS".Split())
            {
                lstrTags.Add(strTag);
            }
        }

        public bool LoadPOS(ref Input input)
        {
            string strPOS = input.InsertStringIntoFilename("-POS");
            bool bReturn = false;

            dPOS.Clear();

            if (File.Exists(strPOS))
            {
                StreamReader srPOS = new StreamReader(strPOS);

                while (!srPOS.EndOfStream)
                {
                    string strLine = srPOS.ReadLine();
                    int intKey = Convert.ToInt32(strLine.Split('^')[0].Trim());
                    string strValue = strLine.Split('^')[1].Trim();

                    dPOS.Add(intKey, strValue);
                }

                srPOS.Close();

                bReturn = true;
            }

            return bReturn;
        }

		public void CreateNounFiles(ref Input input, ref Words words){
			StreamWriter swNounsByCount = new StreamWriter(input.InsertStringIntoFilename("-NounsByCount"));
			StreamWriter swNounsBySententialPrimaries = new StreamWriter(input.InsertStringIntoFilename("-NounsBySententialPrimaries")); //If a sentence reduces all determiners, modifiers, structures and otherwise (getting primary or head words) to NN VB NN NN then the nouns would be numbered in the file 1,3,and 4 (some other file should hold this sentence's number 2 verb)
			Dictionary<string, int> dNounsByCount = new Dictionary<string, int>();
			Dictionary<int, Dictionary<int, string>> dNounsBySententialPrimaries = new Dictionary<int, Dictionary<int, string>> (); //D<SentenceID, D<position, word>>

			foreach (int intSentenceID in words.SentencesObject.dStructures.Keys.OrderBy(a=>a)) {
				int intHeadCounter = 0;

				dNounsBySententialPrimaries.Add (intSentenceID, new Dictionary<int, string> ());

				foreach (int intUWID in words.SentencesObject.dStructures[intSentenceID].Keys.OrderBy(a=>a)) {
					string strWord = words.SentencesObject.dStructures [intSentenceID] [intUWID].First ().Key;
					string strPOS = words.SentencesObject.dStructures [intSentenceID] [intUWID].First ().Value;

					if (strPOS.StartsWith ("NN")) {
						intHeadCounter++;

						if (!dNounsByCount.ContainsKey (strWord)) {
							dNounsByCount.Add (strWord, 0);
						}

						dNounsByCount [strWord]++;
						dNounsBySententialPrimaries [intSentenceID].Add (intHeadCounter, strWord);
					}
				}
			}

			foreach (string strWord in dNounsByCount.OrderByDescending(a=>a.Value).Select(a=>a.Key)) {
				swNounsByCount.WriteLine (strWord + " ^ " + dNounsByCount [strWord].ToString ());
			}

			foreach (int intSentenceID in dNounsBySententialPrimaries.Keys.OrderBy(a=>a)) {
				foreach (int intUWID in dNounsBySententialPrimaries[intSentenceID].Keys.OrderBy(a=>a)) {
					swNounsBySententialPrimaries.WriteLine (intSentenceID.ToString () + " ^ " + intUWID.ToString () + " ^ " + dNounsBySententialPrimaries[intSentenceID][intUWID]);
				}
			}

			swNounsByCount.Close ();
			swNounsBySententialPrimaries.Close ();
		}

        public void CreatePOS(ref Input input, ref Words words)
        {
            string strPOS = input.InsertStringIntoFilename("-POS");
            string strArff = input.InsertStringIntoArffFilename("-POS");

            string[] tokens = null;
            string[] tags = null;

            dPOS.Clear();

            foreach (int intCurrentSentenceID in words.SentenceList.Keys)
            {
                int intFirstUWID = words.SentenceFirstPositions[intCurrentSentenceID];
                int intLastUWID = words.SentenceFirstPositions[intCurrentSentenceID] +
                    words.SentenceLengths[intCurrentSentenceID] - 1;
                string[] strsTagsTemp;
                StringBuilder sbSentence = new StringBuilder();

                tokens = words.PositionWords.Where(a => a.Key >= intFirstUWID && a.Key <= intLastUWID)
                    .OrderBy(a => a.Key).Select(a => a.Value).ToArray<string>();

                for (int intTokenIndex = 0; intTokenIndex < tokens.Count(); intTokenIndex++)
                {
                    sbSentence.Append(tokens[intTokenIndex].Trim());
                    sbSentence.Append(" ");
                }

                tags = mPosTagger.TagSentence(sbSentence.ToString().Trim()).Split();
                strsTagsTemp = (string[])Array.CreateInstance(typeof(string), tags.Length);

                for (int intTagsIndex = 0; intTagsIndex < tags.Length; intTagsIndex++)
                {
                    string strWordTemp = tags[intTagsIndex].Split('/')[0];
                    string strPOSTemp = tags[intTagsIndex].Split('/')[1];

                    //put these declarations up in the hierarchy
                    //then fill them with premade list files
                    List<string> lWordsIN = new List<string>();
                    List<string> lWordsREF = new List<string>();
                    List<string> lWordsREFD = new List<string>();

                    //fix pos here
                    if (lWordsIN.Contains(strWordTemp))
                    {
                        strPOSTemp = "IN";
                    }

                    if (lWordsREF.Contains(strWordTemp))
                    {
                        strPOSTemp = "REF";
                    }

                    if (lWordsREFD.Contains(strWordTemp))
                    {
                        strPOSTemp = "REF$";
                    }

                    strsTagsTemp[intTagsIndex] = strPOSTemp;
                }

                tags = strsTagsTemp;
                //CC begins a new sentence if NN is an object of the last sentence; 
                //when NN is a subject then CC is almost certainly in a list of subjective parameters or conditions
                //tags = ModifyTags2Back(tags, "", "", ""); //ex. "DT", "JJ", "NN" = DT (JJ~NN) -> DT NN where ~ means "implies"
                //tags = ModifyTags2Ahead(tags, "JJ", "CC", "NN");
                //tags = ModifyTags3BackAhead(tags, "IN", "JJ", "CC", "NN"); //ex. "IN", "JJ", "CC", "NN" -> IN NN CC
                //tags = ModifyTags3BackAhead(tags, "DT", "JJ", "CC", "NN"); //ex. "DT", "JJ", "CC", "NN" -> DT NN CC
                //tags = ModifyTags3BackAhead(tags, "VB", "JJ", "CC", "NN"); //ex. "VB", "JJ", "CC", "NN" -> VB NN CC
                // in/dt/nn/cc/vbz/to/md <-- Word 1
                // dt/jj/prp$/in/prp/nn/vb/vbz <-- Word 2
                // nn/wp/md/prp$/nns <-- Word 3
                // 1  2  3    4    5   6  7  8  9  10
                // ___________________________________
                //                     
                //                 PRP    VB 
                //    WP           NNS
                //
                // ,,NN:10,,VBZ:13,,MD:10 ,,,, ,
                // 2-WP/JJ,3-DT,4-CC,5-IN/PRP/NNS,6-TO,7-PRP$/VB,10-MD/NN,13-VBZ <-- Try to use this as a POS Rank
                //tags = ModifyTags3BackAhead(tags, "IN", "DT", "NN", "JJ");

                try
                {
                    for (int intTokenIndex = 0; intTokenIndex < tokens.Length; intTokenIndex++)
                    {
                        string strToken = tokens[intTokenIndex];
                        string strTag = tags[intTokenIndex].Trim();

                        //if (!lstrTags.Contains(strTag.ToUpper()))
                        //{
                        //    strTag = "?";
                        //}

                        dPOS.Add(intFirstUWID + intTokenIndex, strTag);
                    }
                }
                catch{}
            }

            ////Add POS info to Sentences.dStructures
            //foreach (int intSentenceID in words.SentencesObject.dStructures.Keys.OrderBy(a => a))
            //{
            //    foreach (int intUWID in words.SentencesObject.dStructures[intSentenceID].Keys.OrderBy(a => a))
            //    {
            //        string strWordA = words.GetPositionWord(intUWID);
            //        string strPOSA = "";

            //        try {
            //            strPOSA = dPOS[intUWID];
            //        }
            //        catch
            //        {
            //            strPOSA = "NA";
            //        }

            //        words.SentencesObject.dStructures[intSentenceID][intUWID][strWordA] = strPOSA;
            //    }
            //}

            WritePOS(strPOS, strArff, input.Base, ref words);
        }

        public void WritePOS(string strPOS, string strArff, string strInputBase, ref Words words)
        {
            StreamWriter swPOS = new StreamWriter(strPOS, false);
            StreamWriter swArff = new StreamWriter(strArff, false);
            StringBuilder sbArff = new StringBuilder();

            //Prepare .arff
            sbArff.AppendLine("@relation " + strInputBase);
            sbArff.AppendLine();
            sbArff.AppendLine("@attribute word_position numeric");
            sbArff.AppendLine("@attribute word_id numeric");
            sbArff.Append("@attribute pos {");

            foreach (string strTag in lstrTags)
            {
                sbArff.Append(strTag + ", ");
            }
            sbArff.AppendLine("?}");
            sbArff.AppendLine();
            sbArff.AppendLine("@data");
            sbArff.AppendLine();

            //Write POS and .arff Data Files
            foreach (int intWordPosition in dPOS.Keys.OrderBy(a => a))
            {
                swPOS.WriteLine(intWordPosition.ToString() + " ^ " + dPOS[intWordPosition]);
                sbArff.AppendLine(intWordPosition.ToString() + ", " +
                    words.GetPositionWordID(intWordPosition).ToString() + ", " +
                    dPOS[intWordPosition]);
            }

            swArff.Write(sbArff.ToString());

            swPOS.Close();
            swArff.Close();
        }

		//Modify tag pairs; this pos context considers the last pos
        public string[] ModifyTags2Back(string[] strsTags, string strTag1Check, string strTag2Check, string strTag2Change) 
        {
            string[] strsReturn = (string[])Array.CreateInstance(typeof(string), strsTags.Length);

            strsReturn[0] = strsTags[0];

            for (int intTagsIndex = 1; intTagsIndex < strsTags.Length; intTagsIndex++)
            {
                string strTag1 = strsTags[intTagsIndex - 1];
                string strTag2 = strsTags[intTagsIndex];
                
                if (strTag2.Length >= 2 && strTag1.Length >= 2)
                {
                    if (strTag2.Substring(0, 2) == strTag2Check)
                    {
                        if (intTagsIndex > 0 && strTag1.Substring(0, 2) == strTag1Check)
                        {
                            strTag2 = strTag2Change;
                        }
                    }
                }

                strsReturn[intTagsIndex] = strTag2;
            }

            return strsReturn;
        }

		//Modify tag pairs; this pos context considers the next pos
        public string[] ModifyTags2Ahead(string[] strsTags, string strTag1Check, string strTag2Check, string strTag1Change)
        {
            string[] strsReturn = (string[])Array.CreateInstance(typeof(string), strsTags.Length);

            strsReturn[0] = strsTags[0];
            strsReturn[strsTags.Length - 1] = strsTags[strsTags.Length - 1];

            for (int intTagsIndex = 0; intTagsIndex < strsTags.Length - 1; intTagsIndex++)
            {
                string strTag1 = strsTags[intTagsIndex];
                string strTag2 = strsTags[intTagsIndex + 1];

                if (strTag2.Length >= 2 && strTag1.Length >= 2)
                {
                    if (strTag2.Substring(0, 2) == strTag2Check)
                    {
                        if (strTag1.Substring(0, 2) == strTag1Check)
                        {
                            strTag1 = strTag1Change;
                        }
                    }
                }

                strsReturn[intTagsIndex] = strTag1;
            }

            return strsReturn;
        }

		//Modify tag triples; this pos context considers the last pos and the next pos
        public string[] ModifyTags3BackAhead(string[] strsTags, string strTag1Check, string strTag2Check, string strTag3Check, string strTag2Change)
        {
            string[] strsReturn = (string[])Array.CreateInstance(typeof(string), strsTags.Length);

            strsReturn[0] = strsTags[0];
            strsReturn[strsTags.Length - 1] = strsTags.Last();

            for (int intTagsIndex = 1; intTagsIndex < strsTags.Length - 1; intTagsIndex++)
            {
                string strTag1 = strsTags[intTagsIndex - 1];
                string strTag2 = strsTags[intTagsIndex];
                string strTag3 = strsTags[intTagsIndex + 1];

                if (strTag3.Length >= 2 && strTag2.Length >= 2 && strTag1.Length >= 2)
                {
                    if (strTag2.Substring(0, 2) == strTag2Check && strTag3.Substring(0, 2) == strTag3Check)
                    {
                        if (intTagsIndex > 0 && strTag1.Substring(0, 2) == strTag1Check)
                        {
                            strTag2 = strTag2Change;
                        }
                    }
                }

                strsReturn[intTagsIndex] = strTag2;
            }

            return strsReturn;
        }

        public List<int> GetPOSPositions(string strPOS)
        {
            List<int> lintReturn = new List<int>();
            var getPOSPositions =
                from posRow in POSs
					where posRow.Value.ToLower() == strPOS.ToLower()
                select posRow.Key;

            var getFuzzyPOSPositions = //ie. if strPOS = "NN", then "NNS", "NNP", etc. will also be found
                from posRow in POSs
					where posRow.Value.ToLower().Contains(strPOS.ToLower())
                select posRow.Key;

            foreach (int intPosition in getPOSPositions)
            {
                if (!lintReturn.Contains(intPosition))
                {
                    lintReturn.Add(intPosition);
                }
            }

            return lintReturn;
        }

        public List<int> GetFuzzyPOSPositions(string strPOS)
        {
            //ie. if strPOS = "NN", then "NNS", "NNP", etc. will also be found

            List<int> lintReturn = new List<int>();
            var getFuzzyPOSPositions = 
                from posRow in POSs
                where posRow.Value.Contains(strPOS)
                select posRow.Key;

            foreach (int intPosition in getFuzzyPOSPositions)
            {
                if (!lintReturn.Contains(intPosition))
                {
                    lintReturn.Add(intPosition);
                }
            }

            return lintReturn;
        }

        public List<string> GetPOSPOSs(string strPOS)
        {
            List<string> lstrReturn = new List<string>();

            foreach (string strTag in strPOS.Trim().Split())
            {
                if (!lstrReturn.Contains(strTag))
                {
                    lstrReturn.Add(strTag);
                }
            }

            return lstrReturn;
        }

        public List<int> GetPOSWordIDs(string strPOS, ref Words words)
        {
            List<int> lintReturn = new List<int>();

            foreach (string strWord in GetPOSWords(strPOS, ref words))
            {
                lintReturn.Add(words.GetWordWordID(strWord));
            }

            return lintReturn.OrderBy(a => a).ToList();
        }

        public List<int> GetFuzzyPOSWordIDs(string strPOS, ref Words words)
        {
            List<int> lintReturn = new List<int>();

            foreach (string strWord in GetFuzzyPOSWords(strPOS, ref words))
            {
                lintReturn.Add(words.GetWordWordID(strWord));
            }

            return lintReturn.OrderBy(a => a).ToList();
        }

        public List<string> GetPOSWords(string strPOS, ref Words words)
        {
            List<string> lstrReturn = new List<string>();

            foreach (int intWordPosition in GetPOSPositions(strPOS))
            {
                if (!lstrReturn.Contains(words.PositionWords[intWordPosition]))
                {
                    lstrReturn.Add(words.PositionWords[intWordPosition]);
                }
            }

            return lstrReturn;
        }

        public List<string> GetFuzzyPOSWords(string strPOS, ref Words words)
        {
            List<string> lstrReturn = new List<string>();

            foreach (int intWordPosition in GetFuzzyPOSPositions(strPOS))
            {
                if (!lstrReturn.Contains(words.PositionWords[intWordPosition]))
                {
                    lstrReturn.Add(words.PositionWords[intWordPosition]);
                }
            }

            return lstrReturn;
        }

        public List<int> GetPOSPairPositions(string strPOSFirst, string strPOSSecond)
        {
            List<int> lReturn = new List<int>();

			foreach (int intPOSFirstUWID in GetPOSPositions(strPOSFirst))
            {
				if (GetPOSPositions(strPOSSecond).Contains(intPOSFirstUWID + 1)){
                    lReturn.Add(intPOSFirstUWID);
                }
            }

            return lReturn;
        }

		public List<int> GetPOSPhrasePositions(string strPOSPhrase)
		{
			List<int> lReturn = new List<int>();
			int intLength = strPOSPhrase.Trim ().Split ().Count ();
			int intPOSCounter = 0;

			foreach (string strPOS in strPOSPhrase.Trim().ToLower().Split()) {
				intPOSCounter++;

				foreach (int intPOSUWID in GetPOSPositions(strPOS)) {
					if (intPOSCounter < intLength){
						string strNextPOS = strPOSPhrase.Trim ().ToLower ().Split () [intLength - 1];

						if (GetPOSPositions (strNextPOS).Contains (intPOSUWID + 1)) {
							if (!lReturn.Contains (intPOSUWID)) {
								lReturn.Add (intPOSUWID);
							}
						}
					}
				}
			}

			return lReturn;
		}
    }
}
