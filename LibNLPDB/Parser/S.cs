using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace LibNLPDB.Parser
{
    [Serializable]
    public class S: Dictionary<int, CC>
    {
        public int intSentenceID = -1;

        protected S(SerializationInfo info, StreamingContext context) { }
        public S() { }
        public void AddCC()
        {
            this.Add(this.Count() + 1, new CC());
        }

        public CC CurrentCC()
        {
            if (this.Count() == 0)
            {
                AddCC();
            }

            return this[this.Count()];
        }
    }
}
