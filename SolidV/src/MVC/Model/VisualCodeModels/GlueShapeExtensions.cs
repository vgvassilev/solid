/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;

using Cairo;

namespace SolidV.MVC
{
  public static class GlueShapeExtensions
  {
    public static IEnumerable<Glue> Glues(this Shape shape) {
      if (shape == null)
        yield break;
      
      foreach (Shape subShape in shape.Items) {
        if (subShape != null && subShape is Glue) yield return subShape as Glue;
      }

      IGluesProvider p = shape as IGluesProvider;
      if (p != null)
        foreach (Glue glue in p.GetGlues()) yield return glue;
    }

    public static Glue SetParent(this Glue glue, Shape parent) {
      glue.Parent = parent;
      return glue;
    }

  }
}
