using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot.ChatCommands {
	public interface IAdminChatCommand : IChatCommand {
		/// <summary>
		/// Whether or not the bot is enabled.
		/// If this is changed it will reflect outside.
		/// </summary>
		bool BotEnabled { get; set; }
	}
}
