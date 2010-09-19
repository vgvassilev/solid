/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.IO;

namespace SolidOpt.Services.Subsystems.HetAccess.Importers
{
	/// <summary>
	/// Description of IGetURI.
	/// </summary>
	public interface IGetURI
	{
		bool CanImport(Uri resource);
		Stream Import(Uri resource);
	}
}
