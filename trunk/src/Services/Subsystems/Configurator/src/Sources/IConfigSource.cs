/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.IO;
using System.Collections.Generic;

namespace SolidOpt.Services.Subsystems.Configurator.Sources
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
