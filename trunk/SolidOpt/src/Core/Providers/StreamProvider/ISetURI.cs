/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 19.8.2008 ?.
 * Time: 13:28
 * 
 */

using System;
using System.IO;

namespace SolidOpt.Core.Providers.StreamProvider
{
	/// <summary>
	/// Description of ISetURI.
	/// </summary>
	public interface ISetURI
	{
		bool CanExport(Uri resource);
		bool Export(Stream stream, Uri resource);
	}
}
