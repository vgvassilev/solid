/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

namespace SolidOpt.Services
{
  /// <summary>
  /// Interface helping implementing custom plugin, which need different contexts.
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

    /// <summary>
    /// The uninitializing method of the plugin. The plugin is unloaded via this method.
    /// </summary>
    /// <param name='context'>
    /// Context.
    /// </param>
    /// 
    void UnInit(object context);
  }
}

