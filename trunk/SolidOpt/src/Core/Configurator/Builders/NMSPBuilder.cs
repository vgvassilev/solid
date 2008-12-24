/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 21.8.2008 ?.
 * Time: 13:25
 * 
 */

using System;
using System.IO;
using System.Collections.Generic;

using SolidOpt.Core.Providers.StreamProvider;

namespace SolidOpt.Core.Configurator.Builders
{
	/// <summary>
	/// Description of NMSPBuilder.
	/// </summary>
	public class NMSPBuilder<TParamName> : IConfigBuilder<TParamName>
	{
		private string namespaceOpenSep = "{";
		private string namespaceCloseSep = "}";
		private string keyValueSep = "=";
		private string referenceSep = "#";
		private string commentSep = ";";
		
		private URIManager uriManager = new URIManager();
		
		public NMSPBuilder()
		{
		}
		
		public NMSPBuilder(string namespaceOpenSep,string namespaceCloseSep, string keyValueSep,
		                   string referenceSep, string commentSep)
		{
			this.namespaceOpenSep = namespaceOpenSep;
			this.namespaceCloseSep = namespaceCloseSep;
			this.keyValueSep = keyValueSep;
			this.referenceSep = referenceSep;
			this.commentSep = commentSep;
		}
		
		public bool CanBuild()
		{
			return true;
		}
		
		/// <summary>
		/// Builds configuration format and saves to given URI using Stream Provider Manager.
		/// </summary>
		/// <param name="configRepresenation">Intermediate Representation of the config params</param>
		public void Build(Dictionary<TParamName, object> configRepresenation)
		{
			StreamWriter streamWriter = new StreamWriter(new MemoryStream());
			
			string key;
			
			foreach(KeyValuePair<TParamName, object> item in configRepresenation){
				key = (string)Convert.ChangeType(item.Key,typeof (TParamName));
				if(!key.Contains(".")){
					
				}
				
				
				Console.WriteLine(key);
				
				
				
				
				
				
//				streamWriter.WriteLine("{0}={1}", item.Key, item.Value);
			}		
			
			
			
			
			
			
			streamWriter.Flush();
			
			Uri path = new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"test.modified.ini"));
			uriManager.SetResource(streamWriter.BaseStream, path);
			
			streamWriter.Close();
		}
	}
}
