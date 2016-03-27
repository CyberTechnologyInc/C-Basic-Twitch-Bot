using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;

namespace TwitchBot.ChatCommands {
	public class VariableData {
		public string Name { get; set; }
		public long Value { get; set; }
		public bool Increment { get; set; }
	}

	public class CommandData{
		public string Name { get; set; }
		public bool Enabled { get; set; }
		public string Description { get; set; }
		public string[] Privileges { get; set; }
		public List<VariableData> Variables { get; set; }

		//Custom command - only accepts static replies for now
		public string CustomReply { get; set; }
		public bool SendViaChat { get; set; }
	}

	internal static class CommandManager {
		public static List<CommandData> commands = new List<CommandData>();
		public static readonly string commandsFile = Application.StartupPath + @"\commands.xml";
		public const string PRIVILEGE_SPLIT_STRING = ", ";

		public static string BoolToYesNo(bool value) {
			return value ? "Yes" : "No";
		}

		public static bool YesNoToBool(string value) {
			return value == "Yes";
		}

		public static void LoadCommands() {
			if(File.Exists(commandsFile)) {
				var doc = XDocument.Load(commandsFile);

				foreach(var el in doc.Elements("Commands").Elements()) {
					var vars = new List<VariableData>();
					foreach(var itm in (el.Attribute("variables")?.Value ?? "").Split(';')) {
						if(!string.IsNullOrEmpty(itm)) {
							var pairs = itm.Split(':');
							vars.Add(new VariableData {
								Name = pairs[0],
								Increment = pairs[1] == "1",
                                Value = Convert.ToInt32(pairs[2])
							});
						}
					}

					commands.Add(new CommandData { Name = el.Attribute("name")?.Value ?? "",
						Enabled = Convert.ToBoolean(el.Attribute("enabled")?.Value ?? "false"),
						Description = el.Attribute("description")?.Value ?? "",
						Privileges = (el.Attribute("privileges")?.Value ?? "").Split(' '),
						CustomReply = el.Attribute("custom_reply")?.Value ?? "",
						SendViaChat = Convert.ToBoolean(el.Attribute("sendViaChat")?.Value ?? "true"),
						Variables = vars
					});
				}
				
				doc.Save(commandsFile);
			}
		}

		/// <summary>
		/// Get all of the data related to the command or null on failure
		/// </summary>
		/// <param name="command">The command's trigger name ("!credits" for example)</param>
		/// <returns></returns>
		public static CommandData GetCommandData(string command) {
			//If command exists in the array, return the data on it
			var commandExists = commands.FindIndex(f => f.Name == command);
			if(commandExists != -1){
				return commands[commandExists];
			} else {
				return null;
			}
		}

		/// <summary>
		/// Adds a new command to the list
		/// </summary>
		/// <param name="command"></param>
		public static void AddCommand(CommandData command) {
			var id = commands.FindIndex(f => f.Name == command.Name);
			if(commands.Count > 0 && id != -1) {
				commands.RemoveAt(id);
			}

			commands.Add(command);
		}

		/// <summary>
		/// Get the command in XML format - used for persistent storage
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		private static XElement GetNewCommandXml(CommandData command) {
			var tempStr = new StringBuilder();
			foreach(var itm in command.Variables) {
				tempStr.Append(itm.Name + ":" + (itm.Increment ? "1" : "0") + ":" + itm.Value + ";");
			}

			return new XElement("command",
						new XAttribute("name", command.Name),
						new XAttribute("enabled", command.Enabled),
						new XAttribute("description", command.Description),
						new XAttribute("privileges", string.Join(PRIVILEGE_SPLIT_STRING, command.Privileges)),
						new XAttribute("custom_reply", command.CustomReply),
						new XAttribute("sendViaChat", command.SendViaChat),
						new XAttribute("variables", tempStr)
					);
		}

		/// <summary>
		/// Saves all of the stored commands into their XML version (persistent saving).
		/// </summary>
		public static void SaveCommands() {
			foreach(var command in commands) {
				if(!File.Exists(commandsFile)) {
					var doc = new XDocument(new XElement("Commands"));
					var data = GetNewCommandXml(command);
                    doc.Root?.Add(data);
					doc.Save(commandsFile);
				} else {
					var doc = XDocument.Load(commandsFile);
					var found = false;
					foreach(var el in doc.Elements("Commands").Elements()) {
						var cmdName = el.Attribute("name");
                        if(cmdName != null) {
							if(cmdName.Value == command.Name) {
								el.ReplaceWith(GetNewCommandXml(command));
								found = true;
								break;
							}
						}
					}

					if(!found) {
						doc.Root?.Add(GetNewCommandXml(command));
					}

					doc.Save(commandsFile);
				}
			} 
		}

		/// <summary>
		/// Saves a single command into the XML version
		/// </summary>
		/// <param name="command"></param>
		public static void SaveCommand(CommandData command) {
			foreach(var cmd in commands) {
				if(!File.Exists(commandsFile)) {
					var doc = new XDocument(new XElement("Commands"));
					var data = GetNewCommandXml(cmd);
					doc.Root?.Add(data);
					doc.Save(commandsFile);
				} else {
					var doc = XDocument.Load(commandsFile);
					var found = false;
					foreach(var el in doc.Elements("Commands").Elements()) {
						var cmdName = el.Attribute("name");
						if(cmdName != null) {
							if(cmdName.Value == command.Name) {
								el.ReplaceWith(GetNewCommandXml(command));
								found = true;
								break;
							}
						}
					}

					if(!found) {
						doc.Root?.Add(GetNewCommandXml(cmd));
					}

					doc.Save(commandsFile);
				}
			}
		}

		/// <summary>
		/// Updates the command listview to list all of the commands
		/// </summary>
		public static void UpdateCommandList() {
			SaveCommands();

			Program.BotForm.lstViewCommands.Items.Clear();

			foreach(var itm in commands) {
                var lvi = new ListViewItem { Text = itm.Name };
                lvi.SubItems.Add(BoolToYesNo(itm.Enabled));
				lvi.SubItems.Add(itm.Description);
				lvi.SubItems.Add(string.Join(PRIVILEGE_SPLIT_STRING, itm.Privileges));
				Program.BotForm.lstViewCommands.Items.Add(lvi);
			}
        }

		/// <summary>
		/// Removes a command
		/// </summary>
		/// <param name="text">The command's activator (Example: !help)</param>
		public static void RemoveCommand(string text) {
			var id = commands.FindIndex(f => f.Name == text);
			if(commands.Count > 0 && id != -1) {
				commands.RemoveAt(id);
			}

			if(File.Exists(commandsFile)) {
				var doc = XDocument.Load(commandsFile);
				foreach(var el in doc.Elements("Commands").Elements()) {
					if(el.Attribute("name")?.Value == text) {
						el.Remove();
						break;
					}
				}

				doc.Save(commandsFile);
			}
		}
	}
}