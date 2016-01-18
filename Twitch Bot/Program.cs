using System;
using System.Windows.Forms;

namespace TwitchBot {
	class Program {
		public static Bot BotForm { get; set; }

		public static double CurrentVersion { get; private set; }
		public static string APICode { get; set; }
		public static string Username { get; set; }
		public static string settingsDirectory;
		public static string loginSettingsFile;
		public static string giveawayPointsFile;

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
