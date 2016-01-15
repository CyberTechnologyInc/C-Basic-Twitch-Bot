using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows.Forms;

namespace TopKekMemeBot {

	public class Misc {
		public Bot BotForm { get; set; }

		public void showErrorMessage(Exception ex) {
			if(MessageBox.Show("An error has occured, you could try running the program as an administrator. Show full error?", "Error!", MessageBoxButtons.YesNo) == DialogResult.Yes) {
				MessageBox.Show(ex.ToString());
			}

			//Close every form
			foreach(Form f in Application.OpenForms) {
				f.Close();
			}
		}
	}
}