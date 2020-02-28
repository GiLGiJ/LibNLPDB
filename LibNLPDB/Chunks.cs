using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using LibNLPDB.Parser;
using OpenNLP.Tools.Chunker;

namespace LibNLPDB
{
	public class Chunk
	{
		public string strChunkType = ""; //ie. "NP"
		public List<string> lstrValues = new List<string> (); //bag of words of the chunk
        public Dictionary<int, Dictionary<string, string>> dChunkPhrase = new Dictionary<int, Dictionary<string, string>>(); //chunk phrase D<uwid, D<word, pos>>
		public List<Chunk> lchnkContains = new List<Chunk> ();
	}

	public class Chunks
    {
		public Dictionary<int, List<Context>> dContexts = new Dictionary<int, List<Context>>(); //D<sentence id, L<context>>
		public Dictionary<int, NN> dThings = new Dictionary<int, NN>();
		public Dictionary<int, PP> dPPs = new Dictionary<int, PP> ();
        private string mModelPath = "";
        private Dictionary<int, string> dChunks = new Dictionary<int, string>();
        private Dictionary<int, Dictionary<int, Chunk>> dChunksOrdered = new Dictionary<int, Dictionary<int, Chunk>>(); //D<SentenceID, D<1-based chunk id, chunk>>

        public Dictionary<int, string> ChunkData
        {
            get
            {
                return dChunks;

            }
            set
            {
                dChunks = value;
            }
        }

		public Chunks(string strModelPath)
        {
            mModelPath = strModelPath;
        }

		public string PPToJJ(string strPP)
		{
			string[] strsPP = strPP.Trim ().Split (" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

			return strsPP.Last() + "ly";
		}

        public string JJNNToNNPP(string strJJ, string strNN)
        {
            //Create a transformation routine for JJ NN -> NN PP
            //NOTE: move from Chunks if a better place is found

            string strReturn = "";

            if (strJJ.EndsWith("ly")) //if strJJ needs to be converted to a noun
            {
                strReturn = strNN + " IN the " + strJJ.Remove(strJJ.Length - 3, 2); //TODO: get the sense of the particular preposition from context
            }

            return strReturn;
        }

		public string NNPPToJJNN(string strNN, string strPP) 
        {
            //Create a transformation routine for NN PP -> JJ NN
            //NOTE: move from Chunks if a better place is found
			return PPToJJ(strPP) + " " + strNN;
        }

        public bool LoadChunks(ref Input input)
        {
            string strChunks = input.InsertStringIntoFilename("-Chunks");
            bool bReturn = false;

            dChunks.Clear();

            if (File.Exists(strChunks))
            {
                StreamReader srChunks = new StreamReader(strChunks);
                int intSentenceID = -1;
                string strLine = "";
                string strTag = "";

                while (!srChunks.EndOfStream)
                {
                    strLine = srChunks.ReadLine();

                    if (Regex.IsMatch(@"[0-9]{1,}", strLine.Split('^')[0].Trim()))
                    {
                        intSentenceID = Convert.ToInt32(strLine.Split('^')[0].Trim());
                        strTag = strLine.Split('^')[1].Trim();

                    }
                    else
                    {
                        //intSentenceID remains what it was before
                        //strTag is found in index 0
                        strTag = strLine.Split('^')[0].Trim();

                    }

                    if (!dChunks.ContainsKey(intSentenceID))
                    {
                        dChunks.Add(intSentenceID, strTag);
                    }
                }

                srChunks.Close();

                bReturn = true;
            }

            return bReturn;
        }

        public void CreateChunksOpenNLP(ref Input input, ref Words words, ref POS pOS)
        {
            StringBuilder output = new StringBuilder();
            //EnglishTreebankChunker mChunker = new EnglishTreebankChunker(mModelPath + "EnglishChunk.nbin");
            int intCurrentPosition = 0;
            int intMaximumWordPositions = words.PositionWords.Count();

            dChunks.Clear();

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
                        if (intWordPosition <= words.PositionWords.Max(a => a.Key))
                        {
                            string strWord = words.PositionWords[intWordPosition];
                            string strPOS = "";

                            intTokenCounter++;
                            intCurrentPosition++;

                            strPOS = words.GetPositionPOS(intCurrentPosition, ref pOS);

                            tokens.Add(intTokenCounter, strWord);
                            tags.Add(intTokenCounter, strPOS);
                        }
                    }
                }

                ////TODO: clean strChunk - move chunk boundaries - ie. [pp [IN at] [NN daybreak]] to include obj of prep with pp; ie. [np [dt the] [jj big] [nn thing]] [vp [vb went]] -> [vp [np [dt the] [jj big] [nn thing]] [vb went]]
                //string strChunk = mChunker.GetChunks(strsTokens, strsTags);
                string strChunk = "";
                int intLastWordNumber = tokens.Count;

                foreach (int intWord1 in tokens.Keys)
                {
                    string strWord1 = tokens[intWord1];
                    string strPOS1 = tags[intWord1];
                    Dictionary<string, float> dExpectNextPOS = new Dictionary<string,float>();
                    Dictionary<string, float> dExpectPreviousPOS = new Dictionary<string,float>();

                    switch (strPOS1)
                    {
                        default:
                            Console.WriteLine("Error: " + strPOS1);
                            break;
                        case "DT":
                            if (!dExpectNextPOS.ContainsKey("JJ"))
                            {
                                dExpectNextPOS.Add("JJ", 0.333f);
                            }
                            else
                            {
                                dExpectNextPOS["JJ"] *= 1.1f;
                            }
                            if (!dExpectNextPOS.ContainsKey("NN")){
                                dExpectNextPOS.Add("NN",0.667f);
                            }
                            else
                            {
                                dExpectNextPOS["NN"] *= 1.1f;
                            }
                            break;
                        case "CC":
                            if (!dExpectNextPOS.ContainsKey("JJ"))
                            {
                                dExpectNextPOS.Add("JJ", 0.333f);
                            }
                            else
                            {
                                dExpectNextPOS["JJ"] *= 1.1f;
                            }
                            if (!dExpectNextPOS.ContainsKey("NN"))
                            {
                                dExpectNextPOS.Add("NN", 0.333f);
                            }
                            else
                            {
                                dExpectNextPOS["NN"] *= 1.1f;
                            }
                            if (!dExpectNextPOS.ContainsKey("DT"))
                            {
                                dExpectNextPOS.Add("DT", 0.2f);
                            }
                            else
                            {
                                dExpectNextPOS["DT"] *= 1.1f;
                            }
                            break;
                        case "JJ":
                            if (!dExpectNextPOS.ContainsKey("JJ"))
                            {
                                dExpectNextPOS.Add("JJ", 0.1f);
                            }
                            else
                            {
                                dExpectNextPOS["JJ"] *= 1.1f;
                            }
                            if (!dExpectNextPOS.ContainsKey("NN"))
                            {
                                dExpectNextPOS.Add("NN", 0.9f);
                            }
                            else
                            {
                                dExpectNextPOS["NN"] *= 1.1f;
                            }
                            break;
                        case "NN":
                            if (!dExpectNextPOS.ContainsKey("IN"))
                            {
                                dExpectNextPOS.Add("IN", 0.333f);
                            }
                            else
                            {
                                dExpectNextPOS["IN"] *= 1.1f;
                            }
                            if (!dExpectNextPOS.ContainsKey("NN"))
                            {
                                dExpectNextPOS.Add("NN", 0.1f);
                            }
                            else
                            {
                                dExpectNextPOS["NN"] *= 1.1f;
                            }
                            if (!dExpectNextPOS.ContainsKey("CC"))
                            {
                                dExpectNextPOS.Add("CC", 0.5f);
                            }
                            else
                            {
                                dExpectNextPOS["CC"] *= 1.1f;
                            }
                            if (!dExpectNextPOS.ContainsKey("DT"))
                            {
                                dExpectNextPOS.Add("DT", 0.4f);
                            }
                            else
                            {
                                dExpectNextPOS["DT"] *= 1.1f;
                            }
                            break;
                        case "IN":
                            if (!dExpectNextPOS.ContainsKey("IN"))
                            {
                                dExpectNextPOS.Add("IN", 0.01f);
                            }
                            else
                            {
                                dExpectNextPOS["IN"] *= 1.1f;
                            }
                            if (!dExpectNextPOS.ContainsKey("NN"))
                            {
                                dExpectNextPOS.Add("NN", 0.4f);
                            }
                            else
                            {
                                dExpectNextPOS["NN"] *= 1.1f;
                            }
                            if (!dExpectNextPOS.ContainsKey("CC"))
                            {
                                dExpectNextPOS.Add("CC", 0.05f);
                            }
                            else
                            {
                                dExpectNextPOS["CC"] *= 1.1f;
                            }
                            if (!dExpectNextPOS.ContainsKey("DT"))
                            {
                                dExpectNextPOS.Add("DT", 0.4f);
                            }
                            else
                            {
                                dExpectNextPOS["DT"] *= 1.1f;
                            }
                            break;
                    }

                    foreach (string strNextPOS in dExpectNextPOS.Keys)
                    {
						strChunk += strPOS1 + " ^ " + strNextPOS + "\r\n"; // + " ^ " + dExpectNextPOS[strNextPOS].ToString() + "\r\n";
                    }
                }
                
                dChunks.Add(intSentenceID, strChunk.Trim(" \r\n".ToCharArray()));
            } 
            
            StreamWriter swChunks = new StreamWriter(input.InsertStringIntoFilename("-Chunks"));
            
            foreach (int intSentenceID in dChunks.Keys.OrderBy(a => a))
            {
                swChunks.WriteLine(intSentenceID.ToString() + " ^ " + dChunks[intSentenceID]);
            }

            swChunks.Close();
        }

        public void FillOrderedChunks(ref Words words)
        {
            Rgxs r = new Rgxs();

            dChunksOrdered.Clear();

            foreach (int intSentenceID in dChunks.Keys.OrderBy(a => a))
            {
                int intCurrentUWID = words.SentenceFirstPositions[intSentenceID] - 1;

                dChunksOrdered.Add(intSentenceID, new Dictionary<int, Chunk>());

                foreach (Match mtchChunk in r.rgxChunks.Matches(dChunks[intSentenceID]))
                {
                    string strChunk = mtchChunk.Groups["chunk"].Value;
                    string[] strsChunks = strChunk.Split();
                    string strChunkLabel = strsChunks[0].ToLower();

                    dChunksOrdered[intSentenceID].Add(dChunksOrdered[intSentenceID].Count() + 1, new Chunk());
                    dChunksOrdered[intSentenceID][dChunksOrdered[intSentenceID].Count()].strChunkType = strChunkLabel;

                    if (r.rgxChunks.IsMatch(strChunk))
                    {
                        //nested chunk
                        throw new Exception("Nested Chunk Detected; Needs More Code!");
                    }

                    foreach (string strChunkPart in strsChunks)
                    {
                        if (strChunkPart.Contains("/")) //it's a chunk part alright - word/pos
                        {
                            string strWord = "";
                            string strPOS = "";

                            intCurrentUWID++;
                            strWord = strChunkPart.Split('/')[0].Trim();
                            strPOS = strChunkPart.Split('/')[1].Trim();

                            dChunksOrdered[intSentenceID][dChunksOrdered[intSentenceID].Count()].dChunkPhrase.Add(intCurrentUWID, new Dictionary<string, string>());
                            dChunksOrdered[intSentenceID][dChunksOrdered[intSentenceID].Count()].dChunkPhrase[intCurrentUWID].Add(strWord, strPOS);
                        }
                    }
                }
            }
        }

        public void CreateChunksFlat(ref Input input, ref Words words)
        {
            FillOrderedChunks(ref words);
            StreamWriter swChunksFlat = new StreamWriter(input.InsertStringIntoFilename("-ChunksFlat"));
            //Guid uid = new Guid();
            
            foreach (int intSentenceID in dChunksOrdered.Keys.OrderBy(a => a))
            {
                foreach(int intSentenceChunkID in dChunksOrdered[intSentenceID].Keys.OrderBy(a=>a))
                {
                    string strChunkLabel = dChunksOrdered[intSentenceID][intSentenceChunkID].strChunkType;
                    string strChunkText = "";
                    Dictionary<int, Dictionary<string, string>> dChunkRecord = new Dictionary<int, Dictionary<string, string>>();
                    
                    foreach (int intUWID in dChunksOrdered[intSentenceID][intSentenceChunkID].dChunkPhrase.Keys.OrderBy(a => a))
                    {
                        string strWord = dChunksOrdered[intSentenceID][intSentenceChunkID].dChunkPhrase[intUWID].Keys.First();
                        string strPOS = dChunksOrdered[intSentenceID][intSentenceChunkID].dChunkPhrase[intUWID][strWord];

                        strChunkText += strWord + " ";
                        dChunkRecord.Add(intUWID, new Dictionary<string, string>());
                        dChunkRecord[intUWID].Add("Word", strWord);
                        dChunkRecord[intUWID].Add("POS", strPOS);
                    }

                    strChunkText = strChunkText.Trim();

                    foreach (int intUWID in dChunkRecord.Keys.OrderBy(a => a))
                    {
                        swChunksFlat.WriteLine(dChunkRecord[intUWID]["Word"] + " ^ " + dChunkRecord[intUWID]["POS"] + " ^ " + 
                            strChunkText + " ^ " + strChunkLabel + " ^ " + words.GetPositionWordID(intUWID).ToString() + " ^ " + 
                            intSentenceID.ToString() + " ^ " + intSentenceChunkID.ToString() + " ^ " + intUWID.ToString());
                    }
                }
            }

            swChunksFlat.Close();
        }

        //public string CreateChunksCoreferents(ref Words wordsCurrent)
        //{
        //    //int intSentenceID = 0;
        //    //List<Parse> lprsSentences = new List<Parse>();
            
        //    if (mCoreferenceFinder == null)
        //    {
        //        mCoreferenceFinder = new OpenNLP.Tools.Lang.English.TreebankLinker(mModelPath + "coref");
        //    }

        //    System.Collections.Generic.List<OpenNLP.Tools.Parser.Parse> lprsSentences = new System.Collections.Generic.List<OpenNLP.Tools.Parser.Parse>();

        //    //foreach (string sentence in slSentences.Values)
        //    //{
        //    //    intSentenceID++;

        //    //    OpenNLP.Tools.Parser.Parse sentenceParse = ParseSentence(sentence);
        //    //    //string findNames = FindNames(sentenceParse);
        //    //    parsedSentences.Add(sentenceParse);
        //    //}
            
        //    for (int intWordPosition = 1; intWordPosition <= wordsCurrent.SentenceList.Count; intWordPosition++)
        //    {
        //        Parse prsTemp = ParseSentence(wordsCurrent.SentenceList[intWordPosition]);
        //        lprsSentences.Add(prsTemp);
        //    }

        //    return mCoreferenceFinder.GetCoreferenceParse(lprsSentences.ToArray());
        //}

        //private OpenNLP.Tools.Parser.Parse ParseSentence(string sentence)
        //{
        //    if (mParser == null)
        //    {
        //        mParser = new OpenNLP.Tools.Parser.EnglishTreebankParser(mModelPath, true, false);
        //    }

        //    return mParser.DoParse(sentence);
        //}
    }
}
