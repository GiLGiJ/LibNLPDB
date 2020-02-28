using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibNLPDB.Parser
{
    public class VB:Word
    {
		public Dictionary<int, VB> dVB = new Dictionary<int, VB>();
		public Dictionary<int, JJ> dModifiers = new Dictionary<int, JJ>();
		public Dictionary<int, string> dTO = new Dictionary<int, string>();

    }
}
