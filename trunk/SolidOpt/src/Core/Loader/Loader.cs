using System;
using System.IO;

using SolidOpt.Core.Services;

namespace SolidOpt.Core.Loader
{
	public class Loader
	{
		private PluginServiceContainer servicesContainer = new PluginServiceContainer();
		public PluginServiceContainer ServicesContainer {
			get { return servicesContainer; }
			set { servicesContainer = value; }
		}
		
		public Loader()
		{
		}

		public int Run(string[] args)
		{
			if (args.Length < 1) return -1;
			
			LoadServices(args);
			Transform(args);
			
			//TODO: Cache invokation
			
			return 0;
		}
		
		public virtual void LoadServices(string[] args)
		{
			ServicesContainer.AddPlugins(AppDomain.CurrentDomain.BaseDirectory + "core");
			ServicesContainer.AddPlugins(AppDomain.CurrentDomain.BaseDirectory + "plugins");
			
			ServicesContainer.LoadPlugins();
		}
		
		public virtual void Transform(string[] args)
		{
			
		}
	}
}
