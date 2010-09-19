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
	/// Description of IConfigMapper.
	/// </summary>
	public interface IConfigMapper<TParamName>
	{
		Dictionary<TParamName, object> Map(Dictionary<TParamName, object> mmCIR);
		Dictionary<TParamName, object> UnMap(Dictionary<TParamName, object> mmCIR);
	}
}
