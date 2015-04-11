using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using SKYPE4COMLib;
using SkypeBot_for_Osu_.SkypeBot;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Scripting.Hosting;
using System.Runtime.InteropServices;

namespace SkypeBot_for_Osu_
{
	

	public partial class Form1 : Form
	{
		BotCore Core;
		string user;
		List<Scripting_Engine> allScripts = new List<Scripting_Engine>();
		List<string> ScriptNames = new List<string>();
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			Core = new BotCore();
			user = Core.getNickName();
			Core.onMessageReceived+=Core_onMessageReceived;
			Console.Title = "Debug Log";
			Console.WriteLine("Program correctly initialized...");

		}	

		private void Core_onMessageReceived(ChatMessage pMessage, TChatMessageStatus Status)
		{
			foreach (var script in allScripts)
			{
				var test = script.getOnMessageFunc();
				if(test != null)
					test(pMessage, Status);
			}
		}

		

		private void setupVars(Scripting_Engine PyEngine)
		{
			PyEngine.addVar("MessageBox", new Action<string>(ShowMessageBox));
			PyEngine.addVar("user", user);
			PyEngine.addVar("sendMessage", new Action<string, string>(Core.sendMessage));
			PyEngine.addVar("sendMessageToChat", new Action<Chat, string>(Core.sendMessageToChat));
			PyEngine.addVar("log", new Action<string>(PyEngine.logSkript));
		}

		private void button1_Click(object sender, EventArgs e)
		{
			
			openFileDialog1.ShowDialog();
			if (!ScriptNames.Contains(openFileDialog1.SafeFileName))
			{
				var file = openFileDialog1.FileName;
				var engine = new Scripting_Engine(openFileDialog1.SafeFileName);
				setupVars(engine);
				var scriptloaded = engine.loadScriptFromFile(file);
				allScripts.Add(engine);
				engine.runScript(scriptloaded);
				string desc = engine.getDescription();
				ScriptNames.Add(openFileDialog1.SafeFileName);
				Console.WriteLine(openFileDialog1.SafeFileName + " was succesfully loaded");
				var stringBuilder = "Selected Scripts:\n";
				foreach (var name in ScriptNames)
				{
					stringBuilder += name + ": " + desc + "\n";
				}
				label1.Text = stringBuilder;
				
			}
			else
			{
				MessageBox.Show("Script already loaded!");
			}
			
		}

		private static void ShowMessageBox(string text){
			MessageBox.Show(text);
		}

		public static void log(string text)
		{
			Console.WriteLine(text);
		}
		
	}
}
