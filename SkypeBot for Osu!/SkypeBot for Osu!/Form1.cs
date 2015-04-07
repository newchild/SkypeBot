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

namespace SkypeBot_for_Osu_
{
	public partial class Form1 : Form
	{
		
		BotCore Core;
		List<CompiledCode> allScripts = new List<CompiledCode>();
		List<string> ScriptNames = new List<string>();
		Scripting_Engine PyEngine;
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			Core = new BotCore();
			PyEngine = new Scripting_Engine();
			setupVars(PyEngine);
			Core.onMessageReceived+=Core_onMessageReceived;
		}	

		private void Core_onMessageReceived(ChatMessage pMessage, TChatMessageStatus Status)
		{
			
			foreach (var script in allScripts)
			{
				PyEngine.runScript(script);
				var test = PyEngine.getOnMessageFunc();
				test(pMessage, Status);
			}
		}

		private void setupVars(Scripting_Engine PyEngine)
		{
			PyEngine.addVar("MessageBox", new Action<string>(ShowMessageBox));
			
		}

		private void button1_Click(object sender, EventArgs e)
		{
			openFileDialog1.ShowDialog();
			if (!ScriptNames.Contains(openFileDialog1.SafeFileName))
			{
				var file = openFileDialog1.FileName;
				var scriptloaded = PyEngine.loadScriptFromFile(file);
				allScripts.Add(scriptloaded);
				PyEngine.runScript(scriptloaded);
				ScriptNames.Add(openFileDialog1.SafeFileName);
				var stringBuilder = "Selected Scripts:\n";
				foreach (var name in ScriptNames)
				{
					stringBuilder += name + "\n";
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
		
	}
}
