/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 30.12.2008 г.
 * Time: 19:15
 * 
 */

using System;
using System.Collections.Generic;

namespace SolidOpt.Core.Configurator.Mappers
{
	/// <summary>
	/// Базовия клас на шаблона композиция, който третира листата и върховете на структурата по
	/// един и същ начин.
	/// </summary>
	public abstract class Mapper<TParamName> : IConfigMapper<TParamName>
	{
		
		public abstract Dictionary<TParamName, object> Map(Dictionary<TParamName, object> mmCIR);
		
		public abstract Dictionary<TParamName, object> UnMap(Dictionary<TParamName, object> mmCIR);
		
		public virtual Mapper<TParamName> AddMapper(Mapper<TParamName> mapper)
		{
			return this;
		}
		
		public virtual void RemoveMapper(Mapper<TParamName> mapper)
		{
			
		}
	}
}
