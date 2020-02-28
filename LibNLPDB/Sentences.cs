using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using OpenNLP.Tools.SentenceDetect;

namespace LibNLPDB
{
    public class SVO
    {
        public int[] intsSVO = (int[])Array.CreateInstance(typeof(int), 3); //WordPositions of Subject(s), Verb(s) and Object(s) Phrase IDs, indexed in that order
        
        public int SVOToSentenceObject(ref Phrases phrases)
        {
            //make recursive - a subject-verb-object triple can be a part of the outer sentence's object
            int intReturn = -1;
            string strSVO = ""; //String representation of the SVO Triple

            //phrases.PhraseIDs.Where(a=>a.Key == )

            return intReturn;
        }
    }

    public class Sentences
    {
        public Dictionary<int, Dictionary<int, Dictionary<string, string>>> dStructures =
            new Dictionary<int, Dictionary<int, Dictionary<string, string>>>(); //D<SentenceID, D<UWID, D<Word, POS Tag>>>
        private SortedList<int, string> slSentences;
        private SortedList<int, int> slFirstPositions;
        private SortedList<int, int> slWordPositionsSentenceIDs;
        private SortedList<int, int> slSentenceLengths;
        //private string strModelPath;
        //private OpenNLP.Tools.SentenceDetect.EnglishMaximumEntropySentenceDetector mSentenceDetector;
        
        // Recursive Sentences
        private Sentences sentenceInner;
        private int intFirstVerbPosition = -1;
        private int intSecondVerbPosition = -1;
        private Input input;
        
    //            //check sentence for a second verb (ie. with a nn between the verbs)
    //            foreach (KeyValuePair<int, int> kvpFirstPositions in slFirstPositions)
    //            {
    //                if (words.GetPositionPOS(kvpFirstPositions.Value, ref pos).ToLower().Contains("vb"))
    //                {
    //                    if (intFirstVerbPosition == -1) //First Verb
    //                    {
    //                        intFirstVerbPosition = kvpFirstPositions.Value;
    //                    }
    //                    else if (intFirstVerbPosition<kvpFirstPositions.Value - 1)
    //                    {
    //                        if (intSecondVerbPosition == -1)
    //                        {
    //                            intSecondVerbPosition = kvpFirstPositions.Value;

    //                            sentenceInner = new Sentences(ref input, ref words, ref pos);
    //sentenceInner.intFirstVerbPosition = intSecondVerbPosition;
    //                        }
    //                    }
    //                    else
    //                    {
    //                        if (intFirstVerbPosition != kvpFirstPositions.Value - 1) //not 2 successive verbs (ie. "will" "be")
    //                        {
    //                            throw new Exception("SENTENCES: Third Verb - Needs More Code!");
    //                        }
    //                    }
    //                }
    //            }

        public Sentences(ref Input input, ref Words words, ref POS pos)
        {   
            slSentences = new SortedList<int, string>();
            slFirstPositions = new SortedList<int, int>();
            slWordPositionsSentenceIDs = new SortedList<int, int>();
            slSentenceLengths = new SortedList<int, int>();
        }

        public SortedList<int, string> SentenceList
        {
            get
            {
                return slSentences;
            }
            set
            {
                slSentences = value;
            }
        }

        public SortedList<int, int> SentenceFirstPositionList
        {
            get
            {
                return slFirstPositions;
            }
            set
            {
                slFirstPositions = value;
            }
        }

        public SortedList<int, int> WordPositionsSentenceIDs
        {
            get
            {
                return slWordPositionsSentenceIDs;
            }
            set
            {
                slWordPositionsSentenceIDs = value;
            }
        }

        public SortedList<int, int> SentenceLengthList
        {
            get
            {
                return slSentenceLengths;
            }
            set
            {
                slSentenceLengths = value;
            }
        }

        public string[] SplitSentencesOpenNLP(string strModelPath, string paragraph)
        {
            int intSentenceID = slSentences.Count;
            int intFirstPosition = 1;
            int intSentenceLength = 0;
            MaximumEntropySentenceDetector mSentenceDetector = new EnglishMaximumEntropySentenceDetector(strModelPath);
            string strNewSentence = "";

            slSentences.Clear();
            slSentenceLengths.Clear();
            slFirstPositions.Clear();

            foreach (string strSentence in mSentenceDetector.SentenceDetect(paragraph))
            {
                strNewSentence = strSentence;
                intSentenceLength = strNewSentence.Trim().Split(@" \r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Count();

                if (intSentenceLength > 0)
                {
                    intSentenceID++;
                    slSentences.Add(intSentenceID, strNewSentence);
                    slSentenceLengths.Add(intSentenceID, intSentenceLength);
                    slFirstPositions.Add(intSentenceID, intFirstPosition);
                    intFirstPosition += intSentenceLength; //First Position of next Sentence
                }
            }

            return slSentences.Values.ToArray();
        }

        public string[] SplitSentences(string strSentences, ref Words words, ref POS pos, bool bVerses = false, bool bRemovePunctuation = true)
        {
            int intSentenceID = 0;
            int intFirstPosition = 1;
            int intSentenceLength = 0;
            int intCharIndex = -1;
            List<int> lintDecimals = new List<int>();
            StringBuilder sbDecimals = new StringBuilder();
            Rgxs r = new Rgxs();
            string strNonTerminatingPunctuation = @";:-,\)(][}{><@|#$%^&*_=+'" + '"';

            slSentences.Clear();
            slSentenceLengths.Clear();
            slWordPositionsSentenceIDs.Clear();
            slFirstPositions.Clear();
            dStructures.Clear();

            //encode decimal points in numbers and change \r\n or \t to single space
            foreach (char cCharacter in strSentences)
            {
                intCharIndex++;

                if (cCharacter == ((char)'.') && r.rgxNumber.IsMatch(strSentences[intCharIndex - 1].ToString()) &&
                        r.rgxNumber.IsMatch(strSentences[intCharIndex + 1].ToString()))
                {
                    sbDecimals.Append("%"); //numeric decimal support
                }
                else
                {
                    if (cCharacter == ((char)'\r'))
                    {
                        //Do nothing
                    }
                    else if (cCharacter == ((char)'\n') || cCharacter == ((char)'\t'))
                    {
                        sbDecimals.Append(" ");
                    }
                    else if (strNonTerminatingPunctuation.Contains(cCharacter)) //separate non-terminating punctuation
                    {
                        sbDecimals.Append(" ");
                        sbDecimals.Append(cCharacter);
                        sbDecimals.Append(" ");
                    }
                    else
                    {
                        sbDecimals.Append(cCharacter);
                    }
                }
            }

            strSentences = sbDecimals.ToString();
            strSentences = r.rgxRemovePunctuation.Replace(strSentences, a => " " + a.Value);
            ///
            ///Make memory-based model of sentences
            ///

            //first pass with common sentence ending punctuation
            foreach (string strNewSentence in strSentences.Split(".?!".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                intSentenceID++;
                intSentenceLength = strNewSentence.Trim().Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Count();

                slSentences.Add(intSentenceID, strNewSentence);
                slSentenceLengths.Add(intSentenceID, intSentenceLength);
                slFirstPositions.Add(intSentenceID, intFirstPosition);

                dStructures.Add(intSentenceID, new Dictionary<int, Dictionary<string, string>>());

                for (int intUWIDTemp = intFirstPosition; intUWIDTemp < intFirstPosition + intSentenceLength;
                    intUWIDTemp++)
                {
                    dStructures[intSentenceID].Add(intUWIDTemp, new Dictionary<string, string>());
                    slWordPositionsSentenceIDs.Add(intUWIDTemp, intSentenceID);
                }

                intFirstPosition += intSentenceLength; //First Position of next Sentence
            }

            //second pass with logical sentencing
            SentenceDetector sdetTemp = new SentenceDetector(slSentences);
            //sdetTemp.ShowDialog();

            return slSentences.Values.ToArray();
        }

        public void CreateSentenceTextFiles(string strDataPath)
        {
            string strOutputDirectory = strDataPath + "/Sentences/";
            
            if (!Directory.Exists(strOutputDirectory))
            {
                Directory.CreateDirectory(strOutputDirectory);
            }

            for (int intSentenceID = 1; intSentenceID <= slSentences.Max(a => a.Key); intSentenceID++)
            {
                File.WriteAllText(strOutputDirectory + intSentenceID.ToString() + ".txt", slSentences[intSentenceID]);
            }
        }
    }
}
