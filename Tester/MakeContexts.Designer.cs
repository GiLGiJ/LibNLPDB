namespace NLPDB
{
    partial class MakeContexts
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
            this.tbxText = new System.Windows.Forms.TextBox();
            this.btnNewContext = new System.Windows.Forms.Button();
            this.lblContextID = new System.Windows.Forms.Label();
            this.gbxAddToContext = new System.Windows.Forms.GroupBox();
            this.rbnConditionalThen = new System.Windows.Forms.RadioButton();
            this.rbnConditionalIf = new System.Windows.Forms.RadioButton();
            this.rbnAddQuestion = new System.Windows.Forms.RadioButton();
            this.rbnAddPreposition = new System.Windows.Forms.RadioButton();
            this.rbnAddOModifier = new System.Windows.Forms.RadioButton();
            this.rbnAddObject = new System.Windows.Forms.RadioButton();
            this.rbnAddAction = new System.Windows.Forms.RadioButton();
            this.rbnAddSModifier = new System.Windows.Forms.RadioButton();
            this.rbnAddSubject = new System.Windows.Forms.RadioButton();
            this.gbxAddToContext.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbxText
            // 
            this.tbxText.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tbxText.Location = new System.Drawing.Point(0, 111);
            this.tbxText.Multiline = true;
            this.tbxText.Name = "tbxText";
            this.tbxText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxText.Size = new System.Drawing.Size(624, 357);
            this.tbxText.TabIndex = 0;
            this.tbxText.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tbxText_MouseUp);
            // 
            // btnNewContext
            // 
            this.btnNewContext.Location = new System.Drawing.Point(12, 12);
            this.btnNewContext.Name = "btnNewContext";
            this.btnNewContext.Size = new System.Drawing.Size(84, 23);
            this.btnNewContext.TabIndex = 1;
            this.btnNewContext.Text = "New Context";
            this.btnNewContext.UseVisualStyleBackColor = true;
            this.btnNewContext.Click += new System.EventHandler(this.btnNewContext_Click);
            // 
            // lblContextID
            // 
            this.lblContextID.AutoSize = true;
            this.lblContextID.Location = new System.Drawing.Point(102, 17);
            this.lblContextID.Name = "lblContextID";
            this.lblContextID.Size = new System.Drawing.Size(60, 13);
            this.lblContextID.TabIndex = 8;
            this.lblContextID.Text = "Context ID:";
            // 
            // gbxAddToContext
            // 
            this.gbxAddToContext.Controls.Add(this.rbnConditionalThen);
            this.gbxAddToContext.Controls.Add(this.rbnConditionalIf);
            this.gbxAddToContext.Controls.Add(this.rbnAddQuestion);
            this.gbxAddToContext.Controls.Add(this.rbnAddPreposition);
            this.gbxAddToContext.Controls.Add(this.rbnAddOModifier);
            this.gbxAddToContext.Controls.Add(this.rbnAddObject);
            this.gbxAddToContext.Controls.Add(this.rbnAddAction);
            this.gbxAddToContext.Controls.Add(this.rbnAddSModifier);
            this.gbxAddToContext.Controls.Add(this.rbnAddSubject);
            this.gbxAddToContext.Location = new System.Drawing.Point(201, 12);
            this.gbxAddToContext.Name = "gbxAddToContext";
            this.gbxAddToContext.Size = new System.Drawing.Size(411, 93);
            this.gbxAddToContext.TabIndex = 11;
            this.gbxAddToContext.TabStop = false;
            this.gbxAddToContext.Text = "Add to Context:";
            // 
            // rbnConditionalThen
            // 
            this.rbnConditionalThen.AutoSize = true;
            this.rbnConditionalThen.Location = new System.Drawing.Point(298, 65);
            this.rbnConditionalThen.Name = "rbnConditionalThen";
            this.rbnConditionalThen.Size = new System.Drawing.Size(105, 17);
            this.rbnConditionalThen.TabIndex = 8;
            this.rbnConditionalThen.TabStop = true;
            this.rbnConditionalThen.Text = "Conditional Then";
            this.rbnConditionalThen.UseVisualStyleBackColor = true;
            // 
            // rbnConditionalIf
            // 
            this.rbnConditionalIf.AutoSize = true;
            this.rbnConditionalIf.Location = new System.Drawing.Point(298, 42);
            this.rbnConditionalIf.Name = "rbnConditionalIf";
            this.rbnConditionalIf.Size = new System.Drawing.Size(86, 17);
            this.rbnConditionalIf.TabIndex = 7;
            this.rbnConditionalIf.TabStop = true;
            this.rbnConditionalIf.Text = "Conditional If";
            this.rbnConditionalIf.UseVisualStyleBackColor = true;
            // 
            // rbnAddQuestion
            // 
            this.rbnAddQuestion.AutoSize = true;
            this.rbnAddQuestion.Location = new System.Drawing.Point(298, 19);
            this.rbnAddQuestion.Name = "rbnAddQuestion";
            this.rbnAddQuestion.Size = new System.Drawing.Size(67, 17);
            this.rbnAddQuestion.TabIndex = 6;
            this.rbnAddQuestion.TabStop = true;
            this.rbnAddQuestion.Text = "Question";
            this.rbnAddQuestion.UseVisualStyleBackColor = true;
            // 
            // rbnAddPreposition
            // 
            this.rbnAddPreposition.AutoSize = true;
            this.rbnAddPreposition.Location = new System.Drawing.Point(113, 42);
            this.rbnAddPreposition.Name = "rbnAddPreposition";
            this.rbnAddPreposition.Size = new System.Drawing.Size(77, 17);
            this.rbnAddPreposition.TabIndex = 5;
            this.rbnAddPreposition.TabStop = true;
            this.rbnAddPreposition.Text = "Preposition";
            this.rbnAddPreposition.UseVisualStyleBackColor = true;
            // 
            // rbnAddOModifier
            // 
            this.rbnAddOModifier.AutoSize = true;
            this.rbnAddOModifier.Location = new System.Drawing.Point(196, 42);
            this.rbnAddOModifier.Name = "rbnAddOModifier";
            this.rbnAddOModifier.Size = new System.Drawing.Size(96, 17);
            this.rbnAddOModifier.TabIndex = 4;
            this.rbnAddOModifier.TabStop = true;
            this.rbnAddOModifier.Text = "Object Modifier";
            this.rbnAddOModifier.UseVisualStyleBackColor = true;
            // 
            // rbnAddObject
            // 
            this.rbnAddObject.AutoSize = true;
            this.rbnAddObject.Location = new System.Drawing.Point(196, 19);
            this.rbnAddObject.Name = "rbnAddObject";
            this.rbnAddObject.Size = new System.Drawing.Size(56, 17);
            this.rbnAddObject.TabIndex = 3;
            this.rbnAddObject.Text = "Object";
            this.rbnAddObject.UseVisualStyleBackColor = true;
            // 
            // rbnAddAction
            // 
            this.rbnAddAction.AutoSize = true;
            this.rbnAddAction.Location = new System.Drawing.Point(112, 19);
            this.rbnAddAction.Name = "rbnAddAction";
            this.rbnAddAction.Size = new System.Drawing.Size(55, 17);
            this.rbnAddAction.TabIndex = 2;
            this.rbnAddAction.Text = "Action";
            this.rbnAddAction.UseVisualStyleBackColor = true;
            // 
            // rbnAddSModifier
            // 
            this.rbnAddSModifier.AutoSize = true;
            this.rbnAddSModifier.Location = new System.Drawing.Point(6, 42);
            this.rbnAddSModifier.Name = "rbnAddSModifier";
            this.rbnAddSModifier.Size = new System.Drawing.Size(101, 17);
            this.rbnAddSModifier.TabIndex = 1;
            this.rbnAddSModifier.Text = "Subject Modifier";
            this.rbnAddSModifier.UseVisualStyleBackColor = true;
            // 
            // rbnAddSubject
            // 
            this.rbnAddSubject.AutoSize = true;
            this.rbnAddSubject.Checked = true;
            this.rbnAddSubject.Location = new System.Drawing.Point(6, 19);
            this.rbnAddSubject.Name = "rbnAddSubject";
            this.rbnAddSubject.Size = new System.Drawing.Size(61, 17);
            this.rbnAddSubject.TabIndex = 0;
            this.rbnAddSubject.TabStop = true;
            this.rbnAddSubject.Text = "Subject";
            this.rbnAddSubject.UseVisualStyleBackColor = true;
            // 
            // MakeContexts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 468);
            this.Controls.Add(this.gbxAddToContext);
            this.Controls.Add(this.lblContextID);
            this.Controls.Add(this.btnNewContext);
            this.Controls.Add(this.tbxText);
            this.Name = "MakeContexts";
            this.Text = "MakeContexts";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MakeContexts_FormClosing);
            this.gbxAddToContext.ResumeLayout(false);
            this.gbxAddToContext.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbxText;
        private System.Windows.Forms.Button btnNewContext;
        private System.Windows.Forms.Label lblContextID;
        private System.Windows.Forms.GroupBox gbxAddToContext;
        private System.Windows.Forms.RadioButton rbnAddObject;
        private System.Windows.Forms.RadioButton rbnAddAction;
        private System.Windows.Forms.RadioButton rbnAddSModifier;
        private System.Windows.Forms.RadioButton rbnAddSubject;
        private System.Windows.Forms.RadioButton rbnConditionalIf;
        private System.Windows.Forms.RadioButton rbnAddQuestion;
        private System.Windows.Forms.RadioButton rbnAddPreposition;
        private System.Windows.Forms.RadioButton rbnAddOModifier;
        private System.Windows.Forms.RadioButton rbnConditionalThen;
    }
}