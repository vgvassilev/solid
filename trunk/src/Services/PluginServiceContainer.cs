/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Runtime.Remoting;
using System.Security.Policy;

using System.Threading;

namespace SolidOpt.Services
{
	public class PluginServiceContainer : ServiceContainer
	{
		public ArrayList plugins;

		public PluginServiceContainer()
		{
			plugins = new ArrayList();
		}
		
		public void AddPlugins(string path)
		{
			AddPlugins(new DirectoryInfo(path));
		}
		
		public void AddPlugins(string path, string mask)
		{
			AddPlugins(new DirectoryInfo(path), mask);
		}
		
		public void AddPlugins(DirectoryInfo dirInfo)
		{
			AddPlugins(dirInfo, "*");
		}
		
		public void AddPlugins(DirectoryInfo dirInfo, string mask)
		{
			try {
				foreach (FileInfo fileInfo in dirInfo.GetFiles(mask))
					AddPlugin(fileInfo.FullName);
				foreach (DirectoryInfo subDirInfo in dirInfo.GetDirectories("*"))
					AddPlugins(subDirInfo);
			} catch (System.IO.DirectoryNotFoundException e) {
				//TODO: log e
				throw e;
			}
		}
		
		public void AddPlugin(string fullName)
		{
			plugins.Add(new PluginInfo(fullName));
		}
		
		public void LoadPlugins()
		{
			foreach (PluginInfo pluginInfo in plugins)
				pluginInfo.Register(this);

			foreach (PluginInfo pluginInfo in plugins)
				if (pluginInfo.status == PluginInfo.Status.Error)
					pluginInfo.assembly = null;
		}
		
	}
	
	public class PluginInfo
	{
		public enum Status {UnLoaded, Loaded, Created, Error};

		public string fullName;
		public Status status;
		public Assembly assembly;
//		private AppDomain appDomain;

		public PluginInfo(string fileName)
		{
			this.fullName = Path.GetFullPath(fileName);
			this.status = Status.UnLoaded;
			
		}
		
//		private static int domainId = 0;

		public void Load() {
			if (status == Status.UnLoaded) {
				try {
					foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies()) {
						if (a.ManifestModule.Name == Path.GetFileName(fullName)) {
							status = Status.Error;
							return;
						}
					}
					
//					AppDomainSetup domaininfo = new AppDomainSetup();
//					domaininfo.ApplicationBase = Path.GetDirectoryName(fullName) + "\\";
////					domaininfo.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
//					//domaininfo.PrivateBinPath += Path.GetDirectoryName(fullName);
//					domaininfo.LoaderOptimization = LoaderOptimization.MultiDomain;
//					Evidence adevidence = AppDomain.CurrentDomain.Evidence;
//					appDomain = AppDomain.CreateDomain("Plugin-"+domainId, adevidence, domaininfo);
//					domainId++;


//					AppDomain.CurrentDomain.AssemblyResolve += delegate(object sender, ResolveEventArgs args) { Console.WriteLine("RRR: "+args.Name); return null; };
//					Console.WriteLine(">>>" + AppDomain.CurrentDomain.SetupInformation.ApplicationBase);
//					Console.WriteLine(">>>" + AppDomain.CurrentDomain.SetupInformation.DynamicBase);
//					Console.WriteLine(">>>" + AppDomain.CurrentDomain.SetupInformation.PrivateBinPath);
//					AppDomain.CurrentDomain.SetupInformation.PrivateBinPath += "D:\\Univ\\Projects\\SolidOpt\\trunk\\src\\Core\\Loader\\demo\\StandardLoader\\Loader\\bin\\Debug\\plugins";
//					Console.WriteLine(">>>" + AppDomain.CurrentDomain.SetupInformation.PrivateBinPath);
					
//					AppDomainSetup domaininfo = new AppDomainSetup();
//					domaininfo.ApplicationBase = System.Environment.CurrentDirectory;
//					Evidence adevidence = AppDomain.CurrentDomain.Evidence;
//					AppDomain domain = AppDomain.CreateDomain("MyDomain", adevidence, domaininfo);
//					Console.WriteLine("Host domain: " + AppDomain.CurrentDomain.FriendlyName);
//					Console.WriteLine("new child domain: " + domain.FriendlyName);
//					Console.WriteLine();
//					Console.WriteLine("Application base is: " + domain.SetupInformation.ApplicationBase);

//					AppDomain.CurrentDomain.AppendPrivatePath(AppDomain.CurrentDomain.BaseDirectory);
//					AppDomain.CurrentDomain.AppendPrivatePath(Path.GetDirectoryName(fullName));
					
//					AppDomainSetup appDomainSetup = new AppDomainSetup();
//					appDomainSetup.ApplicationName = "Plugins";
//					appDomainSetup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
////					appDomainSetup.PrivateBinPath += Path.GetDirectoryName(fullName);
////					appDomainSetup.PrivateBinPath += Path.PathSeparator + AppDomain.CurrentDomain.BaseDirectory;
//					appDomain = AppDomain.CreateDomain("Plugins", null, appDomainSetup);
//					appDomain.AppendPrivatePath(AppDomain.CurrentDomain.BaseDirectory);

//					assembly = appDomain.Load(fullName);
					assembly = Assembly.LoadFrom(fullName);
					//if (assembly == null) assembly = Assembly.LoadWithPartialName(fullName);
					status = Status.Loaded;
				} catch { assembly = null; }
				if (assembly == null) status = Status.Error;
			}
		}
		
		public void Register(IServiceContainer serviceContainer)
		{
			Load();
			if (status != Status.Loaded) return;

			IService service;
			foreach (Type type in assembly.GetTypes())
				if (type.IsClass && !type.IsAbstract && typeof(IService).IsAssignableFrom(type))
					try {
						service = (IService)(assembly.CreateInstance(type.FullName));
						////////service = (IService)(AppDomain.CurrentDomain.CreateInstanceAndUnwrap(assembly.FullName, type.FullName));
						////service = (IService)(appDomain.CreateInstanceAndUnwrap(assembly.FullName, type.FullName));
						if (service != null)
							serviceContainer.AddService(service);
					} catch (Exception e) {
						Console.WriteLine("Error:{0}", e.ToString());
					}
			status = Status.Created;
		}
	}
}
