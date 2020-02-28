using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NLPDB
{
    public class Noun
    {
        public int intSWID = 0;
        public string strWord = "";
        public List<string> lstrModifiers = new List<string>();
        public SemanticType semType; //Subject, Object, Verb, Modifier

        public enum SemanticType
        {
            Subject,
            Object
        }

        public Noun(){}
    }
}
