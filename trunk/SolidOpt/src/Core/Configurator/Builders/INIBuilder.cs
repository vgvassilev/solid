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
		public void Build(Dictionary<TParamName, object> configRepresenation)
		{
			StreamWriter streamWriter = new StreamWriter(new MemoryStream());
			foreach(KeyValuePair<TParamName, object> item in configRepresenation){
				streamWriter.WriteLine("{0}={1}", item.Key, item.Value );
			}		
			streamWriter.Flush();
			
			Uri path = new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"test.modified.ini"));
			uriManager.SetResource(streamWriter.BaseStream, path);
			
			streamWriter.Close();
		}
	}
}
