/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.IO;
using System.Collections.Generic;

namespace SolidOpt.Services.Subsystems.Configurator.Targets
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
