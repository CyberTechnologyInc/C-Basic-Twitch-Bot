using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopKekMemeBot {
	class Twitch_User {
		public string Username { set; get; }
		public string User_Type { set; get; }
		public bool Sub { set; get; }

		public Twitch_User(string _Username, string _User_Type, bool _Sub) {
			Username = _Username;
			User_Type = _User_Type;
			Sub = _Sub;
		}
	}
}
