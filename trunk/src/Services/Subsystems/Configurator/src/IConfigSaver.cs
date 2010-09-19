/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

namespace SolidOpt.Services.Subsystems.Configurator
{
	/// <summary>
	/// Description of IConfigSaver.
	/// </summary>
	public interface IConfigSaver
	{
		bool CanSave();
		void Save();
		void SaveAs();
	}
}
