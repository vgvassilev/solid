/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

namespace SolidOpt.Services.Subsystems.Configurator
{
	
	//FIXME: Fix the link between this interface and ConfigurationManager
	public interface IConfigSaver
	{
		bool CanSave();
		void Save();
		void SaveAs();
	}
}
