using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace TwitchBot.ChatCommands.Giveaway {
	static class Misc {

		public class GiveawayLinks {
		}

		public class GiveawayChatters {
			public List<object> Moderators { get; set; }
			public List<object> Staff { get; set; }
			public List<object> Admins { get; set; }
			public List<object> GlobalMods { get; set; }
			public List<string> Viewers { get; set; }
		}

		public class GiveawayTwitchData {
			public GiveawayLinks Links { get; set; }
			public int ChatterCount { get; set; }
			public GiveawayChatters Chatters { get; set; }
		}

		public static bool RemovePoints(string user, int points) {
			if(File.Exists(Program.GiveawayPointsFile)) {
				var doc = XDocument.Load(Program.GiveawayPointsFile);
				var removedPoints = false;

				var userElements = doc.Element("Users")?.Elements();
				if(userElements != null) {
					foreach(var el in userElements) {
						if(el.Element("Username")?.Value == user) {
							var curPoints = Convert.ToInt64(el.Element("Points")?.Value);
							if(curPoints >= points) {
								var newPoints = curPoints - points;
								var newUser = new XElement("User",
								  new XElement("Username", user),
								  new XElement("Points", newPoints));
								el.ReplaceWith(newUser);
								doc.Save(Program.GiveawayPointsFile);
								removedPoints = true;
								break;
							} else {
								break;
							}
						}
					}
				}

				return removedPoints;
			} else {
				return false;
			}
		}

		public static void AddPoints(string user, long points) {
			if(File.Exists(Program.GiveawayPointsFile)) {
				var doc = XDocument.Load(Program.GiveawayPointsFile);

				var userInFile = false;
				var userElements = doc.Element("Users")?.Elements();
                if(userElements != null) {
					foreach(var el in userElements) {
						var curUser = el.Element("Username")?.Value;
						if(curUser != null && curUser == user) {
							userInFile = true;
							break;
						}
					}

					if(userInFile) {
						//Update existing user's points
						foreach(var el in doc.Element("Users").Elements()) {
							if(el.Element("Username")?.Value == user) {
								points = points + Convert.ToInt64(el.Element("Points")?.Value);
								var newUser = new XElement("User",
								  new XElement("Username", user),
								  new XElement("Points", points));
								el.ReplaceWith(newUser);
								break;
							}
						}
					} else {
						//Add new user
						var newUser = new XElement("User",
								  new XElement("Username", user),
								  new XElement("Points", points));
						doc.Element("Users").Add(newUser);
					}
				}
				doc.Save(Program.GiveawayPointsFile);
			} else {
				var doc = new XDocument(
					new XElement("Users",
						new XElement("User",
							new XElement("Username", user),
							new XElement("Points", points)
						)
					)
				);
				doc.Save(Program.GiveawayPointsFile);
			}
		}
	}
}
