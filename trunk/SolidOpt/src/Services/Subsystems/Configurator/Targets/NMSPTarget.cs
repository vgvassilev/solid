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

using SolidOpt.Services.Subsystems.HetAccess;

namespace SolidOpt.Services.Subsystems.Configurator.Targets
{
	/// <summary>
	/// Description of NMSPBuilder.
	/// </summary>
	public class NMSPTarget<TParamName> : IConfigTarget<TParamName>
	{
		private StreamWriter streamWriter = null;
		
		public NMSPTarget()
		{
		}
		
		public bool CanBuild(string fileFormat)
		{
			return fileFormat == "nmsp";
		}
		
		/// <summary>
		/// Builds configuration format and saves to given URI using Stream Provider Manager.
		/// </summary>
		/// <param name="configRepresenation">Intermediate Representation of the config params</param>
		public Stream Build(Dictionary<TParamName, object> configRepresenation)
		{
			streamWriter = new StreamWriter(new MemoryStream());
			
			Build(configRepresenation, "");
			
			streamWriter.Flush();
			
			return streamWriter.BaseStream;
		}
		
		internal void Build(Dictionary<TParamName, object> configRepresenation, string tab)
		{
			TParamName key;
			
			foreach(KeyValuePair<TParamName, object> item in configRepresenation){
				key = (TParamName)Convert.ChangeType(item.Key, typeof (TParamName));
				if (item.Value is Dictionary<TParamName, object>){
					streamWriter.WriteLine("{0}{1} {2}", tab, key,"{");
					Build(item.Value as Dictionary<TParamName, object>, tab + "  ");
					streamWriter.WriteLine("{0}{1}", tab, "}");
				}
				else {
					streamWriter.WriteLine("{0}{1} = {2}",tab, key, item.Value);
				}
			}
		}
		
	}
}
