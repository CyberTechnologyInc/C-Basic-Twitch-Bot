using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using TopKekMemeBot.JSON_Objects;

namespace TopKekMemeBot {

	public partial class Bot : Form {
		private string bot_name = "Username";
		private string bot_password = "oauth:password";

		private Dictionary<string, double> cooldownList = new Dictionary<string, double>();
		private Dictionary<string, string> commandList = new Dictionary<string, string>();

		private WebClient wc = new WebClient();
		private string website;

		private bool BotEnabled = true;

		private Misc MiscFuncs = new Misc();

		private const string LOG_PREFIX = "[TKMB]";

		public Bot() {
			InitializeComponent();
		}

		private TwitchIRCConnection ChannelConnection;
		private TwitchIRCConnection WhisperConnection;

		private void Bot_Load(object sender, EventArgs e) {
			wc.Proxy = null;

			setup_commands_list();

			setup_tooltips();

			MiscFuncs.BotForm = this;

			//Get API Code from Misc.cs
			txtAPICode.Text = Program.APICode;

			website = "http://cybertechnologyinc.co.uk/API/SMITE/getData.php?pid=1&api_user=" + Program.Username + "&api_code=" + Program.APICode;

			if(File.Exists(Program.settingsFile)) {
				loadSettings();
			}

			//Setup connections to IRC servers
			//Setup connection to normal chat
			ChannelConnection = new TwitchIRCConnection("irc.twitch.tv", txtChannel.Text, bot_name, bot_password);
			//Setup connection to be able to whisper to users
			WhisperConnection = new TwitchIRCConnection("199.9.253.119", txtChannel.Text, bot_name, bot_password);

			if(chkboxAutoStart.Checked) {
				cmdStartBot.PerformClick();
			}
		}

		private bool BotEnabledSwitch = false;

		private void cmdStartBot_Click(object sender, EventArgs e) {
			if(BotEnabledSwitch) {
				BotEnabledSwitch = false;
				ChannelConnection.Disconnect();
				WhisperConnection.Disconnect();
				cmdStartBot.Text = "Start";
			} else {
				BotEnabledSwitch = true;
				ChannelConnection.ConnectToServer();
				WhisperConnection.ConnectToServer();
				cmdStartBot.Text = "Stop";
			}
		}

		#region "Save/Load Settings"

		public Bot_Settings Settings = new Bot_Settings();

		public class Bot_Settings {
			public bool AutoStartProgram { get; set; }
			public string Channel { get; set; }
			public string WelcomeMessage { get; set; }
			public string LeavingMessage { get; set; }
			public string CMDCooldown { get; set; }
			public bool LogChatMessages { get; set; }
			public bool PurgeNonSubLinks { get; set; }

			//Giveaway Settings
			public int Giveaway_PointsPerMinute { get; set; }

			public string Giveaway_EnterCommand { get; set; }
			public string Giveaway_ShowPointsCommand { get; set; }
		}

		private void chkboxAutoStart_CheckedChanged(object sender, EventArgs e) {
			saveSettings();
		}

		private void btnSaveSettings_Click(object sender, EventArgs e) {
			saveSettings();
		}

		public void saveSettings() {
			Settings = new Bot_Settings {
				AutoStartProgram = chkboxAutoStart.Checked,
				Channel = ChannelConnection.get_channel(true),
				WelcomeMessage = txtWelcomeMessage.Text,
				LeavingMessage = txtLeavingMessage.Text,
				CMDCooldown = txtCooldownTime.Text,
				LogChatMessages = chkboxLogChatMessages.Checked,
				PurgeNonSubLinks = chkboxPurgeNonSubsLinks.Checked,

				//Giveaway settings
				Giveaway_PointsPerMinute = Convert.ToInt32(txtGiveawayPointsEarnedPerX.Text)
			};

			StreamWriter sw = new StreamWriter(Program.settingsFile);
			sw.WriteLine(JsonConvert.SerializeObject(Settings, Formatting.Indented));
			sw.Close();
		}

		public void loadSettings() {
			if(File.Exists(Program.settingsFile)) {
				StreamReader srBot = new StreamReader(Program.settingsFile);
				var settings = JsonConvert.DeserializeObject<Bot_Settings>(srBot.ReadToEnd());
				FileInfo FileInf = new FileInfo(Program.settingsFile);

				if(!(FileInf.Length == 0)) {
					//Bot settings
					chkboxAutoStart.Checked = settings.AutoStartProgram;
					txtChannel.Text = settings.Channel;

					txtWelcomeMessage.Text = settings.WelcomeMessage;

					txtLeavingMessage.Text = settings.LeavingMessage;
					txtCooldownTime.Text = settings.CMDCooldown;

					chkboxLogChatMessages.Checked = settings.LogChatMessages;
					chkboxPurgeNonSubsLinks.Checked = settings.PurgeNonSubLinks;

					//Giveaway settings
					txtGiveawayPointsEarnedPerX.Text = Convert.ToString(settings.Giveaway_PointsPerMinute);
				}

				srBot.Close();
			}
		}

		#endregion "Save/Load Settings"

		#region "Cooldown system"

		private double unixTimestamp() {
			return (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds;
		}

		private string getCooldown(string commandName) {
			if(cooldownList.Count == 0) {
				return "Something went wrong :(";
			} else {
				if(cooldownList.ContainsKey(commandName)) {
					return "Command is on cooldown for " + Math.Round((cooldownList[commandName] - unixTimestamp()), 2) + " seconds.";
				} else {
					return "Something went wrong :(";
				}
			}
		}

		private bool commandOnCooldown(string commandName) {
			//If cooldown is set to blank or 0 then disable cooldown functionality.
			if(txtCooldownTime.Text == 0.ToString() | string.IsNullOrEmpty(txtCooldownTime.Text)) {
				return false;
			}

			//Set timeout to currentTime + cooldownTime
			double timeoutEnd = unixTimestamp() + Convert.ToDouble(txtCooldownTime.Text);
			if(cooldownList.Count == 0) {
				//Nothing in cooldown list, set command on cooldown.
				cooldownList.Add(commandName, timeoutEnd);
				return false;
			} else {
				if(cooldownList.ContainsKey(commandName)) {
					//Command is in the cooldown list,
					if(unixTimestamp() >= cooldownList[commandName]) {
						//Command has gone off cooldown, remove from list and report back.
						cooldownList.Remove(commandName);
						return false;
					} else {
						//Command still on cooldown
						return true;
					}
				} else {
					//Command not on cooldown, add it to the list and let the command run once.
					cooldownList.Add(commandName, timeoutEnd);
					return false;
				}
			}
		}

		#endregion "Cooldown system"

		public void setup_commands_list() {
			commandList["!turnon"] = "turnOnBot";
			commandList["!turnoff"] = "turnOffBot";

			//commandList["!uptime"] = "uptimeCMD";

			commandList["!maker"] = "creditCMD";

			commandList["!testers"] = "testersCMD";

			commandList["!commands"] = "helpCMD";
			commandList["!help"] = "helpCMD";

			commandList["!song"] = "songCMD";

			commandList["!joust"] = "joustCMD";
			commandList["!jelo"] = "joustCMD";

			commandList["!conquest"] = "conquestCMD";
			commandList["!elo"] = "conquestCMD";

			commandList["!give"] = "givePointsCMD";
			commandList["!toppoints"] = "topPointsCMD";
			commandList["!points"] = "showPointsCMD";

			commandList["!enter"] = "enterGiveawayCMD";
		}

		public void setup_tooltips() {
			ToolTip toolTip1 = new ToolTip();
			toolTip1.AutoPopDelay = 5000;
			toolTip1.InitialDelay = 500;
			toolTip1.ReshowDelay = 500;
			toolTip1.ShowAlways = true;
			toolTip1.SetToolTip(txtChannel, "Enter your twitch channel here, example: ManselD or #ManselD");
			toolTip1.SetToolTip(txtCooldownTime, "Enter the delay (in seconds) between each command entered. If 0 is entered then there'll be no cooldown.");
			toolTip1.SetToolTip(txtWelcomeMessage, "Enter the message that you want the bot to say when it enters the chat.");
			toolTip1.SetToolTip(txtLeavingMessage, "Enter the message that you want the bot to say when it leaves the chat.");
		}

		public Stream_Data StreamData = new Stream_Data();

		public void ReceiveData(string message, string IP) {
			string sender_message = string.Empty;
			Twitch_User sender;
			string userMessage = string.Empty;
			string[] arr = message.Split(' ');

			//logMessage("Received: " + message);

			if(arr[2] == "PRIVMSG") {
				sender = ChannelConnection.get_sender(message);
				var weirdString = ChannelConnection.get_channel(true) + " :";
				userMessage = message.Substring(message.IndexOf(weirdString));
				userMessage = userMessage.Remove(0, weirdString.Length);
				var msg = userMessage.Split(' ');

				//Only log messages if the checkbox is ticked.
				if(chkboxLogChatMessages.Checked) {
					logMessage(sender.Username + ": " + userMessage);
				}

				//Purge non-sub links (Moderator exception)
				if(chkboxPurgeNonSubsLinks.Checked) {
					if(ChannelConnection.get_channel(false) != sender.Username.ToLower()) {
						if(sender.User_Type != "mod") {
							if(!sender.Sub) {
								foreach(string potentialURL in msg) {
									string[] lines = Properties.Resources.TLDS.Split(new string[1] { Environment.NewLine }, StringSplitOptions.None);
									bool illegal = false;
									string newUrl = potentialURL;

									if(!potentialURL.Contains("http://") && !potentialURL.Contains("https://") && !potentialURL.Contains("www.")) {
										newUrl = "http://" + potentialURL;
									}

									foreach(string line in lines) {
										if(line != null) {
											if(potentialURL.Contains("." + line.ToLower() + "?")) {
												illegal = true;
												break;
											} else if(potentialURL.Contains("." + line.ToLower() + "/")) {
												illegal = true;
												break;
											} else if(potentialURL.Contains("." + line.ToLower())) {
												illegal = true;
												break;
											}
										}
									}

									//Last check (if url hasn't already found to be illegal)
									/*
									if(!illegal) {
										if(Uri.IsWellFormedUriString(potentialURL, UriKind.RelativeOrAbsolute)) {
											string newUrl = potentialURL;
											if(!potentialURL.Contains("http://") && !potentialURL.Contains("https://") && !potentialURL.Contains("www.")) {
												newUrl = "http://" + potentialURL;
											}

											try {
												//This needs speeding up, really slow.
												HttpWebRequest req = (HttpWebRequest)WebRequest.Create(new Uri(newUrl));
												req.Proxy = null;
												req.Method = WebRequestMethods.Http.Head;
												HttpWebResponse response = (HttpWebResponse)req.GetResponse();
												if(response.StatusCode == HttpStatusCode.OK) {
													illegal = true;
												}
											} catch(WebException ex) {
												if(ex.Response != null) {
													illegal = true;
												}
											} catch {
											}
										}

										if(illegal) {
											ChannelConnection.send_message("/timeout " + sender.Username + " 1");
											ChannelConnection.send_message("Naughty " + sender.Username + ", only subs can link!");
											break;
										}
									}
									*/

									if(illegal) {
										ChannelConnection.send_message("/timeout " + sender.Username + " 1");
										ChannelConnection.send_message("Naughty " + sender.Username + ", only subs can link!");
										break;
									}
								}
							}
						}
					}

					//Admin commands.
					if(sender.Username.ToLower() == ChannelConnection.get_channel(false).ToLower() || sender.Username == "ManselD") {
						if(commandList.ContainsKey(msg[0])) {
							switch(commandList[msg[0]]) {
								case "turnOffBot":
									ChannelConnection.send_message(sender + " is abusing me, I'm off!");
									logMessage(LOG_PREFIX + " Bot has been turned off by " + sender);
									BotEnabled = false;
									break;

								case "turnOnBot":
									ChannelConnection.send_message(sender + " has summoned me. Hello everyone!");
									logMessage(LOG_PREFIX + " Bot has been turned on by " + sender);
									BotEnabled = true;
									break;
							}
						}
					}

					//Don't process any other commands if the bot has been turned off through chat.
					if(BotEnabled) {
						if(commandList.ContainsKey(msg[0])) {
							switch(commandList[msg[0]]) {
								case "testersCMD":
									if(!commandOnCooldown("testersCMD")) {
										ChannelConnection.send_message("Thank the lab rats Itzkydvil and Poul76 for testing this bot!");
									}
									break;

								case "creditCMD":
									if(!commandOnCooldown("creditCMD")) {
										ChannelConnection.send_message("This bot was made by ManselD!");
									}
									break;

								case "helpCMD":
									if(!commandOnCooldown("helpCMD")) {
										var sb = new StringBuilder();
										sb.Append("Here is the full list of commands: ");
										foreach(string cmd in commandList.Keys) {
											sb.Append(cmd + ", ");
										}
										ChannelConnection.send_message(sb.ToString());
									}
									break;

								case "uptimeCMD":
									if(!commandOnCooldown("uptimeCMD")) {
										var check = new WebClient();
										check.Proxy = null;
										check.DownloadStringAsync(new Uri("https://api.twitch.tv/kraken/streams/samdadude"));
										check.DownloadStringCompleted += (s, e) => {
											dynamic StreamDat = JObject.Parse(e.Result);
											//DateTime dt = new DateTime(long.Parse(StreamDat.stream.created_at));

											//ChannelConnection.send_message("The stream has been up for " + dt.Hour + " hours and " + dt.Minute + " minutes.");
										};
									}
									break;

								case "songCMD":
									string[] songList = { "https://www.youtube.com/watch?v=z6qaO-vX080", "https://www.youtube.com/watch?v=dQw4w9WgXcQ", "https://www.youtube.com/watch?v=oT3mCybbhf0" };
									var rnd = new Random();
									if(!commandOnCooldown("songCMD")) {
										ChannelConnection.send_message("The current song playing is: " + songList[rnd.Next(0, songList.Length)]);
									}
									break;

								case "joustCMD":
									if(!(msg.Length == 2)) { break; }
									if(!commandOnCooldown("joustCommand")) {
										ChannelConnection.send_message(wc.DownloadString(website + "&mode=joust&code=" + txtAPICode.Text + "&username=" + msg[1]));
									} else {
										//send_message(getCooldown("joustCommand"));
									}
									break;

								case "conquestCMD":
									if(!(msg.Length == 2)) { break; }
									if(!commandOnCooldown("conquestCommand")) {
										ChannelConnection.send_message(wc.DownloadString(website + "&mode=conquest&code=" + txtAPICode.Text + "&username=" + msg[1]));
									} else {
										//send_message(getCooldown("conquestCommand"));
									}
									break;

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

										ChannelConnection.send_message(topUser + " currently has the most points with " + topPoints + " points.");
									} catch(Exception ex) {
										logMessage("Error with command '" + msg[0] + "': " + ex.ToString());
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
							}
						}
					}
				} else {
				}
			}
		}

		#region "Giveaway"

		public class Giveaway_Links {
		}

		public class Giveaway_Chatters {
			public List<object> moderators { get; set; }
			public List<object> staff { get; set; }
			public List<object> admins { get; set; }
			public List<object> global_mods { get; set; }
			public List<string> viewers { get; set; }
		}

		public class Giveaway_Twitch_Data {
			public Giveaway_Links _links { get; set; }
			public int chatter_count { get; set; }
			public Giveaway_Chatters chatters { get; set; }
		}

		private bool removePoints(string user, int points) {
			if(File.Exists(Program.giveawayPointsFile)) {
				var doc = XDocument.Load(Program.giveawayPointsFile);
				var removedPoints = false;
				foreach(XElement el in doc.Element("Users").Elements()) {
					if(el.Element("Username").Value == user) {
						var curPoints = Convert.ToInt64(el.Element("Points").Value);
						if(curPoints >= points) {
							var newPoints = curPoints - points;
							var newUser = new XElement("User",
							  new XElement("Username", user),
							  new XElement("Points", newPoints));
							el.ReplaceWith(newUser);
							doc.Save(Program.giveawayPointsFile);
							removedPoints = true;
							break;
						} else {
							removedPoints = false;
							break;
						}
					}
				}

				if(removedPoints) {
					return true;
				} else {
					return false;
				}
			} else {
				return false;
			}
		}

		private void addPoints(string user, long points) {
			if(File.Exists(Program.giveawayPointsFile)) {
				var doc = XDocument.Load(Program.giveawayPointsFile);

				var userInFile = false;
				foreach(XElement el in doc.Element("Users").Elements()) {
					var curUser = el.Element("Username").Value;
					if(curUser == user) {
						userInFile = true;
						break;
					}
				}

				if(userInFile) {
					//Update existing user's points
					foreach(XElement el in doc.Element("Users").Elements()) {
						if(el.Element("Username").Value == user) {
							points = points + Convert.ToInt64(el.Element("Points").Value);
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
				doc.Save(Program.giveawayPointsFile);
			} else {
				var doc = new XDocument(
					new XElement("Users",
						new XElement("User",
							new XElement("Username", user),
							new XElement("Points", points)
						)
					)
				);
				doc.Save(Program.giveawayPointsFile);
			}
		}

		private delegate void addGiveawayPointsInvoker(object s, EventArgs e);

		public void addGiveawayPoints(object s, EventArgs e) {
			if(this.InvokeRequired) {
				this.Invoke(new addGiveawayPointsInvoker(addGiveawayPoints), s, e);
			} else {
				try {
					var twitchDL = new WebClient();
					var data = twitchDL.DownloadString("http://tmi.twitch.tv/group/user/" + ChannelConnection.get_channel() + "/chatters");
					var JSON = JsonConvert.DeserializeObject<Giveaway_Twitch_Data>(data);
					//Add points to the moderators
					foreach(string user in JSON.chatters.moderators) {
						addPoints(user, Convert.ToInt64(txtGiveawayPointsEarnedPerX.Text));
					}

					//Add points to the viewers
					foreach(string user in JSON.chatters.viewers) {
						addPoints(user, Convert.ToInt64(txtGiveawayPointsEarnedPerX.Text));
					}
				} catch(Exception) {
					//MessageBox.Show(ex.ToString());
				}
			}
		}

		private bool GiveawayActive = false;
		private Timer GiveawayTimer;
		private Timer GiveawayLengthProgressTimer;
		private double GiveawayFinishTime;
		private double GiveawayTotalLength;

		private void cmdGiveawayStart_Click(object sender, EventArgs e) {
			if(!(GiveawayActive)) {
				//If a giveaway isn't currently hapenning.
				GiveawayTotalLength = Convert.ToInt16(txtGiveawayLength.Text) * 60;
				GiveawayFinishTime = unixTimestamp() + GiveawayTotalLength;
				lblGiveawayLengthProgress.Visible = true;

				GiveawayActive = true;
				GiveawayTimer = new Timer();
				GiveawayTimer.Interval = Convert.ToInt16(txtGiveawayLength.Text) * 1000 * 60;
				GiveawayTimer.Tick += GiveawayTimerFinished;
				GiveawayTimer.Enabled = true;

				//Report back on progress every second.
				GiveawayLengthProgressTimer = new Timer();
				GiveawayLengthProgressTimer.Interval = 1000;
				GiveawayLengthProgressTimer.Tick += GiveawayLengthProgressReport;
				GiveawayLengthProgressTimer.Enabled = true;

				lblGiveawayProgress.Text = "A giveaway has been started. Entered users: " + giveawayEnteredUsers.Items.Count + ".";
				ChannelConnection.send_message("A giveaway has started for " + txtGiveawayAmountOfGiveawayItems.Text + " item(s). This will last " + txtGiveawayLength.Text + " minutes. If you have " + txtGiveawayPointsRequired.Text + " points type !enter to enter the giveaway.");
			}
		}

		public delegate void addGiveawayUserToListInvoker(string user);

		private void addGiveawayUserToList(string user) {
			if(this.InvokeRequired) {
				this.Invoke(new addGiveawayUserToListInvoker(addGiveawayUserToList), user);
			} else {
				giveawayEnteredUsers.Items.Add(user);
				if(giveawayEnteredUsers.Items.Count >= 1) {
					lblGiveawayProgress.Text = "A giveaway has been started. Entered users: " + giveawayEnteredUsers.Items.Count + ".";
				}
			}
		}

		private delegate void GiveawayTimerFinishedInvoke(object s, EventArgs e);

		private List<string> giveawayWinners = new List<string>();

		private void GiveawayTimerFinished(object s, EventArgs e) {
			if(this.InvokeRequired) {
				this.Invoke(new GiveawayTimerFinishedInvoke(GiveawayTimerFinished), s, e);
			} else {
				if(GiveawayActive) {
					//Determine winner
					logMessage("Giveaway has just finished at " + DateTime.Now);
					GiveawayActive = false;
					GiveawayTimer.Enabled = false;
					GiveawayLengthProgressTimer.Enabled = false;

					if(giveawayEnteredUsers.Items.Count >= 1) {
						var giveawayItems = Convert.ToInt32(txtGiveawayAmountOfGiveawayItems.Text);
						var rand = new Random();
						for(int i = 0; i < giveawayItems; i++) {
							var user = giveawayEnteredUsers.Items[rand.Next(0, giveawayEnteredUsers.Items.Count)].ToString();
							while(giveawayWinners.Contains(user)) {
								user = giveawayEnteredUsers.Items[rand.Next(0, giveawayEnteredUsers.Items.Count)].ToString();
							}

							giveawayWinners.Add(user);
						}

						if(giveawayWinners.Count > 1) {
							var winnerMsg = "The giveaway has finished. The winners are... ";
							foreach(string winner in giveawayWinners) {
								//If it's the last winner, don't add a comma at the end, add a period.
								if(winner == giveawayWinners[giveawayWinners.Count - 1]) {
									winnerMsg += " and " + winner + ".";
								} else {
									winnerMsg += " " + winner + ",";
								}
							}
							ChannelConnection.send_message(winnerMsg + " Congratulations, the winners will get their prize(s) shortly.");
							lblGiveawayProgress.Text = "A giveaway has finished, the winners were: " + winnerMsg;
							WhisperConnection.send_message("/w " + WhisperConnection.get_channel(false) + " A giveaway has finished, the winners were: " + winnerMsg);
						} else {
							ChannelConnection.send_message("The giveaway has finished. The winner is " + giveawayWinners[0] + ", congratulations! You will get your prize(s) soon!");
							lblGiveawayProgress.Text = "A giveaway has finished, the winner was: " + giveawayWinners[0];
							WhisperConnection.send_message("/w " + WhisperConnection.get_channel(false) + " A giveaway has finished, the winner was: " + giveawayWinners[0]);
						}
					} else {
						ChannelConnection.send_message("Not enough people entered the giveaway so there was no winner.");
						lblGiveawayProgress.Text = "A giveaway has finished with no winner.";
					}

					//Hide giveaway length
					lblGiveawayLengthProgress.Visible = false;

					//Clear entered users and winners list.
					giveawayEnteredUsers.Items.Clear();
					giveawayWinners.Clear();
				}
			}
		}

		private delegate void GiveawayLengthProgressReportInvoke(object s, EventArgs e);

		private void GiveawayLengthProgressReport(object s, EventArgs e) {
			if(this.InvokeRequired) {
				this.Invoke(new GiveawayLengthProgressReportInvoke(GiveawayLengthProgressReport), s, e);
			} else {
				var curTimeLeft = Math.Round(GiveawayFinishTime - unixTimestamp());
				lblGiveawayLengthProgress.Text = curTimeLeft + " seconds left on giveaway.";
			}
		}

		#endregion "Giveaway"

		#region "Misc"

		private delegate void addLogMessageInvoker(string msg);

		public void logMessage(string msg) {
			if(this.InvokeRequired) {
				this.Invoke(new addLogMessageInvoker(logMessage), msg);
			} else {
				var logText = DateTime.Now.Hour + ":" + DateTime.Now.Minute + " " + msg;
				logs.Items.Add(logText);
			}
		}

		private void txtCommand_KeyDown(object sender, KeyEventArgs e) {
			if(e.KeyCode == Keys.Enter) {
				cmdSendCommand.PerformClick();
			}
		}

		private void cmdSendCommand_Click(object sender, EventArgs e) {
			if(!(txtCommand.Text == "")) {
				if(txtCommand.Text.StartsWith("/w ")) {
					WhisperConnection.send_message(txtCommand.Text);
					txtCommand.Clear();
				} else {
					ChannelConnection.send_message(txtCommand.Text);
					txtCommand.Clear();
				}
			}
		}

		private void chkboxShowAPICode_CheckedChanged(object sender, EventArgs e) {
			if(chkboxShowAPICode.Checked) {
				txtAPICode.UseSystemPasswordChar = false;
			} else {
				txtAPICode.UseSystemPasswordChar = true;
			}
		}

		private void chkboxLogChatMessages_CheckedChanged(object sender, EventArgs e) {
			if(chkboxLogChatMessages.Checked) {
				logMessage(LOG_PREFIX + " Chat logging has been enabled.");
			} else {
				logMessage(LOG_PREFIX + " Chat logging has been disabled.");
			}
		}

		#endregion "Misc"
	}
}