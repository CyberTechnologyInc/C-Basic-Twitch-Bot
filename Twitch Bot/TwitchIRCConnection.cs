using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TwitchBot {

	internal class TwitchIRCConnection {
		private TcpClient Client = new TcpClient();
		private NetworkStream Stream;
		private byte[] DataBuffer = new byte[1024];
		private int port = 6667;

		private Timer GiveawayPointAdder;

		public string CON_IP { get; private set; }
		public string CON_Username { get; private set; }
		public string CON_Password { get; private set; }
		public string CON_Channel { get; private set; }
		public bool Connected { get; private set; }

		public TwitchIRCConnection(string IP, string channel, string user, string pass) {
			CON_IP = IP;
			CON_Channel = channel;
			CON_Username = user;
			CON_Password = pass;
			Connected = false;
		}

		#region "Misc"

		public string get_channel(bool addHashtag = false) {
			if(CON_Channel.Substring(0, 1) == "#") {
				if(addHashtag) {
					return Program.BotForm.txtChannel.Text.ToLower();
				} else {
					return Program.BotForm.txtChannel.Text.Substring(1, CON_Channel.Length - 1).ToLower();
				}
			} else {
				if(addHashtag) {
					return "#" + CON_Channel.ToLower();
				} else {
					return CON_Channel.ToLower();
				}
			}
		}

		public Twitch_User get_sender(string msg) {
			//This could probably be made faster, will do it soon.
			//Gather all of the user's data (everything before a space), which is concatenated together with ;
			Twitch_User TwitchUser;
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
				Program.BotForm.logMessage("Error: " + ex.ToString());
			}
			TwitchUser = new Twitch_User(name, moderator, sub);
			return TwitchUser;
		}

		public void send_data(byte[] data) {
			if(!Client.Connected) { return; }
			Stream.Write(data, 0, data.Length);
			Stream.Flush();
		}

		public void send_message(string msg) {
			string data = "PRIVMSG " + get_channel(true) + " :" + msg + Environment.NewLine;
			send_data(Encoding.UTF8.GetBytes(data));
			Program.BotForm.logMessage("Sent message: " + msg);
		}

		public void send_pass() {
			string data = "PASS " + CON_Password + Environment.NewLine;
			send_data(Encoding.UTF8.GetBytes(data));
			Program.BotForm.logMessage("Sent password to authenticate");
		}

		public void send_nick() {
			string data = "NICK " + CON_Username + Environment.NewLine;
			send_data(Encoding.UTF8.GetBytes(data));
			Program.BotForm.logMessage("Sent nick to authenticate: " + CON_Username);
		}

		public void join_channel() {
			string data = "JOIN " + get_channel(true) + Environment.NewLine;
			send_data(Encoding.UTF8.GetBytes(data));
			Program.BotForm.logMessage("Sent request to join channel: " + get_channel(true));
		}

		public void disconnect_channel() {
			string data = "PART " + get_channel(true) + Environment.NewLine;
			send_data(Encoding.UTF8.GetBytes(data));
			Program.BotForm.logMessage("Disconnected from channel: " + get_channel(true));
		}

		#endregion "Misc"

		public void ConnectToServer() {
			try {
				if(!Client.Connected) {
					Client = new TcpClient();
					int attemptNum = 1;
					Program.BotForm.logMessage("Connecting to host " + CON_IP + ", attempt " + attemptNum + ".");
					while(!Client.Connected) {
						Client.Connect(CON_IP, port);
						System.Threading.Thread.Sleep(250);
						attemptNum += 1;
					}

					if(Client.Connected) {
						Stream = Client.GetStream();

						//Start to read data received from the network connection.
						Stream.BeginRead(DataBuffer, 0, DataBuffer.Length, receiveData, null);

						Program.BotForm.logMessage("Connected to " + CON_IP + " on port " + port);

						Program.BotForm.logMessage("Attempting to authenticate " + CON_Username + " on " + get_channel(true));

						send_pass();
						send_nick();

						//send_data(Encoding.UTF8.GetBytes("CAP REQ :twitch.tv/membership"));
						//Adds IRC v3 message tags to PRIVMSG, USERSTATE, NOTICE and GLOBALUSERSTATE (if enabled with commands CAP)
						send_data(Encoding.UTF8.GetBytes("CAP REQ :twitch.tv/tags" + Environment.NewLine));

						join_channel();

						Connected = true;

						//Start point maker (activates every minute)
						GiveawayPointAdder = new Timer();
						GiveawayPointAdder.Interval = 1000 * 60;
						GiveawayPointAdder.Tick += new EventHandler(Program.BotForm.AddGiveawayPoints);
						GiveawayPointAdder.Enabled = true;

						send_message(Program.BotForm.txtWelcomeMessage.Text);
					}
				} else {
					Disconnect();
				}
			} catch(Exception ex) {
				Program.BotForm.logMessage("An error has occured: " + ex.ToString());
			}
		}

		public void Disconnect() {
			send_message(Program.BotForm.txtLeavingMessage.Text);
			Connected = false;
			Client.Close();
			Stream.Close();
			disconnect_channel();
			GiveawayPointAdder.Enabled = false;
		}

		public void receiveData(IAsyncResult ar) {
			try {
				int length = Stream.EndRead(ar);
			} catch(ObjectDisposedException) {
				return;
			}

			string message = string.Empty;

			try {
				while(true) {
					//If we want to close the connection, stop reading from the stream so that it can be ended.
					if(!Connected) {
						Stream.EndRead(ar);
						break;
					}

					Stream.Read(DataBuffer, 0, DataBuffer.Length);
					message = Encoding.UTF8.GetString(DataBuffer).Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0];

					if(message.Length >= 1) {

						string[] arr = message.Split(' ');

						//Process PING/PONG or send data to main form for processing
						if(arr[0] == "PING") {
							Program.BotForm.logMessage("Received PING, sending PONG to " + CON_IP);
							send_data(Encoding.UTF8.GetBytes("PONG " + CON_IP + Environment.NewLine));
						} else {
							Program.BotForm.ReceiveData(message, CON_IP);
						}
					}
				}
				//} catch() {
			} catch(System.IO.IOException) { 

			} catch(Exception ex) {
				Program.BotForm.logMessage("Error: " + ex.ToString());
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