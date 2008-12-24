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
using System.IO;

using SolidOpt.Core.Configurator.Builders;
using SolidOpt.Core.Configurator.Parsers;
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
		
		private List<IConfigBuilder<TParamName>> savers = new List<IConfigBuilder<TParamName>>();
		private List<IConfigParser<TParamName>> loaders = new List<IConfigParser<TParamName>>();
		private Dictionary<TParamName, object> ir = new Dictionary<TParamName, object>();
		public Dictionary<TParamName, object> IR {
			get { return ir; }
			set { ir = value; }
		}
		
		private CacheManager<TParamName, object> cacheManager;
		

		public ConfigurationManager()
		{
//			savers.Add(new INIBuilder<TParamName>());
//			loaders.Add(new INIParser<TParamName>());
//			savers.Add(new NMSPBuilder<TParamName>());
			loaders.Add(new NMSPParser<TParamName>());
			savers.Add(new Converters.IR2Assembly<TParamName>());
			loaders.Add(new Converters.IR2Assembly<TParamName>());			
		}
		
		public ConfigurationManager(CacheManager<TParamName, object> cacheManager) : this()
		{
			this.cacheManager = cacheManager;
		}
		
		//TODO:Да се прегледат методите, които определят дали може да бъде обработен обекта
		public bool SaveConfiguration(Dictionary<TParamName, object> configRepresenation)
		{
			foreach (IConfigBuilder<TParamName> s in savers){
				if (s.CanBuild()){
					s.Build(configRepresenation);
					return true;
				}
			}
			return false;
		}
		
		//TODO:Да се прегледат методите, които определят дали може да бъде обработен обекта
		public Dictionary<TParamName, object> LoadConfiguration(Uri resource)
		{
			foreach (IConfigParser<TParamName> l in loaders){
				if (l.CanParse(resource)){
					return l.LoadConfiguration(resource);
				}
			}
			return new Dictionary<TParamName, object>();
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
