/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 19.8.2008 ?.
 * Time: 13:28
 * 
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
