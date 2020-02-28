using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibNLPDB
{
    class SentencesByVerbs
    {
        public Dictionary<int, string> dVerbLocations = new Dictionary<int, string>();
        
        public SentencesByVerbs(ref Input input, ref Words words, ref POS pos)
        {
            foreach (int intPosition in pos.GetFuzzyPOSPositions("VB"))
            {
                dVerbLocations.Add(intPosition, words.GetPositionWord(intPosition));
            }
        }
    }
}
