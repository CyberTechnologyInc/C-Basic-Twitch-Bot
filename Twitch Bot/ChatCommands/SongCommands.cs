using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot.ChatCommands {
	class SongCommands : IChatCommand {
		//Commands, description, privileges
		private List<string[]> _commands = new List<string[]> { new string[] { "!requestsong", "Adds a song to the request list", ""},
		new string[] { "!getsong", "Gets the details of a song from the request list PM'd", ""},
		new string[] { "!removesong", "Removed a song from the request list", ""}};

		public List<string[]> commands {
			get {
				return _commands;
			}

			set {
				_commands = value;
			}
		}


		//Username, music, link
		private List<string[]> musicQueue = new List<string[]>();

		public void ProcessCommand(Twitch_User user, string[] command, out bool sendViaChat, out string message) {
			sendViaChat = true;
			if(command[0] == commands[0][0]) {
				//if(args.Length == 2 || args.Length == 3 ) { message = null; return; }
				if(command.Length == 2) {
					if(command[1] != null) {
						musicQueue.Add(new string[] { user.Username, command[1] });
						message = user.Username + ", your song (" + command[1] + ") has been added to the queue.";
						return;
					}
				} else if(command.Length == 3) {
					if(command[1] != null && command[2] != null) {
						musicQueue.Add(new string[] { user.Username, command[1], command[2] });
						message = user.Username + ", your song (" + command[1] + ") has been added to the queue.";
						return;
					}
				}

				message = user.Username + ", your song wasn't added as it didn't have the correct parameters." + Environment.NewLine + "Correct syntax is: " + command[0] + " <song> <~url>";
				return;
			} else if(command[0] == commands[1][0]) {
				//Retrieve the details of a song and PM them to the user
				sendViaChat = false;
				for(int i = 0; i < musicQueue.Count; i++) {
					if(musicQueue[i][1] == command[1]) {
						if(musicQueue[i].Length == 2) {
							message = "Here's the song data you requested: Name: " + command[1];
							return;
						} else if(musicQueue[i].Length == 3) {
							message = "Here's the song data you requested: Name: " + command[1] + ", URL: " + musicQueue[i][2];
							return;
						}
					}
				}

				message = "Couldn't find the song \"" + command[1] + "\" in the queue.";
				return;
			} else if(command[0] == commands[2][0]) {
				//Only allow the streamer to remove music from the queue
				if(user.Username.ToLower() == ChatSettings.Channel) {
					if(command[1] != null) {
						for(int i = 0; i < musicQueue.Count; i++) {
							if(musicQueue[i][1] == command[1]) {
								musicQueue.Remove(musicQueue[i]);
								message = command[1] + " has been removed from the music queue.";
								return;
							}
						}
					}
				}
			}

			message = null;
		}
	}
}
