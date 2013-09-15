/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

// Inspired (copied and modified) by Monodevelop with MIT license.

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SolidV.Gtk
{
  using Cairo = global::Cairo; // We need those because the lookup won't fall back into the global
  using Gtk = global::Gtk; // namespace.

  /// <summary>
  /// Represents a 'single' cell in the 'table' of properties.
  /// </summary>
  public class PropertyEditorCell {
    private Pango.Layout layout;
    private PropertyDescriptor property; 
    private object obj;
    private Gtk.Widget container;

    internal void Initialize (Gtk.Widget container, PropertyDescriptor property, object obj) {
      this.container = container;

      layout = new Pango.Layout (container.PangoContext);
      layout.Width = -1;
      
      Pango.FontDescription des = container.Style.FontDescription.Copy();
      layout.FontDescription = des;
      
      this.property = property;
      this.obj = obj;
      Initialize();
    }

    protected virtual void Initialize() {
      string s = null;//GetValueMarkup();
      if (s != null)
        layout.SetMarkup(GetNormalizedText(s));
      else
        layout.SetText (GetNormalizedText(GetValueText()));
    }

    protected virtual string GetValueText() {
      string result = "";
      if (obj == null)
        return result;
      object val = property.GetValue(obj);
      if (val != null)
        result = property.Converter.ConvertToString(val);

      return result;
    }

    private string GetNormalizedText (string s) {
      if (s == null)
        return "";
      
      int i = s.IndexOf ('\n');
      if (i == -1)
        return s;
      
      s = s.TrimStart ('\n',' ','\t');
      i = s.IndexOf ('\n');
      if (i != -1)
        return s.Substring (0, i) + "...";
      else
        return s;
    }

    public virtual void GetSize (int availableWidth, out int width, out int height) {
      layout.GetPixelSize (out width, out height);
    }

    public virtual void Render (Gdk.Drawable window, Gdk.Rectangle bounds, Gtk.StateType state) {
      int w, h;
      layout.GetPixelSize (out w, out h);
      int dy = (bounds.Height - h) / 2;
      window.DrawLayout (container.Style.TextGC (state), bounds.X, dy + bounds.Y, layout);
    }
  }
}
