/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 17.8.2008 г.
 * Time: 13:18
 * 
 */

using System;
using System.Collections.Generic;

namespace SolidOpt.Core.Configurator
{
	/// <summary>
	/// Description of IConfigLoader.
	/// </summary>
	public interface IConfigLoader<TParamName>
	{
		bool CanLoad();
		Dictionary<TParamName, object> LoadConfiguration();
	}
}
