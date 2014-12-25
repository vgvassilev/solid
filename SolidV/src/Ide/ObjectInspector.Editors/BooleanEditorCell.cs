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
  [InspectorEditorType(typeof(bool))]
  public class BooleanEditorCell : InspectorEditorCell 
  {
    static int indicatorSize;
    static int indicatorSpacing;
    
    static BooleanEditorCell()
    {
      Gtk.CheckButton cb = new Gtk.CheckButton();
      indicatorSize =(int) cb.StyleGetProperty("indicator-size");
      indicatorSpacing =(int) cb.StyleGetProperty("indicator-spacing");
    }
    
    public override void GetSize(int availableWidth, out int width, out int height)
    {
      width = 20;
      height = 20;
    }
    
    public override void Render(Gdk.Drawable window, Gdk.Rectangle bounds, Gtk.StateType state)
    {
      Gtk.ShadowType sh =(bool) Value ? Gtk.ShadowType.In : Gtk.ShadowType.Out;
      int s = indicatorSize - 1;
      if(s > bounds.Height)
        s = bounds.Height;
      if(s > bounds.Width)
        s = bounds.Width;
      Gtk.Style.PaintCheck(Container.Style, window, state, sh, bounds, Container, "checkbutton", bounds.X + indicatorSpacing - 1, bounds.Y +(bounds.Height - s)/2, s, s);
    }
    
    protected override IInspectorEditor CreateEditor(Gdk.Rectangle cell_area, Gtk.StateType state)
    {
      return new BooleanEditor();
    }
  }
  
  public class BooleanEditor : Gtk.CheckButton, IInspectorEditor 
  {
    public void Initialize(EditSession session)
    {
      if(session.Inspector.PropertyType != typeof(bool))
        throw new ApplicationException("Boolean editor does not support editing values of type " + session.Inspector.PropertyType);
    }
    
    public object Value { 
      get { return Active; } 
      set { Active =(bool) value; }
    }
    
    protected override void OnToggled()
    {
      base.OnToggled();
      if(ValueChanged != null)
        ValueChanged(this, EventArgs.Empty);
    }

    public event EventHandler ValueChanged;
  }
}
