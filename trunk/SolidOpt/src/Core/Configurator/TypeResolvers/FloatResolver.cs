/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 25.1.2009 г.
 * Time: 13:35
 * 
 */
using System;

namespace SolidOpt.Core.Configurator.TypeResolvers
{
	/// <summary>
	/// Description of FloatResolver.
	/// </summary>
	public class FloatResolver : StringResolver
	{
		public FloatResolver()
		{
		}
		
		public override object TryResolve(object paramValue)
		{
			try {
				return Convert.ChangeType(paramValue, TypeCode.Single);
			}
			catch {
				try {
					return Convert.ChangeType(paramValue, TypeCode.Double);
				}
				catch {
					return base.TryResolve(paramValue);
				}
			}
		}
		
	}
}
