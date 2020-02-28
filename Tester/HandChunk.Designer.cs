namespace NLPDB
{
    partial class HandChunk
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
            this.tbxWorking = new System.Windows.Forms.TextBox();
            this.lblHighlightedPhrase = new System.Windows.Forms.Label();
            this.btnCustomTextEnter = new System.Windows.Forms.Button();
            this.tbxCustomText = new System.Windows.Forms.TextBox();
            this.gbxTags = new System.Windows.Forms.GroupBox();
            this.rbnExistenceProperty = new System.Windows.Forms.RadioButton();
            this.rbnExistence = new System.Windows.Forms.RadioButton();
            this.rbnActionProperty = new System.Windows.Forms.RadioButton();
            this.rbnThingProperty = new System.Windows.Forms.RadioButton();
            this.rbnAction = new System.Windows.Forms.RadioButton();
            this.rbnThing = new System.Windows.Forms.RadioButton();
            this.gbxTagUntag = new System.Windows.Forms.GroupBox();
            this.rbnUntag = new System.Windows.Forms.RadioButton();
            this.rbnTag = new System.Windows.Forms.RadioButton();
            this.cbxTagOn = new System.Windows.Forms.CheckBox();
            this.gbxSpecialTags = new System.Windows.Forms.GroupBox();
            this.cbxName = new System.Windows.Forms.CheckBox();
            this.gbxTags.SuspendLayout();
            this.gbxTagUntag.SuspendLayout();
            this.gbxSpecialTags.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbxWorking
            // 
            this.tbxWorking.Location = new System.Drawing.Point(12, 169);
            this.tbxWorking.Multiline = true;
            this.tbxWorking.Name = "tbxWorking";
            this.tbxWorking.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxWorking.Size = new System.Drawing.Size(292, 250);
            this.tbxWorking.TabIndex = 0;
            this.tbxWorking.Text = "This is a test on the Hand Chunk form, HandChunk.";
            this.tbxWorking.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tbxWorking_MouseUp);
            // 
            // lblHighlightedPhrase
            // 
            this.lblHighlightedPhrase.AutoSize = true;
            this.lblHighlightedPhrase.Location = new System.Drawing.Point(12, 143);
            this.lblHighlightedPhrase.Name = "lblHighlightedPhrase";
            this.lblHighlightedPhrase.Size = new System.Drawing.Size(99, 13);
            this.lblHighlightedPhrase.TabIndex = 1;
            this.lblHighlightedPhrase.Text = "Highlighted Phrase:";
            // 
            // btnCustomTextEnter
            // 
            this.btnCustomTextEnter.Location = new System.Drawing.Point(12, 12);
            this.btnCustomTextEnter.Name = "btnCustomTextEnter";
            this.btnCustomTextEnter.Size = new System.Drawing.Size(109, 23);
            this.btnCustomTextEnter.TabIndex = 2;
            this.btnCustomTextEnter.Text = "Custom Text Enter";
            this.btnCustomTextEnter.UseVisualStyleBackColor = true;
            this.btnCustomTextEnter.Click += new System.EventHandler(this.btnCustomTextEnter_Click);
            // 
            // tbxCustomText
            // 
            this.tbxCustomText.Location = new System.Drawing.Point(127, 14);
            this.tbxCustomText.Name = "tbxCustomText";
            this.tbxCustomText.Size = new System.Drawing.Size(177, 20);
            this.tbxCustomText.TabIndex = 3;
            // 
            // gbxTags
            // 
            this.gbxTags.Controls.Add(this.rbnExistenceProperty);
            this.gbxTags.Controls.Add(this.rbnExistence);
            this.gbxTags.Controls.Add(this.rbnActionProperty);
            this.gbxTags.Controls.Add(this.rbnThingProperty);
            this.gbxTags.Controls.Add(this.rbnAction);
            this.gbxTags.Controls.Add(this.rbnThing);
            this.gbxTags.Location = new System.Drawing.Point(12, 41);
            this.gbxTags.Name = "gbxTags";
            this.gbxTags.Size = new System.Drawing.Size(207, 92);
            this.gbxTags.TabIndex = 4;
            this.gbxTags.TabStop = false;
            this.gbxTags.Text = "Tags";
            // 
            // rbnExistenceProperty
            // 
            this.rbnExistenceProperty.AutoSize = true;
            this.rbnExistenceProperty.Location = new System.Drawing.Point(90, 65);
            this.rbnExistenceProperty.Name = "rbnExistenceProperty";
            this.rbnExistenceProperty.Size = new System.Drawing.Size(113, 17);
            this.rbnExistenceProperty.TabIndex = 5;
            this.rbnExistenceProperty.Text = "Existence Property";
            this.rbnExistenceProperty.UseVisualStyleBackColor = true;
            // 
            // rbnExistence
            // 
            this.rbnExistence.AutoSize = true;
            this.rbnExistence.Location = new System.Drawing.Point(6, 65);
            this.rbnExistence.Name = "rbnExistence";
            this.rbnExistence.Size = new System.Drawing.Size(71, 17);
            this.rbnExistence.TabIndex = 4;
            this.rbnExistence.Text = "Existence";
            this.rbnExistence.UseVisualStyleBackColor = true;
            // 
            // rbnActionProperty
            // 
            this.rbnActionProperty.AutoSize = true;
            this.rbnActionProperty.Location = new System.Drawing.Point(90, 42);
            this.rbnActionProperty.Name = "rbnActionProperty";
            this.rbnActionProperty.Size = new System.Drawing.Size(97, 17);
            this.rbnActionProperty.TabIndex = 3;
            this.rbnActionProperty.Text = "Action Property";
            this.rbnActionProperty.UseVisualStyleBackColor = true;
            // 
            // rbnThingProperty
            // 
            this.rbnThingProperty.AutoSize = true;
            this.rbnThingProperty.Location = new System.Drawing.Point(90, 19);
            this.rbnThingProperty.Name = "rbnThingProperty";
            this.rbnThingProperty.Size = new System.Drawing.Size(94, 17);
            this.rbnThingProperty.TabIndex = 2;
            this.rbnThingProperty.Text = "Thing Property";
            this.rbnThingProperty.UseVisualStyleBackColor = true;
            // 
            // rbnAction
            // 
            this.rbnAction.AutoSize = true;
            this.rbnAction.Location = new System.Drawing.Point(6, 42);
            this.rbnAction.Name = "rbnAction";
            this.rbnAction.Size = new System.Drawing.Size(55, 17);
            this.rbnAction.TabIndex = 1;
            this.rbnAction.Text = "Action";
            this.rbnAction.UseVisualStyleBackColor = true;
            // 
            // rbnThing
            // 
            this.rbnThing.AutoSize = true;
            this.rbnThing.Checked = true;
            this.rbnThing.Location = new System.Drawing.Point(6, 19);
            this.rbnThing.Name = "rbnThing";
            this.rbnThing.Size = new System.Drawing.Size(52, 17);
            this.rbnThing.TabIndex = 0;
            this.rbnThing.TabStop = true;
            this.rbnThing.Text = "Thing";
            this.rbnThing.UseVisualStyleBackColor = true;
            // 
            // gbxTagUntag
            // 
            this.gbxTagUntag.Controls.Add(this.rbnUntag);
            this.gbxTagUntag.Controls.Add(this.rbnTag);
            this.gbxTagUntag.Location = new System.Drawing.Point(225, 67);
            this.gbxTagUntag.Name = "gbxTagUntag";
            this.gbxTagUntag.Size = new System.Drawing.Size(79, 66);
            this.gbxTagUntag.TabIndex = 5;
            this.gbxTagUntag.TabStop = false;
            this.gbxTagUntag.Text = "Tag/Untag";
            // 
            // rbnUntag
            // 
            this.rbnUntag.AutoSize = true;
            this.rbnUntag.Location = new System.Drawing.Point(6, 42);
            this.rbnUntag.Name = "rbnUntag";
            this.rbnUntag.Size = new System.Drawing.Size(54, 17);
            this.rbnUntag.TabIndex = 1;
            this.rbnUntag.TabStop = true;
            this.rbnUntag.Text = "Untag";
            this.rbnUntag.UseVisualStyleBackColor = true;
            // 
            // rbnTag
            // 
            this.rbnTag.AutoSize = true;
            this.rbnTag.Checked = true;
            this.rbnTag.Location = new System.Drawing.Point(6, 19);
            this.rbnTag.Name = "rbnTag";
            this.rbnTag.Size = new System.Drawing.Size(44, 17);
            this.rbnTag.TabIndex = 0;
            this.rbnTag.TabStop = true;
            this.rbnTag.Text = "Tag";
            this.rbnTag.UseVisualStyleBackColor = true;
            // 
            // cbxTagOn
            // 
            this.cbxTagOn.AutoSize = true;
            this.cbxTagOn.Checked = true;
            this.cbxTagOn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxTagOn.Location = new System.Drawing.Point(231, 44);
            this.cbxTagOn.Name = "cbxTagOn";
            this.cbxTagOn.Size = new System.Drawing.Size(62, 17);
            this.cbxTagOn.TabIndex = 2;
            this.cbxTagOn.Text = "Tag On";
            this.cbxTagOn.UseVisualStyleBackColor = true;
            // 
            // gbxSpecialTags
            // 
            this.gbxSpecialTags.Controls.Add(this.cbxName);
            this.gbxSpecialTags.Location = new System.Drawing.Point(311, 7);
            this.gbxSpecialTags.Name = "gbxSpecialTags";
            this.gbxSpecialTags.Size = new System.Drawing.Size(84, 412);
            this.gbxSpecialTags.TabIndex = 7;
            this.gbxSpecialTags.TabStop = false;
            this.gbxSpecialTags.Text = "Special Tags";
            // 
            // cbxName
            // 
            this.cbxName.AutoSize = true;
            this.cbxName.Location = new System.Drawing.Point(15, 19);
            this.cbxName.Name = "cbxName";
            this.cbxName.Size = new System.Drawing.Size(54, 17);
            this.cbxName.TabIndex = 0;
            this.cbxName.Text = "Name";
            this.cbxName.UseVisualStyleBackColor = true;
            // 
            // HandChunk
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 433);
            this.Controls.Add(this.gbxSpecialTags);
            this.Controls.Add(this.cbxTagOn);
            this.Controls.Add(this.gbxTagUntag);
            this.Controls.Add(this.gbxTags);
            this.Controls.Add(this.tbxCustomText);
            this.Controls.Add(this.btnCustomTextEnter);
            this.Controls.Add(this.lblHighlightedPhrase);
            this.Controls.Add(this.tbxWorking);
            this.Name = "HandChunk";
            this.Text = "Hand Chunk";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HandChunk_FormClosing);
            this.gbxTags.ResumeLayout(false);
            this.gbxTags.PerformLayout();
            this.gbxTagUntag.ResumeLayout(false);
            this.gbxTagUntag.PerformLayout();
            this.gbxSpecialTags.ResumeLayout(false);
            this.gbxSpecialTags.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbxWorking;
        private System.Windows.Forms.Label lblHighlightedPhrase;
        private System.Windows.Forms.Button btnCustomTextEnter;
        private System.Windows.Forms.TextBox tbxCustomText;
        private System.Windows.Forms.GroupBox gbxTags;
        private System.Windows.Forms.RadioButton rbnExistenceProperty;
        private System.Windows.Forms.RadioButton rbnExistence;
        private System.Windows.Forms.RadioButton rbnActionProperty;
        private System.Windows.Forms.RadioButton rbnThingProperty;
        private System.Windows.Forms.RadioButton rbnAction;
        private System.Windows.Forms.RadioButton rbnThing;
        private System.Windows.Forms.GroupBox gbxTagUntag;
        private System.Windows.Forms.RadioButton rbnUntag;
        private System.Windows.Forms.RadioButton rbnTag;
        private System.Windows.Forms.CheckBox cbxTagOn;
        private System.Windows.Forms.GroupBox gbxSpecialTags;
        private System.Windows.Forms.CheckBox cbxName;
    }
}