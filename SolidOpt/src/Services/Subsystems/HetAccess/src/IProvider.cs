/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

namespace SolidOpt.Services.Subsystems.HetAccess
{
  /// <summary>
  /// Resource management is very common thing in every system/framework. The purpose of
  /// the HetAccess subsystem is to encapsulate the storing/loading of resources and to 
  /// provide a stream representation, wherever is needed. 
  /// Exporter is a class, which implements the store procedure at specific place. For 
  /// example FileExporter can store a stream to specified location. 
  /// Impoter is a class, which implements the load procedure from specific place. For 
  /// example FileImporter can load a file to stream.
  /// Many importers/exporters can be easily implemented for DB access, web access...
  /// </summary>
  public interface IProvider
  {
    
  }
}
