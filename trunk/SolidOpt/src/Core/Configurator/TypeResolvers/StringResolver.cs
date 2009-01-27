/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 25.1.2009 г.
 * Time: 13:19
 * 
 */
using System;

namespace SolidOpt.Core.Configurator.TypeResolvers
{
	/// <summary>
	/// Description of StringResolver.
	/// </summary>
	public class StringResolver : ITypeResolver
	{
		public StringResolver()
		{
		}
		
		public virtual object TryResolve(object paramValue)
		{
			try {
				return Convert.ChangeType(paramValue, TypeCode.String);
			}
			catch {
				return null;
			}
		}
	}
}
