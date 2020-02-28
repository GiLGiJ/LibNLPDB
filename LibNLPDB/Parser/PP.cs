using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibNLPDB.Parser
{
    public class PP:Word
    {
		public Dictionary<int, NN> dNouns = new Dictionary<int, NN>();
		public Dictionary<int, VB> dVerbs = new Dictionary<int, VB>();
    }
}
