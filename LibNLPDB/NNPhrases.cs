using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibNLPDB
{
    public class NNPhrases
    {
        
        Dictionary<int, Dictionary<int, string>> dSequencesPOSs = new Dictionary<int, Dictionary<int, string>>(); //D<SentenceID, D<RowID, POS Phrase>>
        Dictionary<int, Dictionary<int, string>> dSequencesWords = new Dictionary<int, Dictionary<int, string>>(); //D<SentenceID, D<RowID, Word Phrase>>
        Dictionary<string, int> dSequenceWordCounts = new Dictionary<string, int>(); //D<PhraseWords, Count>
        Dictionary<string, int> dSequencePOSCounts = new Dictionary<string, int>(); //D<PhrasePOS, Count>
        
        public void CreateNNPhrases(ref Input input, ref Words words, ref POS pos)
        {
            StreamWriter swNNPhrasesPOSs = new StreamWriter(input.InsertStringIntoFilename("-NNPhrases-POSs"));
            StreamWriter swNNPhrasesWords = new StreamWriter(input.InsertStringIntoFilename("-NNPhrases-Words"));
            StreamWriter swNNPhrasesWordCounts = new StreamWriter(input.InsertStringIntoFilename("-NNPhrases-Word-Counts"));
            StreamWriter swNNPhrasesPOSCounts = new StreamWriter(input.InsertStringIntoFilename("-NNPhrases-POS-Counts"));
        
            //For each Sentence, create a sequence of NN (of various POS, not just NN proper), phrase between them  
            foreach (int intSentenceID in words.SentencesObject.SentenceFirstPositionList.Keys.OrderBy(a => a))
            {
                Dictionary<int, string> dNNPhrasesPOSs = new Dictionary<int, string>();
                Dictionary<int, string> dNNPhrasesWords = new Dictionary<int, string>();
                string strCurrentPhrasePOSs = "";
                string strCurrentPhraseWords = "";

                for (int intPosition = words.SentencesObject.SentenceFirstPositionList[intSentenceID];
                    intPosition <= words.SentencesObject.SentenceFirstPositionList[intSentenceID] +
                    words.SentencesObject.SentenceLengthList[intSentenceID] - 1; intPosition++)
                {
                    string strWord = words.GetPositionWord(intPosition);
                    string strPOS = words.GetPositionPOS(intPosition, ref pos);
                    string strTag = "<NN>";

                    //special tag for name
                    if (strPOS.Contains("NNP"))
                    {
                        strTag = "<NM>";
                    }

                    if (strPOS.Contains("NN") || strPOS.Contains("PRP") || strPOS.Contains("RB") || strPOS.Contains("WRB") || strPOS.Contains("EX")) //currently, "EX" is sometimes tagged to "there"
                    {
                        //only capture last non-NN phrase if it's there
                        if (strCurrentPhrasePOSs.TrimEnd() != "")
                        {
                            //capture non-NN phrase first
                            dNNPhrasesPOSs.Add(dNNPhrasesPOSs.Count + 1, strCurrentPhrasePOSs.TrimEnd());
                            dNNPhrasesWords.Add(dNNPhrasesWords.Count + 1, strCurrentPhraseWords.TrimEnd());
                        }

                        //then capture NN phrase
                        strCurrentPhrasePOSs = strPOS;
                        strCurrentPhraseWords = strWord;

                        dNNPhrasesPOSs.Add(dNNPhrasesPOSs.Count + 1, strTag + strCurrentPhrasePOSs);
                        dNNPhrasesWords.Add(dNNPhrasesWords.Count + 1, strTag + strCurrentPhraseWords);


                        //and reset strCurrentPhrase
                        strCurrentPhrasePOSs = "";
                        strCurrentPhraseWords = "";
                    }
                    else
                    {
                        //place non-NN word into phrase
                        strCurrentPhrasePOSs += strPOS; // + " ";
                        strCurrentPhraseWords += strWord; // + " ";
                    }
                }

                if (strCurrentPhrasePOSs.TrimEnd() != "")
                {
                    string strSemanticTag = "";

                    //add semantic tags to non-nn phrases
                    if (strCurrentPhrasePOSs.StartsWith("IN"))
                    { //Prepositional Phrase ending with
                        strSemanticTag = "<IN>";
                    }

                    if (strCurrentPhrasePOSs.StartsWith("DT") || strCurrentPhrasePOSs.StartsWith("W"))
                    { //Deterministic Phrase; implies some level of particularity
                        strSemanticTag = "<DT>";
                    }

                    strCurrentPhraseWords = strSemanticTag + " " + strCurrentPhraseWords;
                    strCurrentPhrasePOSs = strSemanticTag + " " + strCurrentPhrasePOSs;

                    dNNPhrasesPOSs.Add(dNNPhrasesPOSs.Count + 1, strCurrentPhrasePOSs.TrimEnd());
                    dNNPhrasesWords.Add(dNNPhrasesWords.Count + 1, strCurrentPhraseWords.TrimEnd());
                }

                //Fill sentence value dictionaries
                dSequencesPOSs.Add(intSentenceID, dNNPhrasesPOSs);
                dSequencesWords.Add(intSentenceID, dNNPhrasesWords);

                foreach (int intPhraseID in dNNPhrasesWords.Keys)
                {
                    if (!dSequenceWordCounts.ContainsKey(dNNPhrasesWords[intPhraseID]))
                    {
                        dSequenceWordCounts.Add(dNNPhrasesWords[intPhraseID], 0);
                    }

                    if (!dSequencePOSCounts.ContainsKey(dNNPhrasesPOSs[intPhraseID]))
                    {
                        dSequencePOSCounts.Add(dNNPhrasesPOSs[intPhraseID], 0);
                    }

                    dSequenceWordCounts[dNNPhrasesWords[intPhraseID]]++;
                    dSequencePOSCounts[dNNPhrasesPOSs[intPhraseID]]++;
                }
            }

            //Write POS and Word Data
            foreach (int intSentenceID in dSequencesPOSs.Keys.OrderBy(a => a))
            {
                foreach (int intPhraseID in dSequencesPOSs[intSentenceID].Keys.OrderBy(a => a))
                {
                    swNNPhrasesPOSs.WriteLine(intSentenceID.ToString() + " ^ " + intPhraseID.ToString() +
                          " ^ " + dSequencesPOSs[intSentenceID][intPhraseID]);
                    swNNPhrasesWords.WriteLine(intSentenceID.ToString() + " ^ " + intPhraseID.ToString() +
                          " ^ " + dSequencesWords[intSentenceID][intPhraseID]);

                }
            }

            //Write Count Data
            foreach (string strPhrase in dSequenceWordCounts.OrderByDescending(a => a.Value).Select(a => a.Key))
            {
                swNNPhrasesWordCounts.WriteLine(strPhrase + " " + dSequenceWordCounts[strPhrase].ToString());
            }

            foreach (string strPhrase in dSequencePOSCounts.OrderByDescending(a => a.Value).Select(a => a.Key))
            {
                swNNPhrasesPOSCounts.WriteLine(strPhrase + " " + dSequencePOSCounts[strPhrase].ToString());
            }

            swNNPhrasesPOSs.Close();
            swNNPhrasesWords.Close();
            swNNPhrasesWordCounts.Close();
            swNNPhrasesPOSCounts.Close();
        }


    }
}
