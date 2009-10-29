/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 19.8.2008 г.
 * Time: 14:01
 * 
 */

using System;
using System.IO;

namespace SolidOpt.Core.Providers.StreamProvider.Importers
{
	/// <summary>
	/// Description of FileImporter.
	/// </summary>
	public class FileImporter : IGetURI
	{
		public FileImporter()
		{
		}
		
		public bool CanImport(Uri resource)
		{
			if (resource.IsFile){
				return true;
			}
			return false;
		}
		
		public Stream Import(Uri resource)
		{
			return new FileStream(resource.AbsolutePath, FileMode.Open);
		}
	}
}
