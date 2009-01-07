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
using SolidOpt.Core.Providers.StreamProvider;

namespace SolidOpt.Core.Configurator
{
	/// <summary>
	/// Description of CofigurationManager.
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
		
		private List<IConfigBuilder<TParamName>> savers = new List<IConfigBuilder<TParamName>>();		
		public List<IConfigBuilder<TParamName>> Savers {
			get { return savers; }
			set { savers = value; }
		}
		
		private List<IConfigParser<TParamName>> loaders = new List<IConfigParser<TParamName>>();
		public List<IConfigParser<TParamName>> Loaders {
			get { return loaders; }
			set { loaders = value; }
		}
		
		
		private Dictionary<TParamName, object> ir = new Dictionary<TParamName, object>();
		public Dictionary<TParamName, object> IR {
			get { return ir; }
			set { ir = value; }
		}
		
		private CacheManager<TParamName, object> cacheManager;
		private URIManager streamProvider = new URIManager();
		

		public ConfigurationManager()
		{		
		}
		
		public ConfigurationManager(CacheManager<TParamName, object> cacheManager) : this()
		{
			this.cacheManager = cacheManager;
		}
		
		//TODO:Да се прегледат методите, които определят дали може да бъде обработен обекта
		public void SaveConfiguration(Dictionary<TParamName, object> configRepresenation, Uri resourse, string fileFormat)
		{
			foreach (IConfigBuilder<TParamName> s in Savers){
				if (s.CanBuild(fileFormat)){
					s.Build(configRepresenation, resourse);
				}
			}
		}
		
		//TODO:Да се прегледат методите, които определят дали може да бъде обработен обекта
		public Dictionary<TParamName, object> LoadConfiguration(Uri resUri)
		{
			Stream resStream = streamProvider.GetResource(resUri);
			foreach (IConfigParser<TParamName> l in Loaders){
				if (l.CanParse(resUri, resStream)){
					return l.LoadConfiguration(resStream);
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
