using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibNLPDB.Parser
{
    public class NN:Word
    {
		public Dictionary<int, JJ> dModifiers = new Dictionary<int, JJ>();
		public DT dt;
		public SemanticThing semThing;
    }
}
