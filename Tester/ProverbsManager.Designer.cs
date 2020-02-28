namespace NLPDB
{
    partial class ProverbsManager
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
            this.tbxProverbsText = new System.Windows.Forms.TextBox();
            this.lblThings = new System.Windows.Forms.Label();
            this.lblPeople = new System.Windows.Forms.Label();
            this.lblPlaces = new System.Windows.Forms.Label();
            this.lbxThings = new System.Windows.Forms.ListBox();
            this.lbxPeople = new System.Windows.Forms.ListBox();
            this.lbxPlaces = new System.Windows.Forms.ListBox();
            this.lblPieces = new System.Windows.Forms.Label();
            this.lbxPieces = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // tbxProverbsText
            // 
            this.tbxProverbsText.Location = new System.Drawing.Point(12, 12);
            this.tbxProverbsText.Multiline = true;
            this.tbxProverbsText.Name = "tbxProverbsText";
            this.tbxProverbsText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxProverbsText.Size = new System.Drawing.Size(582, 55);
            this.tbxProverbsText.TabIndex = 0;
            // 
            // lblThings
            // 
            this.lblThings.AutoSize = true;
            this.lblThings.Location = new System.Drawing.Point(9, 85);
            this.lblThings.Name = "lblThings";
            this.lblThings.Size = new System.Drawing.Size(42, 13);
            this.lblThings.TabIndex = 1;
            this.lblThings.Text = "Things:";
            // 
            // lblPeople
            // 
            this.lblPeople.AutoSize = true;
            this.lblPeople.Location = new System.Drawing.Point(135, 85);
            this.lblPeople.Name = "lblPeople";
            this.lblPeople.Size = new System.Drawing.Size(43, 13);
            this.lblPeople.TabIndex = 2;
            this.lblPeople.Text = "People:";
            // 
            // lblPlaces
            // 
            this.lblPlaces.AutoSize = true;
            this.lblPlaces.Location = new System.Drawing.Point(261, 85);
            this.lblPlaces.Name = "lblPlaces";
            this.lblPlaces.Size = new System.Drawing.Size(42, 13);
            this.lblPlaces.TabIndex = 3;
            this.lblPlaces.Text = "Places:";
            // 
            // lbxThings
            // 
            this.lbxThings.FormattingEnabled = true;
            this.lbxThings.Location = new System.Drawing.Point(12, 101);
            this.lbxThings.Name = "lbxThings";
            this.lbxThings.Size = new System.Drawing.Size(120, 147);
            this.lbxThings.TabIndex = 4;
            // 
            // lbxPeople
            // 
            this.lbxPeople.FormattingEnabled = true;
            this.lbxPeople.Location = new System.Drawing.Point(138, 101);
            this.lbxPeople.Name = "lbxPeople";
            this.lbxPeople.Size = new System.Drawing.Size(120, 147);
            this.lbxPeople.TabIndex = 5;
            // 
            // lbxPlaces
            // 
            this.lbxPlaces.FormattingEnabled = true;
            this.lbxPlaces.Location = new System.Drawing.Point(264, 101);
            this.lbxPlaces.Name = "lbxPlaces";
            this.lbxPlaces.Size = new System.Drawing.Size(120, 147);
            this.lbxPlaces.TabIndex = 6;
            // 
            // lblPieces
            // 
            this.lblPieces.AutoSize = true;
            this.lblPieces.Location = new System.Drawing.Point(387, 85);
            this.lblPieces.Name = "lblPieces";
            this.lblPieces.Size = new System.Drawing.Size(42, 13);
            this.lblPieces.TabIndex = 7;
            this.lblPieces.Text = "Pieces:";
            // 
            // lbxPieces
            // 
            this.lbxPieces.FormattingEnabled = true;
            this.lbxPieces.Location = new System.Drawing.Point(390, 101);
            this.lbxPieces.Name = "lbxPieces";
            this.lbxPieces.Size = new System.Drawing.Size(120, 147);
            this.lbxPieces.TabIndex = 8;
            // 
            // ProverbsManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(606, 417);
            this.Controls.Add(this.lbxPieces);
            this.Controls.Add(this.lblPieces);
            this.Controls.Add(this.lbxPlaces);
            this.Controls.Add(this.lbxPeople);
            this.Controls.Add(this.lbxThings);
            this.Controls.Add(this.lblPlaces);
            this.Controls.Add(this.lblPeople);
            this.Controls.Add(this.lblThings);
            this.Controls.Add(this.tbxProverbsText);
            this.Name = "ProverbsManager";
            this.Text = "Proverbs Manager";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbxProverbsText;
        private System.Windows.Forms.Label lblThings;
        private System.Windows.Forms.Label lblPeople;
        private System.Windows.Forms.Label lblPlaces;
        private System.Windows.Forms.ListBox lbxThings;
        private System.Windows.Forms.ListBox lbxPeople;
        private System.Windows.Forms.ListBox lbxPlaces;
        private System.Windows.Forms.Label lblPieces;
        private System.Windows.Forms.ListBox lbxPieces;
    }
}