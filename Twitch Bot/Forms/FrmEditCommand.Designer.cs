namespace TwitchBot.Forms {
	partial class FrmEditCommand {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.cmdSaveChanges = new System.Windows.Forms.Button();
			this.txtCommand = new System.Windows.Forms.TextBox();
			this.lblCommand = new System.Windows.Forms.Label();
			this.lblDescription = new System.Windows.Forms.Label();
			this.txtDescription = new System.Windows.Forms.TextBox();
			this.chkEnabled = new System.Windows.Forms.CheckBox();
			this.lblPrivileges = new System.Windows.Forms.Label();
			this.txtPrivileges = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// cmdSaveChanges
			// 
			this.cmdSaveChanges.Location = new System.Drawing.Point(12, 113);
			this.cmdSaveChanges.Name = "cmdSaveChanges";
			this.cmdSaveChanges.Size = new System.Drawing.Size(331, 23);
			this.cmdSaveChanges.TabIndex = 0;
			this.cmdSaveChanges.Text = "Save changes";
			this.cmdSaveChanges.UseVisualStyleBackColor = true;
			this.cmdSaveChanges.Click += new System.EventHandler(this.cmdSaveChanges_Click);
			// 
			// txtCommand
			// 
			this.txtCommand.Location = new System.Drawing.Point(72, 12);
			this.txtCommand.Name = "txtCommand";
			this.txtCommand.Size = new System.Drawing.Size(271, 20);
			this.txtCommand.TabIndex = 1;
			// 
			// lblCommand
			// 
			this.lblCommand.AutoSize = true;
			this.lblCommand.Location = new System.Drawing.Point(12, 15);
			this.lblCommand.Name = "lblCommand";
			this.lblCommand.Size = new System.Drawing.Size(54, 13);
			this.lblCommand.TabIndex = 2;
			this.lblCommand.Text = "Command";
			// 
			// lblDescription
			// 
			this.lblDescription.AutoSize = true;
			this.lblDescription.Location = new System.Drawing.Point(12, 41);
			this.lblDescription.Name = "lblDescription";
			this.lblDescription.Size = new System.Drawing.Size(60, 13);
			this.lblDescription.TabIndex = 4;
			this.lblDescription.Text = "Description";
			// 
			// txtDescription
			// 
			this.txtDescription.Location = new System.Drawing.Point(72, 38);
			this.txtDescription.Name = "txtDescription";
			this.txtDescription.Size = new System.Drawing.Size(271, 20);
			this.txtDescription.TabIndex = 3;
			// 
			// chkEnabled
			// 
			this.chkEnabled.AutoSize = true;
			this.chkEnabled.Location = new System.Drawing.Point(72, 90);
			this.chkEnabled.Name = "chkEnabled";
			this.chkEnabled.Size = new System.Drawing.Size(65, 17);
			this.chkEnabled.TabIndex = 5;
			this.chkEnabled.Text = "Enabled";
			this.chkEnabled.UseVisualStyleBackColor = true;
			// 
			// lblPrivileges
			// 
			this.lblPrivileges.AutoSize = true;
			this.lblPrivileges.Location = new System.Drawing.Point(12, 67);
			this.lblPrivileges.Name = "lblPrivileges";
			this.lblPrivileges.Size = new System.Drawing.Size(52, 13);
			this.lblPrivileges.TabIndex = 7;
			this.lblPrivileges.Text = "Privileges";
			// 
			// txtPrivileges
			// 
			this.txtPrivileges.Location = new System.Drawing.Point(72, 64);
			this.txtPrivileges.Name = "txtPrivileges";
			this.txtPrivileges.Size = new System.Drawing.Size(271, 20);
			this.txtPrivileges.TabIndex = 6;
			// 
			// FrmEditCommand
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(355, 145);
			this.Controls.Add(this.lblPrivileges);
			this.Controls.Add(this.txtPrivileges);
			this.Controls.Add(this.chkEnabled);
			this.Controls.Add(this.lblDescription);
			this.Controls.Add(this.txtDescription);
			this.Controls.Add(this.lblCommand);
			this.Controls.Add(this.txtCommand);
			this.Controls.Add(this.cmdSaveChanges);
			this.Name = "FrmEditCommand";
			this.ShowIcon = false;
			this.Text = "Edit Command";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button cmdSaveChanges;
		public System.Windows.Forms.TextBox txtCommand;
		private System.Windows.Forms.Label lblCommand;
		private System.Windows.Forms.Label lblDescription;
		public System.Windows.Forms.TextBox txtDescription;
		public System.Windows.Forms.CheckBox chkEnabled;
		private System.Windows.Forms.Label lblPrivileges;
		public System.Windows.Forms.TextBox txtPrivileges;
	}
}