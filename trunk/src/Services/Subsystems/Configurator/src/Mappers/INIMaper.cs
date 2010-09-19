/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;

namespace SolidOpt.Services.Subsystems.Configurator.Mappers
{
	/// <summary>
	/// Description of INIMaper.
	/// </summary>
	public class INIMaper<TParamName> : Mapper<TParamName>
	{
		internal Dictionary<TParamName, object> tCIR = new Dictionary<TParamName, object>();
		
		public INIMaper()
		{
		}
		
		public override Dictionary<TParamName, object> Map(Dictionary<TParamName, object> mmCIR)
		{
			return MapCIR(mmCIR, "");
		}
		
		internal Dictionary<TParamName, object> MapCIR(Dictionary<TParamName, object> mmCIR, string key)
		{
			foreach(KeyValuePair<TParamName, object> item in mmCIR){
				if (item.Value is Dictionary<TParamName, object>) {
					if (key != "")
						key += ".";
					
					key += item.Key;
					MapCIR(item.Value as Dictionary<TParamName, object>, key);
				}
				else {
					tCIR[(TParamName)Convert.ChangeType(
						key + ((key == "") ? "" : ".") + item.Key, typeof (TParamName))] = item.Value;
				}
			}
			return tCIR;
		}
		
		public override Dictionary<TParamName, object> UnMap(Dictionary<TParamName, object> mmCIR)
		{
			foreach(KeyValuePair<TParamName, object> item in mmCIR){
				Dictionary<TParamName, object> tempDict;
				UnMapParam(out tempDict, item);
//				tCIR[keyCol[0]] = tempDict;

			}
			return tCIR;
		}
		
		internal void UnMapParam(out Dictionary<TParamName, object> dict, KeyValuePair<TParamName, object> item)
		{
			TParamName part1 = default(TParamName);
			TParamName part2 = default(TParamName);
			dict = new Dictionary<TParamName, object>();
			string [] strarr;
			if (item.Key.ToString().Contains(".")) {
				strarr = item.Key.ToString().Split('.');
				part1 = (TParamName)Convert.ChangeType(
					strarr[0], typeof (TParamName));
				part2 = (TParamName)Convert.ChangeType(
					item.Key.ToString().Substring(item.Key.ToString().IndexOf('.') + 1), typeof (TParamName));
				Dictionary<TParamName, object> subDict;
				UnMapParam(out subDict, new KeyValuePair<TParamName, object>(part2, item.Value));
				dict[part1] = subDict;
			}
			else {
				dict[item.Key] = item.Value;
			}
			if (part1 != null)
				tCIR[part1] = dict;
		}
	}
}
