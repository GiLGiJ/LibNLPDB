using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibNLPDB.Parser
{
    public class Word
    {
        public int intDocumentID;
        public int intUWID;
        public int intSWID;
        public string strWord;
        public string strPOS;
        public SemanticType semType; //Subject, Object, Verb, Modifier, Connector, Determiner, Preposition
        public SemanticThing semThing;
        public SemanticAction semAction;
        public SemanticModifier semModifier;
        public int intModifiesUWID; //UWID of Word modified by this Word (eg. Modifier Semantic Type is set)
        public List<int> lintModifiedByUWIDs = new List<int>(); //UWIDs of Words that modify this Word
        
		public enum SemanticType
        {
            Subject = 1,
            Object = 2,
            Verb = 4,
            Modifier = 8,
            Connector = 16,
            Determiner = 32, //includes "that", "where", etc
            Preposition = 64
        }

		public enum SemanticThing
		{
			Agent,
			Object
		}

		public enum SemanticAction
		{
			Action,
			Existence,
			Having
		}

		public enum SemanticModifier
		{
			Preposition,
			Determiner,
			Conjunction,
			TimeModifier,
			PersonalModifier,
			PlaceModifier,
			OtherModifier
		}

        public Word(){}
        
        public Word(int intUWIDTemp, int intSWIDTemp, string strWordTemp)
        {
            intUWID = intUWIDTemp;
            intSWID = intSWIDTemp;
            strWord = strWordTemp;
        }
    }
}
