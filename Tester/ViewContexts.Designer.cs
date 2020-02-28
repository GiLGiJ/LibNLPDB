namespace NLPDB
{
    partial class ViewContexts
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
            this.cboSubjects = new System.Windows.Forms.ComboBox();
            this.cboActions = new System.Windows.Forms.ComboBox();
            this.cboObjects = new System.Windows.Forms.ComboBox();
            this.cboSModifiers = new System.Windows.Forms.ComboBox();
            this.cboOModifiers = new System.Windows.Forms.ComboBox();
            this.cboQuestions = new System.Windows.Forms.ComboBox();
            this.cboConditionalIfs = new System.Windows.Forms.ComboBox();
            this.cboPrepositions = new System.Windows.Forms.ComboBox();
            this.cboConditionalThens = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // cboSubjects
            // 
            this.cboSubjects.FormattingEnabled = true;
            this.cboSubjects.Location = new System.Drawing.Point(12, 12);
            this.cboSubjects.Name = "cboSubjects";
            this.cboSubjects.Size = new System.Drawing.Size(121, 21);
            this.cboSubjects.TabIndex = 0;
            // 
            // cboActions
            // 
            this.cboActions.FormattingEnabled = true;
            this.cboActions.Location = new System.Drawing.Point(12, 39);
            this.cboActions.Name = "cboActions";
            this.cboActions.Size = new System.Drawing.Size(121, 21);
            this.cboActions.TabIndex = 1;
            // 
            // cboObjects
            // 
            this.cboObjects.FormattingEnabled = true;
            this.cboObjects.Location = new System.Drawing.Point(12, 66);
            this.cboObjects.Name = "cboObjects";
            this.cboObjects.Size = new System.Drawing.Size(121, 21);
            this.cboObjects.TabIndex = 2;
            // 
            // cboSModifiers
            // 
            this.cboSModifiers.FormattingEnabled = true;
            this.cboSModifiers.Location = new System.Drawing.Point(139, 12);
            this.cboSModifiers.Name = "cboSModifiers";
            this.cboSModifiers.Size = new System.Drawing.Size(121, 21);
            this.cboSModifiers.TabIndex = 3;
            // 
            // cboOModifiers
            // 
            this.cboOModifiers.FormattingEnabled = true;
            this.cboOModifiers.Location = new System.Drawing.Point(139, 66);
            this.cboOModifiers.Name = "cboOModifiers";
            this.cboOModifiers.Size = new System.Drawing.Size(121, 21);
            this.cboOModifiers.TabIndex = 4;
            // 
            // cboQuestions
            // 
            this.cboQuestions.FormattingEnabled = true;
            this.cboQuestions.Location = new System.Drawing.Point(12, 135);
            this.cboQuestions.Name = "cboQuestions";
            this.cboQuestions.Size = new System.Drawing.Size(121, 21);
            this.cboQuestions.TabIndex = 5;
            // 
            // cboConditionalIfs
            // 
            this.cboConditionalIfs.FormattingEnabled = true;
            this.cboConditionalIfs.Location = new System.Drawing.Point(12, 162);
            this.cboConditionalIfs.Name = "cboConditionalIfs";
            this.cboConditionalIfs.Size = new System.Drawing.Size(121, 21);
            this.cboConditionalIfs.TabIndex = 6;
            // 
            // cboPrepositions
            // 
            this.cboPrepositions.FormattingEnabled = true;
            this.cboPrepositions.Location = new System.Drawing.Point(12, 108);
            this.cboPrepositions.Name = "cboPrepositions";
            this.cboPrepositions.Size = new System.Drawing.Size(121, 21);
            this.cboPrepositions.TabIndex = 7;
            // 
            // cboConditionalThens
            // 
            this.cboConditionalThens.FormattingEnabled = true;
            this.cboConditionalThens.Location = new System.Drawing.Point(139, 162);
            this.cboConditionalThens.Name = "cboConditionalThens";
            this.cboConditionalThens.Size = new System.Drawing.Size(121, 21);
            this.cboConditionalThens.TabIndex = 8;
            // 
            // ViewContexts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(656, 427);
            this.Controls.Add(this.cboConditionalThens);
            this.Controls.Add(this.cboPrepositions);
            this.Controls.Add(this.cboConditionalIfs);
            this.Controls.Add(this.cboQuestions);
            this.Controls.Add(this.cboOModifiers);
            this.Controls.Add(this.cboSModifiers);
            this.Controls.Add(this.cboObjects);
            this.Controls.Add(this.cboActions);
            this.Controls.Add(this.cboSubjects);
            this.Name = "ViewContexts";
            this.Text = "ViewContexts";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cboSubjects;
        private System.Windows.Forms.ComboBox cboActions;
        private System.Windows.Forms.ComboBox cboObjects;
        private System.Windows.Forms.ComboBox cboSModifiers;
        private System.Windows.Forms.ComboBox cboOModifiers;
        private System.Windows.Forms.ComboBox cboQuestions;
        private System.Windows.Forms.ComboBox cboConditionalIfs;
        private System.Windows.Forms.ComboBox cboPrepositions;
        private System.Windows.Forms.ComboBox cboConditionalThens;
    }
}