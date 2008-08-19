/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 17.8.2008 г.
 * Time: 13:17
 * 
 */

using System;
using System.Configuration;
using System.Collections.Generic;

using SolidOpt.Core.Configurator.Loaders;
using SolidOpt.Core.Configurator.Savers;
using SolidOpt.Cache;


namespace SolidOpt.Core.Configurator
{
	/// <summary>
	/// Description of CofigurationManager.
	/// </summary>
	public sealed class ConfigurationManager<TParamName>
	{
		
		private static ConfigurationManager<TParamName> instance = 
										new ConfigurationManager<TParamName>();
		
		public static ConfigurationManager<TParamName> Instance {
			get {return instance;}
		}
		
		private List<IConfigSaver> savers = new List<IConfigSaver>();
		private List<IConfigLoader<TParamName>> loaders = new List<IConfigLoader<TParamName>>();
		private Dictionary<TParamName, object> ir = new Dictionary<TParamName, object>();
		
		private CacheManager<TParamName, object> cacheManager;
		

		public ConfigurationManager()
		{
			savers.Add(new INIBuilder());
			loaders.Add(new INIParser<TParamName>(@"test.ini"));
		}
		
		public ConfigurationManager(CacheManager<TParamName, object> cacheManager) : this()
		{
			this.cacheManager = cacheManager;
		}
		
		public bool SaveConfiguration()
		{
			foreach (IConfigSaver s in savers){
				if (s.CanSave()){
					s.Save();
					return true;
				}
			}
			return false;
		}
		
		public Dictionary<TParamName, object> LoadConfiguration()
		{
			foreach (IConfigLoader<TParamName> l in loaders){
				if (l.CanLoad()){
					return l.LoadConfiguration();
				}
			}
			return new Dictionary<TParamName, object>();
		}
		
		public TParam GetParam<TParam>(TParamName name)
		{
			object result;
			if (ir.TryGetValue(name, out result)){
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
