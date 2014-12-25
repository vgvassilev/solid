/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using Gdk;

namespace SolidV.GdkExtensions
{
  using Cairo = global::Cairo;
  public static class GdkExtensions
  {
    public static Cairo.Color ToCairoColor (this Gdk.Color color)
    {
      return new Cairo.Color ((double)color.Red / ushort.MaxValue, (double)color.Green / ushort.MaxValue, (double)color.Blue / ushort.MaxValue);
    }
  }
}
