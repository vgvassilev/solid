/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 17.8.2008 ?.
 * Time: 13:17
 * 
 */

using System;
using System.Collections.Generic;

namespace SolidOpt.Core.Configurator
{
	/// <summary>
	/// Description of IConfigSaver.
	/// </summary>
	public interface IConfigBuilder<TParamName>
	{
		bool CanBuild(string fileFormat);
		void Build(Dictionary<TParamName, object> configRepresenation, Uri resourse);
	}
}
