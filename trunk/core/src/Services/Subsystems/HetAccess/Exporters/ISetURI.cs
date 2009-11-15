/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 19.8.2008 ?.
 * Time: 13:28
 * 
 */

using System;
using System.IO;

namespace SolidOpt.Services.Subsystems.HetAccess.Exporters
{
	/// <summary>
	/// Description of ISetURI.
	/// </summary>
	public interface ISetURI
	{
		bool CanExport(Uri resource);
		void Export(Stream stream, Uri resource);
	}
}
