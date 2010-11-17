/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

//using System.Runtime.InteropServices;

namespace SolidOpt.Services.Subsystems.Configurator.TypeResolvers
{
	/// <summary>
	/// Tries to resolve ints (int32) and longs (int64).
	/// </summary>
	public class IntResolver : Resolver
	{
//		[DllImport("kernel32.dll", SetLastError=true)] static extern void DebugBreak ();
		
		public IntResolver()
		{
		}
		
		public override object TryResolve(object paramValue)
		{
			Int32 Int32result;
			if (Int32.TryParse(paramValue.ToString(), out Int32result)) {
				return Int32result;
			}
			else {
				Int64 Int64result;
				if (Int64.TryParse(paramValue.ToString(), out Int64result))
					return Int64result;
				else {
					return base.TryResolve(paramValue);
				}
			}
		}
		
	}
}
