namespace NLPDB
{
    partial class CurrentInfo
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
            this.lblPOSCount = new System.Windows.Forms.Label();
            this.lblMostFrequentPOSPhrase = new System.Windows.Forms.Label();
            this.lblSentenceCount = new System.Windows.Forms.Label();
            this.lblHighestWordCount = new System.Windows.Forms.Label();
            this.lblLongestPhrase = new System.Windows.Forms.Label();
            this.lblWordPositions = new System.Windows.Forms.Label();
            this.lblWordIDs = new System.Windows.Forms.Label();
            this.lblCurrentFilename = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblPOSCount
            // 
            this.lblPOSCount.AutoSize = true;
            this.lblPOSCount.Location = new System.Drawing.Point(12, 167);
            this.lblPOSCount.Name = "lblPOSCount";
            this.lblPOSCount.Size = new System.Drawing.Size(63, 13);
            this.lblPOSCount.TabIndex = 20;
            this.lblPOSCount.Text = "POS Count:";
            // 
            // lblMostFrequentPOSPhrase
            // 
            this.lblMostFrequentPOSPhrase.AutoSize = true;
            this.lblMostFrequentPOSPhrase.Location = new System.Drawing.Point(12, 196);
            this.lblMostFrequentPOSPhrase.Name = "lblMostFrequentPOSPhrase";
            this.lblMostFrequentPOSPhrase.Size = new System.Drawing.Size(163, 13);
            this.lblMostFrequentPOSPhrase.TabIndex = 19;
            this.lblMostFrequentPOSPhrase.Text = "Most Frequent POS Phrase Text:";
            // 
            // lblSentenceCount
            // 
            this.lblSentenceCount.AutoSize = true;
            this.lblSentenceCount.Location = new System.Drawing.Point(12, 113);
            this.lblSentenceCount.Name = "lblSentenceCount";
            this.lblSentenceCount.Size = new System.Drawing.Size(87, 13);
            this.lblSentenceCount.TabIndex = 18;
            this.lblSentenceCount.Text = "Sentence Count:";
            // 
            // lblHighestWordCount
            // 
            this.lblHighestWordCount.AutoSize = true;
            this.lblHighestWordCount.Location = new System.Drawing.Point(12, 86);
            this.lblHighestWordCount.Name = "lblHighestWordCount";
            this.lblHighestWordCount.Size = new System.Drawing.Size(106, 13);
            this.lblHighestWordCount.TabIndex = 17;
            this.lblHighestWordCount.Text = "Highest Word Count:";
            // 
            // lblLongestPhrase
            // 
            this.lblLongestPhrase.AutoSize = true;
            this.lblLongestPhrase.Location = new System.Drawing.Point(12, 139);
            this.lblLongestPhrase.Name = "lblLongestPhrase";
            this.lblLongestPhrase.Size = new System.Drawing.Size(84, 13);
            this.lblLongestPhrase.TabIndex = 16;
            this.lblLongestPhrase.Text = "Longest Phrase:";
            // 
            // lblWordPositions
            // 
            this.lblWordPositions.AutoSize = true;
            this.lblWordPositions.Location = new System.Drawing.Point(12, 59);
            this.lblWordPositions.Name = "lblWordPositions";
            this.lblWordPositions.Size = new System.Drawing.Size(81, 13);
            this.lblWordPositions.TabIndex = 15;
            this.lblWordPositions.Text = "Word Positions:";
            // 
            // lblWordIDs
            // 
            this.lblWordIDs.AutoSize = true;
            this.lblWordIDs.Location = new System.Drawing.Point(12, 34);
            this.lblWordIDs.Name = "lblWordIDs";
            this.lblWordIDs.Size = new System.Drawing.Size(55, 13);
            this.lblWordIDs.TabIndex = 14;
            this.lblWordIDs.Text = "Word IDs:";
            // 
            // lblCurrentFilename
            // 
            this.lblCurrentFilename.AutoSize = true;
            this.lblCurrentFilename.Location = new System.Drawing.Point(12, 9);
            this.lblCurrentFilename.Name = "lblCurrentFilename";
            this.lblCurrentFilename.Size = new System.Drawing.Size(89, 13);
            this.lblCurrentFilename.TabIndex = 13;
            this.lblCurrentFilename.Text = "Current Filename:";
            // 
            // CurrentInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(546, 219);
            this.Controls.Add(this.lblPOSCount);
            this.Controls.Add(this.lblMostFrequentPOSPhrase);
            this.Controls.Add(this.lblSentenceCount);
            this.Controls.Add(this.lblHighestWordCount);
            this.Controls.Add(this.lblLongestPhrase);
            this.Controls.Add(this.lblWordPositions);
            this.Controls.Add(this.lblWordIDs);
            this.Controls.Add(this.lblCurrentFilename);
            this.Name = "CurrentInfo";
            this.Text = "Current Info";
            this.Load += new System.EventHandler(this.CurrentInfo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPOSCount;
        private System.Windows.Forms.Label lblMostFrequentPOSPhrase;
        private System.Windows.Forms.Label lblSentenceCount;
        private System.Windows.Forms.Label lblHighestWordCount;
        private System.Windows.Forms.Label lblLongestPhrase;
        private System.Windows.Forms.Label lblWordPositions;
        private System.Windows.Forms.Label lblWordIDs;
        private System.Windows.Forms.Label lblCurrentFilename;
    }
}