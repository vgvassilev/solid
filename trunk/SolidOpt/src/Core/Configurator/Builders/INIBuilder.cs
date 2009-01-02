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

namespace SolidOpt.Core.Configurator.Builders
{
	/// <summary>
	/// This class builds ini file format from the intermediate representation
	/// </summary>
	public class INIBuilder<TParamName> : IConfigBuilder<TParamName>
	{
		private URIManager uriManager = new URIManager();
		private StreamWriter streamWriter = null;
		
		public INIBuilder()
		{
		}
		
		public bool CanBuild(string fileFormat)
		{
			return fileFormat == "ini";
		}
		
		/// <summary>
		/// Builds configuration format and saves to given URI using Stream Provider Manager.
		/// </summary>
		/// <param name="configRepresenation">Intermediate Representation of the config params</param>
		public void Build(Dictionary<TParamName, object> configRepresenation, Uri resourse)
		{
			streamWriter = new StreamWriter(new MemoryStream());
			
			BuildInDepth(BuildFirstLevel(configRepresenation));
			
//			//TODO if uriManager.SetResource == false throw exception... Да се помисли за йерархия на изключенията
			uriManager.SetResource(streamWriter.BaseStream, resourse);
			
			streamWriter.Close();
			
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
