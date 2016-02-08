using System;
using System.Windows.Forms;
using TwitchBot.ChatCommands;

namespace TwitchBot.Forms {

	public partial class FrmEditCommand : Form {

		public FrmEditCommand() {
			InitializeComponent();
		}

		private void cmdSaveChanges_Click(object sender, EventArgs e) {
			if(txtCommand.Text != null && txtDescription.Text != null) {
				CommandManager.AddCommand(new CommandData { Name = txtCommand.Text, Enabled = chkEnabled.Checked, Description = txtDescription.Text, Privileges = txtPrivileges.Text.Split(' ') });
				Close();
			} else {
				MessageBox.Show("Some fields are blank.", "Invalid input!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}
	}
}