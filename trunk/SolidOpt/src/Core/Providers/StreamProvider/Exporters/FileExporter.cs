/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 19.8.2008 ?.
 * Time: 14:01
 * 
 */

using System;
using System.IO;

namespace SolidOpt.Core.Providers.StreamProvider.Exporters
{
	/// <summary>
	/// Description of FileExporter.
	/// </summary>
	public class FileExporter : ISetURI
	{
		public FileExporter()
		{
		}
		
		public bool CanExport(Uri resource)
		{
			if (resource.IsFile){
				return true;
			}
			return false;
		}
		
		public bool Export(Stream stream, Uri resource)
		{
			FileStream fs = new FileStream(resource.AbsolutePath, FileMode.Truncate);
			if (fs.CanWrite){
				 
			}
			return true;//TODO: change logics
		}
	}
}
