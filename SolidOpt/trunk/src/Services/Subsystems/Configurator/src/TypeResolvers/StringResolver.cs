/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Text;

namespace SolidOpt.Services.Subsystems.Configurator.TypeResolvers
{
	/// <summary>
	/// Tries to resolve strings.
	/// </summary>
	public class StringResolver : Resolver
	{
		public StringResolver()
		{
		}
		
		public override object TryResolve(object paramValue)
		{
			return paramValue.ToString();
		}
	}
}
