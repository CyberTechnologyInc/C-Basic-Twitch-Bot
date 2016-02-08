using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot.ChatCommands {
	class SongData {
		public string Name { get; set; }
		public string Url { get; set; }
		public string Requester { get; set; }
	}

	class SongCommands : IChatCommand {
		//Commands, description, privileges

		public List<string[]> Commands { get; set; } = new List<string[]> { new string[] { "!requestsong", "Adds a song to the request list", ""},
			new string[] { "!getsong", "Gets the details of a song from the request list PM'd", ""},
			new string[] { "!removesong", "Removed a song from the request list", ChatSettings.Channel.ToLower()},
			new string[] { "!nextsong", "Gets the next song to be played", ""}};


		//Username, music, link
		private readonly List<SongData> _musicQueue = new List<SongData>();

		public void ProcessCommand(TwitchUser user, string[] command, out bool sendViaChat, out string message) {
			sendViaChat = true;
			if(command[0] == Commands[0][0]) {
				//if(args.Length == 2 || args.Length == 3 ) { message = null; return; }
				if(command.Length == 2) {
					if(command[1] != null) {
						_musicQueue.Add(new SongData { Requester = user.Username, Name = command[1] });
						message = user.Username + ", your song (" + command[1] + ") has been added to the queue.";
						return;
					}
				} else if(command.Length == 3) {
					if(command[1] != null && command[2] != null) {
						_musicQueue.Add(new SongData { Requester = user.Username, Name = command[1], Url = command[2] });
						message = user.Username + ", your song (" + command[1] + ") has been added to the queue.";
						return;
					}
				}

				message = user.Username + ", your song wasn't added as it didn't have the correct parameters." + Environment.NewLine + "Correct syntax is: " + command[0] + " <song> <~url>";
				return;
			} else if(command[0] == Commands[1][0]) {
				//Retrieve the details of a song and PM them to the user
				sendViaChat = false;
				foreach (var song in _musicQueue){
					if(song.Name == command[1]) {
						if(song.Url == null) {
							message = "Here's the song data you requested: Name: " + command[1];
							return;
						} else {
							message = "Here's the song data you requested: Name: " + command[1] + ", URL: " + song.Url;
							return;
						}
					}
				}

				message = "Couldn't find the song \"" + command[1] + "\" in the queue.";
				return;
			} else if(command[0] == Commands[2][0]) {
				//Only allow the streamer to remove music from the queue
				if(user.Username.ToLower() == ChatSettings.Channel) {
					if(command[1] != null) {
						for(int i = 0; i < _musicQueue.Count; i++) {
							if(_musicQueue[i].Name == command[1]) {
								_musicQueue.Remove(_musicQueue[i]);
								message = command[1] + " has been removed from the music queue.";
								return;
							}
						}
					}
				}
			}else if(command[0] == Commands[3][0]) {
				//Get the next song to be played
				foreach(var song in _musicQueue) {
					sendViaChat = false;
					if(song.Url == null) {
						message = "The next song to be played is: " + song.Name;
					} else {
						message = "The next song to be played is: " + song.Name + ", URL: " + song.Url;
					}
					
					return;
				}
			}

			message = null;
		}
	}
}
