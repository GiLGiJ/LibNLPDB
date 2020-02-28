namespace NLPDB
{
    partial class GrammarParser
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
			//tbxTest
			this.tbxTest = new System.Windows.Forms.TextBox();
			this.tbxTest.Location = new System.Drawing.Point(12, 169);
			this.tbxTest.Multiline = true;
			this.tbxTest.Name = "tbxTest";
			this.tbxTest.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.tbxTest.Size = new System.Drawing.Size(292, 250);
			this.tbxTest.TabIndex = 0;
			this.tbxTest.Text = "This is a test on the Hand Chunk form, HandChunk.";
//			this.tbxTest.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tbxTest_MouseUp);

            this.SuspendLayout();
            // 
            // GrammarParser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(536, 469);
            this.Name = "GrammarParser";
            this.Text = "GrammarParser";
            this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.TextBox tbxTest;
    }
}