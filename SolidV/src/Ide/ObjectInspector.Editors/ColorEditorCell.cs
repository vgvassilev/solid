/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.ComponentModel;

namespace SolidV.Gtk.InspectorGrid.InspectorEditors 
{
  using Gtk = global::Gtk;

  [InspectorEditorType(typeof(System.Drawing.Color))]
  public class ColorEditorCell: InspectorEditorCell 
  {
    const int ColorBoxSize = 16;
    const int ColorBoxSpacing = 3;
    
    public override void GetSize(int availableWidth, out int width, out int height)
    {
      base.GetSize(availableWidth - ColorBoxSize - ColorBoxSpacing, out width, out height);
      width += ColorBoxSize + ColorBoxSpacing;
      if(height < ColorBoxSize) height = ColorBoxSize;
    }
    
    protected override string GetValueText()
    {
      System.Drawing.Color color =(System.Drawing.Color) Value;
      //TODO: dropdown known color selector so this does something
      if(color.IsKnownColor)
        return color.Name;
      else if(color.IsEmpty)
        return "";
      else
        return String.Format("#{0:x2}{1:x2}{2:x2}", color.R, color.G, color.B);
    }
    
    public override void Render(Gdk.Drawable window, Gdk.Rectangle bounds, Gtk.StateType state)
    {
      Gdk.GC gc = new Gdk.GC(window);
         gc.RgbFgColor = GetColor();
         int yd =(bounds.Height - ColorBoxSize) / 2;
      window.DrawRectangle(gc, true, bounds.X, bounds.Y + yd, ColorBoxSize - 1, ColorBoxSize - 1);
      window.DrawRectangle(Container.Style.BlackGC, false, bounds.X, bounds.Y + yd, ColorBoxSize - 1, ColorBoxSize - 1);
      bounds.X += ColorBoxSize + ColorBoxSpacing;
      bounds.Width -= ColorBoxSize + ColorBoxSpacing;
      base.Render(window, bounds, state);
    }
    
    private Gdk.Color GetColor()
    {
      System.Drawing.Color color =(System.Drawing.Color) Value;
      //TODO: Inspector.Converter.ConvertTo() fails: why?
      return new Gdk.Color(color.R, color.G, color.B);
    }

    protected override IInspectorEditor CreateEditor(Gdk.Rectangle cell_area, Gtk.StateType state)
    {
      return new ColorEditor();
    }
  }
  
  public class ColorEditor : Gtk.ColorButton, IInspectorEditor
  {
    public void Initialize(EditSession session)
    {
      if(session.Inspector.PropertyType != typeof(System.Drawing.Color))
        throw new ApplicationException("Color editor does not support editing values of type " + session.Inspector.PropertyType);
    }
    
    public object Value { 
      get {
        int red =(int)(255 *(float) Color.Red / ushort.MaxValue);
        int green =(int)(255 *(float) Color.Green / ushort.MaxValue);
        int blue =(int)(255 *(float) Color.Blue / ushort.MaxValue);
        return System.Drawing.Color.FromArgb(red, green, blue);
      }
      set {
        System.Drawing.Color color =(System.Drawing.Color) value;
        Color = new Gdk.Color(color.R, color.G, color.B);
      }
    }
    
    protected override void OnColorSet()
    {
      base.OnColorSet();
      if(ValueChanged != null)
        ValueChanged(this, EventArgs.Empty);
    }

    public event EventHandler ValueChanged;
  }
}
