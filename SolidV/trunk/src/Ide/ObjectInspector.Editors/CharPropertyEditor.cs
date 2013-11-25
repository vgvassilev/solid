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
  [InspectorEditorType(typeof(char))]
  public class CharInspectorEditor : Gtk.Entry, IInspectorEditor 
  {
    public CharInspectorEditor()
    {
      MaxLength = 1;
      HasFrame = false;
    }

    public void Initialize(EditSession session)
    {
      if(session.Inspector.PropertyType != typeof(char))
        throw new ApplicationException("Char editor does not support editing values of type " + session.Inspector.PropertyType);
    }
    
    char last;

    public object Value {
      get {
        if(Text.Length == 0)
          return last;
        else
          return Text[0];
      }
      set {
        Text = value.ToString();
        last =(char) value;
      }
    }

    protected override void OnChanged()
    {
      if(ValueChanged != null)
        ValueChanged(this, EventArgs.Empty);
    }

    public event EventHandler ValueChanged;
  }
}
