using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot.ChatCommands {
	public interface IChatCommand {
		/// <summary>
		/// A list of all commands and their descriptions and privileges
		/// </summary>
		List<string[]> commands { get; set; }

		/// <summary>
		/// Add a new chat command
		/// </summary>
		/// <param name="user">The user object</param>
		/// <param name="command">The chat message (in it's entirety) that was sent to execute said command</param>
		void ProcessCommand(Twitch_User user, string[] command, out bool sendViaChat, out string message);
	}
}
