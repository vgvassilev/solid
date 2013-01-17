/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;

namespace SolidOpt.Services.Subsystems.Configurator.TypeResolvers
{
  /// <summary>
  /// The identity resolver is used for testing and demonstration what should contain
  /// a resolver. When it is used it returns the same type.
  /// </summary>
  public class IdentityResolver : ITypeResolver
  {
    public IdentityResolver()
    {
    }
    
    public object TryResolve(object paramValue)
    {
      return paramValue;
    }
    
  }
}
