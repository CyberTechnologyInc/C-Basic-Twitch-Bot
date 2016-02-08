using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot.ChatCommands {
	class FunCommands : IChatCommand {
		public List<string[]> Commands { get; set; } = new List<string[]> { new string[] { "", "", ""}};

		public void ProcessCommand(TwitchUser user, string[] command, out bool sendViaChat, out string message) {
			sendViaChat = true;
			if(command[0] == Commands[0][0]) {
				//message = user.Username + ", your song wasn't added as it didn't have the correct parameters." + Environment.NewLine + "Correct syntax is: " + command[0] + " <song> <~url>";
				//return;
			}

			message = null;
		}
	}
}
