using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibNLPDB;

namespace LibNLPDB
{
//start with the subject of each clause
//these are made into intra-subjective clauses
//those without subjects are glue, or inter-subjective clauses, and they have one, two or more subjects in surrounding (or attaching) intra-subjective clauses
/*
thought =
 sentence {1,} =
  subj {1,} ->
   intra-s {1,} -> inter-s {1,} (opt) -> intra-s {1,} (opt)
*/

//clauses gather into sentences, each noun encoding intra-phrasal information with a per-word structure that is internal to the noun, and each inter-s clause dictating the amounts and methods of integrating those intra-s clauses; inter-s clause may take as a glued (or connected) subject any noun in the sentence, not just the ones immediately around them, (conjecture: most will take the closest noun)
//sentences gather into whole thoughts (or paragraphs)
//thoughts combine into situations
//situations combine into books
//conjecture: there will be significantly different ranges of separation of property sets when combining; or, look for similar amounts of property connections between the participants of level integration

    class Sentence
    {
        class SubjectiveClause
        {
            class Integration { }
        }

        class NonSubjectiveClause
        {
            class Integration { }
        }

        class Integration { }

        public Sentence(string[] strsSentence, string[] strsPOS)
        {
            List<int> lNouns = new List<int>(); //contains WordPositions
            int intWordCount = strsSentence.Length;

            for (int intWordCounter = 0; intWordCounter < intWordCount; intWordCounter++)
            {
                string strWord = strsSentence[intWordCounter];
                string strPOS = strsPOS[intWordCounter];

                if (strPOS.Contains("NN"))
                {
                    lNouns.Add(intWordCounter);
                }

                //if (strPOS.Contains(..

                //open a search space of all definitions of strWord, and start decreasing possible instance definitions -OR- build minimal definition given other words in this clause
            }

            //foreach noun
            // examine words before and after ->
            //  look for certain phrase boundary words to build subjectives
            //  all other words build into non-subjectives, which are optional and which inform upon transformation of properties during clause integration
        }
    }

    class Paragraph
    {//an example of a purely coinciding sentence base would keep the same property-wise definition through each instance of each noun
    }

    class Situation
    { }

    class Book
    {//this holds one primary (most general) dataset
    }

    class Interbook
    {//make another dataset that combines all knowledge
    }
}
