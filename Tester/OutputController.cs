using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NLPDB
{
    public partial class OutputController : Form
    {
        TextBox tbxOutputCurrent;
        DataRecord drcCurrent;

        public OutputController(ref DataRecord drcMain, ref TextBox tbxOutput)
        {
            InitializeComponent();

            tbxOutputCurrent = tbxOutput;
            tbxOutputCurrent.Visible = true;
            drcCurrent = drcMain;
        }

        private void cbxWordPositions_CheckedChanged(object sender, EventArgs e)
        {
            tbxOutputCurrent.Text = BuildText().ToString();
        }

        private void cbxSentenceNumbers_CheckedChanged(object sender, EventArgs e)
        {
            tbxOutputCurrent.Text = BuildText().ToString();
        }

        private void cbxPOS_CheckedChanged(object sender, EventArgs e)
        {
            tbxOutputCurrent.Text = BuildText().ToString();
        }

        private void cbxChunks_CheckedChanged(object sender, EventArgs e)
        {
            tbxOutputCurrent.Text = BuildText().ToString();
        }

        private StringBuilder BuildText()
        {
            StringBuilder sbReturn = new StringBuilder();
            
			if (drcCurrent.libChunks.ChunkData.Count > 0) {
				foreach (int intUWID in drcCurrent.libWords.PositionWords.Keys.OrderBy(a => a)) {
					if (cbxChunks.Checked) {
						if (drcCurrent.libWords.SentenceFirstPositions.ContainsValue (intUWID)) {
							sbReturn.AppendLine ();
							sbReturn.Append ("<Chunk:");
							sbReturn.Append (drcCurrent.libChunks.ChunkData [drcCurrent.libWords.SentenceFirstPositions.Where (a => a.Value == intUWID).First ().Key]);
							sbReturn.Append (">");
						}
					} 
                
					if (cbxSentenceNumbers.Checked) {
						if (drcCurrent.libWords.SentenceFirstPositions.ContainsValue (intUWID)) {
							sbReturn.AppendLine ();
							sbReturn.Append ("<SentenceID:");
							sbReturn.Append (drcCurrent.libWords.SentenceFirstPositions.Where (a => a.Value == intUWID).First ().Key.ToString ());
							sbReturn.Append (">");
						}
					}

					if (cbxWordPositions.Checked) {
						sbReturn.Append ("<Position:");
						sbReturn.Append (intUWID.ToString ());
						sbReturn.Append (">");
					}

					if (cbxPOS.Checked) {
						sbReturn.Append ("<POS:");
						sbReturn.Append (drcCurrent.libPOS.POSs [intUWID]);
						sbReturn.Append (">");
					}

					sbReturn.Append (drcCurrent.libWords.GetPositionWord (intUWID));
					sbReturn.Append (" ");
				}
			} else {
				sbReturn.AppendLine("Load Chunks First");
			}

            return sbReturn;
        }

        private void OutputController_FormClosing(object sender, FormClosingEventArgs e)
        {
            tbxOutputCurrent.Visible = false;
        }
    }
}
