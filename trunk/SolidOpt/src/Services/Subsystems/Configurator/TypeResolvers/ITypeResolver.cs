/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 13.10.2008 г.
 * Time: 17:04
 * 
 */

using System;
using System.Collections.Generic;

namespace SolidOpt.Services.Subsystems.Configurator
{
	/// <summary>
	/// 
	/// </summary>
	public interface ITypeResolver
	{
		object TryResolve(object paramValue);
	}
}
