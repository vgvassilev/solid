/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 17.8.2008 ?.
 * Time: 13:17
 * 
 */

using System;
using System.IO;
using System.Collections.Generic;

namespace SolidOpt.Core.Configurator.Targets
{
	/// <summary>
	/// Description of IConfigSaver.
	/// </summary>
	public interface IConfigTarget<TParamName>
	{
		bool CanBuild(string fileFormat);
		Stream Build(Dictionary<TParamName, object> configRepresenation);
	}
}
