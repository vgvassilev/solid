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
	/// Maps and unmaps the Configuration Intermediate Representation to and from INI
	/// </summary>
	public class INIMaper<TParamName> : Mapper<TParamName>
	{
		internal Dictionary<TParamName, object> tCIR = new Dictionary<TParamName, object>();
		
		public INIMaper()
		{
		}
		
		/// <summary>
		/// Maps the Configuration Intermediate Representation, which supports nesting. On the other
		/// hand INI doesn't. It maps the nested namespaces with dots(.)
		/// For example:
		/// </summary>
		/// <example>
		/// X {
		///   a = 10
		/// 	Y {
		///       b = 'one'
		/// 	}
		/// }
		/// is mapped to:
		/// X.a = 10
		/// X.Y.b = 'one'
		/// </example>
		/// <param name="mmCIR">
		/// A <see cref="Dictionary<TParamName, System.Object>"/>
		/// </param>
		/// <returns>
		/// A <see cref="Dictionary<TParamName, System.Object>"/>
		/// </returns>
		public override Dictionary<TParamName, object> Map(Dictionary<TParamName, object> mmCIR)
		{
			return MapCIR(mmCIR, "");
		}

		/// <summary>
		/// Recursive function doing the actual mapping. 
		/// </summary>
		/// <param name="mmCIR">
		/// A <see cref="Dictionary<TParamName, System.Object>"/>
		/// </param>
		/// <param name="key">
		/// A <see cref="System.String"/>
		/// </param>
		/// <returns>
		/// A <see cref="Dictionary<TParamName, System.Object>"/>
		/// </returns>
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
		
		/// <summary>
		/// Tries to unmap INI to nested Configuration Intermediate Representation. It is applicable
		/// only when the INI was previosly mapped. Or it is organized so.
		/// </summary>
		/// <param name="mmCIR">
		/// A <see cref="Dictionary<TParamName, System.Object>"/>
		/// </param>
		/// <returns>
		/// A <see cref="Dictionary<TParamName, System.Object>"/>
		/// </returns>
		public override Dictionary<TParamName, object> UnMap(Dictionary<TParamName, object> mmCIR)
		{
			foreach(KeyValuePair<TParamName, object> item in mmCIR){
				Dictionary<TParamName, object> tempDict;
				UnMapParam(out tempDict, item);
//				tCIR[keyCol[0]] = tempDict;

			}
			return tCIR;
		}
		
		/// <summary>
		/// Recursive function doing the actual unmapping. 
		/// </summary>
		/// <param name="dict">
		/// A <see cref="Dictionary<TParamName, System.Object>"/>
		/// </param>
		/// <param name="item">
		/// A <see cref="KeyValuePair<TParamName, System.Object>"/>
		/// </param>
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
