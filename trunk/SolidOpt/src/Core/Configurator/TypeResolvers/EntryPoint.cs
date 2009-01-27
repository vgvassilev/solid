/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 25.1.2009 г.
 * Time: 14:07
 * 
 */
using System;
using System.Collections.Generic;

namespace SolidOpt.Core.Configurator.TypeResolvers
{
	/// <summary>
	/// Description of EntryPoint.
	/// </summary>
	public class EntryPoint : IntResolver
	{
		public EntryPoint()
		{
		}
		
		public override object TryResolve(object paramValue)
		{
			return base.TryResolve(paramValue);
		}
		
	}
}
