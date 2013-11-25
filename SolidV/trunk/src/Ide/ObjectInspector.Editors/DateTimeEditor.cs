/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using Gtk;
using Gdk;
using System.Text;
using System.ComponentModel;

namespace SolidV.Gtk.InspectorGrid.InspectorEditors 
{
  using Gtk = global::Gtk;

  [InspectorEditorType(typeof(DateTime))]
  public class DateTimeEditorCell: InspectorEditorCell
  {
    protected override string GetValueText()
    {
      return((DateTime)Value).ToLongDateString();
    }
    
    protected override IInspectorEditor CreateEditor(Gdk.Rectangle cell_area, Gtk.StateType state)
    {
      return new DateTimeEditor();
    }
  }
  
  public class DateTimeEditor: Gtk.HBox, IInspectorEditor
  {
    Gtk.Entry entry;
    DateTime time;
    
    public DateTimeEditor()
    {
      entry = new Gtk.Entry();
      entry.Changed += OnChanged;
      entry.HasFrame = false;
      PackStart(entry, true, true, 0);
      ShowAll();
    }
    
    public void Initialize(EditSession session)
    {
    }
    
    public object Value {
      get { return time; }
      set {
        time =(DateTime) value;
        entry.Changed -= OnChanged;
        entry.Text = time.ToString("G");
        entry.Changed += OnChanged;
      }
    }
    
    void OnChanged(object o, EventArgs a)
    {
      string s = entry.Text;
      
      foreach(string form in formats) {
        try {
          time = DateTime.ParseExact(s, form, null);
          if(ValueChanged != null)
            ValueChanged(this, a);
          break;
        } catch {
        }
      }
    }
    
    public event EventHandler ValueChanged;
    
    static string[] formats = {"u", "G", "g", "d", "T", "t"};
  }
}
