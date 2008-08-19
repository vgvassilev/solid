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

namespace SolidOpt.Core.Configurator.Loaders
{
	/// <summary>
	/// Description of BinaryLoader.
	/// </summary>
	public class INIParser<TParamName> : IConfigLoader<TParamName>
	{
		private string filePath;
		
		public INIParser()
		{
		}
		
		public INIParser(string filePath)
		{
			this.filePath = filePath;	
		}
		
		public bool CanLoad()
		{
			return !String.IsNullOrEmpty(filePath);
		}
		
		public Dictionary<TParamName, object> LoadConfiguration()
		{
			Dictionary<TParamName, object> result = new Dictionary<TParamName, object>();
			FileStream fs = new FileStream(filePath, FileMode.Open);
			StreamReader reader = new StreamReader(fs);
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
