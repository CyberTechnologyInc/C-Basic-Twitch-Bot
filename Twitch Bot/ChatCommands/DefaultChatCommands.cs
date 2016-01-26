using System;
using System.Collections.Generic;

namespace TwitchBot.ChatCommands {

	internal class DefaultChatCommands : IChatCommand {
		//Commands, description, privileges

		public List<string[]> Commands { get; set; } = new List<string[]> { new string[] { "!labrats", "Gives credit to all of the testers of the bot", "" },
			new string[] { "!credits", "Gives credits to the maker of the bot", ""},
			new string[] { "!song", "Shows the song currently playing", "" } };

		//TODO: Work out a better way to send over other details that are required for some functionality.
		public void ProcessCommand(TwitchUser user, string[] command, out bool sendViaChat, out string message) {
			//If the bot is disabled
			if(!ChatSettings.BotEnabled) {
				sendViaChat = false;
				message = null;
				return;
			}

			sendViaChat = true;
			foreach(var cmds in Commands) {
				if(command[0] == Commands[0][0]) {
					//Credit testers
					message = "Thank the lab rats Itzkydvil and Poul76 for testing this bot!";
					return;
				} else if(command[0] == Commands[1][0]) {
					//Credit creator
					message = "This bot was made by ManselD! :D";
					return;
				} else if(command[0] == Commands[2][0]) {
					//Retrieve troll song
					string[] songList = { "https://www.youtube.com/watch?v=z6qaO-vX080", "https://www.youtube.com/watch?v=dQw4w9WgXcQ", "https://www.youtube.com/watch?v=oT3mCybbhf0" };
					var rnd = new Random();
					message = "The current song playing is: " + songList[rnd.Next(0, songList.Length)];
					return;
				}
			}

			sendViaChat = false;
			message = null;

			/*
				case "uptimeCMD":
					//if(!commandOnCooldown("uptimeCMD")) {
						var check = new WebClient();
						check.Proxy = null;
						check.DownloadStringAsync(new Uri("https://api.twitch.tv/kraken/streams/samdadude"));
						check.DownloadStringCompleted += (s, e) => {
							dynamic StreamDat = JObject.Parse(e.Result);
							//DateTime dt = new DateTime(long.Parse(StreamDat.stream.created_at));

							//ChannelConnection.send_message("The stream has been up for " + dt.Hour + " hours and " + dt.Minute + " minutes.");
						};
					//}
					*/
			/*

			case "joustCMD":
				if(!(command.Length == 2)) { break; }
				//if(!commandOnCooldown("joustCommand")) {
				using(WebClient wc = new WebClient()) {
					return wc.DownloadString(website + "&mode=joust&code=" + txtAPICode.Text + "&username=" + msg[1]);
				}
				//} else {
					//send_message(getCooldown("joustCommand"));
				//}

			case "conquestCMD":
				if(!(msg.Length == 2)) { break; }
				if(!commandOnCooldown("conquestCommand")) {
					ChannelConnection.send_message(wc.DownloadString(website + "&mode=conquest&code=" + txtAPICode.Text + "&username=" + msg[1]));
				} else {
					//send_message(getCooldown("conquestCommand"));
				}
				break;
				*/
		}
	}
}