using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibNLPDB.Parser
{
    class Sentence:Dictionary<int, Word>
    {
        public Sentence(){}

        public int AddWord(string strWord)
        {
            int intReturn = -1;

            try
            {
                intReturn = this.Count + 1;

                this.Add(intReturn, new Word(0, 0, strWord));
            }
            catch (Exception ex) { }

            return intReturn;
        }

        public int AddWord(int intUWID, int intSWID, string strWord)
        {
            int intReturn = -1;

            try
            {
                intReturn = this.Count + 1;

                this.Add(intReturn, new Word(intUWID, intSWID, strWord));
            }
            catch (Exception ex) { }

            return intReturn;
        }
    }
}
