using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LibNLPDB
{
    public class Rgxs
    {
        public Regex rgxNumber = new Regex("[0-9]{1}");
        public Regex rgxNumbers = new Regex("[0-9]+");
        public Regex rgxOnlyNumbers = new Regex(@"^[0-9]{1,}$");
        public Regex rgxReferenceNoBook = new Regex("[0-9]{1,}:[0-9]{1,}");
        public Regex rgxReferenceWithBook = new Regex("[1-3]{0,1} {1,}[a-zA-Z]{1,} {1,}[0-9]{1,}:[0-9]{1,}");
        //public Regex rgxRemove = new Regex(@".?!;-,\)(][}{><@|#$%^&*_=+'" + '"' + "\t");
        public Regex rgxVerseRefs = new Regex(@"^[1-3]{0,1} {0,}([A-Za-z]{1,} {1,}){1,}[0-9]{1,}:[0-9]{1,}");
        public Regex rgxVerseRefsNoBook = new Regex(@"[0-9]{1,}:[0-9]{1,}");
        public Regex rgxRemovePunctuation = new Regex(@"[^0-9A-Za-z ]{1,}");
        public Regex rgxBibleReference1 = new Regex(@"[1-3]{0,1}[A-Za-z ]{4,}[0-9]{1,3}:[0-9]{1,3}"); //The letters in the book name (and ending space) are limited to 4 minimum (eg. "Job ")
        public Regex rgxSentencePunctuation = new Regex(@"[\.\?!]");
        public Regex rgxChunks = new Regex(@"\[(?<chunk>[^\]]{1,})\]");
        public Regex rgxParenthesis = new Regex(@"\([^\)\(]{1,}\)");
        public Regex rgxParse = new Regex(@"(?<o>\([^\(]{1,} ){1}|(?<word>[^\)]{1,}\){1}){1}|(?<c>\){1}){1}"); //open and close matches
        public Regex rgxOpen = new Regex(@"(?<o>\([A-Z]{1,} )");
        public Regex rgxWord = new Regex(@"(?<word>[^\)]{1,}\){1})");
        public Regex rgxClose = new Regex(@"(?<c>\){1})");
    }
}
