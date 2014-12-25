/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.IO;

namespace SolidOpt.Services.Subsystems.HetAccess.Importers
{
  /// <summary>
  /// Loads file from specified location.
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
