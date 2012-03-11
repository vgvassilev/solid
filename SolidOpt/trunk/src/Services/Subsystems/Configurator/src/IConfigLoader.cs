/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;

namespace SolidOpt.Services.Subsystems.Configurator
{
	//FIXME: Fix the link between this interface and ConfigurationManager
	public interface IConfigLoader<TParamName>
	{
		bool CanLoad();
		Dictionary<TParamName, object> LoadConfiguration(Uri resUri);
	}
}
