/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

namespace SolidOpt.Services.Subsystems.Configurator.TypeResolvers
{
  /// <summary>
  /// Type that is being returned when the type resolution fails or
  /// the type is impossible to be determined. 
  /// </summary>
  internal class CannotResolve
  {
    private static CannotResolve instance = new CannotResolve();
    
    public static CannotResolve Instance {
      get {return instance;}
    }
    
    private CannotResolve()
    {
    }
    
  }
}
