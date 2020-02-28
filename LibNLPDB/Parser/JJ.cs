using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibNLPDB.Parser
{
    public class JJ:Word //adverbs and adjectives; modifiers
    {
		public Dictionary<int, JJ> dModifiers = new Dictionary<int, JJ>();
		public SemanticModifier semModifier;
    }
}
