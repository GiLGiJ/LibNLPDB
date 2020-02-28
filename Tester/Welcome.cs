using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using LibNLPDB;
//using ThreeDichotomies;
//using numl;
using SharpEntropy;
using OpenNLP.Tools.PosTagger;

namespace NLPDB
{
    public partial class Welcome : Form
    {
        //Thing("word"){contexts[1..14543]}  ///This is the index of Context instances that include "word"
        //Context("subject", "verb", "object"){clauses[1..2359340], thingSub.clauseContextSearch["object"].clauseContextSearch["verb"]}
		//string strModelPath = "e:/programming/LibNLPDB/LibNLPDB/NBin Models 1.0.0 Production/";
		string strModelPath = "/media/jeremy/Backups/Programming/LibNLPDB/LibNLPDB/NBin Models 1.0.0 Production/";
		DataStore dsrMain = new DataStore();
        DataRecord drcMain;
        string strFilename = "";
        CurrentInfo ciInfo;
        //Properties.Settings stgCurrent = new Properties.Settings();
        //NLPDataSet dsMain = new NLPDataSet();
        //NLPDBDS dsNLP = new NLPDBDS();
        //NLPDBDSTableAdapters.TableAdapterManager tamNLP = new NLPDBDSTableAdapters.TableAdapterManager();
        bool bPOSJustCreated = false;  //helper for the automatic creation/load button
        bool bReloadChunks = false;

        private void Welcome_Load(object sender, EventArgs e){}

        public Welcome()
        {
			InitializeComponent();

			//Writings ("/media/jeremy/Backups/AAACreated/Writings/");
			//CompareAll ("/media/jeremy/Backups/Programming/Corpora/NLPData");
//          AutoLoadText();
//			AutoTokenizeDirectory("/home/jeremy/programming/MRD");
//			Words.CombineCounts("/home/jeremy/programming/MRD/NLPData");
//
//			foreach (string strFilenameInput in Directory.GetFiles("/home/jeremy/kjv/OT")) {
//				DataRecord drdNew = new DataRecord ();
//
//				strFilename = strFilenameInput;
//
//				drdNew.libInput = new Input (strFilename);
//
//				//Only have one record at a time in the DataStore
//				dsrMain.ldrMain.Clear ();
//
//				dsrMain.Add (drdNew);
//
//				tokenizeToolStripMenuItem_Click (null, null);
//				phrasalConcordanceToolStripMenuItem_Click (null, null);
//				reducePhrasesToolStripMenuItem_Click (null, null);
//				phrasalCompositionToolStripMenuItem_Click (null, null);
//				pOSTagToolStripMenuItem_Click (null, null);
//				combinePOSTagsToolStripMenuItem_Click (null, null);
//				pOSPhrasesToolStripMenuItem_Click (null, null);
//				pOSPairsToolStripMenuItem_Click (null, null);
//				enrichToolStripMenuItem_Click (null, null);
//			}
//
//			Words.CombineCounts("/home/jeremy/kjv/OT");
//			RichWords.CombineRichWords("/home/jeremy/kjv/OT");
//			RichWords.CombineRichWords("/home/jeremy/kjv/NT");
//			AutoLoadText ("/home/jeremy/programming/BBE-Romans.txt");
//			AutoTokenizeDirectory("/home/jeremy/shakespeare/comedies");
//			AutoTokenizeDirectory("/home/jeremy/shakespeare/histories");
//			AutoTokenizeDirectory("/home/jeremy/shakespeare/poetry");
//			AutoTokenizeDirectory("/home/jeremy/shakespeare/tragedies");
        }


		public class Comp
		{
			public string strDocumentName;
			public string strWord;
			public int intRank;
			public int intCount;
			public string Output()
			{
				return strDocumentName + " " + strWord + " " + intRank.ToString() + " " + intCount.ToString();
			}
		}

		private void Writings(string strDir){
			Dictionary<string, int> dNounCounts = new Dictionary<string, int> (); //D<noun/adj/rb, count>
			Dictionary<string, int> dVerbCounts = new Dictionary<string, int> (); //D<verb/adv/prep, count>
			Dictionary<string, int> dOtherCounts = new Dictionary<string, int>(); //D<other, count>
			Dictionary<int, string[]> d = new Dictionary<int, string[]> ();
			EnglishMaximumEntropyPosTagger mPosTagger = new EnglishMaximumEntropyPosTagger(strModelPath + "EnglishPOS.nbin", strModelPath + "Parser/tagdict");

			//D<count-based and refereed rank, ["filename", "heaviest ^ words"

			//clear the main DataStore
			dsrMain = new DataStore();

			foreach (string strFilename in Directory.GetFiles(strDir)) {
				string strFilenameBase = strFilename.Remove (0, strFilename.LastIndexOf ('/') + 1);
				int intLastDotIndex = 0;

				try{
				 	intLastDotIndex = strFilenameBase.LastIndexOf ('.');
					strFilenameBase = strFilenameBase.Remove (intLastDotIndex, strFilenameBase.Length - intLastDotIndex);
				}catch(Exception ex){
				}

				strFilenameBase = strFilenameBase.Trim ((char[])@"[^A-Za-z]".ToCharArray ()).Trim();
				strFilenameBase = strFilenameBase.Replace ('-', ' ');
				strFilenameBase = strFilenameBase.Replace ('_', ' ');
				strFilenameBase = strFilenameBase.Replace ('.', ' ');

				string[] tags = mPosTagger.TagSentence(strFilenameBase).Split();

				for (int intX = 0; intX < tags.Length; intX++) {
					//Console.WriteLine ("==>" + tags [intX]);
					string strWord = tags[intX].Split('/')[0].ToUpper();
					string strTag = tags [intX].Split ('/') [1].ToUpper();
						
					if (strTag.Contains ("NN") || strTag.Contains ("PRP") || strTag.Contains ("RB") || strTag.Contains("JJ")) {
						if (!dNounCounts.ContainsKey (strWord)) {
							dNounCounts.Add (strWord, 0);
						}
						dNounCounts [strWord]++;
					} else if (strTag.Contains ("VB") || strTag.Contains ("IN")) {
						if (!dVerbCounts.ContainsKey (strWord)) {
							dVerbCounts.Add (strWord, 0);
						}
						dVerbCounts [strWord]++;
					} else if (strTag.Contains ("DT") || strTag.Contains ("CC")) {
					} else {
						if (!dOtherCounts.ContainsKey (strWord)) {
							dOtherCounts.Add (strWord, 0);
						}
						dOtherCounts [strWord]++;
					}

				}

				tags = mPosTagger.TagSentence(File.ReadAllText(strFilename)).Split();

				for (int intX = 0; intX < tags.Length; intX++) {
					//Console.WriteLine ("==>" + tags [intX]);
					string strWord = Regex.Match(tags[intX].Split('/')[0].Trim().ToUpper(), @"([^A-Za-z0-9']{1,})").Groups[0].Value;
					string strTag = tags [intX].Split ('/') [1].ToUpper();

					if (strTag.Contains ("NN") || strTag.Contains ("PRP") || strTag.Contains ("RB") || strTag.Contains("JJ")) {
						if (!dNounCounts.ContainsKey (strWord)) {
							dNounCounts.Add (strWord, 0);
						}
						dNounCounts [strWord]++;
					} else if (strTag.Contains ("VB") || strTag.Contains ("IN") || strTag.Contains("TO")) {
						if (!dVerbCounts.ContainsKey (strWord)) {
							dVerbCounts.Add (strWord, 0);
						}
						dVerbCounts [strWord]++;
					} else if (strTag.Contains ("DT") || strTag.Contains ("CC")) { //throw these away
					} else {
						if (!dOtherCounts.ContainsKey (strWord)) {
							dOtherCounts.Add (strWord, 0);
						}
						dOtherCounts [strWord]++;
					}

				}
			}

			StreamWriter sw = new StreamWriter ("/home/jeremy/writings-Counts.txt");

			foreach (string strWord in dNounCounts.OrderByDescending(a=>a.Value).Select(a=>a.Key)) {
				sw.WriteLine (strWord + " ^N " + (dNounCounts [strWord]).ToString ());
			}

			foreach (string strWord in dVerbCounts.OrderByDescending(a=>a.Value).Select(a=>a.Key)) {
				sw.WriteLine (strWord + " ^V " + (dVerbCounts [strWord]).ToString ());
			}

			foreach (string strWord in dOtherCounts.OrderByDescending(a=>a.Value).Select(a=>a.Key)) {
				sw.WriteLine (strWord + " ^O " + (dOtherCounts [strWord]).ToString ());
			}

			sw.Close ();
		}

		private void CompareAll(string strNLPDataDirectory){ //Under Construction...
			Dictionary<string, Dictionary<string, Dictionary<int, int>>> dDocumentProfiles =
				new Dictionary<string, Dictionary<string, Dictionary<int, int>>> (); //D<DocumentName, D<word, D<rank, count>>>
			Dictionary<string, List<Comp>> d = new Dictionary<string, List<Comp>>();

			foreach (string strSubdirectoryName in Directory.GetDirectories(strNLPDataDirectory)) {
				foreach (string strCountFilename in Directory.GetFiles (strSubdirectoryName)) {
					if (strCountFilename.EndsWith ("-Counts.txt")) {
						if (!dDocumentProfiles.ContainsKey (strSubdirectoryName)) { //if more than one counts.txt is found
							dDocumentProfiles.Add (strSubdirectoryName, new Dictionary<string, Dictionary<int, int>> ());
							int intRank = 0;

							foreach (string strLine in File.ReadAllLines(strCountFilename)) {
								string strWord = strLine.Split ('^') [0].Trim ();
								int intCount = Convert.ToInt32 (strWord = strLine.Split ('^') [1].Trim ());

								intRank = intRank + 1;

//								if (!d.ContainsKey (strWord)) {
//									d.Add (strWord, new List<Comp> ());
//								}

//								Comp c = new Comp ();
//
//								c.strDocumentName = strSubdirectoryName;
//								c.strWord = strWord;
//								c.intRank = intRank;
//								c.intCount = intCount;
								
								//d [strWord].Add (c);
								dDocumentProfiles [strSubdirectoryName].Add (strWord, new Dictionary<int, int> ());
								dDocumentProfiles [strSubdirectoryName] [strWord].Add (intRank + 1, intCount);
							}
						}
					}
				}
			}

			//d["the"].ForEach(a=>Console.WriteLine(a.Output()));
			//Console.WriteLine(
		}

		private void AutoTokenizeDirectory(string strDirectory){
			foreach (string strFilenameInput in Directory.GetFiles(strDirectory)) {
				DataRecord drdNew = new DataRecord ();

				strFilename = strFilenameInput;

				drdNew.libInput = new Input (strFilename);

				//Only have one record at a time in the DataStore
				dsrMain.ldrMain.Clear ();

				dsrMain.Add (drdNew);

				tokenizeToolStripMenuItem_Click (null, null);
			}
		}

		private void AutoLoadText(string strFilenameInput){
			DataRecord drdNew = new DataRecord();

			strFilename = strFilenameInput;

			drdNew.libInput = new Input(strFilename);

			//Only have one record at a time in the DataStore
			dsrMain.ldrMain.Clear();

			dsrMain.Add(drdNew);

			ciInfo = new CurrentInfo(ref drdNew);

			tokenizeToolStripMenuItem_Click(null, null);
			phrasalConcordanceToolStripMenuItem_Click(null, null);
			reducePhrasesToolStripMenuItem_Click(null, null);
			phrasalCompositionToolStripMenuItem_Click(null, null);
			pOSTagToolStripMenuItem_Click(null, null);
            //handPOSTagToolStripMenuItem_Click(null, null);
            combinePOSTagsToolStripMenuItem_Click (null, null);
			pOSPhrasesToolStripMenuItem_Click(null, null);
			pOSPairsToolStripMenuItem_Click(null, null);
			enrichToolStripMenuItem_Click(null, null);
			propertyRelatedNounsToolStripMenuItem_Click(null, null);
			makeContextsToolStripMenuItem_Click(null, null);

			//textToClasses ();

        }

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ofdlgCurrent.ShowDialog() == DialogResult.OK)
            {
                DataRecord drdNew = new DataRecord();

                strFilename = ofdlgCurrent.FileName; //strFilename = @"F:\Programming\NLP Data\Bible\KJV\Ruth.txt";
                drdNew.libInput = new Input(strFilename);
                
                //Only have one record at a time in the DataStore
                dsrMain.ldrMain.Clear();
                dsrMain.Add(drdNew);

                //
                drcMain = dsrMain.GetRecord(strFilename);

                ciInfo = new CurrentInfo(ref drdNew);

                //

                //PerCharacter pcMain = new PerCharacter();

                //pcMain.Step1(drdNew.libInput.Filename);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void currentInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataRecord drdInfo = dsrMain.GetRecord(strFilename);

            ciInfo = new CurrentInfo(ref drdInfo);
            ciInfo.ShowDialog();
        }

        private void gOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FixPOS fpTemp = new FixPOS(ref dsrMain);

            tokenizeToolStripMenuItem_Click(null, null);
            phrasalConcordanceToolStripMenuItem_Click(null, null);
            reducePhrasesToolStripMenuItem_Click(null, null);
            phrasalCompositionToolStripMenuItem_Click(null, null);
            pOSTagToolStripMenuItem_Click(null, null);

            //if (bPOSJustCreated)  //only runs the names replace function if the pos was just created
            //{
            //    fpTemp.btnNames_Click(null, null);
            //}

            combinePOSTagsToolStripMenuItem_Click(null, null);
            pOSPhrasesToolStripMenuItem_Click(null, null);
            parseToolStripMenuItem_Click(null, null);
            createParsePartsToolStripMenuItem_Click(null, null);
            buildParseTreeToolStripMenuItem_Click(null, null);
            chunkToolStripMenuItem_Click(null, null);
            pOSPairsToolStripMenuItem_Click(null, null);
            enrichToolStripMenuItem_Click(null, null);
            propertyRelatedNounsToolStripMenuItem_Click(null, null);
                        
            ////foreach char
            ////  is word end? .. new word [words follow words in phrases]
            ////  is phrase end? .. new phrase [phrases follow phrases in sentences; pos tag words; characterize phrase]
            ////  is sentence end? .. new sentence [sentences follow sentences in ideas; characterize sentence (ie. declarative, interrogative)]
            ////  is idea end? .. new idea [ideas can contain other ideas in a hierarchy; characterize idea (eg. clause interactions)]
            ////  is hierarchy (or domain) end? .. new hierarchy (or domain) [hierarchies mix their particular elements in many different ways; this is the play field for creative thought/visualization/etc.]
            //if (drcMain.libInput != null)
            //{
            //    drcMain.libWords = new Words();

            //    if (!drcMain.libWords.LoadWordIDs(ref drcMain.libInput))
            //    {
            //        drcMain.libWords.CreateWordIDs(ref drcMain.libInput);

            //        if (!drcMain.libWords.LoadWordPositions(ref drcMain.libInput))
            //        {
            //            drcMain.libWords.CreateWordPositions(ref drcMain.libInput);

            //            if (!drcMain.libWords.LoadCounts(ref drcMain.libInput))
            //            {
            //                drcMain.libWords.CreateCounts(ref drcMain.libInput);
            //            }
            //        }
            //    }

            //    //if (!drcMain.
            //}
            //else
            //{
            //    //mbox warning
            //}
        }

        private void tokenizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (drcMain.libInput != null)
            { 
                drcMain.libWords = new Words();

                //
                if (!drcMain.libWords.LoadWordPositions(ref drcMain.libInput,ref drcMain.libWords, ref drcMain.libPOS))
                {
                    drcMain.libWords.CreateWordPositions(ref drcMain.libInput, ref drcMain.libWords, ref drcMain.libPOS);
                }

                if (!drcMain.libWords.LoadWordIDs(ref drcMain.libInput))
                {
                    drcMain.libWords.CreateWordIDs(ref drcMain.libInput);
                }

                if (!drcMain.libWords.LoadCounts(ref drcMain.libInput))
                {
                    drcMain.libWords.CreateCounts(ref drcMain.libInput);
                }

                //Make .arff output file
                foreach (int intWordID in drcMain.libWords.WordIDs.Values)
                {

                }

				if (ciInfo == null) {
					DataRecord drdInfo = dsrMain.GetRecord(strFilename);

					ciInfo = new CurrentInfo(ref drdInfo);
				}

                //Update ciInfo
                if (drcMain.libWords.WordIDs.Count() > 0)
                {
                    ciInfo.ChangeWordIDs(drcMain.libWords.WordIDs.Count().ToString());
                }

                if (drcMain.libWords.PositionWords.Count() > 0)
                {
                    ciInfo.ChangeWordPositions(drcMain.libWords.PositionWords.Count().ToString());
                }

                if (drcMain.libWords.Counts.Count() > 0)
                {
                    ciInfo.ChangeHighestWordCount(drcMain.libWords.Counts.Max(a => a.Value).ToString());
                }

                if (drcMain.libWords.SentenceList.Count() > 0)
                {
                    ciInfo.ChangeSentenceCount(drcMain.libWords.SentenceList.Count().ToString());
                }
            }
            else
            {
                MessageBox.Show("Open a Text File First");
            }
        }

        private void phrasalConcordanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (drcMain.libInput != null && drcMain.libWords != null)
            {
                drcMain.libPhrases = new Phrases();

                if (!drcMain.libPhrases.LoadPhraseCounts(ref drcMain.libInput))
                {
                    drcMain.libPhrases.CreatePhrases(ref drcMain.libInput, ref drcMain.libWords);
                }
                else
                {   
                    for (int intPhraseLength = 1; intPhraseLength <= drcMain.libPhrases.GetHighestPhraseLength(ref drcMain.libInput); intPhraseLength++)
                    {
                        drcMain.libPhrases.LoadFirstWordIDs(ref drcMain.libInput, intPhraseLength);
                        drcMain.libPhrases.LoadPhraseIDs(ref drcMain.libInput, intPhraseLength);
                        drcMain.libPhrases.LoadPhraseWordIDs(ref drcMain.libInput, intPhraseLength);
                    }
                }

                if (drcMain.libPhrases.PhraseCounts.Count() > 0)
                {
                    ciInfo.ChangeLongestPhrase(drcMain.libPhrases.PhraseCounts.Max(a => a.Key.Split().Count()).ToString());
                }
            }
            else
            {
                MessageBox.Show("Open a Text File and Tokenize First");
            }
        }
        
        private void reducePhrasesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (drcMain.libInput != null && drcMain.libWords != null &&
                drcMain.libInput.PhraseFilesCount() > 0)
            {
                //drcMain.libPhrases = new Phrases();

                drcMain.libPhrases.ReducePhrases(ref drcMain.libInput);
            }
            else
            {
                MessageBox.Show("Open a Text File, Tokenize and Create Phrases First");
            }
        }

        private void phrasalCompositionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (drcMain.libInput != null && drcMain.libWords != null &&
                drcMain.libInput.PhraseFilesCount() > 0)
            {
                PhraseComposition pcTemp = new PhraseComposition(ref drcMain.libPhrases);

                pcTemp.Compose(ref drcMain.libInput, ref drcMain.libWords);
            }
            else
            {
                MessageBox.Show("Open a Text File, Tokenize and Create Phrases First");
            }
        }

        private void pOSTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (drcMain.libInput != null && drcMain.libWords != null)
            {
				drcMain.libPOS = new POS(strModelPath, ref drcMain.libInput, ref drcMain.libWords);

                //if (drcMain.libPOS.POSs.Count() > 0)
                //{
                //    ciInfo.ChangePOSCount(drcMain.libPOS.POSs.Count().ToString());
                //}
            }
            else
            {
                MessageBox.Show("Open a Text File and Tokenize First");
            }
        }

        private void combinePOSTagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (drcMain.libInput != null && drcMain.libWords != null && drcMain.libPOS != null)
            {
                drcMain.libCombinedPOS = new CombinedPOS();

                if (!drcMain.libCombinedPOS.LoadCombinedPOS(ref drcMain.libInput))
                {
                    drcMain.libCombinedPOS.CreateCombinedPOS(ref drcMain.libInput, ref drcMain.libWords, ref drcMain.libPOS);
                }
            }
            else
            {
                MessageBox.Show("Open a Text File and Tokenize First");
            }
        }

        private void pOSPhrasesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (drcMain.libInput != null && drcMain.libWords != null && drcMain.libPhrases != null && drcMain.libPOS != null)
            {
                drcMain.libPOSPhrases = new POSPhrases();

                if (!drcMain.libPOSPhrases.LoadPOSPhrases(ref drcMain.libInput))
                {
                    drcMain.libPOSPhrases.CreatePOSPhrases(ref drcMain.libInput, ref drcMain.libWords,
                        ref drcMain.libPhrases, ref drcMain.libPOS);
                }

                if (drcMain.libPOSPhrases.POSPhrasesD.Count() > 0)
                {
                    ciInfo.ChangeMostFrequentPOSPhrase(drcMain.libPOSPhrases.POSPhrasesD.Max(a => a.Value[0]).ToString());
                }
            }
            else
            {
                MessageBox.Show("Open a Text File, Tokenize, and Create POS and Phrases First");
            }
        }

        private void parseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (drcMain.libInput != null &&
                drcMain.libWords != null &&
                drcMain.libPOS != null)
            {
                dsrMain.GetRecord (strFilename).libParse = new Parse (strModelPath);

                //CHECKME: Can the code yet parse the next meaning in some meaning system, both sentences together?
                //If LoadParse() doesn't work, call CreateParse().  This is a very useful pattern.
                if (!drcMain.libParse.LoadParse(
                                   ref drcMain.libInput))
                {
                    drcMain.libParse.CreateParseOpenNLP(strModelPath, ref drcMain.libInput,
                        ref drcMain.libWords, ref drcMain.libPOS);
                }
            }
            else
            {
                MessageBox.Show("Open a Text File, Tokenize and Create POS First");
            }
        }

        private void chunkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (drcMain.libInput != null &&
                drcMain.libWords != null &&
                drcMain.libPOS != null)
            {
				drcMain.libChunks = new Chunks(strModelPath);

				if (!drcMain.libChunks.LoadChunks(
                    ref drcMain.libInput) || bReloadChunks)
                {
                    //drcMain.libChunks.CreateChunksCoreferents(ref drcMain.libWords);
                    drcMain.libChunks.CreateChunksOpenNLP(ref drcMain.libInput,
                        ref drcMain.libWords, ref drcMain.libPOS);
                }

				//analyze chunks
				drcMain.libChunks.CreateChunksFlat(ref drcMain.libInput, ref drcMain.libWords);

				foreach (LibNLPDB.Parser.NN nn in drcMain.libChunks.dThings.Values) {
					MessageBox.Show(nn.strWord);
				}
            }
            else
            {
                MessageBox.Show("Open a Text File, Tokenize and Create POS First");
            }
        }

        private void deleteAllDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (drcMain.libInput != null)
            {
                string strDataDirectory = drcMain.libInput.DataPath + "/" + drcMain.libInput.Base;

                try
                {
                    Directory.Delete(strDataDirectory, true);
                }
                catch
                {
                    MessageBox.Show("Close programs that are accessing files under the " + strDataDirectory + " directory.");
                }

                drcMain.libInput = null;
                drcMain.libWords = null;
                drcMain.libPOS = null;
                drcMain.libCombinedPOS = null;
                drcMain.libPhrases = null;
                drcMain.libPOSPhrases = null;
                drcMain.libParse = null;
                drcMain.libChunks = null;
            }
            else
            {
                MessageBox.Show("Open a Text File First");
            }
        }

        private void changeModelsFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbdlgModel = new FolderBrowserDialog();

            fbdlgModel.Description = "Choose NBin Models Folder";

            if (fbdlgModel.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //stgCurrent.mModelPath = fbdlgModel.SelectedPath + @"\";
                //stgCurrent.Save();
            }
        }

        private void handChunkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (drcMain.libInput != null &&
                drcMain.libWords != null)
            {
                HandChunk hcCurrent = new HandChunk(ref dsrMain);

                hcCurrent.ShowDialog();
            }
            else
            {
                MessageBox.Show("Open a Text File and Tokenize First");
            }
        }

        private void regexpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (drcMain.libInput != null)
            {
                RegexpSearch r = new RegexpSearch(ref drcMain.libInput);
                r.ShowDialog();
            }
            else
            {
                MessageBox.Show("Open a Text File First");
            }
        }

        private void begatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (drcMain.libWords != null)
            {
                Dictionary<string, List<string>> dBegat = new Dictionary<string, List<string>>();

                foreach (int intBegatUWID in drcMain.libWords.GetWordPositions("begat"))
                {
                    string strParent = drcMain.libWords.GetPositionWord(intBegatUWID - 1);
                    string strChild = drcMain.libWords.GetPositionWord(intBegatUWID + 1);

                    if (!dBegat.ContainsKey(strParent))
                    {
                        dBegat.Add(strParent, new List<string>());
                    }

                    if (!dBegat[strParent].Contains(strChild))
                    {
                        dBegat[strParent].Add(strChild);
                    }
                }
            }
            else
            {
                MessageBox.Show("Open a Text File and Tokenize First");
            }
        }

        private void makeContextsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (drcMain.libInput != null &&
                drcMain.libWords != null &&
                drcMain.libPOS != null)
            {
                MakeContexts mcCurrent = new MakeContexts(
					ref drcMain.libInput,
                	ref drcMain.libWords,
					ref drcMain.libPOSPhrases);
                mcCurrent.ShowDialog();
            }
            else
            {
                MessageBox.Show("Open a Text File, Tokenize and Create POS First");
            }
        }

        private void viewContextsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (drcMain.libInput != null)
            {
                ViewContexts vcCurrent = new ViewContexts(drcMain.libInput.InsertStringIntoFilename("-Contexts"));
                vcCurrent.ShowDialog();
            }
            else
            {
                MessageBox.Show("Open a Text File First");
            }
        }

        private void pOSPairsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (drcMain.libInput != null &&
                drcMain.libWords != null &&
                drcMain.libPOS != null &&
                drcMain.libPOSPhrases != null)
            {
                if (Directory.Exists(drcMain.libInput.GetPOSPairsDirectory()))
                {
                    drcMain.libPOSPhrases.LoadPOSPhrases(ref drcMain.libInput);
                }
                else
                {
                    Directory.CreateDirectory(drcMain.libInput.GetPOSPairsDirectory());

                    StreamWriter swPOSPairCounts = new StreamWriter(drcMain.libInput.GetPOSPairsFilename("Counts"));
                    StringBuilder sbPOSPairCounts = new StringBuilder();

                    foreach (string strPOS1 in drcMain.libPOS.lstrTags.OrderBy(a => a))
                    {
                        foreach (string strPOS2 in drcMain.libPOS.lstrTags.OrderBy(a => a))
                        {
                            try
                            {
                                List<int> lPOSPair = drcMain.libPOS.GetPOSPairPositions(strPOS1, strPOS2);
                                Dictionary<string, int> dPOSPairCount = new Dictionary<string, int>();

                                if (lPOSPair.Count() > 0) //don't create files for nonexistent POS combinations
                                {
                                    string strPOSPairFilename = drcMain.libInput.GetPOSPairsFilename(strPOS1 + "-" + strPOS2);
                                    StreamWriter swPOSPair = new StreamWriter(strPOSPairFilename);
                                    StringBuilder sbPOSPair = new StringBuilder();

                                    foreach (int intFirstUWID in lPOSPair)
                                    {
                                        sbPOSPair.Append(drcMain.libWords.GetPositionWord(intFirstUWID)
                                            + " " + drcMain.libWords.GetPositionWord(intFirstUWID + 1));
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
            }
            else
            {
                MessageBox.Show("Open a Text File, Tokenize and Create POS and POSPhrases First");
            }
        }

        private void dichotomiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (drcMain.libInput != null &&
                drcMain.libWords != null &&
                drcMain.libPOS != null)
            {
    //            ThreeDichotomies.Hierarchy hierarchy = new Hierarchy(ref drcMain.libInput,
    //            	ref drcMain.libWords, ref drcMain.libPOS);
				//ThreeDichotomies.SimilaritiesDifferences sd = new SimilaritiesDifferences ();
				//ThreeDichotomies.Similarity s = new Similarity ();
    //            StreamWriter swModifiers = new StreamWriter(drcMain.libInput.InsertStringIntoFilename("-HierarchyModifiers"));
    //            StreamWriter swNouns = new StreamWriter(drcMain.libInput.InsertStringIntoFilename("-HierarchyNouns"));
    //            StreamWriter swVerbs = new StreamWriter(drcMain.libInput.InsertStringIntoFilename("-HierarchyVerbs"));

    //            foreach (string strModifier in hierarchy.lstrModifiers)
    //            {
    //                swModifiers.WriteLine(strModifier);
    //            }

    //            foreach(string strNoun in hierarchy.lstrNouns)
    //            {
    //                swNouns.WriteLine(strNoun);
    //            }

    //            foreach (string strVerb in hierarchy.lstrVerbs)
    //            {
    //                swVerbs.WriteLine(strVerb);
    //            }

    //            swModifiers.Close();
    //            swNouns.Close();
    //            swVerbs.Close();
            }
            else
            {
                MessageBox.Show("Open a Text File, Tokenize and Create POS First");
            }
        }

        private void handPOSTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (drcMain.libInput != null &&
                drcMain.libWords != null &&
                drcMain.libPOS != null)
            {
                HandPOSTag hptTemp = new HandPOSTag(ref dsrMain, strFilename);

                hptTemp.Show();
            }
            else
            {
                MessageBox.Show("Open a Text File, Tokenize and Create POS First");
            }
        }

        private void outputControllerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataRecord drcCurrent = dsrMain.GetRecord(strFilename);
            OutputController ocCurrent = new OutputController(ref drcCurrent, ref tbxOutput);

            ocCurrent.Show();
        }

        private void enrichToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (drcMain.libInput != null &&
                drcMain.libWords != null &&
                drcMain.libPOS != null)
            {
                drcMain.libRichWords = new RichWords();
                dsrMain.GetRecord(strFilename).libRichWords.CompareModifiers(
                    ref dsrMain.ldrMain[dsrMain.ldrMain.Count() - 1].libWords,
                    ref dsrMain.ldrMain[dsrMain.ldrMain.Count() - 1].libPOS);

                dsrMain.GetRecord (strFilename).libRichWords.Enrich (
					ref dsrMain.ldrMain [dsrMain.ldrMain.Count () - 1].libInput, 
					ref dsrMain.ldrMain [dsrMain.ldrMain.Count () - 1].libWords, 
					ref dsrMain.ldrMain [dsrMain.ldrMain.Count () - 1].libPOS,
					strModelPath);
            }
            else
            {
                MessageBox.Show("Open a Text File, Tokenize and Create POS First");
            }
        }

        private void exportToSQLServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //NLPDBDS dsMain = new NLPDBDS();

            //NLPDBDSTableAdapters.TableAdapterManager tam = new NLPDBDSTableAdapters.TableAdapterManager();
            //tam.WordsTableAdapter = new NLPDBDSTableAdapters.WordsTableAdapter();
            //tam.TextsTableAdapter = new NLPDBDSTableAdapters.TextsTableAdapter();
            //tam.ContextsTableAdapter = new NLPDBDSTableAdapters.ContextsTableAdapter();
            //tam.WordPositionsTableAdapter = new NLPDBDSTableAdapters.WordPositionsTableAdapter();
            //tam.POSPhrasesTableAdapter = new NLPDBDSTableAdapters.POSPhrasesTableAdapter();
            //tam.POSTableAdapter = new NLPDBDSTableAdapters.POSTableAdapter();
            //tam.SentencesTableAdapter = new NLPDBDSTableAdapters.SentencesTableAdapter();
            //tam.CombinedPOSTableAdapter = new NLPDBDSTableAdapters.CombinedPOSTableAdapter();
            //tam.ChunksTableAdapter = new NLPDBDSTableAdapters.ChunksTableAdapter();
            //tam.PhrasesTableAdapter = new NLPDBDSTableAdapters.PhrasesTableAdapter();

            //foreach (DataRecord drcTemp in dsrMain.ldrMain)
            //{
            //    int intTID = 0;

            //    try
            //    {
            //        intTID = dsMain.Texts.Max(a => a.TID) + 1;
            //    }
            //    catch 
            //    {
            //        intTID = 1; 
            //    }

            //    if (dsNLP.Texts.Where(a => a.TID == intTID).Count() == 0)
            //    {
            //        dsNLP.Texts.AddTextsRow(intTID, intTID, drcTemp.libInput.Base, drcTemp.libInput.InputText);

            //        foreach (int intSID in drcTemp.libWords.SentenceList.Keys)
            //        {
            //            if (dsNLP.Sentences.Where(a => a.SID == intSID).Count() == 0)
            //            {
            //                dsNLP.Sentences.AddSentencesRow(dsNLP.Sentences.Rows.Count + 1, dsNLP.Sentences.Rows.Count + 1, drcTemp.libWords.SentenceList[intSID], dsNLP.Texts.Where(a => a.TID == intTID).First());

            //                foreach (int intWID in drcTemp.libWords.WordIDs.Values.Where(a=>a >= drcTemp.libWords.SentenceFirstPositions[intSID] && a <= drcTemp.libWords.SentenceFirstPositions[intSID] + drcTemp.libWords.SentenceLengths[intSID] - 1))
            //                {
            //                    int intPID = intWID;
            //                    string strPOS = drcTemp.libPOS.POSs[intPID]; 

            //                    if (dsNLP.Words.Where(a => a.WID == intWID).Count() == 0)
            //                    {
            //                        dsNLP.Words.AddWordsRow(dsNLP.Words.Rows.Count + 1, intWID, drcTemp.libWords.WordIDs.Where(a => a.Value == intWID).First().Key);
            //                    }

            //                    foreach (int intWPID in (drcTemp.libWords.WordPositions.Where(a => a.Value == dsNLP.Words.Where(b => b.WID == intWID).First().Text).Select(c => c.Key)))
            //                    {
            //                        if (dsNLP.WordPositions.Where(a => a.WPID == intWPID).Count() == 0)
            //                        {
            //                            dsNLP.WordPositions.AddWordPositionsRow(dsNLP.WordPositions.Rows.Count + 1, dsNLP.Words.Where(a => a.WID == intWID).First(), intWPID, intPID, intWPID, dsNLP.Texts.Where(b => b.TID == intTID).First());
            //                        }

            //                        if (dsNLP.POS.Where(a => a.PID == intPID).Count() == 0)
            //                        {
            //                            dsNLP.POS.AddPOSRow(dsNLP.POS.Rows.Count + 1, intPID, strPOS, dsNLP.WordPositions.Where(a => a.WPID == intWPID).First());
            //                        }

            //                        if (dsNLP.CombinedPOS.Where(a => a.WID == intWID).Count() == 0)
            //                        {
            //                            dsNLP.CombinedPOS.AddCombinedPOSRow(dsNLP.CombinedPOS.Rows.Count + 1, dsNLP.CombinedPOS.Rows.Count + 1, dsNLP.Words.Where(a => a.WID == intWID).First(), dsNLP.POS.Where(b => b.PID == intPID).First());
            //                        }
            //                    }
            //                }

            //                foreach (int[] intsPID in drcTemp.libPhrases.PhraseWordsIDs.Values)
            //                {
            //                    int intPID = intsPID[0];

            //                    if (dsNLP.Phrases.Where(a => a.Text == drcTemp.libPhrases.PhraseWordsIDs.Where(b => b.Value.Contains(intPID)).First().Key).Count() == 0)
            //                    {
            //                        dsNLP.Phrases.AddPhrasesRow(dsNLP.Phrases.Rows.Count + 1, intPID, drcTemp.libPhrases.PhraseWordsIDs.Where(a => a.Value.Contains(intPID)).First().Key, dsNLP.Sentences.Where(b => b.SID == intSID).First());
            //                    }
            //                }

            //                foreach (int intCID in drcTemp.libChunks.ChunkData.Keys)
            //                {
            //                    if (dsNLP.Chunks.Where(a => a.CID == intCID).Count() == 0)
            //                    {
            //                        dsNLP.Chunks.AddChunksRow(dsNLP.Chunks.Rows.Count + 1, intCID, drcTemp.libChunks.ChunkData[intCID], dsNLP.Sentences.Where(a => a.SID == intSID).First());
            //                    }
            //                }
            //            }
            //        }

            //        foreach (string strPP in drcTemp.libPOSPhrases.POSPhrasesD.Keys)
            //        {
            //            if (dsNLP.POSPhrases.Where(a => a.Text == strPP).Count() == 0)
            //            {
            //                dsNLP.POSPhrases.AddPOSPhrasesRow(dsNLP.POSPhrases.Rows.Count + 1, dsNLP.POSPhrases.Rows.Count + 1, strPP, dsNLP.Texts.Where(b => b.TID == intTID).First());
            //            }
            //        }
            //    }
            //}

            //tam.WordsTableAdapter.Update(dsNLP.Words);
            //tam.TextsTableAdapter.Update(dsNLP.Texts);
            //tam.ContextsTableAdapter.Update(dsNLP.Contexts);
            //tam.WordPositionsTableAdapter.Update(dsNLP.WordPositions);
            //tam.POSPhrasesTableAdapter.Update(dsNLP.POSPhrases);
            //tam.POSTableAdapter.Update(dsNLP.POS);
            //tam.SentencesTableAdapter.Update(dsNLP.Sentences);
            //tam.CombinedPOSTableAdapter.Update(dsNLP.CombinedPOS);
            //tam.ChunksTableAdapter.Update(dsNLP.Chunks);
            //tam.PhrasesTableAdapter.Update(dsNLP.Phrases);

            //            NLPDBDS dsMain = new NLPDBDS();
            //            NLPDBDSTableAdapters.TableAdapterManager tam = new NLPDBDSTableAdapters.TableAdapterManager();
            //
            //            tam.SentencesTableAdapter = new NLPDBDSTableAdapters.SentencesTableAdapter();
            //            tam.SentenceFirstWordPositionsTableAdapter = new NLPDBDSTableAdapters.SentenceFirstWordPositionsTableAdapter();
            //            tam.SentenceLengthsTableAdapter = new NLPDBDSTableAdapters.SentenceLengthsTableAdapter();
            //            tam.WordIDsTableAdapter = new NLPDBDSTableAdapters.WordIDsTableAdapter();
            //            tam.WordPositionsTableAdapter = new NLPDBDSTableAdapters.WordPositionsTableAdapter();
            //            tam.WordCountsTableAdapter = new NLPDBDSTableAdapters.WordCountsTableAdapter();
            //            tam.ChunksTableAdapter = new NLPDBDSTableAdapters.ChunksTableAdapter();
            //            tam.ParseTableAdapter = new NLPDBDSTableAdapters.ParseTableAdapter();
            //            tam.PhraseIDsTableAdapter = new NLPDBDSTableAdapters.PhraseIDsTableAdapter();
            //            tam.PhraseWordIDsTableAdapter = new NLPDBDSTableAdapters.PhraseWordIDsTableAdapter();
            //            tam.PhraseFirstWordPositionsTableAdapter = new NLPDBDSTableAdapters.PhraseFirstWordPositionsTableAdapter();
            //            tam.PhraseCountsTableAdapter = new NLPDBDSTableAdapters.PhraseCountsTableAdapter();
            //            tam.POSTableAdapter = new NLPDBDSTableAdapters.POSTableAdapter();
            //            tam.POSPairsTableAdapter = new NLPDBDSTableAdapters.POSPairsTableAdapter();
            //            tam.CombinedPOSTableAdapter = new NLPDBDSTableAdapters.CombinedPOSTableAdapter();
            //            tam.ModifiedActionsTableAdapter = new NLPDBDSTableAdapters.ModifiedActionsTableAdapter();
            //            tam.ModifiedThingsTableAdapter = new NLPDBDSTableAdapters.ModifiedThingsTableAdapter();
            //            tam.POSPhrasesTableAdapter = new NLPDBDSTableAdapters.POSPhrasesTableAdapter();
            //
            //            fillNLPDataSet(ref dsMain);
            //            tam.UpdateAll(dsMain);

            //tam.WordIDsTableAdapter.Update(dsMain.WordIDs);
            //tam.ChunksTableAdapter.Update(dsMain.Chunks);
            //tam.PhraseIDsTableAdapter.Update(dsMain.PhraseIDs);
            //tam.POSPhrasesTableAdapter.Update(dsMain.POSPhrases);
        }

//        private void fillNLPDataSet(ref NLPDBDS dsMain)
//        {
//            foreach (DataRecord drcTemp in dsrMain.ldrMain)
//            {
//                //int intTID = 0;
//                string strTitle = drcTemp.libInput.Base;
//                List<string> lstrAddedWordIDRows = new List<string>();
//
//                //try
//                //{
//                //    LibNLPDB.NLPDataSetTableAdapters.QueriesTableAdapter taQueries =
//                //        new LibNLPDB.NLPDataSetTableAdapters.QueriesTableAdapter();
//
//                //    intTID = (int)taQueries.GetMaxTextID() + 1;
//                //}
//                //catch
//                //{
//                //    intTID = 1;
//                //}
//
//                foreach (int intSID in drcTemp.libWords.SentenceList.Keys)
//                {
//                    int intSentenceFirstPosition = drcTemp.libWords.SentenceFirstPositions[intSID];
//                    int intSentenceLength = drcTemp.libWords.SentenceLengths[intSID];
//
//                    dsMain.Sentences.AddSentencesRow(intSID, drcTemp.libWords.SentenceList[intSID]);
//                    dsMain.SentenceFirstWordPositions.AddSentenceFirstWordPositionsRow(intSID, intSentenceFirstPosition);
//                    dsMain.SentenceLengths.AddSentenceLengthsRow(intSID, intSentenceLength);
//
//                    for (int intWPID = intSentenceFirstPosition; intWPID <= intSentenceFirstPosition + intSentenceLength - 1; intWPID++)
//                    {
//                        int intWID = drcTemp.libWords.GetPositionWordID(intWPID);
//                        string strPOS = drcTemp.libPOS.POSs[intWPID];
//                        string strWord = drcTemp.libWords.WordIDs.Where(a => a.Value == intWID).First().Key;
//                        string strCombinedPOS = drcTemp.libCombinedPOS.CombinedPOSs[strWord];
//                        int intWordCount = drcTemp.libWords.Counts[strWord];
//                        
//                        dsMain.WordPositions.AddWordPositionsRow(intWPID, strWord);
//                        dsMain.POS.AddPOSRow(Guid.NewGuid(), strWord, strPOS);
//
//                        if (!lstrAddedWordIDRows.Contains(strWord))
//                        {
//                            dsMain.WordIDs.AddWordIDsRow(strWord, intWID);
//                            dsMain.WordCounts.AddWordCountsRow(strWord, intWordCount);
//
//                            lstrAddedWordIDRows.Add(strWord);
//                        }
//                    }
//                }
//
//                foreach (string strPhrase in drcTemp.libPhrases.PhraseIDs.Keys)
//                {
//                    if (dsMain.PhraseIDs.Where(a => a.Phrase == strPhrase).Count() == 0)
//                    {
//                        dsMain.PhraseIDs.AddPhraseIDsRow(drcTemp.libPhrases.PhraseIDs[strPhrase], strPhrase);
//                    }
//                }
//
//                foreach (string strPhrase in drcTemp.libPhrases.PhraseCounts.Keys)
//                {
//                    if (dsMain.PhraseCounts.Where(a => a.Phrase == strPhrase).Count() == 0)
//                    {
//                        dsMain.PhraseCounts.AddPhraseCountsRow(drcTemp.libPhrases.PhraseIDs[strPhrase], strPhrase, drcTemp.libPhrases.PhraseCounts[strPhrase]);
//                    }
//                }
//
//
//                //foreach (int intCID in drcTemp.libChunks.ChunkData.Keys)
//                //{//TODO: Get FirstWordPositionID for this Chunk; currently set to 1
//                //    if (dsMain.Chunks.Where(a => a.ChunkID == intCID).Count() == 0)
//                //    {
//                //        dsMain.Chunks.AddChunksRow(Guid.NewGuid(), intCID, drcTemp.libChunks.ChunkData[intCID], 1, intTID);
//                //    }
//                //}
//
//                //foreach (string strPP in drcTemp.libPOSPhrases.POSPhrasesD.Keys)
//                //{
//                //    if (dsMain.POSPhrases.Where(a => a.POSPhrase == strPP).Count() == 0)
//                //    {
//                //        dsMain.POSPhrases.AddPOSPhrasesRow(Guid.NewGuid(), dsMain.POSPhrases.Count + 1, strPP, drcTemp.libPOSPhrases.POSPhrasesD[strPP][1], intTID, drcTemp.libPOSPhrases.POSPhrasesD[strPP][0]);
//                //    }
//                //}
//            }
//        }

		private void textToClasses(){
			try{
				dsrMain.ldrMain [0].libPhrases.WordPropertiesToClasses ();
			}catch(Exception ex){
				MessageBox.Show ("Open File, Tokenize, POS Tag and Create Phrases First.");
			}
		}

        private void analyzeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dsrMain.ldrMain[0].libPOSPhrases.Analyze();
        }

        private void fixPOSFromFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FixPOS fpTemp = new FixPOS(ref dsrMain);

            fpTemp.Show();
        }

        private void createParsePartsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (drcMain.libInput != null &&
                drcMain.libWords != null &&
                drcMain.libPOS != null &&
                drcMain.libParse != null)
            {
                if (!drcMain.libParse.LoadParseParts(ref drcMain.libInput))
                {
                    drcMain.libParse.CreateParseParts(ref drcMain.libInput);
                }

                if (!File.Exists(drcMain.libInput.InsertStringIntoFilename("-ParsePartsFlatTree")))
                {
                    drcMain.libParse.CreateParsePartsTree();
                    drcMain.libParse.WriteParsePartsTree(drcMain.libInput.InsertStringIntoFilename("-ParsePartsFlatTree"), drcMain.libInput.InsertStringIntoFilename("-ParsePartsFlatTreeText"), drcMain.libInput.InsertStringIntoFilename("-ParsePartsCount"));
                }

                //
                //WORK ON ME
                //Dictionary<string, string> dParsablePhrases = drcMain.libParse.CompareParsePartsFlatTreeAndPhrasalComposition(ref drcMain.libInput, ref drcMain.libPhrases);
                //
            }
            else
            {
                MessageBox.Show("Open a Text File, Tokenize and Create POS and Parse First");
            }
        }

        private void buildParseTreeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (drcMain.libInput != null &&
                drcMain.libParse != null)
            {
               LibNLPDB.Parser.BuildParseTree bptCurrent = new LibNLPDB.Parser.BuildParseTree(ref drcMain.libInput, ref drcMain.libParse);

               bptCurrent.Parse();
            }
            else
            {
                MessageBox.Show("Open a Text File and Parse First");
            }
        }

        private void propertyRelatedNounsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (drcMain.libRichWords != null)
            {
                GeneralitySimilarity gsTemp = new GeneralitySimilarity(ref dsrMain);

                gsTemp.ShowDialog();
            }
        }

        private void exportToXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ////fillNLPDataSet();

            ////dtXML = new NLPDBDS.ChunksDataTable();
            ////foreach(int intKey in drcMain.libChunks.ChunkData.Keys)
            ////{
            ////    dtXML.Rows.Add(intKey, drcMain.libChunks.ChunkData[intKey]);
            ////}
            ////dtXML.WriteXml(drcMain.libInput.DataPath + "Chunks.xml");
            ////dtXML = new NLPDBDS.CombinedPOSDataTable();
            ////dtXML.WriteXml(drcMain.libInput.DataPath + "CombinedPOS.xml");
            ////dtXML = new NLPDBDS.ContextsDataTable();
            ////dtXML.WriteXml(drcMain.libInput.DataPath + "Contexts.xml");
            ////dtXML = new NLPDBDS.PhrasesDataTable();
            ////dtXML.WriteXml(drcMain.libInput.DataPath + "Phrases.xml");
            ////dtXML = new NLPDBDS.POSDataTable();
            ////dtXML.WriteXml(drcMain.libInput.DataPath + "POS.xml");
            ////dtXML = new NLPDBDS.POSPhrasesDataTable();
            ////dtXML.WriteXml(drcMain.libInput.DataPath + "POSPhrases.xml");
            ////dtXML = new NLPDBDS.SentencesDataTable();
            ////dtXML.WriteXml(drcMain.libInput.DataPath + "Sentences.xml");
            ////dtXML = new NLPDBDS.TextsDataTable();
            ////dtXML.WriteXml(drcMain.libInput.DataPath + "Texts.xml");

            //NLPDBDS.WordsDataTable dtXML = new NLPDBDS.WordsDataTable();

            //foreach (string strWord in drcMain.libWords.WordIDs.Keys)
            //{
            //    int intWordID = drcMain.libWords.WordIDs[strWord];
            //    dtXML.AddWordsRow(dtXML.Rows.Count + 1, intWordID, strWord);
            //}

            //dtXML.WriteXml(drcMain.libInput.DataPath + "Words.xml");

            ////NLPDBDS.WordPositionsDataTable dtWordPositionsXML = new NLPDBDS.WordPositionsDataTable();

            ////foreach (int intPosition in drcMain.libWords.PositionWords.Keys)
            ////{
            ////    int intWordID = drcMain.libWords.WordIDs[strWord];
            ////    dtWordPositionsXML.AddWordPositionsRow(dtWordPositionsXML.Rows.Count + 1, intWordID, strWord);
            ////}

            ////dtXML.WriteXml(drcMain.libInput.DataPath + "Words.xml");

            ////dtXML = 
            ////dtXML.WriteXml(drcMain.libInput.DataPath + "WordPositions.xml");
        }

        private void createSentenceTextFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drcMain.libWords.SentencesObject.CreateSentenceTextFiles(drcMain.libInput.DataPath + "/" + drcMain.libInput.Base);
        }

        private void createWordPOSFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GrammarParser gpTemp = new GrammarParser(ref dsrMain);

            if (drcMain.libPOS.POSs.Count > 0)
            {
                gpTemp.ShowDialog();
            }
            else
            {
                MessageBox.Show("Open a Text File, Tokenize and Create POS First.");
            }
        }

        private void createSNNOrderedPhrasesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NNPhrases nnpsMain = new NNPhrases();
            nnpsMain.CreateNNPhrases(ref drcMain.libInput, ref drcMain.libWords, ref drcMain.libPOS);
        }

        private void codifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textToClasses();
        }

        private void proverbsManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (drcMain.libWords.WordIDs.Count > 0)
            {
                ProverbsManager pm = new ProverbsManager(ref dsrMain);
                pm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Open a Text File and Tokenize First.");
            }
        }

        private void createConglomerateFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try {
                if (drcMain.libWords.WordIDs.Count > 0 &&
                    drcMain.libPOS.POSs.Count > 0 &&
                    drcMain.libChunks.ChunkData.Count > 0 &&
                    drcMain.libPhrases.FWIDs.Count > 0 &&
                    drcMain.libPOSPhrases.POSPhrasesD.Count > 0
                    )
                {
                    StringBuilder sbConglomerate = new StringBuilder();
                    StreamWriter swConglomerate = new StreamWriter(drcMain.libInput.InsertStringIntoFilename("-Conglomerate"));

                    foreach (string strWordText in drcMain.libWords.WordIDs.Keys)
                    {
                        int intWordID = drcMain.libWords.GetWordWordID(strWordText);
                        string strPOS = drcMain.libWords.GetWordIDPOSs(intWordID, ref drcMain.libPOS).First();
                        Dictionary<int, string> dPositionedChunkMembers = new Dictionary<int, string>();
                        List<int> lPosition = drcMain.libWords.GetWordIDPositions(intWordID);

                        foreach (int intPosition in lPosition)
                        {
                            int intSentenceID = drcMain.libWords.GetPositionSentenceID(intPosition);

                            dPositionedChunkMembers.Add(intPosition, drcMain.libChunks.ChunkData[intSentenceID]);
                            sbConglomerate.AppendLine(strWordText + " ^ " + intWordID.ToString() + " ^ " + strPOS + " ^ " + dPositionedChunkMembers[intPosition] + " ^ " + intPosition.ToString());
                        }
                    }

                    swConglomerate.Write(sbConglomerate.ToString());
                    swConglomerate.Close();
                }
            }
            catch
            {
                MessageBox.Show("Open a Text File, Tokenize and Load POS Tags, Chunks, Phrases and POSPhrases First.");
            }
        }

        private void folderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (string strFilenameInput in Directory.EnumerateFiles(folderBrowserDialog1.SelectedPath))
                {
                    if (!strFilenameInput.Contains("NLPData") &&
                        strFilename.Length < 120)//??
                    {
                        DataRecord drdNew = new DataRecord();

                        drcMain = drdNew;
                        strFilename = strFilenameInput;

                        drdNew.libInput = new Input(strFilename);

                        //Only have one record at a time in the DataStore //??
                        dsrMain.ldrMain.Clear();

                        dsrMain.Add(drdNew);

                        tokenizeToolStripMenuItem_Click(null, null);
                        phrasalConcordanceToolStripMenuItem_Click(null, null);
                        reducePhrasesToolStripMenuItem_Click(null, null);
                        phrasalCompositionToolStripMenuItem_Click(null, null);
                        pOSTagToolStripMenuItem_Click(null, null);
                        combinePOSTagsToolStripMenuItem_Click(null, null);
                        pOSPhrasesToolStripMenuItem_Click(null, null);
                        parseToolStripMenuItem_Click(null, null);
                        createParsePartsToolStripMenuItem_Click(null, null);
                        buildParseTreeToolStripMenuItem_Click(null, null);
                        chunkToolStripMenuItem_Click(null, null);
                        pOSPairsToolStripMenuItem_Click(null, null);
                        enrichToolStripMenuItem_Click(null, null);

                        GeneralitySimilarity gsTemp = new GeneralitySimilarity(ref dsrMain);
                        gsTemp.Auto();

                        gsTemp = null;
                        drdNew = null;
                        drcMain = null;
                    }
                }
            }
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bReloadChunks = true;
            chunkToolStripMenuItem_Click(null, null);
            bReloadChunks = false;
        }

        private void createHMMPOSNetFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //assume libInput and libWords are loaded since libPOS is loaded
            if (drcMain.libPOS != null)
            {
                string strHMMPOSNetDataFilename = drcMain.libInput.InsertStringIntoFilename("-HMMPOSNet");
                StreamWriter swHMMPOSNet = new StreamWriter(strHMMPOSNetDataFilename);

                foreach (int intSentenceID in drcMain.libWords.SentenceFirstPositions.Keys.OrderBy(a=>a))

                    //drcMain.libWords.PositionWords.Keys.OrderBy(a => a)
                {
                    int intFirstPosition = drcMain.libWords.SentenceFirstPositions[intSentenceID];

                    swHMMPOSNet.WriteLine("<s> <s>");

                    for (int intPositionOffset = 0; intPositionOffset < drcMain.libWords.SentenceLengths[intSentenceID]; intPositionOffset++)
                    {
                        string strWord = drcMain.libWords.GetPositionWord(intFirstPosition + intPositionOffset);
                        string strPOS = "";

                        try
                        {
                            strPOS = drcMain.libPOS.POSs[intFirstPosition + intPositionOffset];
                        }
                        catch
                        {
                            strPOS = "NA";
                        }

                        swHMMPOSNet.WriteLine(strPOS + " " + strWord);
                    }
                }

                swHMMPOSNet.Close();
            }
            else
            {
                MessageBox.Show("Open a Text File, Tokenize and Create POS First");
            }
        }

        private void classifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Classify cfyMain = new Classify();
            //cfyMain.ShowDialog();
			//BigDump();
			//PPChunkedConcordance2();
			//SuperChunk();
			//drcMain.libPhrases.CreatePhraseParts(ref drcMain.libInput, drcMain.libPhrases);
			//PPChunkedConcordance3();
			//CreatePOSPhraseComplete();
			//CreateCompletePOSPhraseFiles();
			//drcMain.libPOSPhrases.LoadCompletePOSPhraseFiles (); //loads all complete files into dCompletePOSPhrasesOrdered
			drcMain.libPOSPhrases.POSPhrasesEnumeration();
			drcMain.libPOSPhrases.WriteTagChains ();
        }

		public void CreateCompletePOSPhraseFiles (){
			drcMain.libPOSPhrases.CreateCompletePOSPhraseFiles (ref drcMain.libPOS, ref drcMain.libWords, ref drcMain.libInput);
		}

		public void CreatePOSPhraseComplete(){
			//using MRD's POSPhrases, excluding lengths to match it
			for (int intLength = 1; intLength < 38; intLength++) {
				if (intLength != 26 && intLength != 28 && intLength != 30 &&
				    intLength != 32 && intLength != 33 && intLength != 34 &&
				    intLength != 35 && intLength != 36) {
					drcMain.libPOSPhrases.FilldCompletePOSPhrasesOrdered (@"/media/jeremy/Backups/Programming/Corpora/NLPData/MRD/POSPhrases/MRD-" + intLength.ToString () + ".txt");
					StreamWriter swComplete = new StreamWriter (@"/media/jeremy/Backups/Programming/Corpora/AAAComplete-POSPhrases-" + intLength.ToString () + ".text");

					foreach (int intPPRank in drcMain.libPOSPhrases.dCompletePOSPhrasesOrdered.Keys) {
						string strWrite = drcMain.libPOSPhrases.dCompletePOSPhrasesOrdered [intPPRank].Trim ();

						swComplete.WriteLine (strWrite.Split ('^') [0].Trim () + " ^ " + strWrite.Split ('^') [1].Trim ());
					}

					swComplete.Close ();
				}
			}
		}

		public void PPChunkedConcordance2()
		{
			StreamWriter swPPChunkedConcordance = new StreamWriter (drcMain.libInput.InsertStringIntoFilename (@"-PPChunkedConcordance2"));

			//Fills POSPhrases.dCompletePOSPhrasesOrdered
			drcMain.libPOSPhrases.FilldCompletePOSPhrasesOrdered (@"/media/jeremy/Backups/Programming/Corpora/AAAComplete-POSPhrases-2.text");

			foreach (int intPOSPhraseOrder in drcMain.libPOSPhrases.dCompletePOSPhrasesOrdered.Keys){
				string strPOSPhraseLine = drcMain.libPOSPhrases.dCompletePOSPhrasesOrdered [intPOSPhraseOrder];
				string[] strsPOSPhrase = (string[])strPOSPhraseLine.Split('^')[0].Trim().Split();
				List<int> lPPFWIDs = drcMain.libPOS.GetPOSPairPositions (strsPOSPhrase[0].Trim('[').Trim(']'), strsPOSPhrase[1].Trim('[').Trim(']'));

				swPPChunkedConcordance.WriteLine (strsPOSPhrase [0].Trim('[').Trim(']') + " " + strsPOSPhrase [1].Trim('[').Trim(']'));

				foreach (int intPPFWID in lPPFWIDs) {
					foreach (StringBuilder sbPPContext in drcMain.libWords.GetSurroundingText(drcMain.libWords.GetPositionWord (intPPFWID), 0)) {
						swPPChunkedConcordance.WriteLine (sbPPContext.ToString ());
					}
				}
			}
						
			swPPChunkedConcordance.Close();
		}

//		public void PPChunkedConcordance3()
//		{
//			StreamWriter swPPChunkedConcordance = new StreamWriter (drcMain.libInput.InsertStringIntoFilename (@"-PPChunkedConcordance3"));
//
//			//Fills POSPhrases.dCompletePOSPhrasesOrdered
//			drcMain.libPOSPhrases.CompletePOSPhrases (@"/media/jeremy/Backups/Programming/Corpora/AAAComplete-POSPhrases-3.text");
//
//			foreach (int intPOSPhraseOrder in drcMain.libPOSPhrases.dCompletePOSPhrasesOrdered.Keys){
//				string strPOSPhraseLine = drcMain.libPOSPhrases.dCompletePOSPhrasesOrdered [intPOSPhraseOrder].ToLower().Trim();
//
//				if (Regex.IsMatch (strPOSPhraseLine, @"\[vb[a-z]{0,}\] \^")) {
//					StringBuilder sbpp
//					swPPChunkedConcordance.WriteLine (sbPPContext.ToString ());
//				}
//			}
//
//			swPPChunkedConcordance.Close();
//		}

		public void BigDump()
		{
			StreamWriter swWords = new StreamWriter (drcMain.libInput.InsertStringIntoFilename ("-Big-Words"));
			StreamWriter swPhrases = new StreamWriter (drcMain.libInput.InsertStringIntoFilename ("-Big-Phrases"));
			StreamWriter swPOSPhrases = new StreamWriter (drcMain.libInput.InsertStringIntoFilename ("-Big-POSPhrases"));

			swWords.WriteLine ("SentenceID ^ SentencePosition ^ WordPosition ^ WordID ^ Word ^ POS");
			swPhrases.WriteLine ("Phrase ^ PhraseID ^ PhraseCount ^ PhraseLength ^ PhraseWordIDs ^ PhraseFWIDs");
			swPOSPhrases.WriteLine ("POSPhrase ^ Count ^ ExampleFWID");

			foreach (int intWordID in drcMain.libWords.WordIDs.Values) {
				string strWord = drcMain.libWords.GetWordIDWord (intWordID);
				List<string> lPOSs = drcMain.libWords.GetWordIDPOSs (intWordID, ref drcMain.libPOS); //all pos
				List<int> lPositions = drcMain.libWords.GetWordIDPositions (intWordID);

				foreach (int intWordPosition in lPositions) {
					int intSentenceID = drcMain.libWords.GetPositionSentenceID (intWordPosition);
					int intSentencePosition = intWordPosition - drcMain.libWords.SentenceFirstPositions [intSentenceID] + 1;
					string strPOS = drcMain.libWords.GetPositionPOS (intWordPosition, ref drcMain.libPOS); //this position's pos

					swWords.WriteLine (intSentenceID.ToString () + " ^ " + intSentencePosition.ToString () + " ^ " +
						intWordPosition.ToString () + " ^ " + intWordID.ToString () + " ^ " + strWord + " ^ " + strPOS);
				}
			}

			foreach (int intPhraseID in drcMain.libPhrases.PhraseIDs.OrderBy(a=>a.Value).Select(a=>a.Value)) {
				string strPhrase = drcMain.libPhrases.PhraseIDs.Where (a => a.Value == intPhraseID).First ().Key;
				int intPhraseCount = drcMain.libPhrases.PhraseCounts [strPhrase];
				int[] intsPhraseWordsIDs = drcMain.libPhrases.PhraseWordsIDs [strPhrase];
				List<int> lPhraseFWIDs = drcMain.libPhrases.FWIDs [strPhrase];

				swPhrases.Write (strPhrase + " ^ " + intPhraseID.ToString () + " ^ " + intPhraseCount.ToString () + " ^ " +
					strPhrase.Trim().Split().Length.ToString() + " ^ ");

				foreach (int intPhraseWordID in intsPhraseWordsIDs) {
					swPhrases.Write (intPhraseWordID.ToString () + " ");
				}

				swPhrases.Write ("^ ");

				foreach (int intPhraseFWID in lPhraseFWIDs) {
					swPhrases.Write (intPhraseFWID.ToString () + " ");
				}

				swPhrases.WriteLine ();
			}

			foreach (string strPOSPhrase in drcMain.libPOSPhrases.POSPhrasesD.OrderByDescending(a=>a.Value[0]).Select(a=>a.Key)) {
				int[] intsInfo = drcMain.libPOSPhrases.POSPhrasesD [strPOSPhrase];

				swPOSPhrases.WriteLine (strPOSPhrase + " ^ " + intsInfo [0].ToString () + " ^ " + intsInfo [1].ToString ());
			}

			swWords.Close ();
			swPhrases.Close ();
			swPOSPhrases.Close ();
		}

        private void phraseDimensionToolStripMenuItem_Click(object sender, EventArgs e)
        {
			DimensionalPhrase dp = new DimensionalPhrase ();
			for (int intLength = 1; intLength <= 10; intLength++) {
				string strPhraseCountsFilename = ((DataRecord)dsrMain.ldrMain [0]).libInput.DataPath + "/" +
				                       ((DataRecord)dsrMain.ldrMain [0]).libInput.Base + "/Phrases/" +
				                       ((DataRecord)dsrMain.ldrMain [0]).libInput.Base + "-" +
				                       intLength.ToString () + "-Counts.txt";
				string strPhraseDimensionFilename = ((DataRecord)dsrMain.ldrMain [0]).libInput.DataPath + "/" +
					((DataRecord)dsrMain.ldrMain [0]).libInput.Base + "/" +
					((DataRecord)dsrMain.ldrMain [0]).libInput.Base + "-PhraseDimension.txt";
				
				dp.Go (dsrMain.ldrMain[0].libWords.SentencesObject, strPhraseCountsFilename, strPhraseDimensionFilename);
			}

//            //returns every phrase (text and id) containing each noun
//            Dictionary<string, List<int>> dReturn = new Dictionary<string, List<int>>();
//            StreamWriter swNounPhraseDimension = new StreamWriter(drcMain.libInput.DataPath + "/" + drcMain.libInput.Base + "/" + drcMain.libInput.Base + "-NounPhraseDimension.txt");
//
//            if (drcMain.libPhrases.PhraseIDs.Count > 0)
//            {
//                //foreach noun, find each phrase that contains it and make a dictionary of a list of those PhraseIDs
//                if (drcMain.libPOS.POSs.Count > 0)
//                {
//                    //get each noun of various types
//                    foreach (string strWord in drcMain.libPOS.GetFuzzyPOSWords("NN", ref drcMain.libWords).Concat(
//                        drcMain.libPOS.GetFuzzyPOSWords("NP", ref drcMain.libWords)).Distinct())
//                    {
//                        dReturn.Add(strWord, new List<int>());

//                        var PhraseContainingWord =
//                            from p in drcMain.libPhrases.PhraseIDs
//                            where p.Key.ToUpper().Split().Contains(strWord.ToUpper())
//                            select p.Value;
//                        
//                        //get each phrase that contains the noun
//                        foreach (int intPhrasesContainingWord in PhraseContainingWord)
//                        {
//                            dReturn[strWord].Add(intPhrasesContainingWord);
//                        }

//                        dReturn[strWord] = dReturn[strWord].Distinct().ToList<int>();
//                    }

                    //format output: word ^ phrase text ^ phraseid ^ phrase text ^ phraseid ^ ..
//                    foreach (string strWord in dReturn.Keys)
//                    {
//                        string strOutput = strWord + " ^ ";

//                        foreach (int intPhraseID in dReturn[strWord])
//                        {
//                            strOutput += drcMain.libPhrases.PhraseIDs.Where(a=>a.Value == intPhraseID).First().Key + " ^ " + intPhraseID.ToString() + " ^ ";
//                        }

//                        strOutput = strOutput.Trim().Trim('^').Trim();
//                        swNounPhraseDimension.WriteLine(strOutput);
//                    }

//                    swNounPhraseDimension.Close();
//                }
//                else
//                {
//                    MessageBox.Show("Open a Text File, Tokenize and Create POS First");
//                }
//            }
//            else
//            {
//                MessageBox.Show("Open a Text File, Tokenize and Create Phrases First");
//            }
        }

		public void SuperChunk()
		{
			foreach (int intSentenceID in drcMain.libWords.SentenceList.Keys.OrderBy(a=>a)) {
				string strSentencePre1 = drcMain.libWords.SentenceList [intSentenceID];
				List<string> lSentence1 = new List<string> ();
				List<string> lSentence2 = new List<string> ();
				List<string> lSentence3 = new List<string> ();
				List<string> lSentence4 = new List<string> ();

				//@"(?<ref>[1-3]{0,1} {0,1}[A-Za-z ]{1,}[0-9]{1,} {0,}: {0,}[0-9]{1,} {0,}){0,1}(?<text>[^\r])").
				foreach (string strSentencePre in Regex.Split(strSentencePre1, @"[1-3]{0,1} {0,1}[A-Za-z ]{1,}[0-9]{1,} {0,}: {0,}[0-9]{1,} {0,}")) {
					foreach (string strSentence in Regex.Split(strSentencePre, 
					@"[0-9]{1,} {0,}: {0,}[0-9]{1,} {0,}")) {
						if (strSentence.Trim () != "") {
							if (strSentence.Contains (";")) {
								lSentence1.AddRange (strSentence.Split (';'));
							} else {
								lSentence1.Add (strSentence);
							}

							foreach (string strSentencePart1 in lSentence1) {
								if (strSentencePart1.Contains ('-')) {
									lSentence2.AddRange (strSentencePart1.Split ('-'));
								} else {
									lSentence2.Add (strSentencePart1);
								}
							}

							foreach (string strSentencePart2 in lSentence2) {
								if (strSentencePart2.Contains (":")) {
									lSentence3.AddRange (strSentencePart2.Split (':'));
								} else {
									lSentence3.Add (strSentencePart2);
								}
							}
							foreach (string strSentencePart3 in lSentence3) {
								if (strSentencePart3.Contains (",")) {
									lSentence4.AddRange (strSentencePart3.Split (','));
								} else {
									lSentence4.Add (strSentencePart3);
								}
							}

							//Write to Console
							Console.Write (intSentenceID.ToString () + ": ");

							foreach (string strSentencePart4 in lSentence4) {
								Console.Write (strSentencePart4 + " ^ ");
							}

							lSentence1.Clear ();
							lSentence2.Clear ();
							lSentence3.Clear ();
							lSentence4.Clear ();

							Console.WriteLine ();
							Console.ReadKey ();
						}
					}
				}
			}
		}
	}
}
