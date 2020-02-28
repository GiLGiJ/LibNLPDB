namespace NLPDB
{
    partial class GeneralitySimilarity
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
            this.lblNoun = new System.Windows.Forms.Label();
            this.lbxNounProperties = new System.Windows.Forms.ListBox();
            this.lbxPropertyRelatedNouns = new System.Windows.Forms.ListBox();
            this.lblNounProperties = new System.Windows.Forms.Label();
            this.lblPropertyRelatedNoun = new System.Windows.Forms.Label();
            this.lbiSharedProperties = new System.Windows.Forms.Label();
            this.lbxSharedProperties = new System.Windows.Forms.ListBox();
            this.lbxNoun = new System.Windows.Forms.ListBox();
            this.btnCreateDataFiles = new System.Windows.Forms.Button();
            this.btnPropertiesToNouns = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblNoun
            // 
            this.lblNoun.AutoSize = true;
            this.lblNoun.Location = new System.Drawing.Point(168, 21);
            this.lblNoun.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNoun.Name = "lblNoun";
            this.lblNoun.Size = new System.Drawing.Size(42, 17);
            this.lblNoun.TabIndex = 1;
            this.lblNoun.Text = "Noun";
            // 
            // lbxNounProperties
            // 
            this.lbxNounProperties.FormattingEnabled = true;
            this.lbxNounProperties.ItemHeight = 16;
            this.lbxNounProperties.Location = new System.Drawing.Point(52, 325);
            this.lbxNounProperties.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lbxNounProperties.Name = "lbxNounProperties";
            this.lbxNounProperties.Size = new System.Drawing.Size(159, 180);
            this.lbxNounProperties.Sorted = true;
            this.lbxNounProperties.TabIndex = 2;
            this.lbxNounProperties.SelectedIndexChanged += new System.EventHandler(this.lbxNounProperties_SelectedIndexChanged);
            // 
            // lbxPropertyRelatedNouns
            // 
            this.lbxPropertyRelatedNouns.FormattingEnabled = true;
            this.lbxPropertyRelatedNouns.ItemHeight = 16;
            this.lbxPropertyRelatedNouns.Location = new System.Drawing.Point(263, 52);
            this.lbxPropertyRelatedNouns.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lbxPropertyRelatedNouns.Name = "lbxPropertyRelatedNouns";
            this.lbxPropertyRelatedNouns.Size = new System.Drawing.Size(159, 180);
            this.lbxPropertyRelatedNouns.Sorted = true;
            this.lbxPropertyRelatedNouns.TabIndex = 3;
            this.lbxPropertyRelatedNouns.SelectedIndexChanged += new System.EventHandler(this.lbxPropertyRelatedNouns_SelectedIndexChanged);
            // 
            // lblNounProperties
            // 
            this.lblNounProperties.AutoSize = true;
            this.lblNounProperties.Location = new System.Drawing.Point(101, 294);
            this.lblNounProperties.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNounProperties.Name = "lblNounProperties";
            this.lblNounProperties.Size = new System.Drawing.Size(111, 17);
            this.lblNounProperties.TabIndex = 4;
            this.lblNounProperties.Text = "Noun Properties";
            // 
            // lblPropertyRelatedNoun
            // 
            this.lblPropertyRelatedNoun.AutoSize = true;
            this.lblPropertyRelatedNoun.Location = new System.Drawing.Point(259, 21);
            this.lblPropertyRelatedNoun.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPropertyRelatedNoun.Name = "lblPropertyRelatedNoun";
            this.lblPropertyRelatedNoun.Size = new System.Drawing.Size(160, 17);
            this.lblPropertyRelatedNoun.TabIndex = 5;
            this.lblPropertyRelatedNoun.Text = "Property Related Nouns";
            // 
            // lbiSharedProperties
            // 
            this.lbiSharedProperties.AutoSize = true;
            this.lbiSharedProperties.Location = new System.Drawing.Point(259, 294);
            this.lbiSharedProperties.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbiSharedProperties.Name = "lbiSharedProperties";
            this.lbiSharedProperties.Size = new System.Drawing.Size(123, 17);
            this.lbiSharedProperties.TabIndex = 6;
            this.lbiSharedProperties.Text = "Shared Properties";
            // 
            // lbxSharedProperties
            // 
            this.lbxSharedProperties.FormattingEnabled = true;
            this.lbxSharedProperties.ItemHeight = 16;
            this.lbxSharedProperties.Location = new System.Drawing.Point(263, 325);
            this.lbxSharedProperties.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lbxSharedProperties.Name = "lbxSharedProperties";
            this.lbxSharedProperties.Size = new System.Drawing.Size(159, 180);
            this.lbxSharedProperties.Sorted = true;
            this.lbxSharedProperties.TabIndex = 7;
            // 
            // lbxNoun
            // 
            this.lbxNoun.FormattingEnabled = true;
            this.lbxNoun.ItemHeight = 16;
            this.lbxNoun.Location = new System.Drawing.Point(52, 52);
            this.lbxNoun.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lbxNoun.Name = "lbxNoun";
            this.lbxNoun.Size = new System.Drawing.Size(159, 180);
            this.lbxNoun.Sorted = true;
            this.lbxNoun.TabIndex = 8;
            this.lbxNoun.SelectedIndexChanged += new System.EventHandler(this.lbxNoun_SelectedIndexChanged);
            // 
            // btnCreateDataFiles
            // 
            this.btnCreateDataFiles.Location = new System.Drawing.Point(548, 510);
            this.btnCreateDataFiles.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCreateDataFiles.Name = "btnCreateDataFiles";
            this.btnCreateDataFiles.Size = new System.Drawing.Size(152, 28);
            this.btnCreateDataFiles.TabIndex = 9;
            this.btnCreateDataFiles.Text = "Create Data Files";
            this.btnCreateDataFiles.UseVisualStyleBackColor = true;
            this.btnCreateDataFiles.Click += new System.EventHandler(this.btnCreateDataFiles_Click);
            // 
            // btnPropertiesToNouns
            // 
            this.btnPropertiesToNouns.Location = new System.Drawing.Point(548, 418);
            this.btnPropertiesToNouns.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnPropertiesToNouns.Name = "btnPropertiesToNouns";
            this.btnPropertiesToNouns.Size = new System.Drawing.Size(152, 28);
            this.btnPropertiesToNouns.TabIndex = 10;
            this.btnPropertiesToNouns.Text = "Properties to Nouns";
            this.btnPropertiesToNouns.UseVisualStyleBackColor = true;
            this.btnPropertiesToNouns.Click += new System.EventHandler(this.btnPropertiesToNouns_Click);
            // 
            // GeneralitySimilarity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(716, 553);
            this.Controls.Add(this.btnPropertiesToNouns);
            this.Controls.Add(this.btnCreateDataFiles);
            this.Controls.Add(this.lbxNoun);
            this.Controls.Add(this.lbxSharedProperties);
            this.Controls.Add(this.lbiSharedProperties);
            this.Controls.Add(this.lblPropertyRelatedNoun);
            this.Controls.Add(this.lblNounProperties);
            this.Controls.Add(this.lbxPropertyRelatedNouns);
            this.Controls.Add(this.lbxNounProperties);
            this.Controls.Add(this.lblNoun);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "GeneralitySimilarity";
            this.Text = "Generality-Similarity";
            this.Load += new System.EventHandler(this.GeneralitySimilarity_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblNoun;
        private System.Windows.Forms.ListBox lbxNounProperties;
        private System.Windows.Forms.ListBox lbxPropertyRelatedNouns;
        private System.Windows.Forms.Label lblNounProperties;
        private System.Windows.Forms.Label lblPropertyRelatedNoun;
        private System.Windows.Forms.Label lbiSharedProperties;
        private System.Windows.Forms.ListBox lbxSharedProperties;
        private System.Windows.Forms.ListBox lbxNoun;
        private System.Windows.Forms.Button btnCreateDataFiles;
        private System.Windows.Forms.Button btnPropertiesToNouns;
    }
}