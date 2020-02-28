using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace LibNLPDB.Parser
{
    public class CC:Word //Dictionary<int, Word>
    {
        //public string strSelf = "";

        //CCs contain sentence head Words that belong to a CC 
        //(which is implied when absent) and other types of connections.
        //
        //Are all NNs subjects of sentences (or clauses)?
        //Are all sentence subjects (semantically?) NNs?
        
        //List types
        //For when this "and", etc. joins a list of 3 or more items,
        //for sentence clauses and for other CC uses
        public bool bNN = false;
        public bool bVB = false;
        public bool bJJ = false;
        public bool bSentence = false;
        public bool bOther = false;
        public CC() { }
        //public void AddWord()
        //{
        //    this.Add(this.Count() + 1, new Word());
        //}
        
        //public Word CurrentWord()
        //{
        //    if (this.Count() == 0)
        //    {
        //        this.AddWord();
        //    }

        //    return this[this.Count()];
        //}

        public string AutoType(string strPrevious, string strNext)
        {
            string strReturn = "Other";

            if (strPrevious.Contains("VB") && strNext.Contains("VB"))
            {
                bVB = true;
                strReturn = "VB";
            }
            else if (strPrevious.Contains("NN") && strNext.Contains("NN"))
            {
                bNN = true;
                strReturn = "NN";
            }
            else if (strPrevious.Contains("JJ") && strNext.Contains("JJ"))
            {
                bJJ = true;
                strReturn = "JJ";
            }
            else if (strPrevious.Trim() == "")
            {
                bSentence = true;
                strReturn = "S";
            }
            else
            {
                bOther = true;
                strReturn = "Other";
            }

            return strReturn;
        }
    }
}
