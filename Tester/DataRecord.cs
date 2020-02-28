using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibNLPDB;

namespace NLPDB
{
    public class DataRecord
    {
        public Input libInput;
        public Words libWords;
        public Phrases libPhrases;
        public POS libPOS;
        public CombinedPOS libCombinedPOS;
        public POSPhrases libPOSPhrases;
        public Parse libParse;
        public Chunks libChunks;
        public RichWords libRichWords;

		public string GetFilename() {
			return libInput.Filename;
		}
    }
}
