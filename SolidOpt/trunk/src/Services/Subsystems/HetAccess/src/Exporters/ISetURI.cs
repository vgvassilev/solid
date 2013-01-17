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
  /// Specifies the interface for the resource exporters, which should
  /// stored.
  /// </summary>
  public interface ISetURI
  {
    bool CanExport(Uri resource);
    void Export(Stream stream, Uri resource);
  }
}
