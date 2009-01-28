/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 25.1.2009 г.
 * Time: 13:44
 * 
 */
using System;

//using System.Runtime.InteropServices;

namespace SolidOpt.Core.Configurator.TypeResolvers
{
	/// <summary>
	/// Description of IntResolver.
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
