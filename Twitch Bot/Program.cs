using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace TwitchBot {
	class Program {
		public static Bot BotForm { get; set; }

		public static double CurrentVersion { get; private set; }
		public static string Username { get; set; }
		public static string SettingsDirectory;
		public static string LoginSettingsFile;
		public static string GiveawayPointsFile;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.SetCompatibleTextRenderingDefault(false);
			Application.EnableVisualStyles();

			BotForm = new Bot();
            Application.Run(BotForm);
        }
    }
}
