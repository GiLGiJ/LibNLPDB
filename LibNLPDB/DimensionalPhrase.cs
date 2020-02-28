using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LibNLPDB
{
    public class DimensionalPhrase
    {
        //Foreach phrase length L, tag complete phrases and uncomplete phrases of different types or parts of speech.
		//That way, you'll know what types of phrases you have for all the complete phrases and which words the 
		//breaks go in between in an incomplete, possibly multiple, phrase.  Does a complete phrase boundary become 
		//incomplete in a longer phrase?
        //
		//<C|I> <PP|NP|Recieving Object|Sender's Existence Verb (The Subject's Being Verb)|etc>
        //foreach (string strPOSPhrase in POSPhraseLCount)
        // if (strPOSPhrase is Complete) //?
        //  if (strPOSPhrase Xchange lastWord\lastWord-2\lastWord-3.. exists)
        //   list = strPOSPhrase Xchange lastWord|.. //ie. 4 word phrases where any 3 are the same
        //
        //   
        // foreach(string strPhrase where POS[] is like POSPhrase)
        //  measure sim&diff
        //  apply all hand crafted rules - this is your work surface

        public void Go(Sentences ss, string strPOSPhrasesPath, string strPhraseDimensionFilename)
        {
            StreamReader srPOSPhrases = new StreamReader(strPOSPhrasesPath);
			StreamWriter swPhraseDimension = new StreamWriter (strPhraseDimensionFilename);
			SortedList<int, string> slPOSPhrases = new SortedList<int, string> ();
			Dictionary<int, Dictionary<string, Dictionary<int, Dictionary<string,string>>>> dDimensionalPhrase = //D<Length, <CompletePOSPhrase, <OffsetIndex, <NewPOS, NewPhraseToTest>>>>
				new Dictionary<int, Dictionary<string, Dictionary<int, Dictionary<string,string>>>>();
			
			while (!srPOSPhrases.EndOfStream) {				
				string[] strsLine = srPOSPhrases.ReadLine ().Split ('^');
				slPOSPhrases.Add (slPOSPhrases.Count () + 1, strsLine [0]);
			}

			foreach (int intSentenceID in ((SortedList<int, string>)ss.SentenceList.OrderBy(a=>a.Key)).Keys) {
				string strSentence = ss.SentenceList [intSentenceID];
				List<int> lSemicolons = new List<int> ();
				List<int> lDashes = new List<int> ();
				List<int> lColons = new List<int> ();
				List<int> lCommas = new List<int> ();

				//foreach (Match m in Regex.Matches (strSentence, @"")) {

				//}
			}

//            for(int intLinePartIdx = 0; intLinePartIdx < strsLine.Length; intLinePartIdx++)
//            {
//					string strLinePart = strsLine[intLinePartIdx].Trim();
//
//                    switch (intLinePartIdx)
//                    {
//                        default:
//                            break;
//					case 0:
//						Console.WriteLine (strLinePart);
//                            break;
//					case 1:
//						Console.WriteLine ("Count: " + Convert.ToInt32 (strsLine [intLinePartIdx].Trim ()));
//						break;
//                    }
//            }
        }
    }
}
