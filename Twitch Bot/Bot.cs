using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using TwitchBot.JSON_Objects;
using TwitchBot.ChatCommands;
using CTI.SimpleSettings;

namespace TwitchBot {

	public partial class Bot : Form {
		//private Dictionary<string, double> cooldownList = new Dictionary<string, double>();
		//private Dictionary<string, string> commandList = new Dictionary<string, string>();

		private readonly SettingsManager _settings = new SettingsManager();

		private readonly WebClient _wc = new WebClient();

		private const string LogPrefix = "[TKMB]";

		public Bot() {
			InitializeComponent();
		}

		private TwitchIrcConnection _channelConnection;
		private TwitchIrcConnection _whisperConnection;
		private readonly List<IChatCommand> _chatCommands = new List<IChatCommand>();
		
		private void AddCommandToList(string command, string description, string privileges)
		{
			var lvi = new ListViewItem{
				Text = command
			};

			lvi.SubItems.Add(description);
			lvi.SubItems.Add(privileges);
			lstViewCommands.Items.Add(lvi);
		}

		//Think of a good way of setting up commands...
		private void SetupCommands() {
			//Sort commands so that admin commands or the most important ones come first.
			for(var i = 0; i<_chatCommands.Count; i++) {
				//Prevent casting twice
				var itm = _chatCommands[i];
				var adminCmd = itm as IAdminChatCommand;
				if (adminCmd != null && adminCmd.PrioritiseCommands){
					_chatCommands.Remove(itm);
					_chatCommands.Insert(0, itm);
				}
			}

			//Add commands to list
			//TODO: make these commands save somehow, then be editable within the commands view.
			foreach(var itm in _chatCommands) {
				foreach (var cmd in itm.Commands){
					AddCommandToList(cmd[0], cmd[1], cmd[2]);
				}
			}
		}

		private void Bot_Load(object sender, EventArgs e) {
			_wc.Proxy = null;

			Program.GiveawayPointsFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "points.txt");
			Program.LoginSettingsFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "login.txt");
			Program.SettingsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

			//Add commands
			_chatCommands.Add(new AdminCommands());
			_chatCommands.Add(new DefaultChatCommands());
			_chatCommands.Add(new SongCommands());
			_chatCommands.Add(new GiveawayCommands());

			//Setup prioritisation and add commands to the command list
			SetupCommands();

			if(File.Exists(_settingsFile)) {
				_settings.LoadSettings(_settingsFile);
				LoadSettings();
			}

			//Setup connections to IRC servers
			ChatSettings.Channel = txtChannel.Text;

			//Setup connection to normal chat
			_channelConnection = new TwitchIrcConnection("irc.twitch.tv", txtChannel.Text, txtBotUsername.Text, txtBotPassword.Text);
			//Setup connection to be able to whisper to users
			_whisperConnection = new TwitchIrcConnection("199.9.253.119", txtChannel.Text, txtBotUsername.Text, txtBotPassword.Text);


			_giveawayAddPointsTimer = new Timer();
			_giveawayAddPointsTimer.Interval = 60*1000;

			_giveawayAddPointsTimer.Tick += AddGiveawayPoints;

			_giveawayAddPointsTimer.Enabled = true;

			if(chkboxAutoStart.Checked) {
				cmdStartBot.PerformClick();
			}
		}

		private bool _botEnabledSwitch = false;

		private void cmdStartBot_Click(object sender, EventArgs e) {
			if(_botEnabledSwitch) {
				_botEnabledSwitch = false;
				_channelConnection.Disconnect();
				_whisperConnection.Disconnect();
				cmdStartBot.Text = "Start";
			} else {
				_botEnabledSwitch = true;
				_channelConnection.ConnectToServer();
				_whisperConnection.ConnectToServer();
				cmdStartBot.Text = "Stop";
			}
		}

		private readonly string _settingsFile = Application.StartupPath + "\\config.xml";
		#region "Save/Load Settings"

		/*
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
		*/


		//This seems icky to me, is it?
		//Defaults to false
		private static bool GetBool(string data) {
			bool tmpBool;
			if(bool.TryParse(data, out tmpBool)) {
				return tmpBool;
			} else {
				return false;
			}
		}

		private void SaveSettings() {
			//Bot connection details
			_settings.AddSetting("bot_username", txtBotUsername.Text);
			_settings.AddSetting("bot_password", txtBotPassword.Text);
			_settings.AddSetting("channel", txtChannel.Text);

			//General bot settings
			_settings.AddSetting("auto_start_bot", chkboxAutoStart.Checked);
			_settings.AddSetting("command_cooldown", txtCooldownTime.Text);
			_settings.AddSetting("welcome_message", txtWelcomeMessage.Text);
			_settings.AddSetting("leaving_message", txtLeavingMessage.Text);
			_settings.AddSetting("purge_non_sub_links", chkboxPurgeNonSubsLinks.Checked);
			_settings.AddSetting("log_chat_messages", chkboxLogChatMessages.Checked);

			//Giveaway settings
			_settings.AddSetting("giveaway_points_per_minute", txtGiveawayPointsEarnedPerX.Text);
			//Settings.AddSetting("giveaway_enter_command", txt.Text);

			//Save all settings that have been added
			_settings.SaveSettings(_settingsFile);
		}

		private void LoadSettings() {
			//TODO: Use generics in settings manager in order to not cast every single thing
			//Bot connection details
			txtBotUsername.Text = (string)_settings.GetSetting("bot_username");
			txtBotPassword.Text = (string)_settings.GetSetting("bot_password");
			txtChannel.Text = (string)_settings.GetSetting("channel");

			//General bot settings
			chkboxAutoStart.Checked = GetBool(_settings.GetSetting("auto_start_bot").ToString());
            txtCooldownTime.Text = (string)_settings.GetSetting("command_cooldown");
            txtWelcomeMessage.Text = (string)_settings.GetSetting("welcome_message");
			txtLeavingMessage.Text = (string)_settings.GetSetting("leaving_message");
			chkboxLogChatMessages.Checked = GetBool(_settings.GetSetting("log_chat_messages").ToString());
			chkboxPurgeNonSubsLinks.Checked = GetBool(_settings.GetSetting("purge_non_sub_links").ToString());

			//Giveaway settings
			txtGiveawayPointsEarnedPerX.Text = (string)_settings.GetSetting("giveaway_points_per_minute");
        }

		private void chkboxAutoStart_CheckedChanged(object sender, EventArgs e) {
			SaveSettings();
		}

		private void btnSaveSettings_Click(object sender, EventArgs e) {
			SaveSettings();
		}

		/*
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
		*/

		/*
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
		*/


		#endregion "Save/Load Settings"

		//TODO: Work on cooldown system. Implement it into the new command system
		#region "Cooldown system"
		/*
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
				*/
		#endregion "Cooldown system"

		private double UnixTimestamp() {
			return (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds;
		}

		public void SetupTooltips() {
			var toolTip1 = new ToolTip() {
				AutoPopDelay = 5000,
				InitialDelay = 500,
				ReshowDelay = 500,
				ShowAlways = true
			};

			toolTip1.SetToolTip(txtChannel, "Enter your twitch channel here, example: ManselD or #ManselD");
			toolTip1.SetToolTip(txtCooldownTime, "Enter the delay (in seconds) between each command entered. If 0 is entered then there'll be no cooldown.");
			toolTip1.SetToolTip(txtWelcomeMessage, "Enter the message that you want the bot to say when it enters the chat.");
			toolTip1.SetToolTip(txtLeavingMessage, "Enter the message that you want the bot to say when it leaves the chat.");
		}

		public StreamData StreamData = new StreamData();

		public void ReceiveData(string message, string ip) {
			var userMessage = string.Empty;
			string[] arr = message.Split(' ');

			//logMessage("Received: " + message);

			if(arr[2] == "PRIVMSG") {
				var sender = _channelConnection.get_sender(message);
				var weirdString = _channelConnection.get_channel(true) + " :";
				//StringComparison.Ordinal is good for comparing English words and provides better speed efficiency.
				userMessage = message.Substring(message.IndexOf(weirdString, StringComparison.Ordinal));
				userMessage = userMessage.Remove(0, weirdString.Length);
				var msg = userMessage.Split(' ');

				//Only log messages if the checkbox is ticked.
				if(chkboxLogChatMessages.Checked) {
					LogMessage(sender.Username + ": " + userMessage);
				}

				//Purge non-sub links (Moderator exception)
				if(chkboxPurgeNonSubsLinks.Checked) {
					if(_channelConnection.get_channel(false) != sender.Username.ToLower()) {
						if(sender.UserType != "mod") {
							if(!sender.Sub) {
								foreach(var potentialUrl in msg) {
									var lines = Properties.Resources.TLDS.Split(new string[1] { Environment.NewLine }, StringSplitOptions.None);
									var illegal = false;

									foreach(var line in lines) {
										if(line != null) {
											if(potentialUrl.Contains("." + line.ToLower() + "?")) {
												illegal = true;
												break;
											} else if(potentialUrl.Contains("." + line.ToLower() + "/")) {
												illegal = true;
												break;
											} else if(potentialUrl.Contains("." + line.ToLower())) {
												illegal = true;
												break;
											}
										}
									}

									if(illegal) {
										_channelConnection.send_message("/timeout " + sender.Username + " 1");
										_channelConnection.send_message("Naughty " + sender.Username + ", only subs can link!");
										break;
									}
								}
							}
						}
					}
				}

				if(msg[0] == "!help" || msg[0] == "!commands"){
					var sb = new StringBuilder();
					sb.Append("Here is a full list of commands: ");
					foreach (var chatCommands in _chatCommands) {
						foreach (var cmd in chatCommands.Commands){
							sb.Append(cmd[0] + ", ");
						}
					}

					var completeData = sb.ToString().Trim();
					completeData = completeData.Remove(completeData.Length - 1);
                    _channelConnection.send_message(completeData);
					return;
				}

				//If it's an admin command and bypassEnabledStatus is true, process command regardless of the bot's enabled status.
				//Otherwise, respect the status.
				foreach(var processor in _chatCommands) {
					bool sendViaChat;
					string returnMessage;
					processor.ProcessCommand(sender, msg, out sendViaChat, out returnMessage);
					if(returnMessage != null) {
						var adminCmd = processor as IAdminChatCommand;
						if(adminCmd != null) {
							if(adminCmd.BypassEnabledStatus) {
								ProcessCommandData(sendViaChat, returnMessage);
								break;
							}
						} else {
							if(ChatSettings.BotEnabled) {
								ProcessCommandData(sendViaChat, returnMessage, sender.Username);
								break;
							}
						}

						return;
					}
				}
			}
		}

		private void ProcessCommandData(bool sendViaChat, string message, string username = "") {
			if(sendViaChat) {
				_channelConnection.send_message(message);
			} else {
				_whisperConnection.send_message(message);
			}
		}

		#region "Giveaway"

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

		private bool RemovePoints(string user, int points) {
			if(File.Exists(Program.GiveawayPointsFile)) {
				var doc = XDocument.Load(Program.GiveawayPointsFile);
				var removedPoints = false;
				foreach(var el in doc.Element("Users").Elements()) {
					if(el.Element("Username").Value == user) {
						var curPoints = Convert.ToInt64(el.Element("Points").Value);
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

				return removedPoints;
			} else {
				return false;
			}
		}

		private void AddPoints(string user, long points) {
			if(File.Exists(Program.GiveawayPointsFile)) {
				var doc = XDocument.Load(Program.GiveawayPointsFile);

				var userInFile = false;
				foreach(var el in doc.Element("Users").Elements()) {
					var curUser = el.Element("Username").Value;
					if(curUser == user) {
						userInFile = true;
						break;
					}
				}

				if(userInFile) {
					//Update existing user's points
					foreach(var el in doc.Element("Users").Elements()) {
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

		private delegate void AddGiveawayPointsInvoker(object s, EventArgs e);

		public void AddGiveawayPoints(object s, EventArgs e) {
			if(this.InvokeRequired) {
				this.Invoke(new AddGiveawayPointsInvoker(AddGiveawayPoints), s, e);
			} else {
				try {
					var twitchDl = new WebClient();
					var data = twitchDl.DownloadString("http://tmi.twitch.tv/group/user/" + _channelConnection.get_channel() + "/chatters");
					var json = JsonConvert.DeserializeObject<GiveawayTwitchData>(data);
					//Add points to the moderators
					foreach(string user in json.Chatters.Moderators) {
						AddPoints(user, Convert.ToInt64(txtGiveawayPointsEarnedPerX.Text));
					}

					//Add points to the viewers
					foreach(string user in json.Chatters.Viewers) {
						AddPoints(user, Convert.ToInt64(txtGiveawayPointsEarnedPerX.Text));
					}
				} catch(Exception) {
					//MessageBox.Show(ex.ToString());
				}
			}
		}

		public bool GiveawayActive;
		private Timer _giveawayAddPointsTimer;
		private Timer _giveawayTimer;
		private Timer _giveawayLengthProgressTimer;
		private double _giveawayFinishTime;
		private double _giveawayTotalLength;

		private void cmdGiveawayStart_Click(object sender, EventArgs e) {
			if(!GiveawayActive) {
				//If a giveaway isn't currently hapenning.
				_giveawayTotalLength = Convert.ToInt16(txtGiveawayLength.Text) * 60;
				_giveawayFinishTime = UnixTimestamp() + _giveawayTotalLength;
				lblGiveawayLengthProgress.Visible = true;

				GiveawayActive = true;
				_giveawayTimer = new Timer{
					Interval = Convert.ToInt16(txtGiveawayLength.Text)*1000*60,
					Enabled = true
				};
				_giveawayTimer.Tick += GiveawayTimerFinished;

				//Report back on progress every second.
				_giveawayLengthProgressTimer = new Timer{
					Interval = 1000,
					Enabled = true
				};
				_giveawayLengthProgressTimer.Tick += GiveawayLengthProgressReport;

				lblGiveawayProgress.Text = "A giveaway has been started. Entered users: " + giveawayEnteredUsers.Items.Count + ".";
				_channelConnection.send_message("A giveaway has started for " + txtGiveawayAmountOfGiveawayItems.Text + " item(s). This will last " + txtGiveawayLength.Text + " minutes. If you have " + txtGiveawayPointsRequired.Text + " points type !enter to enter the giveaway.");
			}
		}

		public delegate void AddGiveawayUserToListInvoker(string user);

		public void AddGiveawayUserToList(string user) {
			if(this.InvokeRequired) {
				this.Invoke(new AddGiveawayUserToListInvoker(AddGiveawayUserToList), user);
			} else {
				giveawayEnteredUsers.Items.Add(user);
				if(giveawayEnteredUsers.Items.Count >= 1) {
					lblGiveawayProgress.Text = "A giveaway has been started. Entered users: " + giveawayEnteredUsers.Items.Count + ".";
				}
			}
		}

		private delegate void GiveawayTimerFinishedInvoke(object s, EventArgs e);

		private readonly List<string> _giveawayWinners = new List<string>();

		private void GiveawayTimerFinished(object s, EventArgs e) {
			if(this.InvokeRequired) {
				this.Invoke(new GiveawayTimerFinishedInvoke(GiveawayTimerFinished), s, e);
			} else {
				if(GiveawayActive) {
					//Determine winner
					LogMessage("Giveaway has just finished at " + DateTime.Now);
					GiveawayActive = false;
					_giveawayTimer.Enabled = false;
					_giveawayLengthProgressTimer.Enabled = false;

					if(giveawayEnteredUsers.Items.Count >= 1) {
						var giveawayItems = Convert.ToInt32(txtGiveawayAmountOfGiveawayItems.Text);
						var rand = new Random();
						for(int i = 0; i < giveawayItems; i++) {
							var user = giveawayEnteredUsers.Items[rand.Next(0, giveawayEnteredUsers.Items.Count)].ToString();
							while(_giveawayWinners.Contains(user)) {
								user = giveawayEnteredUsers.Items[rand.Next(0, giveawayEnteredUsers.Items.Count)].ToString();
							}

							_giveawayWinners.Add(user);
						}

						if(_giveawayWinners.Count > 1) {
							var winnerMsg = "The giveaway has finished. The winners are... ";
							foreach(string winner in _giveawayWinners) {
								//If it's the last winner, don't add a comma at the end, add a period.
								if(winner == _giveawayWinners[_giveawayWinners.Count - 1]) {
									winnerMsg += " and " + winner + ".";
								} else {
									winnerMsg += " " + winner + ",";
								}
							}
							_channelConnection.send_message(winnerMsg + " Congratulations, the winners will get their prize(s) shortly.");
							lblGiveawayProgress.Text = "A giveaway has finished, the winners were: " + winnerMsg;
							_whisperConnection.send_message("/w " + _whisperConnection.get_channel(false) + " A giveaway has finished, the winners were: " + winnerMsg);
						} else {
							_channelConnection.send_message("The giveaway has finished. The winner is " + _giveawayWinners[0] + ", congratulations! You will get your prize(s) soon!");
							lblGiveawayProgress.Text = "A giveaway has finished, the winner was: " + _giveawayWinners[0];
							_whisperConnection.send_message("/w " + _whisperConnection.get_channel(false) + " A giveaway has finished, the winner was: " + _giveawayWinners[0]);
						}
					} else {
						_channelConnection.send_message("Not enough people entered the giveaway so there was no winner.");
						lblGiveawayProgress.Text = "A giveaway has finished with no winner.";
					}

					//Hide giveaway length
					lblGiveawayLengthProgress.Visible = false;

					//Clear entered users and winners list.
					giveawayEnteredUsers.Items.Clear();
					_giveawayWinners.Clear();
				}
			}
		}

		private delegate void GiveawayLengthProgressReportInvoke(object s, EventArgs e);

		private void GiveawayLengthProgressReport(object s, EventArgs e) {
			if(this.InvokeRequired) {
				this.Invoke(new GiveawayLengthProgressReportInvoke(GiveawayLengthProgressReport), s, e);
			} else {
				var curTimeLeft = Math.Round(_giveawayFinishTime - UnixTimestamp());
				lblGiveawayLengthProgress.Text = curTimeLeft + " seconds left on giveaway.";
			}
		}

		#endregion "Giveaway"

		#region "Misc"

		private delegate void AddLogMessageInvoker(string msg);

		public void LogMessage(string msg) {
			if(this.InvokeRequired) {
				this.Invoke(new AddLogMessageInvoker(LogMessage), msg);
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
					_whisperConnection.send_message(txtCommand.Text);
					txtCommand.Clear();
				} else {
					_channelConnection.send_message(txtCommand.Text);
					txtCommand.Clear();
				}
			}
		}

		

		private void chkboxLogChatMessages_CheckedChanged(object sender, EventArgs e) {
			if(chkboxLogChatMessages.Checked) {
				LogMessage(LogPrefix + " Chat logging has been enabled.");
			} else {
				LogMessage(LogPrefix + " Chat logging has been disabled.");
			}
		}

		#endregion "Misc"
	}
}