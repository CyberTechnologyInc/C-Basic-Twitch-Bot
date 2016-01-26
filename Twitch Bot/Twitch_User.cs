using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot {
	public class TwitchUser {
		public string Username { set; get; }
		public string UserType { set; get; }
		public bool Sub { set; get; }

		public TwitchUser(string username, string userType, bool sub) {
			Username = username;
			UserType = userType;
			Sub = sub;
		}
	}
}
