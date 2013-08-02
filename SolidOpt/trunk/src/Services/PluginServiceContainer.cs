/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Security.Policy;

using System.Threading;

namespace SolidOpt.Services
{
  /// <summary>
  /// A service container for plugins.
  /// </summary>
  /// <description>
  /// It contains plugins, which might provide services. Once the plugin is loaded,
  /// its services (if any) get registered to the service container. 
  /// </description>
  /// 
  public class PluginServiceContainer : ServiceContainer
  {
    private List<PluginInfo> plugins = new List<PluginInfo>();
    public List<PluginInfo> Plugins {
      get { return plugins; }
    }
    
    public PluginServiceContainer(): base() {}
    public PluginServiceContainer(IServiceProvider parent): base(parent) {}

    /// <summary>
    /// Adds the plugins from a file.
    /// </summary>
    /// <returns><c>false</c>, if the specified file doesn't exist.</returns>
    /// <param name="file">File (must exist).</param>
    /// 
    public bool AddPluginsFromFile(string file)
    {
      if (!File.Exists(file))
        return false;

      foreach (string s in File.ReadAllLines(file))
        if (File.Exists(s))
          AddPlugin(s);

      return true;
    }

    /// <summary>
    /// Saves the plugins to file, overwrites the file if it exists.
    /// </summary>
    /// <param name="file">File (created if it doesn't exist).</param>
    /// 
    public void SavePluginsToFile(string file)
    {
      List<string> paths = new List<string>();
      foreach (PluginInfo p in plugins)
        paths.Add(p.codeBase);

      File.WriteAllLines(file, paths.ToArray());
    }

    /// <summary>
    /// Adds all plugins found in the given path.
    /// </summary>
    /// <description>
    /// Search the path with mask * and try to load each file found.
    /// </description>
    /// <param name="path">Path.</param>
    ///
    public void AddPlugins(string path)
    {
      AddPlugins(new DirectoryInfo(path));
    }

    /// <summary>
    /// Adds all plugins found in the given path, with a given mask.
    /// </summary>
    /// <param name="path">Path.</param>
    /// <param name="mask">Mask.</param>
    ///
    public void AddPlugins(string path, string mask)
    {
      AddPlugins(new DirectoryInfo(path), mask);
    }

    /// <summary>
    /// Adds all plugins found in the given path.
    /// </summary>
    /// <description>
    /// Search the path with mask * and try to load each file found.
    /// </description>
    /// <param name="dirInfo">Dir info.</param>
    /// 
    public void AddPlugins(DirectoryInfo dirInfo)
    {
      AddPlugins(dirInfo, "*");
    }

    /// <summary>
    /// Adds all plugins found in the given path.
    /// </summary>
    /// <description>
    /// Search the path with a given mask and try to load each file found.
    /// </description>
    /// <param name="dirInfo">Dir info.</param>
    /// <param name="mask">Mask.</param>
    /// 
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

    /// <summary>
    /// Adds a given plugin.
    /// </summary>
    /// <param name="fullName">Full name.</param>
    ///
    public void AddPlugin(string fullName)
    {
      plugins.Add(new PluginInfo(fullName));
    }

    public void RemovePlugin(string fullName)
    {
      plugins.Remove(plugins.Find(x => x.codeBase == fullName));
    }

    /// <summary>
    /// Loads all plugins.
    /// </summary>
    /// <description>
    /// Iterates over all added plugins and tries to call the plugin's default
    /// constructor. If the plugin implements the IService interface the instance
    /// gets registered to the services, provided by the PluginServiceContainer.
    /// </description>
    /// 
    public void LoadPlugins()
    {
      foreach (PluginInfo pluginInfo in plugins)
        pluginInfo.CreateAndRegisterAnInstance(this);

      foreach (PluginInfo pluginInfo in plugins)
        if (pluginInfo.status == PluginInfo.Status.Error)
          pluginInfo.assembly = null;
    }

    public List<IService> GetServices(PluginInfo pInfo) {
      List<IService> result = new List<IService>(2);
      foreach (IService srv in GetServices())
        if (srv.GetType().Assembly.Location == pInfo.codeBase)
          result.Add(srv);
      return result;
    }
  }

  /// <summary>
  /// Plugin info. This class wraps a plugin. It is responsible for loading, unloading and creating
  /// a plugin.
  /// </summary>
  /// <description>
  /// Once a PluginInfo object for a plugin is created it describes its state. In the terms of
  /// the PluginInfo class a plugin can be in the following states:
  /// <item>Unloaded</item> - An initial state of every plugin.
  /// <item>Loaded</item> - Plugin's codebase is loaded in the current application domain.
  /// <item>Created</item> - The class is created/instantiated using its default no arg ctor.
  /// <item>Error</item> - An error has occured while trying to load the assembly.
  /// </description>
  public class PluginInfo
  {
    public enum Status {UnLoaded, Loaded, Created, Error};

    public string codeBase;
    public Status status;
    public Assembly assembly;
    private AppDomain appDomain;

    public PluginInfo(string fileName)
    {
      this.codeBase = Path.GetFullPath(fileName);
      this.status = Status.UnLoaded;
      this.appDomain = AppDomain.CurrentDomain;
    }

    internal void LoadPluginInCurrentAppDomain() {
      if (status == Status.UnLoaded) {
        try {
//          foreach (Assembly a in appDomain.GetAssemblies()) {
//            if (a.ManifestModule.Name == Path.GetFileName(fullName)) {
//              status = Status.Error;
//              return;
//            }
//          }
          
//          AppDomainSetup domaininfo = new AppDomainSetup();
//          domaininfo.ApplicationBase = Path.GetDirectoryName(fullName) + "\\";
////          domaininfo.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
//          //domaininfo.PrivateBinPath += Path.GetDirectoryName(fullName);
//          domaininfo.LoaderOptimization = LoaderOptimization.MultiDomain;
//          Evidence adevidence = AppDomain.CurrentDomain.Evidence;
//          appDomain = AppDomain.CreateDomain("Plugin-"+domainId, adevidence, domaininfo);
//          domainId++;

//          AppDomain.CurrentDomain.AppendPrivatePath(AppDomain.CurrentDomain.BaseDirectory);
//          AppDomain.CurrentDomain.AppendPrivatePath(Path.GetDirectoryName(fullName));
          
          AssemblyName an = null;
          try { an = AssemblyName.GetAssemblyName(codeBase); } catch {}
          if (an == null) {
            an = new AssemblyName();
            an.CodeBase = codeBase;
          }
          assembly = appDomain.Load(an);
          
//          AssemblyName an = new AssemblyName();
//          an.CodeBase = fullName;
//          assembly = appDomain.Load(an);
          //assembly = Assembly.LoadFrom(fullName);
          //if (assembly == null) assembly = Assembly.LoadWithPartialName(fullName);
          status = Status.Loaded;
        } catch { assembly = null; }
        if (assembly == null) status = Status.Error;
      }
    }

    /// <summary>
    /// Creates and register an instance of the plugin to the plugin container.
    /// </summary>
    /// <param name="serviceContainer">Service container.</param>
    /// 
    internal void CreateAndRegisterAnInstance(IServiceContainer serviceContainer)
    {
      LoadPluginInCurrentAppDomain();
      if (status != Status.Loaded) return;

      IService service;
      foreach (Type type in assembly.GetTypes())
        if (type.IsClass && !type.IsAbstract && typeof(IService).IsAssignableFrom(type))
          try {
            service = (IService)(assembly.CreateInstance(type.FullName));
            if (service != null)
              serviceContainer.AddService(service);
          } catch (Exception e) {
            Console.WriteLine("Error:{0}", e.ToString());
          }
      status = Status.Created;
    }
  }
}
