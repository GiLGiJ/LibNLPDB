using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NLPDB
{
    //Generality/Specifics Dichotomy combined with
    //Similarities/Differences Dichotomy
    //Generality/Similarity = Structure
	public partial class GeneralitySimilarity : Form
    {
        DataStore dsrInput;
		Dictionary<string, List<string>> dWordForms = new Dictionary<string, List<string>>(); //D<noun, L<modifiers>> Modifiers are different forms of the Noun

        public GeneralitySimilarity(ref DataStore dsrInputTemp)
        {
            InitializeComponent();

            dsrInput = dsrInputTemp;
        }

        private void GeneralitySimilarity_Load(object sender, EventArgs e)
        {
            foreach (string strNoun in dsrInput.ldrMain.Last().libRichWords.dEnrichedNouns.Keys)
            {
                lbxNoun.Items.Add(strNoun);
            }
        }

        private void lbxNoun_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbxNounProperties.Items.Clear();
            
            foreach(string strProperty in dsrInput.ldrMain.Last().libRichWords.dEnrichedNouns[lbxNoun.Text])
            {
                lbxNounProperties.Items.Add(strProperty);
            }

            if (lbxNounProperties.Items.Count > 0)
            {
                lbxNounProperties.SelectedIndex = 0;
            }
        }

        private void lbxNounProperties_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbxPropertyRelatedNouns.Items.Clear();

            foreach (string strNoun2 in dsrInput.ldrMain.Last().libRichWords.dEnrichingAdjectives[lbxNounProperties.Text])
            {
                lbxPropertyRelatedNouns.Items.Add(strNoun2);
            }
        }

        private void lbxPropertyRelatedNouns_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbxSharedProperties.Items.Clear();

            foreach (string strProperty in dsrInput.ldrMain.Last().libRichWords.dEnrichedNouns[lbxNoun.Text].Intersect(
                dsrInput.ldrMain.Last().libRichWords.dEnrichedNouns[lbxPropertyRelatedNouns.Text]))
            {
                lbxSharedProperties.Items.Add(strProperty);
            }
        }

        private void btnCreateDataFiles_Click(object sender, EventArgs e)
        {
			StreamWriter swWordProperties = new StreamWriter(dsrInput.ldrMain.Last().libInput.InsertStringIntoFilename("-AutomaticNounProperties"));

			foreach (string strNoun in dWordForms.Where(a=>a.Value.Count > 0).OrderByDescending(a=>a.Value.Count).Select(a=>a.Key)) {
				swWordProperties.Write(strNoun + " ^ ");

				foreach (string strProperty in dWordForms[strNoun]){
					swWordProperties.Write(strProperty + " ");
				}

				swWordProperties.WriteLine();
			}

				swWordProperties.Close();
        }

        public void Auto()
        {
            btnPropertiesToNouns_Click(null, null);
            btnCreateDataFiles_Click(null, null);
        }

        //Group Adjectives and Adverbs (etc.?) with their own Noun Forms.
        private void btnPropertiesToNouns_Click(object sender, EventArgs e)
		{
			//go through dictionary first using each noun
			//add associated modifier word forms to dWordForms

			//delete the final characters of the modifiers
			//match them with the first letters of nouns to make
			//the rule: property belongs to noun
			if (dsrInput.ldrMain.Last ().libCombinedPOS == null) {
				MessageBox.Show ("Select Input File, Tokenize and Load CombinedPOS");
			}
			else if (!File.Exists (dsrInput.ldrMain.Last ().libInput.InsertStringIntoFilename ("-AutomaticNounProperties"))) {
				StreamReader srDictionary; //Outside source
				StreamWriter swAutomaticNounProperties = new StreamWriter(dsrInput.ldrMain.Last().libInput.InsertStringIntoFilename("-AutomaticNounProperties"));

				Dictionary<string, string> dDictionary; //Lookup word forms in dictionary
				List<string> lModifiers = new List<string> (); //list of Modifiers for a given Noun
				List<string> lModifierEndings = new List<string> ();
				List<string> lRemoveNouns = new List<string> ();

				//Load Dictionary - TODO

				//Load Modifier Endings List
				lModifierEndings.Add ("ous");
				lModifierEndings.Add ("ly");
				lModifierEndings.Add ("ed");
				lModifierEndings.Add ("est");
				lModifierEndings.Add ("eth");
				lModifierEndings.Add ("ing");
				lModifierEndings.Add ("ion");
				lModifierEndings.Add ("ions");

				//each noun
				foreach (string strNoun in dsrInput.ldrMain.Last().libRichWords.dEnrichedNouns.Keys){
                //.Concat(dsrInput.ldrMain.Last().libRichWords.dEnrichedVerbs.Keys)) {
					//skip names
					if (!dsrInput.ldrMain.Last ().libWords.GetWordPOSs (strNoun, ref dsrInput.ldrMain.Last ().libCombinedPOS).Contains ("NNP")) {
						//if word is NN
						if (dsrInput.ldrMain.Last ().libWords.GetWordPOSs (strNoun, ref dsrInput.ldrMain.Last ().libCombinedPOS).Contains ("NN")) {
							//add new nouns to dWordForms
							if (!dWordForms.ContainsKey (strNoun)) {
								dWordForms.Add (strNoun, new List<string> ());
							}

							//reset lModifiers
							lModifiers.Clear();

							//each noun's adjectives and adverbs
							foreach (string strProperty in dsrInput.ldrMain.Last().libRichWords.dEnrichingAdjectives.Keys){//.Concat(
                                //dsrInput.ldrMain.Last().libRichWords.dEnrichingAdverbs.Keys)) {
								foreach (string strEnding in lModifierEndings) {
                                    if (strNoun.Length > 1)
                                    {
                                        if (strNoun + strEnding == strProperty || strNoun.Remove(strNoun.Length - 1) + strEnding == strProperty)
                                        {
                                            //						            if (strProperty.Length > strNoun.Length) {
                                            //							        if (lModifierEndings.Contains (strProperty.Remove (0, strNoun.Length))) {
                                            //                        
                                            if (dsrInput.ldrMain.Last().libCombinedPOS.CombinedPOSs.ContainsKey(strProperty))
                                            {
                                                if (dsrInput.ldrMain.Last().libCombinedPOS.CombinedPOSs[strProperty].Contains("JJ"))
                                                {
                                                    if (!lModifiers.Contains(strProperty))
                                                    {
                                                        //									int intSameLetters = 0;
                                                        //
                                                        //									for (int intNounIndex = 0; intNounIndex < strNoun.Length; intNounIndex++) {
                                                        //										if (strProperty [intNounIndex] == strNoun [intNounIndex]) {
                                                        //											intSameLetters++;
                                                        //										}
                                                        //									}
                                                        //
                                                        //									if (strNoun.Length - 1 <= intSameLetters)
                                                        //									{
                                                        //string strPropertyReduced = strProperty.Substring(0, strProperty.Length - 3);
                                                        //string strNounReduced = strNoun.Substring(0, strPropertyReduced.Length);
                                                        //double dblIntersectionCount = (double)0;// (strPropertyReduced.SequenceEqual(strNoun.Substring(0)).Count();
                                                        //double dblPropertyCount = (double)strProperty.Count();


                                                        //if (dblIntersectionCount / dblPropertyCount < .3)
                                                        //										{
                                                        //										var hasJ = 
                                                        //											from pos in dsrInput.ldrMain.Last ().libWords.GetWordPOSs (strProperty, ref dsrInput.ldrMain.Last ().libCombinedPOS)
                                                        //											where pos.Contains ("JJ")
                                                        //											select strProperty.Count () > 0;

                                                        //if (hasJ.All (a => a == true)) {
                                                        lModifiers.Add(strProperty);
                                                        dWordForms[strNoun].Add(strProperty); //this property should only go to this one noun
                                                                                              //}
                                                    }
                                                }
                                            }
                                            //								}
                                        }
							
									}
								}
							}
						}
					}
				}

				foreach (string strNoun in dWordForms.Keys) {
					if (dWordForms [strNoun].Count == 0) {
						lRemoveNouns.Add (strNoun);
					}
				}

				foreach (string strNoun in lRemoveNouns) {
					dWordForms.Remove (strNoun);
				}

                //now do it another way
                //consider last chunk and next chunk
                foreach (string strNoun in dsrInput.ldrMain[0].libRichWords.dEnrichedNouns.Keys)
                {

                }

				foreach (string strNoun in dWordForms.Keys) {
					swAutomaticNounProperties.Write (strNoun + " ");

					foreach (string strModifier in dWordForms[strNoun]) {
						swAutomaticNounProperties.Write(strModifier + " ");
					}

					swAutomaticNounProperties.WriteLine ();
				}

				swAutomaticNounProperties.Close ();
			} else { //Load file
				StreamReader srAutomaticNounProperties = new StreamReader(dsrInput.ldrMain.Last().libInput.InsertStringIntoFilename("-AutomaticNounProperties"));

				while (!srAutomaticNounProperties.EndOfStream) {
					string strLine = srAutomaticNounProperties.ReadLine ();
					string strWord = strLine.Split ('^') [0].Trim ();

					dWordForms.Add (strWord, new List<string> ());

					foreach (string strProperty in strLine.Split('^')[1].Trim().Split()) {
						dWordForms [strWord].Add (strProperty);
					}
				}

				srAutomaticNounProperties.Close ();
			}
		}
    }
}
