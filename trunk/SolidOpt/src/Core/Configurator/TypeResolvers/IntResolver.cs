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
	public class IntResolver : FloatResolver
	{
//		[DllImport("kernel32.dll", SetLastError=true)] static extern void DebugBreak ();
		
		public IntResolver()
		{
		}
		
		public override object TryResolve(object paramValue)
		{
			try {
//				DebugBreak();
				return (Int64) Convert.ChangeType(paramValue, TypeCode.Int64);
			}
			catch {
				return base.TryResolve(paramValue);
			}
		}
		
	}
}
