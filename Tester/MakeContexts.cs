using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using LibNLPDB;

namespace NLPDB
{
    public partial class MakeContexts : Form
    {
        Input input;
        Words words;
        
		List<string> lstrNNVBs = new List<string> ();//NNx <optional>{0,2} VBx
		List<int> lintNNVBFirstUWIDs = new List<int> ();//NNx VBx
		List<int> lintNNoVBFirstUWIDs = new List<int> ();//NNx <optional> VBx
		List<int> lintNNooVBFirstUWIDs = new List<int>();//NNx <optional> <optional> VBx
		List<int> lintJJNN = new List<int>();//JJx NNx
		List<int> lintDTNN = new List<int>();//DT NNx
		List<int> lintDTJJNN = new List<int>();//DT JJx NNx
		List<int> lintVBNN = new List<int>();//VBx NNx
		List<int> lintVBoNN = new List<int>();//VBx <optional> NNx
		List<int> lintVBooNN = new List<int>();//VBx <optional> <optional> NNx
		Regex rgxNNVB = new Regex (@"NN VB");//NN VB
		Regex rgxVBNN = new Regex(@"");//VB NN
		Regex rgxINiNN = new Regex(@"");//IN <optional{1,}> NN
		Regex rgxJJNN = new Regex(@"");//JJ NN
		Regex rgxDToNN = new Regex(@"");//DT <optional{0,}> NN
		StreamReader srContexts = null;
		StreamWriter swContexts = null;
		Dictionary<int, Context> dContexts;
		List<Dictionary<int, string>> lContextsAuto = new List<Dictionary<int, string>>();
		string strFilename = "";
		int intCurrentContextID = 0;
		int intCurrentSubject = 0;
		int intCurrentObject = 0;
		int intCurrentAction = 0;
		Dictionary<int, Dictionary<int, int>> dIndexUWIDs =
			new Dictionary<int, Dictionary<int, int>>();//d<firstindex, d<length, uwid>>
		int intFirstIndex = -1;
		int intLastIndex = -1;
		string strHighlightedText = "";
		Dictionary<string, List<string>> dSModifiers= new Dictionary<string, List<string>>(); //d<subject, l<modifiers>>
		Dictionary<string, List<string>> dActions = new Dictionary<string, List<string>>(); //d<subject, l<actions>>
		Dictionary<string, List<string>> dObjects = new Dictionary<string, List<string>>(); //d<verb, l<objects>>
		Dictionary<string, List<string>> dOModifiers = new Dictionary<string, List<string>>(); //d<object, l<modifiers>>


		public string MakePatternsCode (string[] strsHighestCountPOSPhrases){
			StringBuilder sbReturn = new StringBuilder ();
			string strPOSPhraseTextNoSpace = "";

			foreach (string strPOSPhraseText in strsHighestCountPOSPhrases) {
				strPOSPhraseTextNoSpace = strPOSPhraseText.Replace (" ", "");
				sbReturn.AppendLine ("Regex rgx" + strPOSPhraseTextNoSpace + " = new Regex(@" + '"' + strPOSPhraseTextNoSpace + '"' + ");//" + strPOSPhraseText);
			}

			return sbReturn.ToString ();
		}

        //Form
		public MakeContexts(ref Input inputTemp, ref Words wordsTemp, ref POSPhrases ppTemp)
        {
            InitializeComponent();

            dContexts = new Dictionary<int, Context>();

            input = inputTemp;
            words = wordsTemp;
            strFilename = input.InsertStringIntoFilename("-Contexts");

            LoadXML();
            PopulateText();

			foreach (int intSentenceID in words.SentenceList.Keys.OrderBy(a=>a))
            {
				//AutoCreateContexts(words.SentenceFirstPositions[intSentenceID], words.SentenceLengths[intSentenceID], ref pos);
//                AutoCreateContexts2(words.SentenceFirstPositions[intSentenceID], words.SentenceLengths[intSentenceID], ref pos);
				//AutoCreateContexts3 (ref ppTemp);
//				dContexts.Add (intSentenceID, new Context ());

            }
        }

        private void MakeContexts_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveXML();
        }

        //New Context Control
        private void btnNewContext_Click(object sender, EventArgs e)
        {
            intCurrentContextID = dContexts.Count + 1;
            intCurrentSubject = 0;
            intCurrentAction = 0;
            intCurrentObject = 0;

            dContexts.Add(intCurrentContextID, new Context());

            lblContextID.Text = "ContextID: " + intCurrentContextID.ToString();
        }

        //File
        private void LoadXML()
        {
            if (File.Exists(strFilename))
            {
                //Create or Load Existing Data File
                srContexts = new StreamReader(strFilename);

                while (!srContexts.EndOfStream)
                {
                    string strLine = srContexts.ReadLine();
                    int intColumnCounter = 0;
                    int intContextID = 0;

                    foreach (string strWord in strLine.Split('`'))
                    {//ContextID`intSubject,intModifier`intS,intVerb`intV,intObject`intO,intOModifier`intS,intPreposition`intS,intQuestion`intS,intConditionalIf`intS,intConditionalThen
                        intColumnCounter++;

                        switch (intColumnCounter)
                        {
                            case 1:
                                //subject
                                intContextID = Convert.ToInt32(strWord.Trim());

                                if (!dContexts.ContainsKey(intContextID))
                                {
                                    dContexts.Add(intContextID, new Context());
                                    lblContextID.Text = "Context ID: " + strWord.Trim();
                                }
                                break;
                            case 2:
                                //subject modifier
                                foreach (string strModifier in strWord.Split())
                                {
                                    if (strModifier != "")
                                    {
                                        string[] strsModifierParts = strModifier.Split(',');

                                        int intSubject = Convert.ToInt32(strsModifierParts[0].Trim());
                                        int intModifier = Convert.ToInt32(strsModifierParts[1].Trim());

                                        dContexts[intContextID].AddSModifier(intSubject, intModifier);
                                    }
                                }
                                break;
                            case 3:
                                //verb
                                foreach (string strVerb in strWord.Split())
                                {
                                    if (strVerb != "")
                                    {
                                        string[] strsVerbParts = strVerb.Split(',');

                                        int intSubject = Convert.ToInt32(strsVerbParts[0]);
                                        int intVerb = Convert.ToInt32(strsVerbParts[1]);

                                        dContexts[intContextID].AddAction(intSubject, intVerb);
                                    }
                                }
                                break;
                            case 4:
                                //object
                                foreach (string strObject in strWord.Split())
                                {
                                    if (strObject != "")
                                    {
                                        string[] strsObjectParts = strObject.Split(',');

                                        int intVerb = Convert.ToInt32(strsObjectParts[0]);
                                        int intObject = Convert.ToInt32(strsObjectParts[1]);

                                        dContexts[intContextID].AddObject(intVerb, intObject);
                                    }
                                }
                                break;
                            case 5:
                                //object modifiers
                                foreach (string strOModifier in strWord.Split())
                                {
                                    if (strOModifier != "")
                                    {
                                        string[] strsOModifierParts = strOModifier.Split(',');

                                        int intObject = Convert.ToInt32(strsOModifierParts[0]);
                                        int intOModifier = Convert.ToInt32(strsOModifierParts[1]);

                                        dContexts[intContextID].AddOModifier(intObject, intOModifier);
                                    }
                                }
                                break;
                            case 6:
                                //prepositions
                                foreach (string strPreposition in strWord.Split())
                                {
                                    if (strPreposition != "")
                                    {
                                        string[] strsPrepositionParts = strPreposition.Split(',');

                                        int intSubject = Convert.ToInt32(strsPrepositionParts[0]);
                                        int intPreposition = Convert.ToInt32(strsPrepositionParts[1]);

                                        dContexts[intContextID].AddPreposition(intSubject, intPreposition);
                                    }
                                }
                                break;
                            case 7:
                                //question
                                foreach (string strQuestion in strWord.Split())
                                {
                                    if (strQuestion != "")
                                    {
                                        string[] strsQuestionParts = strQuestion.Split(',');

                                        int intSubject = Convert.ToInt32(strsQuestionParts[0]);
                                        int intQuestion = Convert.ToInt32(strsQuestionParts[1]);

                                        dContexts[intContextID].AddQuestion(intSubject, intQuestion);
                                    }
                                }
                                break;
                            case 8:
                                //conditional if
                                foreach (string strConditionalIf in strWord.Split())
                                {
                                    if (strConditionalIf != "")
                                    {
                                        string[] strsConditionalIfParts = strConditionalIf.Split(',');

                                        int intSubject = Convert.ToInt32(strsConditionalIfParts[0]);
                                        int intConditionalIf = Convert.ToInt32(strsConditionalIfParts[1]);

                                        dContexts[intContextID].AddConditionalIf(intSubject, intConditionalIf);
                                    }
                                }
                                break;
                            case 9:
                                //conditional then
                                foreach (string strConditionalThen in strWord.Split())
                                {
                                    if (strConditionalThen != "")
                                    {
                                        string[] strsConditionalThenParts = strConditionalThen.Split(',');

                                        int intSubject = Convert.ToInt32(strsConditionalThenParts[0]);
                                        int intConditionalThen = Convert.ToInt32(strsConditionalThenParts[1]);

                                        dContexts[intContextID].AddConditionalThen(intSubject, intConditionalThen);
                                    }
                                }
                                break;
                        }
                    }
                }
            } //else {} //Create it

            try
            {
                srContexts.Close();
            }
            catch { }
        }

        //public void SaveXML()
        //{//ContextID`intSubject,intModifier intSubject,intModifier`intS,intVerb intS,intV`intV,intObject intV...
        //    StringBuilder sbSave = new StringBuilder();
        //    int intRecordCount = 0;
            
        //    try
        //    {
        //        srContexts.Close();
        //    }
        //    catch { }

        //    swContexts = new StreamWriter(strFilename);

        //    try
        //    {
        //        foreach (int intContextID in dContexts.Keys.OrderBy(a => a))
        //        {
        //            Context cxtContext = dContexts[intContextID];
        //            //Dictionary<int, List<int>> dSMTemp = new Dictionary<int,List<int>>();
        //            //Dictionary<int, List<int>> dATemp = new Dictionary<int, List<int>>();
        //            //Dictionary<int, List<int>> dOTemp = new Dictionary<int, List<int>>();
        //            //Dictionary<int, List<int>> dOMTemp = new Dictionary<int, List<int>>();
        //            //Dictionary<int, List<int>> dPTemp = new Dictionary<int, List<int>>();
        //            //Dictionary<int, List<int>> dQTemp = new Dictionary<int, List<int>>();
        //            //Dictionary<int, List<int>> dCITemp = new Dictionary<int, List<int>>();
        //            //Dictionary<int, List<int>> dCTTemp = new Dictionary<int, List<int>>();
                    
        //            sbSave.Append(intContextID.ToString());
        //            sbSave.Append('`');
        //            sbSave.Append("cxtContext");
                    
        //            try
        //            {
        //                foreach (int intSubjectID in cxtContext.GetSubjectIDs())
        //                {
        //                    try
        //                    {
        //                        foreach (int intSModifier in cxtContext.GetSubjectModifiers(intSubjectID))
        //                        {
        //                            //if (!dSMTemp.ContainsKey(intSubjectID))
        //                            //{
        //                            //    dSMTemp.Add(intSubjectID, new List<int>());
        //                            //}

        //                            //dSMTemp[intSubjectID].Add(intSModifier);

        //                            sbSave.Append(intSubjectID.ToString());
        //                            sbSave.Append(',');
        //                            sbSave.Append(intSModifier.ToString());
        //                            sbSave.Append(' ');
        //                        }

        //                        sbSave.Remove(sbSave.Length - 1, 1);
        //                    }
        //                    catch { }

        //                    sbSave.Append('`');
        //                    sbSave.Append("intSModifier");

        //                    try
        //                    {
        //                        foreach (int intAction in cxtContext.GetSubjectActions(intSubjectID))
        //                        {
        //                            //if (!dATemp.ContainsKey(intSubjectID))
        //                            //{
        //                            //    dATemp.Add(intSubjectID, new List<int>());
        //                            //}

        //                            //dATemp[intSubjectID].Add(intAction);

        //                            sbSave.Append(intSubjectID.ToString());
        //                            sbSave.Append(',');
        //                            sbSave.Append(intAction.ToString());
        //                            sbSave.Append(' ');
        //                        }

        //                        sbSave.Remove(sbSave.Length - 1, 1);
        //                    }
        //                    catch { }

        //                    sbSave.Append('`');
        //                    sbSave.Append("intAction");

        //                    try
        //                    {
        //                        foreach (int intVerb in cxtContext.GetSubjectActions(intSubjectID))
        //                        {
        //                            foreach (int intObject in cxtContext.GetActionObject(intVerb))
        //                            {
        //                                //if (!dOTemp.ContainsKey(intVerb))
        //                                //{
        //                                //    dOTemp.Add(intVerb, new List<int>());
        //                                //}

        //                                //dOTemp[intVerb].Add(intObject);

        //                                sbSave.Append(intVerb.ToString());
        //                                sbSave.Append(',');
        //                                sbSave.Append(intObject.ToString());
        //                                sbSave.Append(' ');
        //                            }

        //                            sbSave.Remove(sbSave.Length - 1, 1);
        //                            sbSave.Append(' ');
        //                        }

        //                        sbSave.Remove(sbSave.Length - 1, 1);
        //                    }
        //                    catch { }

        //                    sbSave.Append('`');
        //                    sbSave.Append("intObject");

        //                    try
        //                    {
        //                        foreach (int intVerb in cxtContext.GetSubjectActions(intSubjectID))
        //                        {
        //                            foreach (int intObject in cxtContext.GetActionObject(intVerb))
        //                            {
        //                                foreach (int intOModifier in cxtContext.GetObjectModifer(intObject))
        //                                {
        //                                    //if (!dOMTemp.ContainsKey(intObject))
        //                                    //{
        //                                    //    dOMTemp.Add(intObject, new List<int>());
        //                                    //}

        //                                    //dOMTemp[intObject].Add(intOModifier);

        //                                    sbSave.Append(intObject.ToString());
        //                                    sbSave.Append(',');
        //                                    sbSave.Append(intOModifier.ToString());
        //                                    sbSave.Append(' ');
        //                                }

        //                            }
        //                        }

        //                        sbSave.Remove(sbSave.Length - 1, 1);
        //                    }
        //                    catch { }

        //                    sbSave.Append('`');
        //                    sbSave.Append("intOModifier");

        //                    try
        //                    {
        //                        foreach (int intPreposition in cxtContext.GetSubjectPreposition(intSubjectID))
        //                        {
        //                            //if (!dPTemp.ContainsKey(intSubjectID))
        //                            //{
        //                            //    dPTemp.Add(intSubjectID, new List<int>());
        //                            //}

        //                            //dPTemp[intSubjectID].Add(intPreposition);

        //                            sbSave.Append(intSubjectID.ToString());
        //                            sbSave.Append(',');
        //                            sbSave.Append(intPreposition.ToString());
        //                            sbSave.Append(' ');
        //                        }

        //                        sbSave.Remove(sbSave.Length - 1, 1);
        //                    }
        //                    catch { }

        //                    sbSave.Append('`');
        //                    sbSave.Append("intPreposition");

        //                    try
        //                    {
        //                        foreach (int intQuestion in cxtContext.GetSubjectQuestion(intSubjectID))
        //                        {
        //                            //if (!dQTemp.ContainsKey(intSubjectID))
        //                            //{
        //                            //    dQTemp.Add(intSubjectID, new List<int>());
        //                            //}

        //                            //dQTemp[intSubjectID].Add(intQuestion);

        //                            sbSave.Append(intSubjectID.ToString());
        //                            sbSave.Append(',');
        //                            sbSave.Append(intQuestion.ToString());
        //                            sbSave.Append(' ');
        //                        }

        //                        sbSave.Remove(sbSave.Length - 1, 1);
        //                    }
        //                    catch { }

        //                    sbSave.Append('`');
        //                    sbSave.Append("intQuestion");

        //                    try
        //                    {
        //                        foreach (int intConditionalIf in cxtContext.GetSubjectConditionalIf(intSubjectID))
        //                        {
        //                            //if (!dCITemp.ContainsKey(intSubjectID))
        //                            //{
        //                            //    dCITemp.Add(intSubjectID, new List<int>());
        //                            //}
        //                            //dCITemp[intSubjectID].Add(intConditionalIf);

        //                            sbSave.Append(intSubjectID.ToString());
        //                            sbSave.Append(',');
        //                            sbSave.Append(intConditionalIf.ToString());
        //                            sbSave.Append(' ');
        //                        }

        //                        sbSave.Remove(sbSave.Length - 1, 1);
        //                    }
        //                    catch { }

        //                    sbSave.Append('`');
        //                    sbSave.Append("intConditionalIf");

        //                    try
        //                    {
        //                        foreach (int intConditionalThen in cxtContext.GetSubjectConditionalThen(intSubjectID))
        //                        {
        //                            //if (dCTTemp.ContainsKey(intSubjectID))
        //                            //{
        //                            //    dCTTemp.Add(intSubjectID, new List<int>());
        //                            //}

        //                            //dCTTemp[intSubjectID].Add(intConditionalThen);

        //                            sbSave.Append(intSubjectID.ToString());
        //                            sbSave.Append(',');
        //                            sbSave.Append(intConditionalThen.ToString());
        //                            sbSave.Append(' ');
        //                        }

        //                        sbSave.Remove(sbSave.Length - 1, 1);
        //                    }
        //                    catch { }

        //                    //Check Line
        //                    intRecordCount = sbSave.ToString().Count(a => a == '`');

        //                    if (intRecordCount < 8)
        //                    {
        //                        for (int intRecordCounter = intRecordCount; intRecordCounter < 8; intRecordCounter++)
        //                        {
        //                            sbSave.Append('`');
        //                        }
        //                    }
        //                }
        //            }
        //            catch { }

        //            sbSave.AppendLine();
        //        }
        //    }
        //    catch { }

        //    swContexts.Write(sbSave.ToString());
        //    swContexts.Close();
        //}

        public void SaveXML()
        {
            try
            {
                srContexts.Close();
            }
            catch { }

            swContexts = new StreamWriter(strFilename);

            foreach (int intContextID in dContexts.Keys.OrderBy(a => a))
            {
                Context cxtContext = dContexts[intContextID];
                swContexts.Write(cxtContext.ToStringBuilder(intContextID).ToString());
            }

            swContexts.Close();

			swContexts = new StreamWriter(input.InsertStringIntoFilename("-Contexts-Auto"));

            foreach (Dictionary<int, string> dContextAuto in lContextsAuto)
            {
                foreach (int intContextID in dContextAuto.Keys)
                {
                    if (dContextAuto[intContextID].Trim() != "")
                    {
                        swContexts.WriteLine(dContextAuto[intContextID]);
                    }
                }
            }

            swContexts.Close();
        }

        //Use Words object to fill textbox and index dictionary
        private void PopulateText()
        {
            StringBuilder sbText = new StringBuilder();
            int intCurrentIndex = 0;

            foreach (int intUWID in words.PositionWords.Keys.OrderBy(a => a))
            {
                //int intFirstIndex = tbxText.pos
                string strWord = words.GetPositionWord(intUWID);
                int intLength = strWord.Length;

                sbText.Append(strWord);
                sbText.Append(" ");

                dIndexUWIDs.Add(intCurrentIndex, new Dictionary<int, int>());
                dIndexUWIDs[intCurrentIndex].Add(intLength, intUWID);

                intCurrentIndex += intLength + 1;

            }

            tbxText.Text = sbText.ToString();
        }

        //Get UWID from TextBox Mouse Controls
        private void ResetSelectionVariables(TextBox tbxWorking)
        {
            if (tbxWorking.SelectionLength == 0)
            {
                intFirstIndex = tbxWorking.SelectionStart;
                intLastIndex = intFirstIndex;
                strHighlightedText = "";
            }
            else
            {
                intFirstIndex = tbxWorking.SelectionStart;
                intLastIndex = intFirstIndex + tbxWorking.SelectionLength - 1;
                strHighlightedText = tbxWorking.SelectedText;
            }
        }

        private void ResetTextBoxSelection(TextBox tbxWorking)
        {
            tbxWorking.SelectionStart = intFirstIndex;
            tbxWorking.SelectionLength = intLastIndex - intFirstIndex + 1;
            strHighlightedText = tbxWorking.SelectedText;
        }

        private void FindBoundaries(TextBox tbxWorking)
        {
            bool bFindingBoundary = true;

            ResetSelectionVariables((TextBox)tbxWorking);

            //reverse highlighting support
            if (intFirstIndex > intLastIndex)
            {
                int intTempIndex = intFirstIndex;

                intFirstIndex = intLastIndex;
                intLastIndex = intTempIndex;
            }

            //find word boundaries
            while (bFindingBoundary)
            {
                if (intFirstIndex > 0)
                {
                    intFirstIndex--;

                    if (((TextBox)tbxWorking).Text[intFirstIndex] == ' ' ||
                        ((TextBox)tbxWorking).Text[intFirstIndex] == '\n')
                    {
                        intFirstIndex++;

                        bFindingBoundary = false;
                    }
                }
                else
                {
                    bFindingBoundary = false;
                }
            }

            bFindingBoundary = true;

            while (bFindingBoundary)
            {
                if (intLastIndex < ((TextBox)tbxWorking).Text.Length - 1)
                {
                    intLastIndex++;

                    if (((TextBox)tbxWorking).Text[intLastIndex] == ' ' ||
                        ((TextBox)tbxWorking).Text[intLastIndex] == '\r')
                    {
                        intLastIndex--;

                        bFindingBoundary = false;
                    }
                }
                else
                {
                    bFindingBoundary = false;
                }
            }

            ResetTextBoxSelection((TextBox)tbxWorking);
        }

        private void tbxText_MouseUp(object sender, MouseEventArgs e)
        {
            FindBoundaries((TextBox)sender);

            AddSelectedWords(((TextBox)sender).SelectedText);
        }

        private void AddSelectedWords(string strWordTemp)
        {
            int intSelectedUWID = 0;
            string strSelectedWord = "";

            foreach (string strPhraseWord in strWordTemp.Split())
            {
                intSelectedUWID = dIndexUWIDs[intFirstIndex][strPhraseWord.Length];
                strSelectedWord = strPhraseWord;

                if (rbnAddSubject.Checked)
                {
                    if (intCurrentContextID > 0) //Add Subject
                    {
                        intCurrentSubject = intSelectedUWID;
                        dContexts[intCurrentContextID].AddWord(intCurrentSubject, strSelectedWord);
                    }
                    else
                    {
                        MessageBox.Show("Create a New Context First");
                    }
                }
                else if (rbnAddSModifier.Checked) //Add Subject Modifier
                {
                    if (intCurrentSubject > 0)
                    {
                        dContexts[intCurrentContextID].AddWord(intSelectedUWID, strSelectedWord);
                        dContexts[intCurrentContextID].AddSModifier(intCurrentSubject, intSelectedUWID);
                    }
                    else
                    {
                        MessageBox.Show("Pick a Subject First");
                    }
                }
                else if (rbnAddAction.Checked) //Add Action
                {
                    if (intCurrentSubject > 0)
                    {
                        intCurrentAction = intSelectedUWID;
                        dContexts[intCurrentContextID].AddWord(intSelectedUWID, strSelectedWord);
                        dContexts[intCurrentContextID].AddAction(intCurrentSubject, intCurrentAction);
                    }
                    else
                    {
                        MessageBox.Show("Pick a Subject First");
                    }
                }
                else if (rbnAddObject.Checked) //Add Object
                {
                    if (intCurrentAction > 0)
                    {
                        intCurrentObject = intSelectedUWID;
                        dContexts[intCurrentContextID].AddWord(intCurrentObject, strSelectedWord);
                        dContexts[intCurrentContextID].AddObject(intCurrentAction, intCurrentObject);
                    }
                    else
                    {
                        MessageBox.Show("Pick a Verb First");
                    }
                }
                else if (rbnAddOModifier.Checked) //Add Object Modifier
                {
                    if (intCurrentObject > 0)
                    {
                        dContexts[intCurrentContextID].AddWord(intSelectedUWID, strSelectedWord);
                        dContexts[intCurrentContextID].AddSModifier(intCurrentObject, intSelectedUWID);
                    }
                    else
                    {
                        MessageBox.Show("Pick an Object First");
                    }
                }
                else if (rbnAddPreposition.Checked) //Add Preposition
                {
                    if (intCurrentSubject > 0)
                    {
                        dContexts[intCurrentContextID].AddWord(intSelectedUWID, strSelectedWord);
                        dContexts[intCurrentContextID].AddPreposition(intCurrentSubject, intSelectedUWID);
                    }
                    else
                    {
                        MessageBox.Show("Pick a Subject First");
                    }
                }
                else if (rbnAddQuestion.Checked) //Add Question
                {
                    if (intCurrentContextID > 0)
                    {
                        dContexts[intCurrentContextID].AddWord(intSelectedUWID, strSelectedWord);
                        dContexts[intCurrentContextID].AddQuestion(intCurrentSubject, intSelectedUWID);
                    }
                    else
                    {
                        MessageBox.Show("Pick a Subject First");
                    }
                }
                else if (rbnConditionalIf.Checked) //Add Conditional If
                {
                    if (intCurrentContextID > 0)
                    {
                        dContexts[intCurrentContextID].AddWord(intSelectedUWID, strSelectedWord);
                        dContexts[intCurrentContextID].AddConditionalIf(intCurrentSubject, intSelectedUWID);
                    }
                    else
                    {
                        MessageBox.Show("Pick a Subject First");
                    }
                }
                else if (rbnConditionalThen.Checked) //Add Conditional Then
                {
                    if (intCurrentContextID > 0)
                    {
                        dContexts[intCurrentContextID].AddWord(intSelectedUWID, strSelectedWord);
                        dContexts[intCurrentContextID].AddConditionalThen(intCurrentSubject, intSelectedUWID);
                    }
                    else
                    {
                        MessageBox.Show("Pick a Subject First");
                    }
                }

                intFirstIndex += strPhraseWord.Length + 1;
            }
        }

        public void CombineContexts()
        {   
            int[] ints = (int[])dContexts.Keys.ToArray<int>();

        }

        public void AnalyzeContext(int intContextID)
        {
            //
            
        }

		Dictionary<int, string> dContextsAuto = new Dictionary<int, string> ();
		int intCurrentPhrase = 1;

		private bool AddWordToPhrase(string strAddedWord){
			bool bReturn = false;

			try{
				dContextsAuto [intCurrentPhrase] += " " + strAddedWord;
				bReturn = true;
			}catch{
			}

			return bReturn;
		}

		private bool AddWordToLastPhrase(string strAddedWord){
			bool bReturn = false;

			try{
				dContextsAuto [intCurrentPhrase - 1] += " " + strAddedWord;
				bReturn = true;
			}catch{
			}

			return bReturn;
		}

		private void NewPhrase(string strFirstWord){
			if (dContexts.ContainsKey (intCurrentPhrase)) {
				if (!IsCurrentContextEmpty ()) {
					intCurrentPhrase++;
				}
			}

			dContextsAuto.Add (intCurrentPhrase, strFirstWord);
		}

		private bool IsCurrentContextEmpty(){
			return dContextsAuto [intCurrentPhrase].Trim() == "";
		}

        public void AutoCreateContexts(int intFirstUWID, int intLength, ref POS pos)
		{
			bool bV = false; //seen verb
			bool bPP = false; //in prep phrase
			bool bN = false; //last was noun
			List<string> lNewPhrase = new List<string>();
			List<string> lAddWordToPhrase = new List<string> ();
			List<string> lAddWordToLastPhrase = new List<string> ();

			for (int intUWID = intFirstUWID; intUWID < intFirstUWID + intLength; intUWID++) {
				string strWord = words.GetPositionWord (intUWID);
				string strPOS = words.GetPositionPOS (intUWID, ref pos);

				if (strWord != "pro") { //Data cleaning
					try {
						switch (strPOS.Substring (0, 2).ToUpper ()) {
						case "IN":
							bPP = true;
	                        
							lNewPhrase.Add (strPOS);

							bV = false;

							break;
						case "NN":
							if (bPP) {
								lAddWordToPhrase.Add (strWord);
								string strNextWord = words.GetPositionWord (intUWID + 1);
								string strNextPOS = words.GetPositionPOS(intUWID + 1, ref pos);

								if (strNextWord != "pro") { //Data cleaning
									if (strNextPOS != "NN") { //& if context(thisNN,nextNN)==ThisModifiesThat then don't inizialize a new context
										if (strNextPOS == "DT" || strNextPOS == "JJ"){
											//NN DT || NN ADJ collapses to NN NN ; continue
											string strNextPOS2 = "";
											try{
												strNextPOS2 = words.GetPositionWord(intUWID + 2);
											}catch{}

											if (strNextPOS2 == "NN"){
												lAddWordToPhrase.Add(strWord);
											}else{
												lNewPhrase.Add(strWord);
											}
										}else{
											lNewPhrase.Add (strWord);
										}
									} else { //NN NN
										//check codependence; if not dependent, start a new phrase, else add word to this phrase
										string strPhraseNext1 = strWord + " " + strNextWord;

		//								if (strPhraseNext1){
		//
		//								}
									}
								}

								bPP = false;
							} else {
								if (!bV) {
									lNewPhrase.Add (strWord);
								} else {
									lAddWordToPhrase.Add (strWord);
									lNewPhrase.Add ("");
								}
							}

							bV = false;

							break;
						case "VB":
							if (bV) {
								//multiple consecutive verbs
								lAddWordToLastPhrase.Add (strWord);
							} else {
								bV = true;
								lAddWordToPhrase.Add (strWord);
								lNewPhrase.Add ("");
							}
							break;
						case "DT":
							lNewPhrase.Add (strWord);
							break;
						case "JJ":
							lAddWordToPhrase.Add (strWord);
							break;
						default:
							if (!dContextsAuto.ContainsKey (intCurrentPhrase)) {
								lNewPhrase.Add ("");
							}

							lAddWordToPhrase.Add (strWord);

							bV = false;
							break;
						}
					} catch (Exception ex) {
					}
				}
			}

			foreach (int intCurrentPhraseTrim in dContextsAuto.Keys.ToList()) {
				dContextsAuto [intCurrentPhraseTrim] = dContextsAuto [intCurrentPhraseTrim].Trim ();
			}

			lContextsAuto.Add (dContextsAuto);
		}

        public void AutoCreateContexts2(int intFirstUWID, int intLength, ref POS pos)
        {
			Dictionary<int, string> dSentenceText = new Dictionary<int, string> ();
			Dictionary<int, string> dSentencePOS = new Dictionary<int, string> ();
			List<string> lstrBannedWords = new List<string> ();

			lstrBannedWords.Add ("rom");
			lstrBannedWords.Add ("pro");

			var getNNVBFirstUWIDs =
				from row in dSentencePOS
					where row.Key >= intFirstUWID
					where row.Key < intFirstUWID + dSentencePOS.Count - 1
					where row.Value.Contains("NN")
					where dSentencePOS [row.Key + 1].Contains("VB")
				select row.Key;
			
			var getNNoVBFirstUWIDs =
				from row in dSentencePOS
					where row.Key >= intFirstUWID
					where row.Key < intFirstUWID + dSentencePOS.Count - 2
					where row.Value.Contains("NN")
					where !dSentencePOS[row.Key + 1].Contains("CC")
					where dSentencePOS [row.Key + 2].Contains("VB")
				select row.Key;

			var getNNooVBFirstUWIDs =
				from row in dSentencePOS
					where row.Key >= intFirstUWID
					where row.Key < intFirstUWID + dSentencePOS.Count - 3
					where row.Value.Contains("NN")
					where !dSentencePOS[row.Key + 1].Contains("CC")
					where !dSentencePOS[row.Key + 2].Contains("CC")
					where dSentencePOS [row.Key + 3].Contains("VB")
				select row.Key;
			
			for (int intUWID = intFirstUWID; intUWID < intFirstUWID + intLength; intUWID++) {
				try{
					if (!lstrBannedWords.Contains(words.GetPositionWord (intUWID))){
						dSentenceText.Add(intUWID, words.GetPositionWord (intUWID));
						dSentencePOS.Add(intUWID, words.GetPositionPOS (intUWID, ref pos));
					}
					else{
						dSentenceText.Add(intUWID, "");
						dSentencePOS.Add(intUWID, "");
					}
				}catch{
				}
			}

			foreach (int intUWID in getNNVBFirstUWIDs) {
				string strWordTemp = words.GetPositionWord (intUWID);
				string strWordNextTemp = words.GetPositionWord (intUWID + 1);

				if (lstrBannedWords.Contains (strWordTemp) ||
				    lstrBannedWords.Contains (strWordNextTemp)) {
					string strA = "";
				} else {
					string strNNVB = strWordTemp + " " + strWordNextTemp;

					if (!lstrNNVBs.Contains (strNNVB)) {
						lintNNVBFirstUWIDs.Add (intUWID);
						lstrNNVBs.Add (strNNVB);
					}
				}
			}

			foreach (int intUWID in getNNoVBFirstUWIDs) {
				string strWordTemp = words.GetPositionWord (intUWID);
				string strWordNextTemp = words.GetPositionWord (intUWID + 2);

				if (lstrBannedWords.Contains (strWordTemp) ||
					lstrBannedWords.Contains (strWordNextTemp)) {
					string strA = "";
				} else {
					string strNNVB = strWordTemp + " " + strWordNextTemp;

					if (!lstrNNVBs.Contains (strNNVB)) {
						lintNNoVBFirstUWIDs.Add (intUWID);
						lstrNNVBs.Add (strNNVB);
					}
				}
			}

			foreach (int intUWID in getNNooVBFirstUWIDs) {
				string strWordTemp = words.GetPositionWord (intUWID);
				string strWordNextTemp = words.GetPositionWord (intUWID + 3);

				if (lstrBannedWords.Contains (strWordTemp) ||
					lstrBannedWords.Contains (strWordNextTemp)) {
					string strA = "";
				} else {
					string strNNVB = strWordTemp + " " + strWordNextTemp;

					if (!lstrNNVBs.Contains (strNNVB)) {
						lintNNooVBFirstUWIDs.Add (intUWID);
						lstrNNVBs.Add (strNNVB);
					}
				}
			}
        }

		public void AutoCreateContexts3(ref POSPhrases ppTemp){
			//28 - and - the last noun is paired with the next noun
			//11 - in - the last noun is inside the next noun
			// 5 - to - the last noun end a semantic phrase and the next word starts another semantic phrase
			// 2 - for - if for --> in service to, then for --> to
			// 2 - with - the last noun is modified by the next noun

			//Most Sure to Least Sure

			//DT NNx - always reduces to NNx, using the same x, and the property DT
			//DT JJx NNx - almost always (in the same clause) reduces to NNx, using the same x, and properties DT and JJx, possibly using different x
			//NNx VBx - often (in the same clause) 
			//NNx <optional> VBx
			//NNx <optional> <optional> VBx
			//JJx NNx
			//VBx NNx
			//VBx <optional> NNx
			//VBx <optional> <optional> NNx	
			//Nominal forms of Adjectives already have that modifier's special property set, ready to be switched for use
			//particular Prepositions can narrow the list of possible meanings selections between homonyms in the Object of the Preposition, ie. "use", as at end of last note
			//NNx <findMyFrequency>{1,2} VBx
			//VBx <findMyFrequency>{1,2} NNx
			//JJx DT NNx - almost always transforms to JJx <end phrase><start phrase> DT NNx, using their own x
			//VBx DT NNx - almost always transforms to VBx NNx; this rule duplicate functionality from the JJx DT NNx rule as well as DT NNx; it could be useful in spot checking the system (aka keeping a sane system)

			//Use POSPhrases to find common phrase types
			//Then find the most Meaningful common phrase types
			//Then map meaning to type


			//Metadata -
			//-Part of (a part of)
			//-Instance of (ie. one tiny propery value is changed)
			//--Copy of (like Instance of, with no property value change)
			//-Specification of (eg. Generalities-Specifics Hierarchy)

			WriteHighestCountPOSPhrasesFile(ref ppTemp);
//			StreamReader srHighestPOSPhrases = new StreamReader(input.InsertStringIntoFilename(@"-POSPhrases-OrderedByTopCount"));
//			StreamWriter swHighestPOSPhrases = new StreamWriter(input.InsertStringIntoFilename(@"-POSPhrases-Ordered-MeaningFiltered"));
//
//			while (!srHighestPOSPhrases.EndOfStream) {
//				string strLine = srHighestPOSPhrases.ReadLine ();
//				string strPOSPhrase = strLine.Split ('^') [0].Trim ();
//				int intPOSPhraseCount = Convert.ToInt32(strLine.Split ('^') [1].Trim ());
//				int intPOSPhraseLength = strPOSPhrase.Split ().Count ();
//				int intSampleFirstUWID = Convert.ToInt32(strLine.Split('^')[2].Trim());
//					
//				StringBuilder sbSample = new StringBuilder();
//
//				for(int intUWID = intSampleFirstUWID; intUWID < strPOSPhrase.Split().Count() + intSampleFirstUWID; intUWID++){
//					sbSample.Append (" ");
//					sbSample.Append(words.GetPositionWord(intUWID));
//				}
//					
//				sbSample.AppendLine ();
//
//				if (MessageBox.Show (sbSample.ToString() + strPOSPhrase, "Meaningful POSPhrase?", MessageBoxButtons.OKCancel) == DialogResult.OK) {
//					swHighestPOSPhrases.WriteLine(strPOSPhrase + " ^ " + intPOSPhraseCount.ToString ());
//				}
//			}
//
//			swHighestPOSPhrases.Close ();
		}

		public void WriteHighestCountPOSPhrasesFile(ref POSPhrases ppTemp){
			StreamWriter swHighestPOSPhrases = new StreamWriter(input.InsertStringIntoFilename(@"-POSPhrases-OrderedByTopCount"));

			var HighestPhraseCount =
				from posphrase in ppTemp.POSPhrasesD
				orderby posphrase.Value[0] descending
				select posphrase;

			int intHighestCount = HighestPhraseCount.First().Value[0];
			int intCutoff = (int) (intHighestCount * 0.067);//o.oo67

			if (intCutoff < 2) {
				intCutoff = 2;
			}

			foreach (KeyValuePair<string, int[]> kvpHighestFirst in HighestPhraseCount) {
				if (kvpHighestFirst.Value [0] < intCutoff) {
					break;
				}

				swHighestPOSPhrases.WriteLine (kvpHighestFirst.Key + " ^ " + kvpHighestFirst.Value [0].ToString() + " ^ " + kvpHighestFirst.Value[1].ToString());
			}

			swHighestPOSPhrases.Close ();
		}
    }
}
