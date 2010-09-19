/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;

namespace SolidOpt.Services.Subsystems.Configurator.TypeResolvers
{
	/// <summary>
	/// Description of TypeManager.
	/// </summary>
	public class TypeManager<TParamName>
	{

		private ITypeResolver resolver;
		public ITypeResolver Resolver {
			get { return resolver; }
			set { resolver = value; }
		}
		
		public TypeManager()
		{	
		}
		
		public Dictionary<TParamName, object> ResolveTypes(Dictionary<TParamName, object> CIR)
		{
			Dictionary<TParamName, object> dict = new Dictionary<TParamName, object>();

			foreach(KeyValuePair<TParamName, object> item in CIR){
				if (item.Value is Dictionary<TParamName, object>) {
					dict[item.Key] = Build(item.Value as Dictionary<TParamName, object>);
				}
				else {
					dict[item.Key] = Resolver.TryResolve(item.Value);
				}
			}
			return dict;
		}
		
		private Dictionary<TParamName, object> Build(Dictionary<TParamName, object> ResolvedCIR)
		{
			Dictionary<TParamName, object> Result = new Dictionary<TParamName, object>();
			foreach(KeyValuePair<TParamName, object> item in ResolvedCIR){
				if (item.Value is Dictionary<TParamName, object>) {
					Result[item.Key] = Build(item.Value as Dictionary<TParamName, object>);;
				}
				else {
					Result[item.Key] = Resolver.TryResolve(item.Value);
				}
			}
			return Result;
		}
	}
}
