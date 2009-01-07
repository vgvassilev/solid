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
	/// Description of IGetURI.
	/// </summary>
	public interface IGetURI
	{
		bool CanImport(Uri resource);
		Stream Import(Uri resource);
	}
}
