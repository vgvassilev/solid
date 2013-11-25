/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using Cairo;
using Gtk;

namespace SolidV.Gtk
{
  using Cairo = global::Cairo;
  using Gtk = global::Gtk;

  public static class GtkExtensions
  {    
    public static Cairo.Color ToCairoColor (this Gdk.Color color)
    {
      return new Cairo.Color ((double)color.Red / ushort.MaxValue,
                              (double)color.Green / ushort.MaxValue,
                              (double)color.Blue / ushort.MaxValue);
    }

    /// <summary>
    /// Places and runs a transient dialog. Does not destroy it, so values can be retrieved from its widgets.
    /// </summary>
    public static int RunCustomDialog (Gtk.Dialog dialog, Gtk.Window parent)
    {
      if (parent == null) {
        if (dialog.TransientFor != null)
          parent = dialog.TransientFor;
        //        else
        //          parent = GetDefaultParent(dialog);
      }
      dialog.TransientFor = parent;
      dialog.DestroyWithParent = true;
      //      PlaceDialog(dialog, parent);
      return dialog.Run ();
    }

    public static int ShowCustomDialog (Gtk.Dialog dialog, Gtk.Window parent)
    {
      try {
        return RunCustomDialog (dialog, parent);
      } finally {
        if (dialog != null)
          dialog.Destroy ();
      }
    }
  }
}

