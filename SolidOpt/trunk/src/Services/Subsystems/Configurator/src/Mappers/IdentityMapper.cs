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
	/// The identity mapper is used for testing and demonstration what should contain
	/// a mapper. When it is used it returns the same name.
	/// </summary>
	public class IdentityMapper<TParamName> : Mapper<TParamName>
	{
		public IdentityMapper()
		{
		}
		
		public override Dictionary<TParamName, object> Map(Dictionary<TParamName, object> mmCIR)
		{
			return mmCIR;
		}
		
		public override Dictionary<TParamName, object> UnMap(Dictionary<TParamName, object> mmCIR)
		{
			return mmCIR;
		}
	}
}
