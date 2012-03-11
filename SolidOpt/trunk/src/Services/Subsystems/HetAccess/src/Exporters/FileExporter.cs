/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.IO;

namespace SolidOpt.Services.Subsystems.HetAccess.Exporters
{
	/// <summary>
	/// Stores file on the at given location.
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
