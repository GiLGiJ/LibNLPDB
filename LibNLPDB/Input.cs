using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LibNLPDB
{
    public class Input
    {
        string strInput = ""; //Full path with filename
        string strFilenameBase = "";
        string strFilenameExt = "";
        string strDataPath = "";
        string strInputText = "";

        public string Filename
        {
            get
            {
                return strInput;
            }
            set
            {
                strInput = value;
            }
        }

        public string Base
        {
            get
            {
                return strFilenameBase;
            }
            set
            {
                strFilenameBase = value;
            }
        }

        public string Ext
        {
            get
            {
                return strFilenameExt;
            }
            set
            {
                strFilenameExt = value;
            }
        }

        public string DataPath
        {
            get
            {
                return strDataPath;
            }
            set
            {
                strDataPath = value;
            }
        }

        public string InputText
        {
            get
            {
                return strInputText; 
            }
            set
            {
                strInputText = value; 
            }
        }

        public Input(string strInputFilename)
        {
            strInput = strInputFilename;

            strDataPath = strInput.Substring(0, strInput.LastIndexOf((char)'/')) + "/NLPData";

            ChopFilename();

            if (!Directory.Exists(strDataPath))
            {
                Directory.CreateDirectory(strDataPath);
            }

            if (!Directory.Exists(strDataPath + "/" + Base))
            {
                Directory.CreateDirectory(strDataPath + "/" + Base);
            }

            InputText = File.ReadAllText(strInput);
        }

        private void ChopFilename()
        {
            int intLastDotIdx = 0;
            string strFilenameNoExt = "";
            string strFilenameNoPath = "";

            intLastDotIdx = strInput.LastIndexOf(".");

			try{
            	strFilenameNoExt = strInput.Substring(0, intLastDotIdx);
			}catch{
				strFilenameNoExt = strInput;
			}

			strFilenameNoPath = strInput.Remove(0, strInput.LastIndexOf('/') + 1);
            strFilenameExt = strInput.TrimStart(strFilenameNoExt.ToCharArray());
            strFilenameBase = strFilenameNoPath.Trim(strFilenameExt.ToCharArray());
        }

        public string GetPOSPairsDirectory() 
        {
            return DataPath + "/" + Base + "/POSPairs";
        }

		public string GetPhrasesDirectory() 
		{
			return DataPath + "/" + Base + "/Phrases";
		}

		public string GetPOSPhrasesDirectory() 
		{
			return DataPath + "/" + Base + "/POSPhrases";
		}

		public string GetPOSPairsFilename(string strInsert)
        {
            return GetPOSPairsDirectory() + "/" + strInsert + Ext;
        }

        public string InsertStringIntoFilename(string strInsert)
        {
            return DataPath + "/" + Base + "/" + Base + strInsert + Ext;
        }

        public string GetArffFilename()
        {
            return DataPath + "/" + Base + "/" + Base + ".arff";
        }

        public string InsertStringIntoArffFilename(string strInsert)
        {
            return DataPath + "/" + Base + "/" + Base + strInsert + ".arff";
        }

        public int PhraseFilesCount()
        {
            int intReturn = 0;

            if (Directory.Exists(strDataPath + "/" + Base + "/Phrases"))
            {
                foreach (string strName in Directory.EnumerateFiles(DataPath + "/" +
                    Base + "/Phrases"))
                {
                    string strNameBase = strName.Remove(0, (DataPath + "/" + Base + "/Phrases/" + Base + "-").Length); //Do I even need this, since I'm using the Regex below?
						
					if (Regex.IsMatch(strNameBase, @"(?<length>[0-9]{1,})-Counts"))
                    {
                        //intReturn++;


						int intResult = (int)Convert.ToInt32(Regex.Match(strNameBase, @"(?<length>[0-9]{1,})-Counts").Groups["length"].Value.ToString());

						if (intResult > intReturn){
							intReturn = intResult;
						}
                    }
                }
            }

            return intReturn;
        }
    }
}
