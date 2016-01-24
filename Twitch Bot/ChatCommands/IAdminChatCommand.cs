using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot.ChatCommands {
	public interface IAdminChatCommand : IChatCommand {
		/// <summary>
		/// If enabled, these commands will activate 
		/// when called in chat regardless of bot status
		/// </summary>
		bool bypassEnabledStatus { get; set; }

		/// <summary>
		/// Whether or not these commands will take priority over others.
		/// If they do, they will be executed first.
		/// </summary>
		bool prioritiseCommands { get; set; }
	}
}
