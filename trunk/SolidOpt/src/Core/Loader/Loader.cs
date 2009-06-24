using System;

using SolidOpt.Core.Services;

namespace SolidOpt.Core.Loader
{
	public class Loader
	{
		public PluginServiceContainer servicesContainer;

		public Loader()
		{
			Console.WriteLine("Initialize...");
			servicesContainer = new PluginServiceContainer();
		}

		public int Run(string[] args)
		{
			foreach(string s in args) Console.WriteLine(s);
			Console.WriteLine("Search plugins...");
			
			servicesContainer.AddPlugins(AppDomain.CurrentDomain.BaseDirectory + "core");
			servicesContainer.AddPlugins(AppDomain.CurrentDomain.BaseDirectory + "plugins");
			
			foreach(PluginInfo p in servicesContainer.plugins) Console.WriteLine(p.fullName);
			Console.WriteLine("Load plugins...");
			servicesContainer.LoadPlugins();
			Console.WriteLine("Configure plugins...");
			//
			return 0;
		}
	}
}
