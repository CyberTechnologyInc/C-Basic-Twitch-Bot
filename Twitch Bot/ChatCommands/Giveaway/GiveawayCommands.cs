using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace TwitchBot.ChatCommands.Giveaway {

	internal class GiveawayCommands : IChatCommand {

		//Commands, description, privileges

		public List<string[]> Commands { get; set; } = new List<string[]> { new string[] { "!enter", "Adds a user into the giveaway", "" },
			new string[] { "!points", "Shows the user's current points", ""},
			new string[] { "!toppoints", "Gets the user with the highest amount of points", ""},
			new string[] { "!give", "Transfer points to another user", "" } };

		public void ProcessCommand(TwitchUser user, string[] command, out bool sendViaChat, out string message) {
			sendViaChat = true;
			if(File.Exists(Program.GiveawayPointsFile)) {
				if(command[0] == Commands[0][0]) {
					//Enter into giveaway
					if(Program.BotForm.giveawayActive) {
						Program.BotForm.AddGiveawayUserToList(user.Username);
						message = user.Username + " has been added into the giveaway.";
						return;
					} else {
						message = null;
						return;
					}
				} else if(command[0] == Commands[1][0]) {
					//Show amount of points the user has.
					var docTwo = XDocument.Load(Program.GiveawayPointsFile);
					var points = new long();
					foreach(var el in docTwo.Element("Users").Elements()) {
						if(el.Element("Username").Value == user.Username.ToLower()) {
							points = Convert.ToInt64(el.Element("Points").Value);
							break;
						}
					}

					message = user.Username + " has " + points + " points.";
					return;
				} else if(command[0] == Commands[2][0]) {
					//Get the user with the most points.
					var topUser = "";
					var topPoints = new long();
					var doc = XDocument.Load(Program.GiveawayPointsFile);
					foreach(var el in doc.Element("Users").Elements()) {
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

					message = topUser + " currently has the most points with " + topPoints + " points.";
					return;
				} else if(command[0] == Commands[3][0]) {
					try {
						message = null;

						//!givepoints name amount
						if(command[1] != null) {
							return;
						}

						var givenPoints = Math.Abs(long.Parse(command[2]));
						var docThree = XDocument.Load(Program.GiveawayPointsFile);
						var canAfford = false;
						foreach(var el in docThree.Element("Users").Elements()) {
							if(el.Element("Username").Value == user.Username.ToLower()) {
								if(Convert.ToInt64(el.Element("Points").Value) >= givenPoints) {
									canAfford = true;
								}
								break;
							}
						}

						if(canAfford) {
							foreach(var el in docThree.Element("Users").Elements()) {
								if(el.Element("Username").Value == user.Username.ToLower()) {
									//Remove points from giver.
									el.Element("Points").SetValue(Convert.ToInt64(el.Element("Points").Value) - givenPoints);
									sendViaChat = false;
									message = "/w " + user.Username + " You've given " + givenPoints + " points to " + command[1];
								} else if(el.Element("Username").Value == command[1].ToLower()) {
									//Add points to receiver.
									el.Element("Points").SetValue(Convert.ToInt64(el.Element("Points").Value) + givenPoints);
									sendViaChat = false;
									message = "/w " + command[1] + " You've received " + givenPoints + " points from " + user.Username;
								}
							}
							//ChannelConnection.send_message(sender + " has given " + givenPoints + " points to " + msg[1] + "!");
							docThree.Save(Program.GiveawayPointsFile);
							return;
						} else {
							message = user.Username + ", you can't afford that you dirty peasant!";
							return;
						}
					} catch(Exception ex) {
						//logMessage("Error with command '" + msg[0] + "': " + ex.ToString());
					}
				}
			}

			message = null;
		}
	}
}