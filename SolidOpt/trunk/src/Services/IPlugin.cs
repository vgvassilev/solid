/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

namespace SolidOpt.Services
{
  /// <summary>
  /// Interface ensuring the plugin is compatible with the SolidReflector/DataMorphose project.
  /// </summary>
  /// 
  public interface IPlugin : IService
  {
    /// <summary>
    /// The initializing method of the plugin. The plugin is loaded via this method.
    /// </summary>
    /// <param name='context'>
    /// Context.
    /// </param>
    /// 
    void Init(object context);
  }
}

