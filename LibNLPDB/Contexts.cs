using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibNLPDB
{
    public class Context
    {
        private Dictionary<int, string> dWords = new Dictionary<int,string>(); //<uwid, word>
		private Dictionary<int, List<int>> dSubjects = new Dictionary<int, List<int>>(); //<sentenceID, List<subjectUWID>>
		private Dictionary<int, List<int>> dSModifiers = new Dictionary<int, List<int>>(); //<subjectUWID, List<modifierUWID>>
        private Dictionary<int, List<int>> dOModifiers = new Dictionary<int, List<int>>(); //<objectUWID, List<modifierUWID>>
        private Dictionary<int, List<int>> dActions = new Dictionary<int, List<int>>(); //<subjectUWID, List<verbUWID>>
        private Dictionary<int, List<int>> dObjects = new Dictionary<int, List<int>>(); //<subjectUWID, List<objectUWID>>
        private Dictionary<int, List<int>> dPrepositions = new Dictionary<int, List<int>>(); //<subjectUWID, List<prepositionUWID>>
        private Dictionary<int, List<int>> dQuestions = new Dictionary<int, List<int>>(); //<subjectUWID, List<questionUWID>>
        private Dictionary<int, List<int>> dConditionalIf = new Dictionary<int, List<int>>(); //<subjectUWID, List<conditionalIfUWID>>
        private Dictionary<int, List<int>> dConditionalThen = new Dictionary<int, List<int>>(); //<subjectUWID, List<conditionalThenUWID>>
        
        public Context() { }

        public void AddWord(int intUWID, string strWord)
        {
            if (!dWords.ContainsKey(intUWID))
            {
                dWords.Add(intUWID, strWord);
            }
        }

		public void AddSubject(int intSentenceID, int intSubjectUWID){
			if (dWords.ContainsKey (intSubjectUWID)) {
				if (!dSubjects.ContainsKey (intSentenceID)) {
					dSubjects.Add (intSentenceID, new List<int> ());
				}

				dSubjects [intSentenceID].Add (intSubjectUWID);
			}
		}

        public void AddSModifier(int intSubjectUWID, int intModifierUWID)
        {
            if (dWords.Keys.Contains(intSubjectUWID) && dWords.Keys.Contains(intModifierUWID))
            {
                if (!dSModifiers.ContainsKey(intSubjectUWID))
                {
                    dSModifiers.Add(intSubjectUWID, new List<int>());
                }

                dSModifiers[intSubjectUWID].Add(intModifierUWID);
            }
        }

        public void AddAction(int intSubjectUWID, int intActionUWID)
        {
            if (dWords.Keys.Contains(intSubjectUWID) && dWords.Keys.Contains(intActionUWID))
            {
                if (!dActions.ContainsKey(intSubjectUWID))
                {
                    dActions.Add(intSubjectUWID, new List<int>());
                }

                dActions[intSubjectUWID].Add(intActionUWID);
            }
        }

        public void AddObject(int intVerbUWID, int intObjectUWID)
        {
            if (dWords.Keys.Contains(intVerbUWID) && dWords.Keys.Contains(intObjectUWID))
            {
                if (!dObjects.ContainsKey(intVerbUWID))
                {
                    dObjects.Add(intVerbUWID, new List<int>());
                }

                dObjects[intVerbUWID].Add(intObjectUWID);
            }
        }

        public void AddOModifier(int intObjectUWID, int intOModifierUWID)
        {
            if (dWords.Keys.Contains(intObjectUWID) && dWords.Keys.Contains(intOModifierUWID))
            {
                if (!dObjects.ContainsKey(intObjectUWID))
                {
                    dObjects.Add(intObjectUWID, new List<int>());
                }

                dOModifiers[intObjectUWID].Add(intOModifierUWID);
            }
        }
        
        public void AddPreposition(int intSubjectUWID, int intPrepositionUWID) 
        {
            if (dWords.Keys.Contains(intSubjectUWID) && dWords.Keys.Contains(intPrepositionUWID))
            {
                if (!dPrepositions.ContainsKey(intSubjectUWID))
                {
                    dPrepositions.Add(intSubjectUWID, new List<int>());
                }

                dPrepositions[intSubjectUWID].Add(intPrepositionUWID);
            }
        }
        
		public void AddPrepositionObject (int intPrepositionUWID, int intPrepositionObjectUWID){
			
		}

        public void AddQuestion(int intSubjectUWID, int intQuestionUWID) 
        {
            if (dWords.Keys.Contains(intSubjectUWID) && dWords.Keys.Contains(intQuestionUWID))
            {
                if (!dQuestions.ContainsKey(intSubjectUWID))
                {
                    dQuestions.Add(intSubjectUWID, new List<int>());
                }

                dQuestions[intSubjectUWID].Add(intQuestionUWID);
            }
        }

        public void AddConditionalIf(int intSubjectUWID, int intConditionalIfUWID)
        {
            if (dWords.Keys.Contains(intSubjectUWID) && dWords.Keys.Contains(intConditionalIfUWID))
            {
                if (!dConditionalIf.ContainsKey(intSubjectUWID))
                {
                    dConditionalIf.Add(intSubjectUWID, new List<int>());
                }

                dConditionalIf[intSubjectUWID].Add(intConditionalIfUWID);
            }
        }

        public void AddConditionalThen(int intSubjectUWID, int intConditionalThenUWID)
        {
            if (dWords.Keys.Contains(intSubjectUWID) && dWords.Keys.Contains(intConditionalThenUWID))
            {
                if (!dConditionalThen.ContainsKey(intSubjectUWID))
                {
                    dConditionalThen.Add(intSubjectUWID, new List<int>());
                }

                dConditionalThen[intSubjectUWID].Add(intConditionalThenUWID);
            }
        }

        public string GetUWIDWord(int intUWIDTemp)
        {
            return dWords[intUWIDTemp];
        }

        public List<int> GetSubjectIDs()
        {
            List<int> lintReturn = new List<int>();

            lintReturn.AddRange(dSModifiers.Keys);
            lintReturn.AddRange(dActions.Keys);

            lintReturn.Sort();

            return lintReturn.Distinct().ToList();
        }

        public List<int> GetSubjectModifiers(int intSubjectUWID)
        {
            return dSModifiers[intSubjectUWID];
        }

        public List<int> GetSubjectActions(int intSubjectUWID)
        {
            return dActions[intSubjectUWID];
        }

        public List<int> GetActionObject(int intActionUWID)
        {
            return dObjects[intActionUWID];
        }

        public List<int> GetObjectModifer(int intObjectUWID)
        {
            return dOModifiers[intObjectUWID];
        }

        public List<int> GetSubjectPreposition(int intSubjectUWID) 
        {
            return dPrepositions[intSubjectUWID];
        }

        public List<int> GetSubjectQuestion(int intSubjectUWID) 
        {
            return dQuestions[intSubjectUWID];
        }

        public List<int> GetSubjectConditionalIf(int intSubjectUWID) 
        {
            return dConditionalIf[intSubjectUWID];
        }

        public List<int> GetSubjectConditionalThen(int intSubjectUWID) 
        {
            return dConditionalThen[intSubjectUWID];
        }

        public StringBuilder ToStringBuilder(int intContextID)
        {
            StringBuilder sbReturn = new StringBuilder();
            List<int> lintSubjects = new List<int>();
            List<int> lintSModifiers = new List<int>();
            List<int> lintActions = new List<int>();
            List<int> lintObjects = new List<int>();
            List<int> lintOModifiers = new List<int>();
            List<int> lintPrepositions = new List<int>();
            List<int> lintQuestions = new List<int>();
            List<int> lintConditionalIf = new List<int>();
            List<int> lintConditionalThen = new List<int>();

            //Prepare to build return object
            foreach (int intSubject in dSModifiers.Keys)
            {
                if (!lintSubjects.Contains(intSubject))
                {
                    lintSubjects.Add(intSubject);
                }

                foreach (int intSModifier in dSModifiers[intSubject])
                {
                    if (!lintSModifiers.Contains(intSModifier))
                    {
                        lintSModifiers.Add(intSModifier);
                    }
                }
            }

            foreach (int intSubject in dActions.Keys)
            {
                if (!lintSubjects.Contains(intSubject))
                {
                    lintSubjects.Add(intSubject);
                }

                foreach (int intAction in dActions[intSubject])
                {
                    if (!lintActions.Contains(intAction))
                    {
                        lintActions.Add(intAction);
                    }
                }
            }

            foreach (int intSubject in dPrepositions.Keys)
            {
                if (!lintSubjects.Contains(intSubject))
                {
                    lintSubjects.Add(intSubject);
                }

                foreach (int intPreposition in dPrepositions[intSubject])
                {
                    if (!lintPrepositions.Contains(intPreposition))
                    {
                        lintPrepositions.Add(intPreposition);
                    }
                }
            }

            foreach (int intSubject in dQuestions.Keys)
            {
                if (!lintSubjects.Contains(intSubject))
                {
                    lintSubjects.Add(intSubject);
                }

                foreach (int intQuestion in dQuestions[intSubject])
                {
                    if (!lintQuestions.Contains(intQuestion))
                    {
                        lintQuestions.Add(intQuestion);
                    }
                }
            }

            foreach (int intSubject in dConditionalIf.Keys)
            {
                if (!lintSubjects.Contains(intSubject))
                {
                    lintSubjects.Add(intSubject);
                }

                foreach (int intConditionalIf in dConditionalIf[intSubject])
                {
                    if (!lintConditionalIf.Contains(intConditionalIf))
                    {
                        lintConditionalIf.Add(intConditionalIf);
                    }
                }
            }

            foreach (int intSubject in dConditionalThen.Keys)
            {
                if (!lintSubjects.Contains(intSubject))
                {
                    lintSubjects.Add(intSubject);
                }

                foreach (int intConditionalThen in dConditionalThen[intSubject])
                {
                    if (!lintConditionalThen.Contains(intConditionalThen))
                    {
                        lintConditionalThen.Add(intConditionalThen);
                    }
                }
            }

            foreach (int intAction in dObjects.Keys)
            {
                if (!lintActions.Contains(intAction))
                {
                    lintActions.Add(intAction);
                }

                foreach (int intObject in dObjects[intAction])
                {
                    if (!lintObjects.Contains(intObject))
                    {
                        lintObjects.Add(intObject);
                    }
                }
            }

            foreach (int intObject in dOModifiers.Keys)
            {
                if (!lintObjects.Contains(intObject))
                {
                    lintObjects.Add(intObject);
                }

                foreach (int intOModifier in dOModifiers[intObject])
                {
                    if (!lintOModifiers.Contains(intOModifier))
                    {
                        lintOModifiers.Add(intOModifier);
                    }
                }
            }

            //Build return object
            sbReturn.Append(intContextID.ToString());
            sbReturn.Append('`');

            foreach (int intSubject in lintSubjects.OrderBy(a => a))
            {
                sbReturn.Append(GetUWIDWord(intSubject));
                sbReturn.Append(' ');
            }

            if (sbReturn[sbReturn.Length - 1] == ' ')
            {
                sbReturn.Remove(sbReturn.Length - 1, 1);
            }

            sbReturn.Append('`');

            foreach (int intSModifier in lintSModifiers.OrderBy(a => a))
            {
                sbReturn.Append(GetUWIDWord(intSModifier));
                sbReturn.Append(' ');
            }

            if (sbReturn[sbReturn.Length - 1] == ' ')
            {
                sbReturn.Remove(sbReturn.Length - 1, 1);
            }

            sbReturn.Append('`');

            foreach (int intObject in lintObjects.OrderBy(a => a))
            {
                sbReturn.Append(GetUWIDWord(intObject));
                sbReturn.Append(' ');
            }

            if (sbReturn[sbReturn.Length - 1] == ' ')
            {
                sbReturn.Remove(sbReturn.Length - 1, 1);
            }

            sbReturn.Append('`');

            foreach (int intOModifier in lintOModifiers.OrderBy(a => a))
            {
                sbReturn.Append(GetUWIDWord(intOModifier));
                sbReturn.Append(' ');
            }

            if (sbReturn[sbReturn.Length - 1] == ' ')
            {
                sbReturn.Remove(sbReturn.Length - 1, 1);
            }

            sbReturn.Append('`');

            foreach (int intPreposition in lintPrepositions.OrderBy(a => a))
            {
                sbReturn.Append(GetUWIDWord(intPreposition));
                sbReturn.Append(' ');
            }

            if (sbReturn[sbReturn.Length - 1] == ' ')
            {
                sbReturn.Remove(sbReturn.Length - 1, 1);
            }

            sbReturn.Append('`');

            foreach (int intQuestion in lintQuestions.OrderBy(a => a))
            {
                sbReturn.Append(GetUWIDWord(intQuestion));
                sbReturn.Append(' ');
            }

            if (sbReturn[sbReturn.Length - 1] == ' ')
            {
                sbReturn.Remove(sbReturn.Length - 1, 1);
            }

            sbReturn.Append('`');

            foreach (int intConditionalIf in lintConditionalIf.OrderBy(a => a))
            {
                sbReturn.Append(GetUWIDWord(intConditionalIf));
                sbReturn.Append(' ');
            }

            if (sbReturn[sbReturn.Length - 1] == ' ')
            {
                sbReturn.Remove(sbReturn.Length - 1, 1);
            }

            sbReturn.Append('`');

            foreach (int intConditionalThen in lintConditionalThen.OrderBy(a => a))
            {
                sbReturn.Append(GetUWIDWord(intConditionalThen));
                sbReturn.Append(' ');
            }

            if (sbReturn[sbReturn.Length - 1] == ' ')
            {
                sbReturn.Remove(sbReturn.Length - 1, 1);
            }

            sbReturn.AppendLine();

            return sbReturn;
        }
    }
}
