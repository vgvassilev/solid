/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.IO;
using System.Collections.Generic;

namespace SolidOpt.Services.Subsystems.Configurator.Sources
{
  /// <summary>
  /// The Source infrastructure allows the backward compatibility. Its main goal is to
  /// hangle different configuration formats and to turn them into Configuration 
  /// Intermediate Representation.
  /// </summary>
  public interface IConfigSource<TParamName>
  {
    /// <summary>
    /// Determines if the current Source parser can handle the format. 
    /// </summary>
    /// <param name="resUri">
    /// A <see cref="Uri"/>
    /// </param>
    /// <param name="resStream">
    /// A <see cref="Stream"/>
    /// </param>
    /// <returns>
    /// A <see cref="System.Boolean"/>
    /// </returns>
    bool CanParse(Uri resUri, Stream resStream);
    /// <summary>
    /// Loads the configuration into Configuration Intermediate Representation. 
    /// </summary>
    /// <param name="resStream">
    /// A <see cref="Stream"/>
    /// </param>
    /// <returns>
    /// A <see cref="Dictionary<TParamName, System.Object>"/>
    /// </returns>
    Dictionary<TParamName, object> LoadConfiguration(Stream resStream);
  }
}
