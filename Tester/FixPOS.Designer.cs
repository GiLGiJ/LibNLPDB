namespace NLPDB
{
    partial class FixPOS
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
            this.tbxNamesFileLocation = new System.Windows.Forms.TextBox();
            this.btnNames = new System.Windows.Forms.Button();
            this.lblUpdatePOS = new System.Windows.Forms.Label();
            this.ofdlgPOSUpdateFile = new System.Windows.Forms.OpenFileDialog();
            this.btnPOSWords = new System.Windows.Forms.Button();
            this.tbxPOSWordsFileLocation = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tbxNamesFileLocation
            // 
            this.tbxNamesFileLocation.Location = new System.Drawing.Point(160, 43);
            this.tbxNamesFileLocation.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbxNamesFileLocation.Name = "tbxNamesFileLocation";
            this.tbxNamesFileLocation.Size = new System.Drawing.Size(201, 22);
            this.tbxNamesFileLocation.TabIndex = 0;
            // 
            // btnNames
            // 
            this.btnNames.Location = new System.Drawing.Point(52, 39);
            this.btnNames.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnNames.Name = "btnNames";
            this.btnNames.Size = new System.Drawing.Size(100, 28);
            this.btnNames.TabIndex = 1;
            this.btnNames.Text = "Names";
            this.btnNames.UseVisualStyleBackColor = true;
            this.btnNames.Click += new System.EventHandler(this.btnNames_Click);
            // 
            // lblUpdatePOS
            // 
            this.lblUpdatePOS.AutoSize = true;
            this.lblUpdatePOS.Location = new System.Drawing.Point(21, 20);
            this.lblUpdatePOS.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUpdatePOS.Name = "lblUpdatePOS";
            this.lblUpdatePOS.Size = new System.Drawing.Size(87, 17);
            this.lblUpdatePOS.TabIndex = 2;
            this.lblUpdatePOS.Text = "Update POS";
            // 
            // ofdlgPOSUpdateFile
            // 
            this.ofdlgPOSUpdateFile.Filter = "Text files (*.txt)|*.txt|XML files (*.xml)|*.xml";
            // 
            // btnPOSWords
            // 
            this.btnPOSWords.Location = new System.Drawing.Point(52, 80);
            this.btnPOSWords.Name = "btnPOSWords";
            this.btnPOSWords.Size = new System.Drawing.Size(100, 28);
            this.btnPOSWords.TabIndex = 3;
            this.btnPOSWords.Text = "POS Words";
            this.btnPOSWords.UseVisualStyleBackColor = true;
            this.btnPOSWords.Click += new System.EventHandler(this.btnPOSWords_Click);
            // 
            // tbxPOSWordsFileLocation
            // 
            this.tbxPOSWordsFileLocation.Location = new System.Drawing.Point(160, 83);
            this.tbxPOSWordsFileLocation.Name = "tbxPOSWordsFileLocation";
            this.tbxPOSWordsFileLocation.Size = new System.Drawing.Size(201, 22);
            this.tbxPOSWordsFileLocation.TabIndex = 4;
            // 
            // FixPOS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 321);
            this.Controls.Add(this.tbxPOSWordsFileLocation);
            this.Controls.Add(this.btnPOSWords);
            this.Controls.Add(this.lblUpdatePOS);
            this.Controls.Add(this.btnNames);
            this.Controls.Add(this.tbxNamesFileLocation);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FixPOS";
            this.Text = "FixPOS";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbxNamesFileLocation;
        private System.Windows.Forms.Button btnNames;
        private System.Windows.Forms.Label lblUpdatePOS;
        private System.Windows.Forms.OpenFileDialog ofdlgPOSUpdateFile;
        private System.Windows.Forms.Button btnPOSWords;
        private System.Windows.Forms.TextBox tbxPOSWordsFileLocation;
    }
}