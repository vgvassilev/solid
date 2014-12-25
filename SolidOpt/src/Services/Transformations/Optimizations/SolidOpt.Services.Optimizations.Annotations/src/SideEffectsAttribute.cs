/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

namespace SolidOpt.Services.Transformations.Optimizations.AST.MethodInline
{
  /// <summary>
  /// Shows if the annotated element has side effect. 
  /// </summary>
  public class SideEffectsAttribute : Attribute
  {
    public bool HasSideEffects = false;
    
    public SideEffectsAttribute(bool HasSideEffects)
    {
      this.HasSideEffects = HasSideEffects;
    }
  }
}
