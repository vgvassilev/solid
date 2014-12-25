/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using Gtk;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;

namespace SolidV.Gtk.InspectorGrid.InspectorEditors 
{
  using Gtk = global::Gtk;

  class ExpandableObjectEditor : InspectorEditorCell
  {
    protected override string GetValueMarkup()
    {
      string val;
      if(Inspector.Converter.CanConvertTo(typeof(string)))
        val = Inspector.Converter.ConvertToString(Value);
      else
        val = Value != null ? Value.ToString() : "";
      
      return "<b>" + GLib.Markup.EscapeText(val) + "</b>";
    }
    
    protected override IInspectorEditor CreateEditor(Gdk.Rectangle cell_area, StateType state)
    {
      if(Inspector.Converter.CanConvertTo(typeof(string)) && Inspector.Converter.CanConvertFrom(typeof(string)))
        return new TextEditor();
      else
        return null;
    }

  }
}
