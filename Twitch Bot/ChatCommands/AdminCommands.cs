using System.Collections.Generic;
using System.Linq;

namespace TwitchBot.ChatCommands {
	/// <summary>
	/// These commands will be executed regardless of if the bot was turned on or off in chat
	/// </summary>
	class AdminCommands : IChatCommand, IAdminChatCommand {
		//public bool BotEnabled { get; set; }
		public bool PrioritiseCommands { get; set; } = true;
		public bool BypassEnabledStatus { get; set; } = true;

		//Commands, description, privileges
		public List<string[]> Commands { get; set; } = new List<string[]> { new string[] { "!turnoff", "Turn off the bot", "mod" },
			new string[] { "!turnon", "Turn on the bot", "mod"} };

		public void ProcessCommand(TwitchUser user, string[] command, out bool sendViaChat, out string message) {
			for(int i = 0; i < Commands.Count(); i++) {
				if((user.UserType == Commands[0][2] || ChatSettings.Channel == user.Username.ToLower()) && command[0] == Commands[0][0]) {
					ChatSettings.BotEnabled = false;
					sendViaChat = true;
					message = user.Username + " is abusing me, I'm off!";
					return;
				} else if((user.UserType == Commands[0][2] || ChatSettings.Channel == user.Username.ToLower()) && command[0] == Commands[1][0]) {
					ChatSettings.BotEnabled = true;
					sendViaChat = true;
					message = user.Username + " has summoned me. Hello everyone!";
					return;
				}
			}

			sendViaChat = false;
			message = null;
		}
	}
}
