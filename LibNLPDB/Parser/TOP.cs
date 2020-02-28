using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace LibNLPDB.Parser
{
    [Serializable]
    public class TOP:Dictionary<int, S>
    {
        public bool bINC = false;

        protected TOP(SerializationInfo info, StreamingContext context) { }
        public TOP() { }
        public void NewSentence()
        {
            this.Add(this.Count() + 1, new S());
            this[this.Count()].intSentenceID = this.Count();
        }
        public S CurrentSentence()
        {
            if(this.Count() == 0)
            {
                NewSentence();
            }

            return this[this.Count()];
        }

    }
}
