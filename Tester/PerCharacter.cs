using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NLPDB
{
    class PerCharacter
    { 
        public PerCharacter() { }

        public void MakeCharBlocks(string strInputPath)
        {
            MakeCharBlocks(strInputPath, 512);
        }

        public void MakeCharBlocks(string strInputPath, int intBlockSize)
        {
            StreamReader srInput = new StreamReader(strInputPath);
            char[] chrsInputBlock = (char[])Array.CreateInstance(typeof(char), intBlockSize);
            int intCurrentInputIndex = 0;

            while (!srInput.EndOfStream)
            {
                char chrNext = (char)Char.ConvertFromUtf32(srInput.Read(chrsInputBlock, intCurrentInputIndex, 1))[0];
                
                intCurrentInputIndex++;

                if (intCurrentInputIndex == 512)
                {
                    intCurrentInputIndex = 0;

                    ProcessCharBlock(chrsInputBlock);
                }
            }
        }

        public void ProcessCharBlock(char[] chrsInputBlock) //Process a block of letters
        {
			foreach (char chrCurrent in chrsInputBlock) {
				switch (chrCurrent) {
				default:
					break;

				case ' ':
					Space (chrCurrent);
					break;

				case '\r':
					Space (chrCurrent);
					break;

				case '\n':
					Space (chrCurrent);
					break;

				case '!':
					SentenceEndingPunctuation (chrCurrent);
					break;

				case '?':
					SentenceEndingPunctuation (chrCurrent);
					break;

				case '.':
					SentenceEndingPunctuation (chrCurrent);
					break;

				case ',':
					SentenceOtherPunctuation (chrCurrent);
					break;

				case '-':
					SentenceOtherPunctuation (chrCurrent);
					break;

				case ';':
					SentenceOtherPunctuation (chrCurrent);
					break;

				case ':':
					SentenceOtherPunctuation (chrCurrent);
					break;
				}
			}
        }

        public enum SentenceEnding
        {
            Period = '.',
            Question = '?',
            Exclamation = '!'
        }

        public enum SentenceOther
        {
            Comma = ',',
            Dash = '-',
            Semicolon = ';',
            Colon = ':'
        }

        public void SentenceEndingPunctuation(char chrCurrent)
        {
            //MostGeneral's MoreSpecific's MoreGeneral's MostSpecific's Type = instance
            //thisMetaData.thisClause.thisSentence.Punctuation.End = chrCurrent;
        }

		public void SentenceOtherPunctuation(char chrCurrent)
		{
			
		}

        public void Space(char chrCurrent)
        {
            //MostGeneral's MoreSpecific's Type = instance
            //if (thisMetaData.thisClause.currentWord == " "){
            //  end of word signal}
            //else {
            //  break}
        }
    }
}
