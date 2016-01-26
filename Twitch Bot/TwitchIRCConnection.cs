using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TwitchBot {

	internal class TwitchIrcConnection {
		private TcpClient _client = new TcpClient();
		private NetworkStream _stream;
		private byte[] _dataBuffer = new byte[1024];
		private int _port = 6667;

		private Timer _giveawayPointAdder;

		public string ConIp { get; private set; }
		public string ConUsername { get; private set; }
		public string ConPassword { get; private set; }
		public string ConChannel { get; private set; }
		public bool Connected { get; private set; }

		public TwitchIrcConnection(string ip, string channel, string user, string pass) {
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
			TwitchUser twitchUser;
			string name = "";
			bool sub = false;
			string moderator = "";
            try {
				//Linq, please.
				var arr = msg.Substring(0, msg.IndexOf(" ")).Split(';').Select(x => x.Split('=')).ToDictionary(x => x[0], x => x[1]);

				name = (arr.ContainsKey("display-name")) ? arr["display-name"] : "";
				if(name == "") {
					var siftedJunk = msg.Substring(msg.IndexOf(" :") + 2);
					foreach(char c in siftedJunk) {
						if(c.ToString() == "!") {
							break;
						} else {
							name = name + c;
						}
					}
				}
				sub = (arr.ContainsKey("subscriber")) ? (arr["subscriber"] == "1") : false;
				moderator = (arr.ContainsKey("user-type")) ? arr["user-type"] : "";

			} catch(Exception ex) {
				Program.BotForm.LogMessage("Error: " + ex.ToString());
			}
			twitchUser = new TwitchUser(name, moderator, sub);
			return twitchUser;
		}

		public void send_data(byte[] data) {
			if(!_client.Connected) { return; }
			_stream.Write(data, 0, data.Length);
			_stream.Flush();
		}

		public void send_message(string msg) {
			string data = "PRIVMSG " + get_channel(true) + " :" + msg + Environment.NewLine;
			send_data(Encoding.UTF8.GetBytes(data));
			Program.BotForm.LogMessage("Sent message: " + msg);
		}

		public void send_pass() {
			string data = "PASS " + ConPassword + Environment.NewLine;
			send_data(Encoding.UTF8.GetBytes(data));
			Program.BotForm.LogMessage("Sent password to authenticate");
		}

		public void send_nick() {
			string data = "NICK " + ConUsername + Environment.NewLine;
			send_data(Encoding.UTF8.GetBytes(data));
			Program.BotForm.LogMessage("Sent nick to authenticate: " + ConUsername);
		}

		public void join_channel() {
			string data = "JOIN " + get_channel(true) + Environment.NewLine;
			send_data(Encoding.UTF8.GetBytes(data));
			Program.BotForm.LogMessage("Sent request to join channel: " + get_channel(true));
		}

		public void disconnect_channel() {
			string data = "PART " + get_channel(true) + Environment.NewLine;
			send_data(Encoding.UTF8.GetBytes(data));
			Program.BotForm.LogMessage("Disconnected from channel: " + get_channel(true));
		}

		#endregion "Misc"

		public void ConnectToServer() {
			try {
				if(!_client.Connected) {
					_client = new TcpClient();
					int attemptNum = 1;
					Program.BotForm.LogMessage("Connecting to host " + ConIp + ", attempt " + attemptNum + ".");
					while(!_client.Connected) {
						_client.Connect(ConIp, _port);
						System.Threading.Thread.Sleep(250);
						attemptNum += 1;
					}

					if(_client.Connected) {
						_stream = _client.GetStream();

						//Start to read data received from the network connection.
						_stream.BeginRead(_dataBuffer, 0, _dataBuffer.Length, ReceiveData, null);

						Program.BotForm.LogMessage("Connected to " + ConIp + " on port " + _port);

						Program.BotForm.LogMessage("Attempting to authenticate " + ConUsername + " on " + get_channel(true));

						send_pass();
						send_nick();

						//send_data(Encoding.UTF8.GetBytes("CAP REQ :twitch.tv/membership"));
						//Adds IRC v3 message tags to PRIVMSG, USERSTATE, NOTICE and GLOBALUSERSTATE (if enabled with commands CAP)
						send_data(Encoding.UTF8.GetBytes("CAP REQ :twitch.tv/tags" + Environment.NewLine));

						join_channel();

						Connected = true;

						//Start point maker (activates every minute)
						_giveawayPointAdder = new Timer();
						_giveawayPointAdder.Interval = 1000 * 60;
						_giveawayPointAdder.Tick += new EventHandler(Program.BotForm.AddGiveawayPoints);
						_giveawayPointAdder.Enabled = true;

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
			_client.Close();
			_stream.Close();
			disconnect_channel();
			_giveawayPointAdder.Enabled = false;
		}

		public void ReceiveData(IAsyncResult ar) {
			try {
				int length = _stream.EndRead(ar);
			} catch(ObjectDisposedException) {
				return;
			}

			string message = string.Empty;

			try {
				while(true) {
					//If we want to close the connection, stop reading from the stream so that it can be ended.
					if(!Connected) {
						_stream.EndRead(ar);
						break;
					}

					_stream.Read(_dataBuffer, 0, _dataBuffer.Length);
					message = Encoding.UTF8.GetString(_dataBuffer).Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0];

					if(message.Length >= 1) {

						string[] arr = message.Split(' ');

						//Process PING/PONG or send data to main form for processing
						if(arr[0] == "PING") {
							Program.BotForm.LogMessage("Received PING, sending PONG to " + ConIp);
							send_data(Encoding.UTF8.GetBytes("PONG " + ConIp + Environment.NewLine));
						} else {
							Program.BotForm.ReceiveData(message, ConIp);
						}
					}
				}
				//} catch() {
			} catch(System.IO.IOException) { 

			} catch(Exception ex) {
				Program.BotForm.LogMessage("Error: " + ex.ToString());
			} finally {
				//If the bot should be connected but error'd out, reconnect.
				if(Connected) {
					ConnectToServer();
					send_message("Sorry about that guys. Something went wrong, but I'm back now!");
				}
			}
		}
	}
}