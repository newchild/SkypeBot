using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronPython.Hosting;
using SKYPE4COMLib;
using IronPython.Runtime;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting; 

namespace SkypeBot_for_Osu_.SkypeBot
{
	
	public class Scripting_Engine
	{
		
		private ScriptEngine pyEngine = null;
		private ScriptRuntime pyRuntime = null;
		private ScriptScope pyScope = null;
		string LoadedSkript;
		public Scripting_Engine(string ScriptName)
		{
			pyEngine = Python.CreateEngine();
			ICollection<string> searchPaths = pyEngine.GetSearchPaths();
			searchPaths.Add("\\Lib");
			pyEngine.SetSearchPaths(searchPaths);
			pyRuntime = Python.CreateRuntime();
			pyScope = pyEngine.CreateScope();
			LoadedSkript = ScriptName;
		}
		
		

		public void addVar(string varName, object value){
			pyScope.SetVariable(varName,value);
		}
		public void addVar(string varName, System.Runtime.Remoting.ObjectHandle handle)
		{
			pyScope.SetVariable(varName, handle);
		}

		public CompiledCode loadScriptFromFile(string Path)
		{
			var script = pyEngine.CreateScriptSourceFromFile(Path);
			var compiled = script.Compile();
			return compiled;
		}
		
		public void runScript(CompiledCode code){
			code.Execute(pyScope);
		}

		public dynamic getOnMessageFunc()
		{
			try
			{
				return pyScope.GetVariable("onMessage");
			}
			catch (Exception e)
			{
				return null;
			}
		}

		public dynamic getImports()
		{
			try {
				return pyScope.GetVariable("imports");
			}
			catch (Exception e)
			{
				return null;
			}
		}

		public dynamic getDescription()
		{
			try {
				return pyScope.GetVariable("description");
			}
			catch (Exception e)
			{
				return "No description set";
			}
			
		}

		public void importModule(string modName)
		{
			pyScope.ImportModule(modName);
		}

		public void logSkript(string text)
		{
			Form1.log(LoadedSkript + ": " + text);
		}
		
	}
}
