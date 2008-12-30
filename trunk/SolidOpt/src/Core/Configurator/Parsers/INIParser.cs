/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 17.8.2008 ?.
 * Time: 13:40
 * 
 */

using System;
using System.IO;
using System.Collections.Generic;

using SolidOpt.Core.Providers.StreamProvider;

namespace SolidOpt.Core.Configurator.Parsers
{
	/// <summary>
	/// Creates IR from stream, i.e loads the configuration into the configuration manager.
	/// </summary>
	public class INIParser<TParamName> : IConfigParser<TParamName>
	{
		private URIManager uriManager = new URIManager();
		
		public INIParser()
		{
		}
		
		/// <summary>
		/// Checks if the URI can be handled.
		/// </summary>
		/// <returns>Can be handled</returns>
		public bool CanParse(Uri resource)
		{
			if (resource.IsFile && Path.GetExtension(resource.LocalPath).ToLower() == ".ini"){
				return true;
			}
			return false;	
		}
		
		/// <summary>
		/// Iterates over the stream delivered by the Stream Provider Manager and creates the IR.
		/// </summary>
		/// <returns>IR</returns>
		public Dictionary<TParamName, object> LoadConfiguration(Uri resource)
		{
			Dictionary<TParamName, object> result = new Dictionary<TParamName, object>();
			
			Stream stream = uriManager.GetResource(resource);
			StreamReader reader = new StreamReader(stream);
		
			string line;
			string key,val;
			int pos = 0;
			while(!reader.EndOfStream){
				line = reader.ReadLine();
				pos = line.IndexOf("=");
				if (pos != -1){
					key = line.Substring(0,pos);
					val = line.Substring(pos+1,line.Length-pos-1);
					result.Add((TParamName)Convert.ChangeType(key, typeof(TParamName)),val);
				}
			}
			return result;
		}
	}
}
