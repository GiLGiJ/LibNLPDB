using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LibNLPDB;

namespace NLPDB
{
    public partial class ViewContexts : Form
    {
        string strContextFilename = "";
        StreamReader srContexts = null;
        Dictionary<int, Context> dContexts = new Dictionary<int,Context>();

        public ViewContexts(string strContextFilenameTemp)
        {
            InitializeComponent();

            strContextFilename = strContextFilenameTemp;

            LoadXML();
            FillCombos();
        }

        private void LoadXML()
        {
            //Create or Load Existing Data File
            if (File.Exists(strContextFilename)) //Load it
            {
                srContexts = new StreamReader(strContextFilename);

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
 //////////                    lblContextID.Text = "Context ID: " + strWord.Trim();
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

        private void FillCombos() 
        {
            foreach (int intContextID in dContexts.Keys)
            {
                foreach (int intSubject in dContexts[intContextID].GetSubjectIDs())
                {
                    string strSubject = dContexts[intContextID].GetUWIDWord(intSubject);
                    
                    if (!cboSubjects.Items.Contains(strSubject))
                    {
                        cboSubjects.Items.Add(strSubject);
                    }

                    foreach (int intSModifier in dContexts[intContextID].GetSubjectModifiers(intSubject))
                    {
                        string strSModifier = dContexts[intContextID].GetUWIDWord(intSModifier);

                        if (!cboSModifiers.Items.Contains(strSModifier))
                        {
                            cboSModifiers.Items.Add(strSModifier);
                        }
                    }

                    foreach (int intAction in dContexts[intContextID].GetSubjectActions(intSubject))
                    {
                        string strAction = dContexts[intContextID].GetUWIDWord(intAction);

                        if (!cboActions.Items.Contains(strAction))
                        {
                            cboActions.Items.Add(strAction);
                        }

                        foreach (int intObject in dContexts[intContextID].GetActionObject(intAction)) 
                        {
                            string strObject = dContexts[intContextID].GetUWIDWord(intObject);

                            if (!cboObjects.Items.Contains(strObject))
                            {
                                cboObjects.Items.Add(strObject);
                            }

                            foreach (int intOModifier in dContexts[intContextID].GetObjectModifer(intObject))
                            {
                                string strOModifier = dContexts[intContextID].GetUWIDWord(intOModifier);

                                if (!cboOModifiers.Items.Contains(strOModifier))
                                {
                                    cboOModifiers.Items.Add(strOModifier);
                                }
                            }
                        }
                    }

                    foreach (int intPreposition in dContexts[intContextID].GetSubjectPreposition(intSubject))
                    {
                        string strPreposition = dContexts[intContextID].GetUWIDWord(intPreposition);

                        if (!cboPrepositions.Items.Contains(strPreposition))
                        {
                            cboPrepositions.Items.Add(strPreposition);
                        }
                    }

                    foreach (int intQuestion in dContexts[intContextID].GetSubjectQuestion(intSubject))
                    {
                        string strQuestion = dContexts[intContextID].GetUWIDWord(intQuestion);

                        if (!cboQuestions.Items.Contains(strQuestion))
                        {
                            cboQuestions.Items.Add(strQuestion);
                        }
                    }

                    foreach (int intConditionalIf in dContexts[intContextID].GetSubjectConditionalIf(intSubject))
                    {
                        string strConditionalIf = dContexts[intContextID].GetUWIDWord(intConditionalIf);

                        if (!cboConditionalIfs.Items.Contains(strConditionalIf))
                        {
                            cboConditionalIfs.Items.Add(strConditionalIf);
                        }
                    }

                    foreach (int intConditionalThen in dContexts[intContextID].GetSubjectConditionalThen(intSubject))
                    {
                        string strConditionalThen = dContexts[intContextID].GetUWIDWord(intConditionalThen);

                        if (!cboConditionalThens.Items.Contains(strConditionalThen))
                        {
                            cboConditionalThens.Items.Add(strConditionalThen);
                        }
                    }
                }
            }
        }
    }
}
