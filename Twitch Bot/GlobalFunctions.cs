using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot {
	public static class GlobalFunctions {
		public static string GetStringBetween(string source, string start, string end) {
			int strt = source.IndexOf(start, StringComparison.Ordinal) + 1;
			int enderino = source.IndexOf(end, strt, StringComparison.Ordinal);
			if(strt != -1 && enderino != -1) {
				return source.Substring(strt, enderino - strt);
			}

			return null;
		}
	}
}
