namespace NLPDB
{
    partial class Classify
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
            this.ofdlgCurrent = new System.Windows.Forms.OpenFileDialog();
            this.lbxWords = new System.Windows.Forms.ListBox();
            this.lbxPOS = new System.Windows.Forms.ListBox();
            this.lbxCategories = new System.Windows.Forms.ListBox();
            this.lblFilename = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbxWords
            // 
            this.lbxWords.FormattingEnabled = true;
            this.lbxWords.ItemHeight = 16;
            this.lbxWords.Location = new System.Drawing.Point(130, 12);
            this.lbxWords.Name = "lbxWords";
            this.lbxWords.Size = new System.Drawing.Size(235, 356);
            this.lbxWords.Sorted = true;
            this.lbxWords.TabIndex = 0;
            // 
            // lbxPOS
            // 
            this.lbxPOS.FormattingEnabled = true;
            this.lbxPOS.ItemHeight = 16;
            this.lbxPOS.Items.AddRange(new object[] {
            "Conjunction",
            "Modifier",
            "Noun",
            "Verb"});
            this.lbxPOS.Location = new System.Drawing.Point(12, 12);
            this.lbxPOS.Name = "lbxPOS";
            this.lbxPOS.Size = new System.Drawing.Size(112, 68);
            this.lbxPOS.Sorted = true;
            this.lbxPOS.TabIndex = 1;
            // 
            // lbxCategories
            // 
            this.lbxCategories.FormattingEnabled = true;
            this.lbxCategories.ItemHeight = 16;
            this.lbxCategories.Location = new System.Drawing.Point(12, 92);
            this.lbxCategories.Name = "lbxCategories";
            this.lbxCategories.Size = new System.Drawing.Size(112, 276);
            this.lbxCategories.Sorted = true;
            this.lbxCategories.TabIndex = 2;
            // 
            // lblFilename
            // 
            this.lblFilename.AutoSize = true;
            this.lblFilename.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFilename.Location = new System.Drawing.Point(3, 18);
            this.lblFilename.Name = "lblFilename";
            this.lblFilename.Size = new System.Drawing.Size(38, 17);
            this.lblFilename.TabIndex = 3;
            this.lblFilename.Text = "File: ";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblFilename);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Location = new System.Drawing.Point(371, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(151, 103);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // Classify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 380);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lbxCategories);
            this.Controls.Add(this.lbxPOS);
            this.Controls.Add(this.lbxWords);
            this.Name = "Classify";
            this.Text = "Classify";
            this.Load += new System.EventHandler(this.Classify_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog ofdlgCurrent;
        private System.Windows.Forms.ListBox lbxWords;
        private System.Windows.Forms.ListBox lbxPOS;
        private System.Windows.Forms.ListBox lbxCategories;
        private System.Windows.Forms.Label lblFilename;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}