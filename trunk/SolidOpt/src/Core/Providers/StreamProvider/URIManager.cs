/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 19.8.2008 ?.
 * Time: 13:26
 * 
 */

using System;
using System.Collections.Generic;
using System.IO;

using SolidOpt.Core.Providers.StreamProvider.Exporters;
using SolidOpt.Core.Providers.StreamProvider.Importers;



namespace SolidOpt.Core.Providers.StreamProvider
{
	/// <summary>
	/// Description of URIManager.
	/// </summary>
	public class URIManager : AbstractResourceProvider
	{
		private List<IGetURI> importers = new List<IGetURI>();
		public List<IGetURI> Importers {
			get { return importers; }
			set { importers = value; }
		}
		
		private List<ISetURI> exporters = new List<ISetURI>();
		public List<ISetURI> Exporters {
			get { return exporters; }
			set { exporters = value; }
		}
		
		public URIManager()
		{
		}
		
		public void SetResource(Stream stream, Uri resource)
		{
			foreach(ISetURI exporter in exporters){
				if (exporter.CanExport(resource)){
					exporter.Export(stream, resource);
				}
			}
			throw new IOException("Resource cannot be exported.");
		}
		
		public Stream GetResource(Uri resource)
		{
			foreach(IGetURI importer in importers){
				if (importer.CanImport(resource)){
					return importer.Import(resource);
				}
			}
			throw new IOException("Resource cannot be imported.");
		}
		
	}
}
