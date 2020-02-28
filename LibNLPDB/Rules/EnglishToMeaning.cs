using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibNLPDB.Rules
{
    static class EnglishToMeaning
    {
        static public Dictionary<int, string> WordsToSentences(Dictionary<int, string> dWords) { return null; } //this might not be needed
        static public Dictionary<int, string> WordsToPhrases(Dictionary<int, string> dWords) { return null; } //this might not be needed
      
        static public Dictionary<int, string> PhrasesToSentences(Dictionary<int, string> dPhrases)
        {
            Dictionary<int, string> dReturn = new Dictionary<int, string>();
            int intCurrentSubject = -1;
            string strCurrentSubject = "";

            //foreach word, look at pos and update nouns with adjective, verb, etc info, choosing current subjects partly by their environments (ie. sentence objects)

            return dReturn;
        }

        static public Dictionary<int, string> SentencesToThoughts(Dictionary<int, string> dSentences, Dictionary<int, Dictionary<int, string>> dSentencePOSs)
        {
            Dictionary<int, string> dReturn = new Dictionary<int, string>();
            
            foreach (int intSentenceID in dSentences.Keys)
            {
                int intWordPosition = 0;

                foreach (string strWord in dSentences[intSentenceID].Trim().Split())
                {
                    intWordPosition++;

                    string strPOS = dSentencePOSs[intSentenceID][intWordPosition];
                }
            }

            return dReturn;
        }

        static public void CreateThought(string strThought) { } //output some hand-crafted Thought object

        static public void ThoughtsToMeanings(/*input Thought objects*/) { } //output some hand-crafted Meaning objects, this extra step may not be needed

        static public bool ApplyMeaningsToDatabase() { return false; } //this should evaluate all database records with the new information
    }

    class Thought { }

    class Meaning { }
}
