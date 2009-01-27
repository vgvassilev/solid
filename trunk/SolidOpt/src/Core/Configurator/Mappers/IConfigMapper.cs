/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 30.12.2008 г.
 * Time: 19:01
 * 
 */

using System;
using System.Collections.Generic;

namespace SolidOpt.Core.Configurator.Mappers
{
	/// <summary>
	/// Description of IConfigMapper.
	/// </summary>
	public interface IConfigMapper<TParamName>
	{
		Dictionary<TParamName, object> Map(Dictionary<TParamName, object> mmCIR);
		Dictionary<TParamName, object> UnMap(Dictionary<TParamName, object> mmCIR);
	}
}
