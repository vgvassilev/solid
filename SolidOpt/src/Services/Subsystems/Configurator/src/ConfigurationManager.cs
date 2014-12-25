/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Configuration;
using System.Collections.Generic;
using System.IO;

using SolidOpt.Services.Subsystems.Cache;
using SolidOpt.Services.Subsystems.Configurator.Targets;
using SolidOpt.Services.Subsystems.Configurator.Sources;
using SolidOpt.Services.Subsystems.HetAccess;
using SolidOpt.Services.Subsystems.Configurator.Mappers;
using SolidOpt.Services.Subsystems.Configurator.TypeResolvers;

namespace SolidOpt.Services.Subsystems.Configurator
{
  /// <summary>
  /// The ConfigurationManager is responsible for the manipulation with the configuration
  /// resources in the developed system.
  /// </summary>
  public sealed class ConfigurationManager<TParamName>
  {
    #region Singleton
    /// <summary>
    /// Makes class singleton
    /// </summary>
    private static ConfigurationManager<TParamName> instance = 
                    new ConfigurationManager<TParamName>();
    
    public static ConfigurationManager<TParamName> Instance {
      get {return instance;}
    }
    #endregion
    
    private List<IConfigTarget<TParamName>> targets = new List<IConfigTarget<TParamName>>();    
    public List<IConfigTarget<TParamName>> Targets {
      get { return targets; }
      set { targets = value; }
    }
    
    private List<IConfigSource<TParamName>> sources = new List<IConfigSource<TParamName>>();
    public List<IConfigSource<TParamName>> Sources {
      get { return sources; }
      set { sources = value; }
    }
    
    
    private Dictionary<TParamName, object> ir = new Dictionary<TParamName, object>();
    public Dictionary<TParamName, object> IR {
      get { return ir; }
      set { ir = value; }
    }
    
    #region Managers
    private CacheManager<TParamName, object> cacheManager;
    public CacheManager<TParamName, object> CacheManager {
      get { return cacheManager; }
      set { cacheManager = value; }
    }
    
    private URIManager streamProvider;
    public URIManager StreamProvider {
      get { return streamProvider; }
      set { streamProvider = value; }
    }
    
    private MapManager<TParamName> mapManager;
    public MapManager<TParamName> MapManager {
      get { return mapManager; }
      set { mapManager = value; }
    }
    
    private TypeManager<TParamName> typeManager;
    public TypeManager<TParamName> TypeManager {
      get { return typeManager; }
      set { typeManager = value; }
    }
    #endregion

    public ConfigurationManager()
    {    
    }
    
    public void SaveConfiguration(Uri resourse, string fileFormat)
    {
//      Dictionary<TParamName, object> repres;
      if (MapManager != null) {
        IR = MapManager.Map(IR);
      }
//      else
//        repres = IR;
      
      if (TypeManager != null)
        IR = TypeManager.ResolveTypes(IR);
      
      SaveConfiguration(IR, resourse, fileFormat);
    }
    
    //TODO:Да се прегледат методите, които определят дали може да бъде обработен обекта
    public void SaveConfiguration(Dictionary<TParamName, object> configRepresenation, Uri resourse, string fileFormat)
    {
      if (StreamProvider == null) throw new AccessViolationException("Stream Provider not set!");
      
      foreach (IConfigTarget<TParamName> s in Targets){
        if (s.CanBuild(fileFormat)){
          StreamProvider.SetResource(s.Build(configRepresenation), resourse);
        }
      }
    }
    
    //TODO:Да се прегледат методите, които определят дали може да бъде обработен обекта
    public Dictionary<TParamName, object> LoadConfiguration(Uri resUri)
    {
      if (StreamProvider == null) throw new AccessViolationException("Stream Provider not set!");
      
      Stream resStream = StreamProvider.GetResource(resUri);
      foreach (IConfigSource<TParamName> l in Sources){
        if (l.CanParse(resUri, resStream)){
//          if (MapManager != null)
//            IR = MapManager.UnMap(l.LoadConfiguration(resStream));
//          else
            IR = l.LoadConfiguration(resStream);
        }
      }
      
      return IR;
    }
    
    public TParam GetParam<TParam>(TParamName name)
    {
      object result;
      if (IR.TryGetValue(name, out result)){
        return (TParam)result;
      }
      else{
        return default(TParam);
      }
    }
    
    public object GetParam(TParamName name)
    {
      return GetParam<object>(name);
    }
    
    public Dictionary<TParamName, TBaseParam> GetParams<TBaseParam>(params TParamName[] names)
    {
      Dictionary<TParamName, TBaseParam> result = new Dictionary<TParamName, TBaseParam>();
      foreach (TParamName name in names){
        result.Add(name, GetParam<TBaseParam>(name));
      }
      return result;
    }
    
    public Dictionary<TParamName, object> GetParams(params TParamName[] names)
    {
      return GetParams<object>(names);
    }
  }
}
