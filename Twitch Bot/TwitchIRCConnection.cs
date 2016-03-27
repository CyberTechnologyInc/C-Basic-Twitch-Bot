using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TwitchBot {

	internal class TwitchIrcConnection {
		private TcpClient client = new TcpClient();
		private NetworkStream stream;
		private readonly byte[] dataBuffer = new byte[1024];
		private const int PORT = 6667;

		private Timer giveawayPointAdder;

		public string ConIp { get; set; }
		public string ConUsername { get; set; }
		public string ConPassword { get; set; }
		public string ConChannel { get; set; }
		public bool Connected { get; set; }
		public List<TwitchUser> Users { get; set; }

		public TwitchIrcConnection(string ip, string channel, string user, string pass) {
			Users = new List<TwitchUser>();
			ConIp = ip;
			ConChannel = channel;
			ConUsername = user;
			ConPassword = pass;
			Connected = false;
		}

		#region "Misc"

		public string get_channel(bool addHashtag = false) {
			if(ConChannel.Substring(0, 1) == "#") {
				if(addHashtag) {
					return Program.BotForm.txtChannel.Text.ToLower();
				} else {
					return Program.BotForm.txtChannel.Text.Substring(1, ConChannel.Length - 1).ToLower();
				}
			} else {
				if(addHashtag) {
					return "#" + ConChannel.ToLower();
				} else {
					return ConChannel.ToLower();
				}
			}
		}

		public TwitchUser get_sender(string msg) {
			//This could probably be made faster, will do it soon.
			//Gather all of the user's data (everything before a space), which is concatenated together with ;
			var name = "";
			var sub = false;
			var moderator = "";
            try {
				//Linq, please.
				var arr = msg.Substring(0, msg.IndexOf(" ", StringComparison.Ordinal)).Split(';').Select(x => x.Split('=')).ToDictionary(x => x[0], x => x[1]);

				name = (arr.ContainsKey("display-name")) ? arr["display-name"] : "";
				if(name == "") {
					var siftedJunk = msg.Substring(msg.IndexOf(" :", StringComparison.Ordinal) + 2);
					foreach(var c in siftedJunk) {
						if(c.ToString() == "!") {
							break;
						} else {
							name = name + c;
						}
					}
				}
				sub = arr.ContainsKey("subscriber") && (arr["subscriber"] == "1");
				moderator = arr.ContainsKey("user-type") ? arr["user-type"] : "";

			} catch(Exception ex) {
				Program.BotForm.LogMessage("Error: " + ex.ToString());
			}

			var curUser = new TwitchUser(name, moderator, sub);
			
			if (!Users.Contains(curUser)) {
				Users.Add(curUser);
			}

			return curUser;
		}

		public void send_data(byte[] data) {
			if(!client.Connected) { return; }
			stream.Write(data, 0, data.Length);
			stream.Flush();
		}

		public void send_message(string msg) {
			var data = "PRIVMSG " + get_channel(true) + " :" + msg + Environment.NewLine;
			send_data(Encoding.UTF8.GetBytes(data));
			Program.BotForm.LogMessage("Sent message: " + msg);
		}

		public void send_pass() {
			var data = "PASS " + ConPassword + Environment.NewLine;
			send_data(Encoding.UTF8.GetBytes(data));
			Program.BotForm.LogMessage("Sent password to authenticate");
		}

		public void send_nick() {
			var data = "NICK " + ConUsername + Environment.NewLine;
			send_data(Encoding.UTF8.GetBytes(data));
			Program.BotForm.LogMessage("Sent nick to authenticate: " + ConUsername);
		}

		public void join_channel() {
			var data = "JOIN " + get_channel(true) + Environment.NewLine;
			send_data(Encoding.UTF8.GetBytes(data));
			Program.BotForm.LogMessage("Sent request to join channel: " + get_channel(true));
		}

		public void disconnect_channel() {
			var data = "PART " + get_channel(true) + Environment.NewLine;
			send_data(Encoding.UTF8.GetBytes(data));
			Program.BotForm.LogMessage("Disconnected from channel: " + get_channel(true));
		}

		#endregion "Misc"

		public void ConnectToServer() {
			try {
				if(!client.Connected) {
					client = new TcpClient();
					var attemptNum = 1;
					Program.BotForm.LogMessage("Connecting to host " + ConIp + ", attempt " + attemptNum + ".");
					while(!client.Connected) {
						client.Connect(ConIp, PORT);
						System.Threading.Thread.Sleep(250);
						attemptNum += 1;
					}

					if(client.Connected) {
						stream = client.GetStream();

						//Start to read data received from the network connection.
						stream.BeginRead(dataBuffer, 0, dataBuffer.Length, ReceiveDataFromStream, null);

						Program.BotForm.LogMessage("Connected to " + ConIp + " on port " + PORT);

						Program.BotForm.LogMessage("Attempting to authenticate " + ConUsername + " on " + get_channel(true));

						send_pass();
						send_nick();

						//send_data(Encoding.UTF8.GetBytes("CAP REQ :twitch.tv/membership"));
						//Adds IRC v3 message tags to PRIVMSG, USERSTATE, NOTICE and GLOBALUSERSTATE (if enabled with commands CAP)
						send_data(Encoding.UTF8.GetBytes("CAP REQ :twitch.tv/tags" + Environment.NewLine));

						join_channel();

						Connected = true;

						//Start point maker (activates every minute)
						giveawayPointAdder = new Timer();
						giveawayPointAdder.Interval = 1000 * 60;
						giveawayPointAdder.Tick += Program.BotForm.AddGiveawayPoints;
						giveawayPointAdder.Enabled = true;

						send_message(Program.BotForm.txtWelcomeMessage.Text);
					}
				} else {
					Disconnect();
				}
			} catch(Exception ex) {
				Program.BotForm.LogMessage("An error has occured: " + ex.ToString());
			}
		}

		public void Disconnect() {
			send_message(Program.BotForm.txtLeavingMessage.Text);
			Connected = false;
			client.Close();
			stream.Close();
			disconnect_channel();
			giveawayPointAdder.Enabled = false;
		}

		public delegate void ReceivedData(object sender, string messageToParse);
		public event ReceivedData ReceiveData;

		public void ReceiveDataFromStream(IAsyncResult ar) {
			try {
				stream.EndRead(ar);
			} catch(ObjectDisposedException) {
				return;
			}

			try {
				while(true) {
					//If we want to close the connection, stop reading from the stream so that it can be ended.
					if(!Connected) {
						stream.EndRead(ar);
						break;
					}

					stream.Read(dataBuffer, 0, dataBuffer.Length);
					var message = Encoding.UTF8.GetString(dataBuffer).Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0];

					if(message.Contains(" ") && message.Length >= 1) {

						var arr = message.Split(' ');

						//Process PING/PONG or send data to main form for processing
						if(arr[0] == "PING"){
							send_data(Encoding.UTF8.GetBytes("PONG " + ConIp + Environment.NewLine));
						}else {
							ReceiveData?.Invoke(this, message);
						}
					}
				}
			} catch(System.IO.IOException) { 

			} catch(Exception ex) {
				Program.BotForm.LogMessage("Error: " + ex);
			} finally {
				//If the bot should be connected but error'd out, reconnect.
				if(Connected) {
					Disconnect();
					ConnectToServer();
					send_message("Sorry about that guys. Something went wrong, but I'm back now!");
				}
			}
		}
	}
}