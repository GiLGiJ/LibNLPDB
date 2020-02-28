using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibNLPDB
{
    //All these sequences will hold their WordPosition and use comparison functions
    //NN Sequence
    //--Tagged with Agent/Object, Sender/Reciever, etc.
    //--Tagged with Time, Place, .. any and all "isa" groups
    //NRef Sequence (includes DT & words like "that", "those")
    //--Holds the WordPosition of the NN that it references
    //VB Sequence
    //--Holds the WordPosition of the NN that it references
    //PP Sequence
    //--Holds the WordPosition of the NN that it references
    //ADJ Sequence
    //--Holds the WordPosition of the NN that it references
    //ADV Sequence
    //--Holds the WordPosition of the NN that it references
    //Try to collapse NRef, VB, PP, ADJ and ADV Sequences into their own NN Sequence first, 
    //  then explore the NN patterns in the places where they correspond with meanings.
    //Functions: PPDistance - It returns all the words between two prepositions in the same sentence.  
    //        ChunkDistance - It returns all the words between two clauses/phrases/chunks/parses in the same sentence.

    class MeaningSequences
    {
        //Input - Chunks, POSPhrases, Phrases, Parses(cross-reference/test?)
        //Data - NN/NRef/VB/PP/ADJ/ADV Sequences
        //Output - Nested NN and Nested VB Sequences
        //Output - The nesting structure in context of different word sequences is the real product of all this..?
        //Output - Meaning structures

        public MeaningSequences()
        {

        }
        //Dictionary<int, LibNLPDB.Chunk>
    }
}
