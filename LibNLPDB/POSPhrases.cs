using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
//using Accord.MachineLearning;
//using Accord.Neuro;
//using Accord.IO;
//using Accord.Collections;

namespace LibNLPDB
{
    public class POSPhrases
    {
		public string strPOSPhrasesDirectory = "";
		public Dictionary<string, int[]> dPOSPhrases = new Dictionary<string, int[]>(); //D<POSPhrase, [Count, FWID]>
		public Dictionary<int, string> dCompletePOSPhrasesOrdered = new Dictionary<int, string>();
		public Dictionary<string, Dictionary<int, string>> dPPPlaces = new Dictionary<string, Dictionary<int, string>>();
		public Dictionary<int, Dictionary<string, int>> dAllCompletePOSPhrasesOrdered = //D<ID, D<POSPhrase, Length>>
			new Dictionary<int, Dictionary<string, int>> ();
		public Dictionary<string, RecursivePhraseParts> dTagIDs = new Dictionary<string, RecursivePhraseParts> (); //d<pos tag, d<length, unique id>>

        public Dictionary<string, int[]> POSPhrasesD
        {
            get
            {
                return dPOSPhrases;
            }
            set
            {
                dPOSPhrases = value;
            }
        }

        public POSPhrases() { }

        public bool LoadPOSPhrases(ref Input input)
        {
            bool bReturn = false;
			Rgxs rr = new Rgxs ();

            dPOSPhrases.Clear();

            strPOSPhrasesDirectory = input.DataPath + "/" + input.Base + "/POSPhrases";

            if (Directory.Exists(strPOSPhrasesDirectory))
            {
                bReturn = true;

                foreach (string strPOSPhrasesFileName in Directory.EnumerateFiles(strPOSPhrasesDirectory))
                {
                    StreamReader srPOSPhrases = new StreamReader(strPOSPhrasesFileName);

                    while (!srPOSPhrases.EndOfStream)
                    {
                        string strLine = srPOSPhrases.ReadLine();
                        int[] intsData = (int[])Array.CreateInstance(typeof(int), 2);

						try{
                        string strCount = strLine.Split('^')[1];
						string strFWID = rr.rgxNumbers.Match(strLine.Split('^')[2]).Value;

						//Console.WriteLine(strCount + "  ...  " + strFWID);
                        intsData[0] = Convert.ToInt32(strCount);
						intsData[1] = Convert.ToInt32(strFWID);

                        dPOSPhrases.Add(strLine.Split('^')[0], intsData);// Convert.ToInt32(strLine.Split('^')[1]));
						}catch(Exception ex) {

						}
                    }

                    srPOSPhrases.Close();
                }
            }

            return bReturn;
        }

        public void CreatePOSPhrases(ref Input input, ref Words words, ref Phrases phrases, ref POS pOS)
        {
            int intPhraseFilesCount = input.PhraseFilesCount();
			StreamWriter swPPPlaces;

			strPOSPhrasesDirectory = input.DataPath + "/" + input.Base + "/POSPhrases";

            if (!Directory.Exists(strPOSPhrasesDirectory))
            {
                Directory.CreateDirectory(strPOSPhrasesDirectory);
            }

			swPPPlaces = new StreamWriter (strPOSPhrasesDirectory + "/" + input.Base + "-PPPhrases.text");

            if (intPhraseFilesCount > 0)
            {
                for (int intPhraseLength = 1; intPhraseLength <= intPhraseFilesCount;
                    intPhraseLength++)
                {
                    //phrases.LoadPhraseWordIDs(ref input);
                    //phrases.LoadPhraseCountsFile(input.DataPath + "/" + input.Base + "/Phrases/" +
                    //    input.Base + "-" + intPhraseLength.ToString() + "-Counts" + input.Ext, true);

                    dPOSPhrases.Clear();

                    foreach (string strPhraseTemp in phrases.FWIDs.Keys.Where(a => a.Trim().Split().Count() == intPhraseLength)) //.PhraseCounts.Keys)
                    {
                        foreach (int intPhraseFWID in phrases.FWIDs[strPhraseTemp])
                        {
                            string strPOSPhrase = "";

                            for (int intCurrentPositionID = intPhraseFWID; intCurrentPositionID <= intPhraseFWID + intPhraseLength - 1; intCurrentPositionID++)
                            {
                                strPOSPhrase += "[";
                                strPOSPhrase += pOS.POSs[intCurrentPositionID].Trim();
                                strPOSPhrase += "] ";
                            }

                            strPOSPhrase = strPOSPhrase.Trim();

                            if (strPOSPhrase != "")
							{
								if (!dPPPlaces.ContainsKey (strPOSPhrase)) {
									dPPPlaces.Add (strPOSPhrase, new Dictionary<int, string> ());
								}

                                if (!dPOSPhrases.ContainsKey(strPOSPhrase))
                                {
                                    int[] intsData = (int[])Array.CreateInstance(typeof(int), 2);

                                    intsData[0] = 1;
                                    intsData[1] = intPhraseFWID;

                                    dPOSPhrases.Add(strPOSPhrase, intsData);
                                }
                                else
                                {
                                    dPOSPhrases[strPOSPhrase][0]++;
                                }

								dPPPlaces [strPOSPhrase].Add (intPhraseFWID, strPhraseTemp);
                            }
                        }
                    }

                    if (dPOSPhrases.Count() > 0)
                    {
                        //SortedList<int, string> slPosPhraseKeys = new SortedList<int, string>();

                        //foreach (string strKey in dPOSPhrases.Keys)
                        //{
                        //    if (strKey.Trim() != "" && strKey.Trim().Split().Count() == intPhraseLength)
                        //    {
                        //        if (dPOSPhrases[strKey][0] > 1)
                        //        {
                        //            slPosPhraseKeys.Add(slPosPhraseKeys.Count + 1, strKey);
                        //        }
                        //    }
                        //}

                        IEnumerable<string> posPhraseKeys =
                            from b in dPOSPhrases.Keys
                            where b.Trim() != ""
                            where b.Trim().Split().Count() == intPhraseLength
                            where dPOSPhrases[b][0] > 1
                            orderby dPOSPhrases[b][0] descending
                            select b;

                        if (posPhraseKeys.Count() > 0)
                        {
                            StreamWriter swPOSPhrases = new StreamWriter(strPOSPhrasesDirectory +
                                "/" + input.Base + "-" + intPhraseLength.ToString() + input.Ext);

                            foreach (string strPOSPhraseWrite in posPhraseKeys)
                            {
                                swPOSPhrases.WriteLine(strPOSPhraseWrite + " ^ " + dPOSPhrases[strPOSPhraseWrite][0].ToString() + " ^ " + dPOSPhrases[strPOSPhraseWrite][1].ToString());
                            }

                            swPOSPhrases.Close();
                        }
                    }
                }

				StreamReader srPPComplete2 = new StreamReader (@"/media/jeremy/Backups/Programming/Corpora/AAAComplete-POSPhrases-2.text");
				List<string> lComplete2 = new List<string> ();

				while (!srPPComplete2.EndOfStream) {
					string strLine = srPPComplete2.ReadLine ();
					string strPP = strLine.Split ('^') [0].Trim ();
					Console.WriteLine ("strPP: " + strPP);
					lComplete2.Add (strPP);
				}

				foreach (string strPOSPhrase in lComplete2) {
					if (dPPPlaces.ContainsKey (strPOSPhrase)) {
						foreach (int intFWID in dPPPlaces[strPOSPhrase].Keys.OrderBy(b=>b)) {
							swPPPlaces.WriteLine (strPOSPhrase + " ^ " + dPPPlaces [strPOSPhrase] [intFWID] + " ^ " + intFWID.ToString ());
						}
					}
				}

				swPPPlaces.Close ();

                //Since this was just created, reload the dictionary with all lengths
                LoadPOSPhrases(ref input);
            }

            //lblMostFrequentPOSPhrase.Text = "Most Frequent POS Phrase Text: " + dPOSPhrases.Keys.OrderBy(a => dPOSPhrases[a]).First();
        }

        public void Analyze()
        {
            if (dPOSPhrases.Count > 0)
            {
                var byCount =
                    from pPhrase in dPOSPhrases
                    orderby pPhrase.Value.Count() descending
					select pPhrase.Key.ToLower();

//                Accord.Neuro.Networks.RestrictedBoltzmannMachine rbm;
//                Accord.Neuro.Layers.StochasticLayer hidden = new Accord.Neuro.Layers.StochasticLayer(dPOSPhrases.Count * 3, dPOSPhrases.Count);
//                Accord.Neuro.Layers.StochasticLayer visible = new Accord.Neuro.Layers.StochasticLayer(dPOSPhrases.Count, dPOSPhrases.Count);
//
//                Accord.Neuro.ActivationFunctions.GaussianFunction b = new Accord.Neuro.ActivationFunctions.GaussianFunction();
//                Accord.Neuro.Neurons.StochasticNeuron a = new Accord.Neuro.Neurons.StochasticNeuron(dPOSPhrases.Count, b);
                //a.
                // Accord.Neuro.Learning.DeepNeuralNetworkLearning a = new Accord.Neuro.Learning.DeepNeuralNetworkLearning(new Accord.Neuro.Networks.DeepBeliefNetwork(dPOSPhrases.Count, params rbm[])

                foreach (string strPOSPhrase in byCount)
                {
                    //make feature objects to give to neural net
					if (strPOSPhrase.StartsWith ("in")) {
						string a = "";
					}
                }

//                rbm = new Accord.Neuro.Networks.RestrictedBoltzmannMachine(hidden, visible);

            }

        }

		public List<int> GetPOSPhrase2FWPositions(ref POS pos, string strPOSPhrase2){ //POSPhrase => Phrase Text
			List<int> lReturn = new List<int> ();

			foreach (int intFWPosition in pos.GetPOSPairPositions(strPOSPhrase2.Split()[0], strPOSPhrase2.Split()[1])){
				lReturn.Add (intFWPosition);
			}

			return lReturn;
		}

		public void FilldCompletePOSPhrasesOrdered(string strCompletePOSPhrasesFilename)
		{
			StreamReader srCompletePhrases = new StreamReader (strCompletePOSPhrasesFilename);

			dCompletePOSPhrasesOrdered.Clear ();

			while (!srCompletePhrases.EndOfStream) {
				string strLine = srCompletePhrases.ReadLine ();

				string strPOSPhrase = strLine.Split ('^') [0].ToLower ().Replace ('[', ' ').Replace (']', ' ').Trim ();
				int intLength = strPOSPhrase.Split ().Count ();

				string strLastPOS = strPOSPhrase.Split () [intLength - 1];

				if (intLength > 1) {
					if (!strLastPOS.Contains ("cc") && !strLastPOS.Contains ("dt") &&
					    !strLastPOS.Contains ("in") && !strLastPOS.Contains ("to") &&
					    !strLastPOS.Contains ("jj")) {
						if (!strPOSPhrase.TrimStart ("dt".ToCharArray ()).Contains ("dt") &&
						    !strPOSPhrase.TrimStart ("in".ToCharArray ()).Contains ("in") &&
						    !strPOSPhrase.TrimStart ("to".ToCharArray ()).Contains ("to") &&
						    !strPOSPhrase.Contains ("cc")) {
							dCompletePOSPhrasesOrdered.Add (dCompletePOSPhrasesOrdered.Count + 1, strLine);
						}
					}
				} else { //intLength == 1
					dCompletePOSPhrasesOrdered.Add (dCompletePOSPhrasesOrdered.Count + 1, strLine);
				}
			}

			srCompletePhrases.Close ();
		}

		public void LoadCompletePOSPhraseFiles(ref Input input){
			dCompletePOSPhrasesOrdered.Clear ();
			for (int intLength = 1; intLength < 38; intLength++) {
				if (intLength != 26 && intLength != 28 && intLength != 30 &&
					intLength != 32 && intLength != 33 && intLength != 34 &&
					intLength != 35 && intLength != 36) {
					StreamReader srPOSPhraseComplete = new StreamReader (@"/media/jeremy/Backups/Programming/Corpora/AAAComplete-POSPhrases-" + intLength.ToString () + ".text");

					while (!srPOSPhraseComplete.EndOfStream){
						string strLine = srPOSPhraseComplete.ReadLine ();
						dCompletePOSPhrasesOrdered.Add (dCompletePOSPhrasesOrdered.Count () + 1, 
							strLine.Split ('^') [0].Trim ());
					}
				}
			}
		}

		public class RecursivePhraseParts{
			public int intID = 0;
			public string strPOSTag = "";
			public Dictionary<int,string> dPOSTag = new Dictionary<int, string>();
			public List<RecursivePhraseParts> lOut = new List<RecursivePhraseParts>();

			public RecursivePhraseParts(int intNewID, string strUncleanedPOSPhrase){
				strPOSTag = strUncleanedPOSPhrase;

				foreach(string strPart in strPOSTag.Trim().ToLower().Replace(']', ' ').Replace('[', ' ').Split()){
					dPOSTag [dPOSTag.Count () + 1] = strPart;
				}
			}

			public string Subphrases(){
				string strReturn = "";

				foreach (RecursivePhraseParts rppSubphrase in lOut){
					if (rppSubphrase.lOut.Count() == 0){
						strReturn += " " + rppSubphrase.strPOSTag + ", ";
					}else{
						foreach (RecursivePhraseParts rppSubSubphrase in rppSubphrase.lOut){
							strReturn += rppSubphrase.Subphrases ();
						}
					}
				}

				strReturn += "\r\n";

				return strReturn;
			}
		}

		public string RemoveEnd(string strPOS){
			return strPOS.Remove (strPOS.LastIndexOf ('[')).Trim();
		}

		public void POSPhrasesEnumeration(){
			StreamReader srCompletePhrases1 = new StreamReader (@"/media/jeremy/Backups/Programming/Corpora/NLPData/MRD/POSPhrases/MRD-1.txt");
			int intNextID = 1;

			while (!srCompletePhrases1.EndOfStream) {
				string strLine = srCompletePhrases1.ReadLine ();
				string strUncleanedPOSPhrase = strLine.Split ('^') [0].Trim ();

				dTagIDs.Add (strUncleanedPOSPhrase, new RecursivePhraseParts(intNextID, strUncleanedPOSPhrase));
				intNextID++;
			}

			for (int intLength = 2; intLength < 36; intLength++) {
				if (intLength != 26 && intLength != 28 && intLength != 30 &&
				    intLength != 32 && intLength != 33 && intLength != 34 &&
				    intLength != 35 && intLength != 36) {
					List<string> lLength = new List<string> ();
					StreamReader srCompletePhrases = new StreamReader (@"/media/jeremy/Backups/Programming/Corpora/NLPData/MRD/POSPhrases/MRD-" + intLength.ToString () + ".txt");

					while (!srCompletePhrases.EndOfStream) {
						string strLine = srCompletePhrases.ReadLine ();
						string strUncleanedPOSPhrase = strLine.Split ('^') [0].Trim ();
						string strCleanedPOSPhrase = strUncleanedPOSPhrase.ToLower ().Replace ('[', ' ').Replace (']', ' ').Trim ();

						if (dTagIDs.ContainsKey (RemoveEnd(strUncleanedPOSPhrase))) {
							RecursivePhraseParts rppAdd = new RecursivePhraseParts (intNextID, strUncleanedPOSPhrase);
							intNextID++;
							dTagIDs [RemoveEnd (strUncleanedPOSPhrase)].lOut.Add (rppAdd);
							dTagIDs [strUncleanedPOSPhrase] = rppAdd;
						}
					}

					srCompletePhrases.Close ();
				}
			}
		}

		public void WriteTagChains(){
			StreamWriter swTagChains = new StreamWriter(@"/media/jeremy/Backups/Programming/Corpora/NLPData/MRD/POSPhrases/TagChains.txt");

			foreach (string strPOSPhrase in dTagIDs.Keys){
				RecursivePhraseParts rppTemp = dTagIDs [strPOSPhrase];
				string strSubphrase = rppTemp.Subphrases ();

				swTagChains.WriteLine (strSubphrase);
			}

			swTagChains.Close ();
		}

		//[xx], [yx] [zp], [ar]..
		public void HashPOSPhrases(){
			//[xx] [yy] ^ 100
			//[xx] [yy] [zz] ^ 2
			//[xx] [yy] [zz] [aa] ^ 0
			//[zz] [aa] ^ 99
			//Therefore, [xx] [yy] [zz] ^ 2 is removed from the list of complete pos phrases.

			for (int intLength = 1; intLength < 36; intLength++) {
				if (intLength != 26 && intLength != 28 && intLength != 30 &&
				    intLength != 32 && intLength != 33 && intLength != 34 &&
					intLength != 35 && intLength != 36) {
					List<string> lLength1 = new List<string> ();
					StreamReader srCompletePhrases = new StreamReader ( @"/media/jeremy/Backups/Programming/Corpora/NLPData/MRD/POSPhrases/MRD-" + intLength.ToString () + ".txt");

					while (!srCompletePhrases.EndOfStream) {
						string strLine = srCompletePhrases.ReadLine ();

						string strPOSPhrase = strLine.Split ('^') [0].ToLower ().Replace ('[', ' ').Replace (']', ' ').Trim ();

						lLength1.Add(strPOSPhrase);
					}

					srCompletePhrases.Close ();

					for (int intLength2 = 2; intLength2 < 38; intLength2++) {
						if (intLength2 != 26 && intLength2 != 28 && intLength2 != 30 &&
						    intLength2 != 32 && intLength2 != 33 && intLength2 != 34 &&
						    intLength2 != 35 && intLength2 != 36) {
							List<string> lLength2 = new List<string> ();
							StreamReader srCompletePhrases2 = new StreamReader ( @"/media/jeremy/Backups/Programming/Corpora/NLPData/MRD/POSPhrases/MRD-" + intLength2.ToString () + ".txt");

							while (!srCompletePhrases.EndOfStream) {
								string strLine = srCompletePhrases.ReadLine ();

								string strPOSPhrase = strLine.Split ('^') [0].ToLower ().Replace ('[', ' ').Replace (']', ' ').Trim ();

								lLength2.Add(strPOSPhrase);
							}

							srCompletePhrases2.Close ();
						}
					}
				}
			}


		}

		public void CreateCompletePOSPhraseFiles(ref POS pos, ref Words words, ref Input input)
		{
			List<int> lPhraseStartPositions;
			string strCurrentPhrase = "";
			int intCurrentCount = 0;
			int intCurrentLength = 0;

			for (int intLength = 1; intLength < 38; intLength++) {
				if (intLength != 26 && intLength != 28 && intLength != 30 &&
				    intLength != 32 && intLength != 33 && intLength != 34 &&
				    intLength != 35 && intLength != 36) {
					FilldCompletePOSPhrasesOrdered (@"/media/jeremy/Backups/Programming/Corpora/NLPData/MRD/POSPhrases/MRD-" + intLength.ToString () + ".txt");
					Console.WriteLine ("Complete>" + dCompletePOSPhrasesOrdered.Count ().ToString ());

					foreach (string strPOSComplete in dCompletePOSPhrasesOrdered.Values) {
						List<string> lPhrasesComplete = new List<string> ();
						strCurrentPhrase = strPOSComplete.Split ('^') [0].Trim ().ToLower ().Replace ('[', ' ').Replace (']', ' ');
						intCurrentCount = Convert.ToInt32 (strPOSComplete.Split ('^') [1].Trim ());
						intCurrentLength = strCurrentPhrase.Split ().Count ();
						StreamWriter swPhrasesComplete = new StreamWriter (input.InsertStringIntoFilename 
						("-CompletePhrases-" + intCurrentLength.ToString () + ".text"));

						lPhraseStartPositions = GetPOSPhrase2FWPositions (ref pos, strPOSComplete);

						if (intCurrentCount == lPhraseStartPositions.Count ()) {
							foreach (int intFWPosition in lPhraseStartPositions) {
								string strPhrase = "";

								for (int intPhraseCounter = intFWPosition; 
								intPhraseCounter <= intFWPosition + intCurrentLength - 1; intPhraseCounter++) {
									strPhrase += " " + words.GetPositionWord (intPhraseCounter);
								}

								if (!lPhrasesComplete.Contains (strPhrase.Trim())) {
									lPhrasesComplete.Add (strPhrase.Trim());
								}
							}
						} else {
							Console.WriteLine ("Count From File: " + intCurrentCount.ToString () +
							"  List Length: " + lPhraseStartPositions.Count ().ToString ());
						}

						foreach (string strPhrase in lPhrasesComplete) {
							swPhrasesComplete.WriteLine (strPhrase);
						}

						swPhrasesComplete.Close ();
					}
				}
			}
		}
	}
}
