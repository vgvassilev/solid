/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using Gtk;

namespace SolidV.Gtk.InspectorGrid.InspectorEditors 
{
  using Gtk = global::Gtk;

  [InspectorEditorType(typeof(Delegate), true)]
  public class EventEditorCell : InspectorEditorCell
  {
    IEventBindingService evtBind;

    protected override void Initialize()
    {
      IComponent comp = Instance as IComponent;
      if (comp != null) {
        evtBind = (IEventBindingService)comp.Site.GetService(typeof(IEventBindingService));
      } else {
        evtBind = null;
      }
      base.Initialize();
    }
    
    protected override IInspectorEditor CreateEditor(Gdk.Rectangle cell_area, Gtk.StateType state)
    {
      //get existing method names
      ICollection IColl = evtBind.GetCompatibleMethods(evtBind.GetEvent(Inspector)) ;
      string[] methods = new string [IColl.Count + 1];
      IColl.CopyTo(methods, 1);
      
      //add a suggestion
      methods [0] = evtBind.CreateUniqueMethodName((IComponent) Instance, evtBind.GetEvent(Inspector));
      
      EventEditor combo = new EventEditor(evtBind, methods);

      if(Value != null)
        combo.Entry.Text =(string) Value;
      
      combo.WidthRequest = 30; //Don't artificially inflate the width. It expands anyway.

      return combo;
    }
    
  }
  
  class EventEditor: ComboBoxEntry, IInspectorEditor
  {
    bool isNull;
    PropertyDescriptor prop;
    IEventBindingService evtBind;
    object component;
    
    public EventEditor(IEventBindingService evtBind, string[] ops): base(ops)
    {
      this.evtBind = evtBind;
    }
    
    public void Initialize(EditSession session)
    {
      this.prop = session.Inspector;
      component = session.Instance;
      Entry.Destroyed += new EventHandler(entry_Changed);
      Entry.Activated += new EventHandler(entry_Activated);
    }

    public object Value {
      get {
        //if value was null and new value is empty, leave as null
        if(Entry.Text.Length == 0 && isNull)
          return null;
        else
          return Entry.Text;
      }
      set {
        isNull = value == null;
        if(isNull)
          Entry.Text = "";
        else
          Entry.Text =(string) value;
      }
    }

    protected override void OnChanged()
    {
      if(component == null)
        return;
      entry_Changed(this, null);
      evtBind.ShowCode((IComponent) component, evtBind.GetEvent(prop));
    }

    void entry_Activated(object sender, EventArgs e)
    {
      entry_Changed(sender, e);
      evtBind.ShowCode((IComponent) component, evtBind.GetEvent(prop));
    }
    
    void entry_Changed(object sender, EventArgs e)
    {
      if(ValueChanged != null)
        ValueChanged(this, EventArgs.Empty);
    }
    
    public event EventHandler ValueChanged;
  }
}