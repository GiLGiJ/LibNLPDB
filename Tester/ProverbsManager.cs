using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NLPDB
{
    public partial class ProverbsManager : Form
    {
        private DataRecord drcProverbs;
        
        public ProverbsManager(ref DataStore dsrIn)
        {
            InitializeComponent();

            drcProverbs = dsrIn.ldrMain[0];

            StringBuilder sbSentences = new StringBuilder();

            foreach (int intSentenceID in drcProverbs.libWords.SentenceList.Keys.OrderBy(a=>a))
            {
                sbSentences.AppendLine(drcProverbs.libWords.SentenceList[intSentenceID]);
                sbSentences.AppendLine();
            }

            tbxProverbsText.Text = sbSentences.ToString();
        }
        
    }
}
