/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 19.8.2008 г.
 * Time: 14:01
 * 
 */

using System;
using System.IO;

namespace SolidOpt.Core.Providers.StreamProvider.Exporters
{
	/// <summary>
	/// Stores file on the local machine at given location.
	/// </summary>
	public class FileExporter : ISetURI
	{
		public FileExporter()
		{
		}
		
		internal void CopyStream(Stream source, Stream dest)
		{
			byte[] buffer = new byte[65536];
			int read;
			do{
				read = source.Read(buffer, 0, buffer.Length);
				dest.Write(buffer, 0, read);
			} while (read != 0);
		}
		
		public bool CanExport(Uri resource)
		{
			if (resource.IsFile){
				return true;
			}
			return false;
		}
		
		public void Export(Stream stream, Uri resource)
		{
			stream.Flush();
			stream.Seek(0, SeekOrigin.Begin);
			
			FileStream fs = new FileStream(resource.AbsolutePath, FileMode.Create);
			CopyStream(stream, fs);
			
			fs.Close();
			stream.Close();
				
		}
	}
}
