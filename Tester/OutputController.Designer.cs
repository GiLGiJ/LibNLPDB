namespace NLPDB
{
    partial class OutputController
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
            this.cbxWordPositions = new System.Windows.Forms.CheckBox();
            this.cbxSentenceNumbers = new System.Windows.Forms.CheckBox();
            this.cbxPOS = new System.Windows.Forms.CheckBox();
            this.cbxChunks = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cbxWordPositions
            // 
            this.cbxWordPositions.AutoSize = true;
            this.cbxWordPositions.Location = new System.Drawing.Point(40, 12);
            this.cbxWordPositions.Name = "cbxWordPositions";
            this.cbxWordPositions.Size = new System.Drawing.Size(97, 17);
            this.cbxWordPositions.TabIndex = 0;
            this.cbxWordPositions.Text = "Word Positions";
            this.cbxWordPositions.UseVisualStyleBackColor = true;
            this.cbxWordPositions.CheckedChanged += new System.EventHandler(this.cbxWordPositions_CheckedChanged);
            // 
            // cbxSentenceNumbers
            // 
            this.cbxSentenceNumbers.AutoSize = true;
            this.cbxSentenceNumbers.Location = new System.Drawing.Point(40, 35);
            this.cbxSentenceNumbers.Name = "cbxSentenceNumbers";
            this.cbxSentenceNumbers.Size = new System.Drawing.Size(117, 17);
            this.cbxSentenceNumbers.TabIndex = 1;
            this.cbxSentenceNumbers.Text = "Sentence Numbers";
            this.cbxSentenceNumbers.UseVisualStyleBackColor = true;
            this.cbxSentenceNumbers.CheckedChanged += new System.EventHandler(this.cbxSentenceNumbers_CheckedChanged);
            // 
            // cbxPOS
            // 
            this.cbxPOS.AutoSize = true;
            this.cbxPOS.Location = new System.Drawing.Point(40, 58);
            this.cbxPOS.Name = "cbxPOS";
            this.cbxPOS.Size = new System.Drawing.Size(48, 17);
            this.cbxPOS.TabIndex = 2;
            this.cbxPOS.Text = "POS";
            this.cbxPOS.UseVisualStyleBackColor = true;
            this.cbxPOS.CheckedChanged += new System.EventHandler(this.cbxPOS_CheckedChanged);
            // 
            // cbxChunks
            // 
            this.cbxChunks.AutoSize = true;
            this.cbxChunks.Location = new System.Drawing.Point(40, 81);
            this.cbxChunks.Name = "cbxChunks";
            this.cbxChunks.Size = new System.Drawing.Size(62, 17);
            this.cbxChunks.TabIndex = 3;
            this.cbxChunks.Text = "Chunks";
            this.cbxChunks.UseVisualStyleBackColor = true;
            this.cbxChunks.CheckedChanged += new System.EventHandler(this.cbxChunks_CheckedChanged);
            // 
            // OutputController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(202, 110);
            this.Controls.Add(this.cbxChunks);
            this.Controls.Add(this.cbxPOS);
            this.Controls.Add(this.cbxSentenceNumbers);
            this.Controls.Add(this.cbxWordPositions);
            this.Name = "OutputController";
            this.Text = "OutputController";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OutputController_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbxWordPositions;
        private System.Windows.Forms.CheckBox cbxSentenceNumbers;
        private System.Windows.Forms.CheckBox cbxPOS;
        private System.Windows.Forms.CheckBox cbxChunks;
    }
}