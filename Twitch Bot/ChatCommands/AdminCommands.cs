using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot.ChatCommands {
	/// <summary>
	/// These commands will be executed regardless of if the bot was turned on or off in chat
	/// </summary>
	class AdminCommands : IChatCommand, IAdminChatCommand{
		public bool BotEnabled { get; set; }

		private string[] _commands;

		public string[] commands {
			get {
				return _commands;
			}

			set {
				_commands = value;
			}
		}

		private string[] _descriptions;

		public string[] descriptions {
			get {
				return _descriptions;
			}

			set {
				_descriptions = value;
			}
		}

		private string[] _privileges;

		public string[] privileges {
			get {
				return _privileges;
			}

			set {
				_privileges = value;
			}
		}

		public void Initialise() {
			this.commands = new string[] {"!turnoff",
				"!turnon" };

			this.descriptions = new string[] { "Turn off the bot.",
				"Turn on the bot." };

			this.privileges = new string[] { "mod",
				"mod" };
		}

		public object[] ProcessCommand(string username, string command) {
			string[] args = command.Split(' ');
			if(args[0] == commands[0]) { 
				BotEnabled = false;
				return new object[] { true, username + " is abusing me, I'm off!" };
			}else if(args[0] == commands[1]) { 
				BotEnabled = true;
				return new object[] { true, username + " has summoned me. Hello everyone!" };
			} else {
				return null;
			}
		}
	}
}
