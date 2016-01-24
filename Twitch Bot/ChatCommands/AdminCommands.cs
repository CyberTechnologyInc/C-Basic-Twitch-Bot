using System.Collections.Generic;
using System.Linq;

namespace TwitchBot.ChatCommands {
	/// <summary>
	/// These commands will be executed regardless of if the bot was turned on or off in chat
	/// </summary>
	class AdminCommands : IChatCommand, IAdminChatCommand {
		//public bool BotEnabled { get; set; }
		public bool prioritiseCommands { get; set; } = true;
		public bool bypassEnabledStatus { get; set; } = true;

		//Commands, description, privileges
		List<string[]> _commands = new List<string[]> { new string[] { "!turnoff", "Turn off the bot", "mod" },
		new string[] { "!turnon", "Turn on the bot", "mod"} };

		public List<string[]> commands {
			get {
				return _commands;
			}

			set {
				_commands = value;
			}
		}

		public void ProcessCommand(Twitch_User user, string[] command, out bool sendViaChat, out string message) {
			for(int i = 0; i < commands.Count(); i++) {
				if((user.User_Type == commands[0][2] || ChatSettings.Channel == user.Username.ToLower()) && command[0] == commands[0][0]) {
					ChatSettings.BotEnabled = false;
					sendViaChat = true;
					message = user.Username + " is abusing me, I'm off!";
					return;
				} else if((user.User_Type == commands[0][2] || ChatSettings.Channel == user.Username.ToLower()) && command[0] == commands[1][0]) {
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
