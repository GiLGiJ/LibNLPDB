namespace LibNLPDB
{
    partial class SentenceDetector
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tbxSentence = new System.Windows.Forms.TextBox();
            this.tbxOutput = new System.Windows.Forms.TextBox();
            this.lblNounGroups = new System.Windows.Forms.Label();
            this.lblVerbGroupCount = new System.Windows.Forms.Label();
            this.btnSet = new System.Windows.Forms.Button();
            this.cbxCombine = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // tbxSentence
            // 
            this.tbxSentence.BackColor = System.Drawing.Color.DarkKhaki;
            this.tbxSentence.Location = new System.Drawing.Point(12, 29);
            this.tbxSentence.Multiline = true;
            this.tbxSentence.Name = "tbxSentence";
            this.tbxSentence.Size = new System.Drawing.Size(268, 73);
            this.tbxSentence.TabIndex = 1;
            this.tbxSentence.Click += new System.EventHandler(this.tbxSentence_Click);
            // 
            // tbxOutput
            // 
            this.tbxOutput.BackColor = System.Drawing.Color.Khaki;
            this.tbxOutput.Location = new System.Drawing.Point(12, 108);
            this.tbxOutput.Multiline = true;
            this.tbxOutput.Name = "tbxOutput";
            this.tbxOutput.Size = new System.Drawing.Size(268, 91);
            this.tbxOutput.TabIndex = 2;
            // 
            // lblNounGroups
            // 
            this.lblNounGroups.AutoSize = true;
            this.lblNounGroups.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblNounGroups.Location = new System.Drawing.Point(12, 9);
            this.lblNounGroups.Name = "lblNounGroups";
            this.lblNounGroups.Size = new System.Drawing.Size(79, 13);
            this.lblNounGroups.TabIndex = 3;
            this.lblNounGroups.Text = "0 Noun Groups";
            // 
            // lblVerbGroupCount
            // 
            this.lblVerbGroupCount.AutoSize = true;
            this.lblVerbGroupCount.ForeColor = System.Drawing.Color.Silver;
            this.lblVerbGroupCount.Location = new System.Drawing.Point(151, 9);
            this.lblVerbGroupCount.Name = "lblVerbGroupCount";
            this.lblVerbGroupCount.Size = new System.Drawing.Size(75, 13);
            this.lblVerbGroupCount.TabIndex = 4;
            this.lblVerbGroupCount.Text = "0 Verb Groups";
            // 
            // btnSet
            // 
            this.btnSet.Location = new System.Drawing.Point(205, 205);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(75, 23);
            this.btnSet.TabIndex = 5;
            this.btnSet.Text = "Set";
            this.btnSet.UseVisualStyleBackColor = true;
            // 
            // cbxCombine
            // 
            this.cbxCombine.AutoSize = true;
            this.cbxCombine.Location = new System.Drawing.Point(29, 209);
            this.cbxCombine.Name = "cbxCombine";
            this.cbxCombine.Size = new System.Drawing.Size(130, 17);
            this.cbxCombine.TabIndex = 6;
            this.cbxCombine.Text = "Combine 2 Sentences";
            this.cbxCombine.UseVisualStyleBackColor = true;
            // 
            // SentenceDetector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Firebrick;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.cbxCombine);
            this.Controls.Add(this.btnSet);
            this.Controls.Add(this.lblVerbGroupCount);
            this.Controls.Add(this.lblNounGroups);
            this.Controls.Add(this.tbxOutput);
            this.Controls.Add(this.tbxSentence);
            this.Name = "SentenceDetector";
            this.Text = "Sentence Detector";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SentenceDetector_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbxSentence;
        private System.Windows.Forms.TextBox tbxOutput;
        private System.Windows.Forms.Label lblNounGroups;
        private System.Windows.Forms.Label lblVerbGroupCount;
        private System.Windows.Forms.Button btnSet;
        private System.Windows.Forms.CheckBox cbxCombine;
    }
}