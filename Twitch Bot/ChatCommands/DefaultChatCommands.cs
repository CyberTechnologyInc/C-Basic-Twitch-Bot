using System;
using System.Collections.Generic;

namespace TwitchBot.ChatCommands {

	internal class DefaultChatCommands : IChatCommand {
		private List<string[]> chatDescriptionData = new List<string[]>();

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
			this.commands = new string[] {"!labrats",
				"!credits",
				"!song" };

			this.descriptions = new string[] { "Gives credit to all of the testers of the bot",
				"Gives credits to the maker of the bot",
				"Shows the song currently playing"};

			this.privileges = new string[] { "",
				"",
				""};
		}

		//Need to work out a good way to send over other details that are required for some functionality.
		public object[] ProcessCommand(string username, string command) {
			string[] args = command.Split(' ');

			if(args[0] == commands[0]) {
				return new object[] { true, "Thank the lab rats Itzkydvil and Poul76 for testing this bot!" };
			} else if(args[0] == commands[1]) {
				return new object[] { true, "This bot was made by ManselD!" };
			} else if(args[0] == commands[2]) {
				string[] songList = { "https://www.youtube.com/watch?v=z6qaO-vX080", "https://www.youtube.com/watch?v=dQw4w9WgXcQ", "https://www.youtube.com/watch?v=oT3mCybbhf0" };
				var rnd = new Random();
				return new object[] { true, "The current song playing is: " + songList[rnd.Next(0, songList.Length)] };
			} else {
				//We didn't process any commands, return null.
				return null;
			}

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
			/*
		case "enterGiveawayCMD":
			//Enter user into giveaway if one is open.
			if(GiveawayActive) {
				var inGiveaway = false;
				foreach(string activeUser in giveawayEnteredUsers.Items) {
					if(activeUser == sender.Username) {
						inGiveaway = true;
					}
				}

				if(!inGiveaway) {
					if(removePoints(sender.Username.ToLower(), Convert.ToInt32(txtGiveawayPointsRequired.Text))) {
						addGiveawayUserToList(sender.Username);
						WhisperConnection.send_message("/w " + sender.Username + " You've been added into the giveaway!");
					} else {
						WhisperConnection.send_message("/w " + sender.Username + " You don't have enough points to enter the giveaway.");
					}
				}
			}
			break;

		case "topPointsCMD":
			try {
				//Get the user with the most points.
				var topUser = "";
				var topPoints = new Int64();
				var doc = XDocument.Load(Program.giveawayPointsFile);
				foreach(XElement el in doc.Element("Users").Elements()) {
					if(topUser == "") {
						topUser = el.Element("Username").Value;
						topPoints = Convert.ToInt64(el.Element("Points").Value);
					} else {
						if(Convert.ToInt64(el.Element("Points").Value) > topPoints) {
							topUser = el.Element("Username").Value;
							topPoints = Convert.ToInt64(el.Element("Points").Value);
						}
					}
				}

				return topUser + " currently has the most points with " + topPoints + " points.";
			} catch(Exception ex) {
				//logMessage("Error with command '" + msg[0] + "': " + ex.ToString());
			}
			break;

		case "showPointsCMD":
			try {
				//Show amount of points the user has.
				var docTwo = XDocument.Load(Program.giveawayPointsFile);
				var points = new Int64();
				foreach(XElement el in docTwo.Element("Users").Elements()) {
					if(el.Element("Username").Value == sender.Username.ToLower()) {
						points = Convert.ToInt64(el.Element("Points").Value);
						break;
					}
				}

				ChannelConnection.send_message(sender.Username + " has " + points + " points.");
			} catch(Exception ex) {
				logMessage("Error with command '" + msg[0] + "': " + ex.ToString());
			}
			break;

		case "givePointsCMD":
			try {
				//!givepoints name amount
				if(!(msg[1] is string) || msg[1] == null) {
					return;
				}

				var givenPoints = Math.Abs(Int64.Parse(msg[2]));
				var docThree = XDocument.Load(Program.giveawayPointsFile);
				var canAfford = false;
				foreach(XElement el in docThree.Element("Users").Elements()) {
					if(el.Element("Username").Value == sender.Username.ToLower()) {
						if(Convert.ToInt64(el.Element("Points").Value) >= givenPoints) {
							canAfford = true;
						} else {
							canAfford = false;
						}
						break;
					}
				}

				if(canAfford) {
					foreach(XElement el in docThree.Element("Users").Elements()) {
						if(el.Element("Username").Value == sender.Username.ToLower()) {
							//Remove points from giver.
							el.Element("Points").SetValue(Convert.ToInt64(el.Element("Points").Value) - givenPoints);
							WhisperConnection.send_message("/w " + sender.Username + " You've given " + givenPoints + " points to " + msg[1]);
						} else if(el.Element("Username").Value == msg[1].ToLower()) {
							//Add points to receiver.
							el.Element("Points").SetValue(Convert.ToInt64(el.Element("Points").Value) + givenPoints);
							WhisperConnection.send_message("/w " + msg[1] + " You've received " + givenPoints + " points from " + sender.Username);
						}
					}
					//ChannelConnection.send_message(sender + " has given " + givenPoints + " points to " + msg[1] + "!");
					docThree.Save(Program.giveawayPointsFile);
				} else {
					ChannelConnection.send_message(sender.Username + ", you can't afford that you dirty peasant!");
				}
			} catch(Exception ex) {
				logMessage("Error with command '" + msg[0] + "': " + ex.ToString());
			}
			break;
			*/
		}
	}
}