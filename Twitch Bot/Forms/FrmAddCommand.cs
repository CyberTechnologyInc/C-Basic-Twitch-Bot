using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TwitchBot.ChatCommands;
using static TwitchBot.GlobalFunctions;

namespace TwitchBot.Forms {
	public partial class FrmAddCommand : Form {
		public FrmAddCommand() {
			InitializeComponent();
		}

		private void cmdSaveChanges_Click(object sender, EventArgs e) {
			var vars = new List<VariableData>();

			//Check for custom variables that will be used to increment/decrement data
			if(txtReply.Text != null) {
				foreach(var dat in txtReply.Text.Split(' ')) {
					var potentialVar = GetStringBetween(dat, "{", "}");
					if(potentialVar != null && potentialVar.StartsWith("$")) {
						if(potentialVar.EndsWith("++") || potentialVar.EndsWith("--")) {
							//This is a variable, add it to the list
							vars.Add(new VariableData {
								Name = potentialVar.Remove(potentialVar.Length - 2, 2),
								Value = 0,
								Increment = potentialVar.Contains("++")
							});
						}
					}
                }
			}

            var cmd = new CommandData {
				Name = txtCommand.Text,
				Description = txtDescription.Text,
				Enabled = chkEnabled.Checked,
				Privileges = txtPrivileges.Text.Split(' '),
				CustomReply = txtReply.Text,
				SendViaChat = chkSendViaChat.Checked,
				Variables = vars
			};

			CommandManager.AddCommand(cmd);
			//CommandManager.SaveCommands();
			CommandManager.UpdateCommandList();
			Close();
		}
	}
}
