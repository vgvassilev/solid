/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;
using System.IO;

using SolidOpt.Services.Subsystems.HetAccess.Exporters;
using SolidOpt.Services.Subsystems.HetAccess.Importers;



namespace SolidOpt.Services.Subsystems.HetAccess
{
  /// <summary>
  /// The manager class, which handles the resource management. The user
  /// specifies only the location and the manager looks for appropriate 
  /// class that could handle the resource.
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
    
    /// <summary>
    /// Traverses the registered store classes and tries to find one that 
    /// can manage the request.
    /// </summary>
    /// <param name="stream">
    /// A <see cref="Stream"/>
    /// </param>
    /// <param name="resource">
    /// A <see cref="Uri"/>
    /// </param>
    public void SetResource(Stream stream, Uri resource)
    {
      foreach(ISetURI exporter in Exporters){
        if (exporter.CanExport(resource)){
          exporter.Export(stream, resource);
          return;
        }
      }
      throw new IOException("Resource cannot be exported.");
    }
    
    /// <summary>
    /// Traverses the registered load classes and tries to find one that 
    /// can manage the request. 
    /// </summary>
    /// <param name="resource">
    /// A <see cref="Uri"/>
    /// </param>
    /// <returns>
    /// A <see cref="Stream"/>
    /// </returns>
    public Stream GetResource(Uri resource)
    {
      foreach(IGetURI importer in Importers){
        if (importer.CanImport(resource)){
          return importer.Import(resource);
        }
      }
      throw new IOException("Resource cannot be imported.");
    }
    
  }
}
