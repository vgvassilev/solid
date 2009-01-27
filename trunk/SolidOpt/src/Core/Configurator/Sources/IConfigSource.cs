/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 17.8.2008 г.
 * Time: 13:18
 * 
 */

using System;
using System.IO;
using System.Collections.Generic;

namespace SolidOpt.Core.Configurator.Sources
{
	/// <summary>
	/// Description of IConfigLoader.
	/// </summary>
	public interface IConfigSource<TParamName>
	{
		bool CanParse(Uri resUri, Stream resStream);
		Dictionary<TParamName, object> LoadConfiguration(Stream resStream);
	}
}
