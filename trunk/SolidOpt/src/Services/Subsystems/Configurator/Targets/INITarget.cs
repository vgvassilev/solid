/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 17.8.2008 ?.
 * Time: 13:40
 * 
 */

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

using SolidOpt.Core.Providers.StreamProvider;
using SolidOpt.Core.Configurator.Mappers;

namespace SolidOpt.Core.Configurator.Targets
{
	/// <summary>
	/// This class builds ini file format from the intermediate representation
	/// </summary>
	public class INITarget<TParamName> : IConfigTarget<TParamName>
	{
		private StreamWriter streamWriter = null;
		private CustomMapperGroup<TParamName> mappers = new CustomMapperGroup<TParamName>();
		
		public INITarget()
		{
			mappers.AddMapper(new IdentityMapper<TParamName>()).AddMapper(null);
		}
		
		public bool CanBuild(string fileFormat)
		{
			return fileFormat == "ini";
		}
		
		/// <summary>
		/// Builds configuration format and saves to given URI using Stream Provider Manager.
		/// </summary>
		/// <param name="configRepresenation">Intermediate Representation of the config params</param>
		public Stream Build(Dictionary<TParamName, object> configRepresenation)
		{
			streamWriter = new StreamWriter(new MemoryStream());
			
			BuildInDepth(BuildFirstLevel(configRepresenation));
			
			return streamWriter.BaseStream;
			
//			uriManager.SetResource(streamWriter.BaseStream, resourse);
//			
//			streamWriter.Close();
			
		}
		
		internal Dictionary<TParamName, object> BuildFirstLevel(Dictionary<TParamName, object> configRepresenation)
		{
			TParamName key;
			Dictionary<TParamName, object> result = new Dictionary<TParamName, object>();
			foreach(KeyValuePair<TParamName, object> item in configRepresenation){
				key = (TParamName)Convert.ChangeType(item.Key, typeof (TParamName));
				if (item.Value is Dictionary<TParamName, object>){
					result[key] = item.Value;
				}
				else {
					streamWriter.WriteLine("{0} = {1}", key, item.Value);
				}
			}			
			
			streamWriter.Flush();
			
			return result;
		}
		
		internal void BuildInDepth(Dictionary<TParamName, object> configRepresenation)
		{
			TParamName key;
			
			foreach(KeyValuePair<TParamName, object> item in configRepresenation){
				key = (TParamName)Convert.ChangeType(item.Key, typeof (TParamName));
				if (item.Value is Dictionary<TParamName, object>){
					streamWriter.WriteLine("[{0}]", key);
					BuildInDepth(item.Value as Dictionary<TParamName, object>);
				}
				else {
					streamWriter.WriteLine("{0} = {1}", key, item.Value);
				}
			}
			
			streamWriter.Flush();
		}
	}
}
