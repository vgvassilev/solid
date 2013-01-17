/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.IO;
using System.Collections.Generic;

namespace SolidOpt.Services.Subsystems.Configurator.Targets
{
  /// <summary>
  /// The Target infrastructure allows the backward compatibility. Its main goal is to
  /// serialize the Configuration Intermediate Representation into different formats.
  /// </summary>
  public interface IConfigTarget<TParamName>
  {
    bool CanBuild(string fileFormat);
    
    /// <summary>
    /// Serializes the configuration into specified format 
    /// </summary>
    /// <param name="configRepresenation">
    /// A <see cref="Dictionary<TParamName, System.Object>"/>
    /// </param>
    /// <returns>
    /// A <see cref="Stream"/>
    /// </returns>
    Stream Build(Dictionary<TParamName, object> configRepresenation);
  }
}
