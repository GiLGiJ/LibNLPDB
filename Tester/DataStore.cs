using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NLPDB
{
    public class DataStore
    {
        public List<DataRecord> ldrMain = new List<DataRecord>();

        public void Add(DataRecord drdAdd)
        {
            ldrMain.Add(drdAdd);
        }

        public DataRecord GetRecord(string strFilename)
        {   
            DataRecord drdReturn = new DataRecord();

            foreach (DataRecord drdTemp in ldrMain)
            {
                try
                {
                    if (drdTemp.libInput.Filename == strFilename)
                    {
                        drdReturn = drdTemp;
                        break;
                    }
                }
                catch { }
            }

            return drdReturn;
        }

		public string[] GetFilenames(){
			List<string> lReturn = new List<string> ();

			foreach (DataRecord drTemp in ldrMain) {
				lReturn.Add (drTemp.GetFilename ());	
			}

			return (string[])lReturn.ToArray();
		}
    }
}
