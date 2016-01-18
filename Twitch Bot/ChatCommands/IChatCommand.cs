using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot.ChatCommands {
	public interface IChatCommand {
		/// <summary>
		/// The commands that the class will handle
		/// </summary>
		string[] commands { get; set; }

		/// <summary>
		/// The descriptions of the commands that the class will handle
		/// </summary>
		string[] descriptions { get; set; }

		/// <summary>
		/// The privileges required for each command that the class will handle
		/// </summary>
		string[] privileges { get; set; }

		/// <summary>
		/// This is where all command details should be added to their respective string arrays (commands, descriptions, privileges)
		/// </summary>
		void Initialise();

		/// <summary>
		/// Add a new chat command
		/// </summary>
		/// <param name="username">The username of the user that sent the command</param>
		/// <param name="command">The chat message (in it's entirety) that was sent to execute said command</param>
		object[] ProcessCommand(string username, string command);
	}
}
