using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LibNLPDB
{
    public class RichWords
    {
        public Dictionary<string, List<string>> dEnrichedNouns = new Dictionary<string, List<string>>();
        public Dictionary<string, List<string>> dEnrichedVerbs = new Dictionary<string, List<string>>();
        
        //reverse entries
        public Dictionary<string, List<string>> dEnrichingAdjectives = new Dictionary<string, List<string>>();
        public Dictionary<string, List<string>> dEnrichingAdverbs = new Dictionary<string, List<string>>();

        public RichWords() { }

        //"DT  IN   CC PRP$ CD     EX  WDT   WP$  TO  WRB PDT RP WP SYM RBR FW  UH RBS  POS LS"
        //NN NNP NNS NNPS PRP 
        //JJ JJR JJS 
        //VBD VBG VBN VB VBP VBZ MD 
        //RB

        ////test
        ////List<string> lstrNounTest = new List<string>();
        ////lstrNounTest.Add("day");
        ////lstrNounTest.Add("month");
        ////lstrNounTest.Add("time");

        ////Dictionary<string, Dictionary<string, List<string>>> dTest = CompareAdjectivesOfNouns(lstrNounTest);
        //Dictionary<string, Dictionary<string, List<string>>> dTest2 = CompareAdverbsOfVerbs(lstrHaveVerbs);

        //List<string> lstrAllVerbs = new List<string>();
        //foreach (int intVerb in lintVerbs)
        //{
        //    lstrAllVerbs.Add(words.GetPositionWord(intVerb));
        //}
        //Dictionary<string, Dictionary<string, List<string>>> dTest3 = CompareAdverbsOfVerbs(lstrAllVerbs);
            
		public void Enrich(ref Input input, ref Words words, ref POS pos, string strModelPath)
        {
            //to encode semantics, use all data files to this point (sentactics, to chunk and parse)
            //and connect meaning to these signs, so that every item (beginning with largest-count phrases)
            //is connected to concepts (meaning) weighted by ratio of times seen meaning this vs. times seen meaning anything
            //

            StreamReader srEnrichedNouns;
            StreamReader srEnrichedVerbs;
            StreamWriter swEnrichedNouns;
            StreamWriter swEnrichedVerbs;
            StreamWriter swEnrichingAdjectives;
            StreamWriter swEnrichingAdverbs;

            List<string> lstrHaveVerbs = new List<string>();
            List<int> lintNouns = new List<int>();
            List<int> lintAdjectives = new List<int>();
            List<int> lintVerbs = new List<int>();
            List<int> lintRB = new List<int>();

			Chunks chunks = new Chunks (strModelPath);

            lstrHaveVerbs.Add("have");
            lstrHaveVerbs.Add("has");
            lstrHaveVerbs.Add("had");
            lstrHaveVerbs.Add("do");
            lstrHaveVerbs.Add("does");
            lstrHaveVerbs.Add("did");
            lstrHaveVerbs.Add("shall");
            lstrHaveVerbs.Add("will");
            lstrHaveVerbs.Add("should");
            lstrHaveVerbs.Add("would");
            lstrHaveVerbs.Add("may");
            lstrHaveVerbs.Add("might");
            lstrHaveVerbs.Add("must");
            lstrHaveVerbs.Add("can");
            lstrHaveVerbs.Add("could");
            
            if (File.Exists(input.InsertStringIntoFilename("-EnrichedNouns")) &&
                File.Exists(input.InsertStringIntoFilename("-EnrichedVerbs")) &&
                File.Exists(input.InsertStringIntoFilename("-EnrichingAdjectives")) &&
                File.Exists(input.InsertStringIntoFilename("-EnrichingAdverbs"))
                )
            {
                srEnrichedNouns = new StreamReader(input.InsertStringIntoFilename("-EnrichedNouns"));
                srEnrichedVerbs = new StreamReader(input.InsertStringIntoFilename("-EnrichedVerbs"));
                
                while (!srEnrichedNouns.EndOfStream)
                {
                    string strLine = srEnrichedNouns.ReadLine();
                    string strAdjective = strLine.Split()[0];
                    string strNoun = strLine.Split()[1];

                    if (!dEnrichedNouns.ContainsKey(strNoun))
                    {
                        dEnrichedNouns.Add(strNoun, new List<string>());
                    }

                    dEnrichedNouns[strNoun].Add(strAdjective);

                    if (!dEnrichingAdjectives.ContainsKey(strAdjective))
                    {
                        dEnrichingAdjectives.Add(strAdjective, new List<string>());
                    }

                    dEnrichingAdjectives[strAdjective].Add(strNoun);
                }

                while (!srEnrichedVerbs.EndOfStream)
                {
                    string strLine = srEnrichedVerbs.ReadLine();
                    string strAdverb = strLine.Split()[0];
                    string strVerb = strLine.Split()[1];

                    if (!dEnrichedVerbs.ContainsKey(strVerb))
                    {
                        dEnrichedVerbs.Add(strVerb, new List<string>());
                    }

                    dEnrichedVerbs[strVerb].Add(strAdverb);

                    if (!dEnrichingAdverbs.ContainsKey(strAdverb))
                    {
                        dEnrichingAdverbs.Add(strAdverb, new List<string>());
                    }

                    dEnrichingAdverbs[strAdverb].Add(strVerb);
                }
                
                srEnrichedNouns.Close();
                srEnrichedVerbs.Close();
            }
            else
            {
                //add the verbs here, because pos isn't perfect and any VB NN pair is modifier-interesting anyway; maybe adverbs, too
                var getModifiedHeadNouns =
                    from modifier in lintAdjectives
                    where lintNouns.Contains(modifier + 1)
                    select modifier + 1;

                var getVerbModifiedHeadNouns =
                    from modifier in lintVerbs
                    where lintNouns.Contains(modifier + 1)
                    select modifier + 1;
                
                var getModifiedHeadVerbs =
                    from modifier in lintRB
                    where lintVerbs.Contains(modifier + 1)
                    select modifier + 1;

                swEnrichedNouns = new StreamWriter(input.InsertStringIntoFilename("-EnrichedNouns"));
                swEnrichedVerbs = new StreamWriter(input.InsertStringIntoFilename("-EnrichedVerbs"));
                swEnrichingAdjectives = new StreamWriter(input.InsertStringIntoFilename("-EnrichingAdjectives"));
                swEnrichingAdverbs = new StreamWriter(input.InsertStringIntoFilename("-EnrichingAdverbs"));

                //preload
                lintNouns.AddRange(pos.GetPOSPositions("NN"));
                lintNouns.AddRange(pos.GetPOSPositions("NNP"));
                lintNouns.AddRange(pos.GetPOSPositions("NNS"));

                lintAdjectives.AddRange(pos.GetPOSPositions("DT")); //a modifier that makes its noun active (out of a group, "that one")
                lintAdjectives.AddRange(pos.GetPOSPositions("JJ"));
                lintAdjectives.AddRange(pos.GetPOSPositions("JJR"));
                lintAdjectives.AddRange(pos.GetPOSPositions("JJS"));
                lintAdjectives.AddRange(pos.GetPOSPositions("PDT"));
                lintAdjectives.AddRange(pos.GetPOSPositions("RB")); //try this for adjectives too
                lintAdjectives.AddRange(pos.GetPOSPositions("WDT"));
				lintAdjectives.AddRange(pos.GetPOSPositions("WRB"));
				lintAdjectives.AddRange(pos.GetPOSPositions("WP"));
				lintAdjectives.AddRange(pos.GetPOSPositions("WP$"));

                //remove from adjectives
                foreach (string strHaving in lstrHaveVerbs) //remove having verbs from adjectives
                {
                    foreach (int intAdjectiveRemove in words.GetWordPositions(strHaving))
                    {
                        lintAdjectives.Remove(intAdjectiveRemove);
                    }
                }

                lintVerbs.AddRange(pos.GetPOSPositions("VB"));
                lintVerbs.AddRange(pos.GetPOSPositions("VBD"));
                lintVerbs.AddRange(pos.GetPOSPositions("VBG"));
                lintVerbs.AddRange(pos.GetPOSPositions("VBN"));
                lintVerbs.AddRange(pos.GetPOSPositions("VBP"));
                lintVerbs.AddRange(pos.GetPOSPositions("VBZ"));
				lintVerbs.AddRange(pos.GetPOSPositions("MD"));

				lintRB.AddRange(pos.GetPOSPositions("DT")); //try this for verbs, too
                lintRB.AddRange(pos.GetPOSPositions("PDT"));
                lintRB.AddRange(pos.GetPOSPositions("RB"));
				lintRB.AddRange(pos.GetPOSPositions("WDT"));
				lintRB.AddRange(pos.GetPOSPositions("WRB"));
				lintRB.AddRange(pos.GetPOSPositions("WP"));
				lintRB.AddRange(pos.GetPOSPositions("WP$"));

                //
                //Encode Richness in the form of POS substitutions
                //

                //encode prepositional phrase/adjective richness (ie. PP NN -> JJ; PP DT JJ NN -> JJ|Adverb)
                //exa. 

                //endcode adjective/noun richness (ie. JJ -> NN)  // NOTE: "endcode" is purposeful - make a trapdoor, activated by a switch that turns off this functionality, for the purpose of making the whole structure unusable (because this is a fundamental piece -TODO: Check if last statement TRUE)  

                //encode various commonly used combinations like previous ones

                //
                //Encode Richness in the form of the consecutive positions of modifier then head
                //

                //encode noun/adjective richness
                foreach (int intUWID in getModifiedHeadNouns)
                {
                    AddAdjective(words.GetPositionWord(intUWID), words.GetPositionWord(intUWID - 1));
                }

                foreach (int intUWID in getVerbModifiedHeadNouns) //(Some) VB as JJ
                {
                    //if pos of intUWID is in lstrHaveVerbs, then DON'T add it to the adjectives list
                    if (!lstrHaveVerbs.Contains(words.GetPositionWord(intUWID - 1)))
                    {
                        AddAdjective(words.GetPositionWord(intUWID), words.GetPositionWord(intUWID - 1));
                    }
                }

                //encode verb/adverb richness
                foreach (int intUWID in getModifiedHeadVerbs)
                {
                    AddAdverb(words.GetPositionWord(intUWID), words.GetPositionWord(intUWID - 1));

                }

                //write xml
                foreach (string strNoun in dEnrichedNouns.Keys.OrderBy(a => a))
                {
                    foreach (string strAdjective in dEnrichedNouns[strNoun].OrderBy(a => a))
                    {
                        swEnrichedNouns.WriteLine(strAdjective + " " + strNoun);
                    }
                }

                foreach (string strAdjective in dEnrichingAdjectives.Keys.OrderBy(a => a))
                {
                    foreach (string strNoun in dEnrichingAdjectives[strAdjective].OrderBy(a => a))
                    {
                        swEnrichingAdjectives.WriteLine(strAdjective + " " + strNoun);
                    }
                }

                foreach (string strVerb in dEnrichedVerbs.Keys.OrderBy(a => a))
                {
                    foreach (string strAdverb in dEnrichedVerbs[strVerb].OrderBy(a => a))
                    {
                        swEnrichedVerbs.WriteLine(strAdverb + " " + strVerb);
                    }
                }
                
                foreach (string strAdverb in dEnrichingAdverbs.Keys.OrderBy(a => a))
                {
                    foreach (string strVerb in dEnrichingAdverbs[strAdverb].OrderBy(a => a))
                    {
                        swEnrichingAdverbs.WriteLine(strAdverb + " " + strVerb);
                    }
                }

                swEnrichedNouns.Close();
                swEnrichedVerbs.Close();
                swEnrichingAdjectives.Close();
                swEnrichingAdverbs.Close();
            }
        }

        //Get the unique modifiers (and their distributions?) that are found in adjective/noun pairs compared with the other adjective/non-noun pairs.
        //This will partition modifiers unique to specify-reducing nouns.
        public void CompareModifiers(ref Words words, ref POS pos)
        {
            Dictionary<string, List<string>> dReturn = new Dictionary<string, List<string>>();

            foreach (string strNNWord in pos.GetFuzzyPOSWords("NN", ref words))
            {
                System.Console.WriteLine(strNNWord);
            }
        }
        
        public void AddAdjective(string strNoun, string strAdjective)
        {
            if (!dEnrichedNouns.ContainsKey(strNoun.ToLower()))
            {
                dEnrichedNouns.Add(strNoun.ToLower(), new List<string>());
            }

            if (!dEnrichedNouns[strNoun.ToLower()].Contains(strAdjective.ToLower()))
            {
                dEnrichedNouns[strNoun.ToLower()].Add(strAdjective.ToLower());
            }

            if (!dEnrichingAdjectives.ContainsKey(strAdjective.ToLower()))
            {
                dEnrichingAdjectives.Add(strAdjective.ToLower(), new List<string>());
            }

            if (!dEnrichingAdjectives[strAdjective.ToLower()].Contains(strNoun.ToLower()))
            {
                dEnrichingAdjectives[strAdjective.ToLower()].Add(strNoun.ToLower());
            }
        }

        public void AddAdverb(string strVerb, string strAdverb)
        {
            if (!dEnrichedVerbs.ContainsKey(strVerb.ToLower()))
            {
                dEnrichedVerbs.Add(strVerb.ToLower(), new List<string>());
            }

            if (!dEnrichedVerbs[strVerb.ToLower()].Contains(strAdverb.ToLower()))
            {
                dEnrichedVerbs[strVerb.ToLower()].Add(strAdverb.ToLower());
            }

            if (!dEnrichingAdverbs.ContainsKey(strAdverb.ToLower()))
            {
                dEnrichingAdverbs.Add(strAdverb.ToLower(), new List<string>());
            }

            if (!dEnrichingAdverbs[strAdverb.ToLower()].Contains(strVerb.ToLower()))
            {
                dEnrichingAdverbs[strAdverb.ToLower()].Add(strVerb.ToLower());
            }
        }

        //Generic Form of These Functions:
        //public void AddPreposition(string strAdjective, string strPreposition)
        //{
        //($dEnriched + X$) "if" are null
        //if (! ($ dEnriching + X $).ContainsKey (($ str + Y $).ToLower()
        //{
        //  dEnriching($ dEnriching + X $).Add(($ str + X $).ToLower(), new List<string>());
        //}
        //
        //if (! ($ dEnriching + X $)[($ str + X $).ToLower()].Contains(($ str + X $).ToLower()))
        //{
        //  ($ dEnriching + X $)[($ str + X $).ToLower()].Add(($ str + Y $).ToLower());
        //}
        //}

        public void AddPreposition(string strAdjective, string strPreposition)
        {
            //($dEnriched + X$) "if" are null
            if (!dEnrichingAdjectives.ContainsKey(strPreposition.ToLower()))
            {
                dEnrichingAdjectives.Add(strAdjective.ToLower(), new List<string>());
            }

            if (!dEnrichingAdjectives[strAdjective.ToLower()].Contains(strPreposition.ToLower()))
            {
                dEnrichingAdjectives[strAdjective.ToLower()].Add(strPreposition.ToLower());
            }
        }

        public Dictionary<string, Dictionary<string, List<string>>> CompareAdjectivesOfNouns(List<string> lstrNouns)
        {
            Dictionary<string, Dictionary<string, List<string>>> dReturn = new Dictionary<string, Dictionary<string, List<string>>>();

            foreach (string strNoun in lstrNouns)
            {
                foreach (string strNoun2 in lstrNouns)
                {
                    if (strNoun != strNoun2)
                    {
                        foreach (string strAdjective in dEnrichedNouns[strNoun])
                        {
                            if (dEnrichedNouns[strNoun2].Contains(strAdjective))
                            {
                                if (!dReturn.ContainsKey(strNoun))
                                {
                                    dReturn.Add(strNoun, new Dictionary<string, List<string>>());
                                }

                                if (!dReturn[strNoun].ContainsKey(strNoun2))
                                {
                                    dReturn[strNoun].Add(strNoun2, new List<string>());
                                }

                                dReturn[strNoun][strNoun2].Add(strAdjective);
                            }
                        }
                    }
                }
            }

            return dReturn;
        }

        public Dictionary<string, Dictionary<string, List<string>>> CompareAdverbsOfVerbs(List<string> lstrVerbs)
        {
            Dictionary<string, Dictionary<string, List<string>>> dReturn = new Dictionary<string, Dictionary<string, List<string>>>();

            foreach (string strVerb in lstrVerbs)
            {
                foreach (string strVerb2 in lstrVerbs)
                {
                    if (strVerb != strVerb2)
                    {
                        try
                        {
                            foreach (string strAdverb in dEnrichedVerbs[strVerb])
                            {
                                if (dEnrichedVerbs[strVerb2].Contains(strAdverb))
                                {
                                    if (!dReturn.ContainsKey(strVerb))
                                    {
                                        dReturn.Add(strVerb, new Dictionary<string, List<string>>());
                                    }

                                    if (!dReturn[strVerb].ContainsKey(strVerb2))
                                    {
                                        dReturn[strVerb].Add(strVerb2, new List<string>());
                                    }

                                    dReturn[strVerb][strVerb2].Add(strAdverb);
                                }
                            }
                        }
                        catch { }
                    }
                }
            }

            return dReturn;
        }

        //TODO: Flip this one around to get PP JJ patterns
        public Dictionary<string, Dictionary<string, List<string>>> ComparePrepositionPhrasesOfAdjectives(List<string> lstrAdjectives)
        {
            Dictionary<string, Dictionary<string, List<string>>> dReturn = new Dictionary<string, Dictionary<string, List<string>>>();

            foreach (string strAdjective in lstrAdjectives)
            {
                foreach (string strAdjective2 in lstrAdjectives)
                {
                    if (strAdjective != strAdjective2)
                    {
                        foreach (string strPrepositionalPhrase in dEnrichedNouns[strAdjective])
                        {
                            if (dEnrichingAdjectives[strAdjective2].Contains(strPrepositionalPhrase))
                            {
                                if (!dReturn.ContainsKey(strAdjective))
                                {
                                    dReturn.Add(strAdjective, new Dictionary<string, List<string>>());
                                }

                                if (!dReturn[strAdjective].ContainsKey(strAdjective2))
                                {
                                    dReturn[strAdjective].Add(strAdjective2, new List<string>());
                                }

                                dReturn[strAdjective][strAdjective2].Add(strPrepositionalPhrase);
                            }
                        }
                    }
                }
            }

            return dReturn;
        }

        public static void CombineRichWords(string strDirectoryName)
		{
			List<string> lCombinedEnrichedNouns = new List<string> ();
			List<string> lCombinedEnrichedVerbs = new List<string> ();
			List<string> lCombinedEnrichingAdjectives = new List<string> ();
			List<string> lCombinedEnrichingAdverbs = new List<string> ();

			StreamWriter swEnrichedNouns = new StreamWriter (strDirectoryName + "/NLPData/EnrichedNouns.txt");
			StreamWriter swEnrichedVerbs = new StreamWriter (strDirectoryName + "/NLPData/EnrichedVerbs.txt");
			StreamWriter swEnrichingAdjectives = new StreamWriter (strDirectoryName + "/NLPData/EnrichingAdjectives.txt");
			StreamWriter swEnrichingAdverbs = new StreamWriter (strDirectoryName + "/NLPData/EnrichingAdverbs.txt");

			foreach (string strDirectory in Directory.EnumerateDirectories(strDirectoryName + "/NLPData")) {
				try {
					string strEnrichedNounsFileName = Directory.EnumerateFiles (strDirectory).Where (a => a.Contains ("EnrichedNouns")).First ();
					StreamReader srEnrichedNouns = new StreamReader (strEnrichedNounsFileName);

					while (!srEnrichedNouns.EndOfStream) {
						string strEnrichedNounsLine = srEnrichedNouns.ReadLine ().Trim ();

						if (!lCombinedEnrichedNouns.Contains (strEnrichedNounsLine)) {
							lCombinedEnrichedNouns.Add (strEnrichedNounsLine);
						}
					}
				
					srEnrichedNouns.Close ();
				} catch {
				}
			}

			foreach (string strDirectory in Directory.EnumerateDirectories(strDirectoryName + "/NLPData")) {
				try {
					string strEnrichedVerbsFileName = Directory.EnumerateFiles (strDirectory).Where (a => a.Contains ("EnrichedVerbs")).First ();
					StreamReader srEnrichedVerbs = new StreamReader (strEnrichedVerbsFileName);

					while (!srEnrichedVerbs.EndOfStream) {
						string strEnrichedVerbsLine = srEnrichedVerbs.ReadLine ().Trim ();

						if (!lCombinedEnrichedVerbs.Contains (strEnrichedVerbsLine)) {
							lCombinedEnrichedVerbs.Add (strEnrichedVerbsLine);
						}
					}

					srEnrichedVerbs.Close ();
				} catch {
				}

			}

			foreach (string strDirectory in Directory.EnumerateDirectories(strDirectoryName + "/NLPData")) {
				try {
					string strEnrichingAdjectivesFileName = Directory.EnumerateFiles (strDirectory).Where (a => a.Contains ("EnrichingAdjectives")).First ();
					StreamReader srEnrichingAdjectives = new StreamReader (strEnrichingAdjectivesFileName);

					while (!srEnrichingAdjectives.EndOfStream) {
						string strEnrichingAdjectivesLine = srEnrichingAdjectives.ReadLine ().Trim ();

						if (!lCombinedEnrichingAdjectives.Contains (strEnrichingAdjectivesLine)) {
							lCombinedEnrichingAdjectives.Add (strEnrichingAdjectivesLine);
						}
					}

					srEnrichingAdjectives.Close ();
				} catch {
				}

			}

			foreach (string strDirectory in Directory.EnumerateDirectories(strDirectoryName + "/NLPData")) {
				try {
					string strEnrichingAdverbsFileName = Directory.EnumerateFiles (strDirectory).Where (a => a.Contains ("EnrichingAdverbs")).First ();
					StreamReader srEnrichingAdverbs = new StreamReader (strEnrichingAdverbsFileName);

					while (!srEnrichingAdverbs.EndOfStream) {
						string strEnrichingAdverbsLine = srEnrichingAdverbs.ReadLine ().Trim ();

						if (!lCombinedEnrichingAdverbs.Contains (strEnrichingAdverbsLine)) {
							lCombinedEnrichingAdverbs.Add (strEnrichingAdverbsLine);
						}
					}

					srEnrichingAdverbs.Close ();
				} catch {
				}

			}

			//write
			foreach (string strPhrase in lCombinedEnrichedNouns.OrderBy(a=>a.Split()[1])) {
				swEnrichedNouns.WriteLine (strPhrase);
			}

			swEnrichedNouns.Close ();

			foreach (string strPhrase in lCombinedEnrichedVerbs.OrderBy(a=>a.Split()[1])) {
				swEnrichedVerbs.WriteLine (strPhrase);
			}

			swEnrichedVerbs.Close ();

			foreach (string strPhrase in lCombinedEnrichingAdjectives.OrderBy(a=>a)) {
				swEnrichingAdjectives.WriteLine (strPhrase);
			}

			swEnrichingAdjectives.Close ();

			foreach (string strPhrase in lCombinedEnrichingAdverbs.OrderBy(a=>a)) {
				swEnrichingAdverbs.WriteLine (strPhrase);
			}

			swEnrichingAdverbs.Close ();
		}
    }
}
