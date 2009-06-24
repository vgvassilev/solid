// OpenF
// Authors: 2004 A.Penev
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//

// 01.01.2003 Initial version. A.Penev (alexander_penev@yahoo.com)

using System;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Runtime.Remoting;

using System.Threading;

namespace SolidOpt.Core.Services
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
				// log e
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

		public PluginInfo(string fileName)
		{
			this.fullName = Path.GetFullPath(fileName);
			this.status = Status.UnLoaded;
			
		}

		public void Load() {
			if (status == Status.UnLoaded) {
				try {
					//MessageBox.Show(fullName,"Load...");
					AppDomain.CurrentDomain.AppendPrivatePath(Path.GetDirectoryName(fullName));
					
//					Thread.GetDomain().SetupInformation.PrivateBinPath = Path.GetDirectoryName(fullName);
//					AppDomain.CurrentDomain.AppendPrivatePath(AppDomain.CurrentDomain.BaseDirectory);
//					AppDomainSetup appDomainSetup = new AppDomainSetup();
//					appDomainSetup.ApplicationName = "Plugins";
//					appDomainSetup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
//					appDomainSetup.PrivateBinPath += Path.GetDirectoryName(fullName);
//					appDomainSetup.PrivateBinPath += Path.PathSeparator + AppDomain.CurrentDomain.BaseDirectory;
					
//					appDomain = AppDomain.CreateDomain("Plugins", null, appDomainSetup);
					
					//AppDomain.CurrentDomain.AppendPrivatePath(Path.GetDirectoryName(fullName));
					assembly = Assembly.LoadFrom(fullName);
					if (assembly == null) assembly = Assembly.Load(fullName);
//					if (assembly == null) assembly = Assembly.LoadWithPartialName(fullName);
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
				if (type.IsClass && !type.IsAbstract && type.GetInterface(typeof(IService).FullName) != null)
					try {
						//MessageBox.Show(type.ToString(),"Provider...");
						service = (IService)(AppDomain.CurrentDomain.CreateInstanceAndUnwrap(assembly.FullName, type.FullName));
						if (service != null)
							serviceContainer.AddService(service);
					} catch (Exception e) {Console.WriteLine("Error:{0}", e.ToString());}
			status = Status.Created;
		}
		
	}
	
}
