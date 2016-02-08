namespace TwitchBot.Forms {
	partial class FrmAddCommand {
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
			this.lblPrivileges = new System.Windows.Forms.Label();
			this.txtPrivileges = new System.Windows.Forms.TextBox();
			this.chkEnabled = new System.Windows.Forms.CheckBox();
			this.lblDescription = new System.Windows.Forms.Label();
			this.txtDescription = new System.Windows.Forms.TextBox();
			this.lblCommand = new System.Windows.Forms.Label();
			this.txtCommand = new System.Windows.Forms.TextBox();
			this.cmdAddNewCommand = new System.Windows.Forms.Button();
			this.lblReply = new System.Windows.Forms.Label();
			this.txtReply = new System.Windows.Forms.TextBox();
			this.chkSendViaChat = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// lblPrivileges
			// 
			this.lblPrivileges.AutoSize = true;
			this.lblPrivileges.Location = new System.Drawing.Point(14, 61);
			this.lblPrivileges.Name = "lblPrivileges";
			this.lblPrivileges.Size = new System.Drawing.Size(52, 13);
			this.lblPrivileges.TabIndex = 14;
			this.lblPrivileges.Text = "Privileges";
			// 
			// txtPrivileges
			// 
			this.txtPrivileges.Location = new System.Drawing.Point(72, 58);
			this.txtPrivileges.Name = "txtPrivileges";
			this.txtPrivileges.Size = new System.Drawing.Size(349, 20);
			this.txtPrivileges.TabIndex = 2;
			// 
			// chkEnabled
			// 
			this.chkEnabled.AutoSize = true;
			this.chkEnabled.Location = new System.Drawing.Point(12, 290);
			this.chkEnabled.Name = "chkEnabled";
			this.chkEnabled.Size = new System.Drawing.Size(115, 17);
			this.chkEnabled.TabIndex = 20;
			this.chkEnabled.Text = "Command Enabled";
			this.chkEnabled.UseVisualStyleBackColor = true;
			// 
			// lblDescription
			// 
			this.lblDescription.AutoSize = true;
			this.lblDescription.Location = new System.Drawing.Point(6, 35);
			this.lblDescription.Name = "lblDescription";
			this.lblDescription.Size = new System.Drawing.Size(60, 13);
			this.lblDescription.TabIndex = 11;
			this.lblDescription.Text = "Description";
			// 
			// txtDescription
			// 
			this.txtDescription.Location = new System.Drawing.Point(72, 32);
			this.txtDescription.Name = "txtDescription";
			this.txtDescription.Size = new System.Drawing.Size(349, 20);
			this.txtDescription.TabIndex = 1;
			// 
			// lblCommand
			// 
			this.lblCommand.AutoSize = true;
			this.lblCommand.Location = new System.Drawing.Point(12, 9);
			this.lblCommand.Name = "lblCommand";
			this.lblCommand.Size = new System.Drawing.Size(54, 13);
			this.lblCommand.TabIndex = 9;
			this.lblCommand.Text = "Command";
			// 
			// txtCommand
			// 
			this.txtCommand.Location = new System.Drawing.Point(72, 6);
			this.txtCommand.Name = "txtCommand";
			this.txtCommand.Size = new System.Drawing.Size(349, 20);
			this.txtCommand.TabIndex = 0;
			// 
			// cmdAddNewCommand
			// 
			this.cmdAddNewCommand.Location = new System.Drawing.Point(12, 313);
			this.cmdAddNewCommand.Name = "cmdAddNewCommand";
			this.cmdAddNewCommand.Size = new System.Drawing.Size(409, 23);
			this.cmdAddNewCommand.TabIndex = 22;
			this.cmdAddNewCommand.Text = "Add a new command";
			this.cmdAddNewCommand.UseVisualStyleBackColor = true;
			this.cmdAddNewCommand.Click += new System.EventHandler(this.cmdSaveChanges_Click);
			// 
			// lblReply
			// 
			this.lblReply.AutoSize = true;
			this.lblReply.Location = new System.Drawing.Point(6, 87);
			this.lblReply.Name = "lblReply";
			this.lblReply.Size = new System.Drawing.Size(60, 13);
			this.lblReply.TabIndex = 17;
			this.lblReply.Text = "Bot\'s Reply";
			// 
			// txtReply
			// 
			this.txtReply.Location = new System.Drawing.Point(72, 84);
			this.txtReply.Multiline = true;
			this.txtReply.Name = "txtReply";
			this.txtReply.Size = new System.Drawing.Size(349, 119);
			this.txtReply.TabIndex = 3;
			// 
			// chkSendViaChat
			// 
			this.chkSendViaChat.AutoSize = true;
			this.chkSendViaChat.Location = new System.Drawing.Point(133, 290);
			this.chkSendViaChat.Name = "chkSendViaChat";
			this.chkSendViaChat.Size = new System.Drawing.Size(92, 17);
			this.chkSendViaChat.TabIndex = 21;
			this.chkSendViaChat.Text = "Send via chat";
			this.chkSendViaChat.UseVisualStyleBackColor = true;
			// 
			// FrmAddCommand
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(434, 348);
			this.Controls.Add(this.chkSendViaChat);
			this.Controls.Add(this.lblReply);
			this.Controls.Add(this.txtReply);
			this.Controls.Add(this.cmdAddNewCommand);
			this.Controls.Add(this.lblPrivileges);
			this.Controls.Add(this.txtPrivileges);
			this.Controls.Add(this.chkEnabled);
			this.Controls.Add(this.lblDescription);
			this.Controls.Add(this.txtDescription);
			this.Controls.Add(this.lblCommand);
			this.Controls.Add(this.txtCommand);
			this.Name = "FrmAddCommand";
			this.ShowIcon = false;
			this.Text = "Add a new command";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblPrivileges;
		public System.Windows.Forms.TextBox txtPrivileges;
		public System.Windows.Forms.CheckBox chkEnabled;
		private System.Windows.Forms.Label lblDescription;
		public System.Windows.Forms.TextBox txtDescription;
		private System.Windows.Forms.Label lblCommand;
		public System.Windows.Forms.TextBox txtCommand;
		private System.Windows.Forms.Button cmdAddNewCommand;
		private System.Windows.Forms.Label lblReply;
		public System.Windows.Forms.TextBox txtReply;
		public System.Windows.Forms.CheckBox chkSendViaChat;
	}
}