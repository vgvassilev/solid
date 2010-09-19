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
	/// Description of EntryPoint.
	/// </summary>
	public class IdentityResolver : ITypeResolver
	{
		public IdentityResolver()
		{
		}
		
		public object TryResolve(object paramValue)
		{
			return paramValue;
		}
		
	}
}
