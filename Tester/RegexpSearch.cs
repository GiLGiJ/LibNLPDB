using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using LibNLPDB;

namespace NLPDB
{
    public partial class RegexpSearch : Form
    {
        Input input = null;

        public RegexpSearch(ref Input inputTemp)
        {
            InitializeComponent();

            input = inputTemp;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            string strRegexp = tbxRegexp.Text;
            strRegexp = "[^ ]{1,} begat [^ ]{1,}";

            foreach (Match mtchText in Regex.Matches(input.InputText, strRegexp))
            {
                tbxOutput.AppendText(mtchText.Value + "\r\n");
            }
        }


    }
}
