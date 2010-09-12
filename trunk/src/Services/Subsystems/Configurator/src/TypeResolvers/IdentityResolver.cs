/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 25.1.2009 г.
 * Time: 14:07
 * 
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
