/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.ComponentModel;
using System.ComponentModel.Design;
 
namespace SolidV.Gtk.InspectorGrid
{
  public class EventInspectorTab : InspectorTab
  {
    public EventInspectorTab()
    {
    }
    
    public override string TabName {
      get {return "Events"; }
    }
    
    public override bool CanExtend(object extendee)
    {
      IComponent comp = extendee as IComponent;
      if(comp == null || comp.Site == null)
        return false;
      
      IEventBindingService evtBind =(IEventBindingService) comp.Site.GetService(typeof(IEventBindingService));
      return !(evtBind == null); 
    }
    
    public override PropertyDescriptor GetDefaultProperty(object component)
    {
      IEventBindingService evtBind = GetEventService(component);
      EventDescriptor e = TypeDescriptor.GetDefaultEvent(component);
      
      return(e == null)? null : evtBind.GetEventProperty(e);      
    }
    
    public override PropertyDescriptorCollection GetProperties(object component, Attribute[] attributes)
    {
      IEventBindingService evtBind = GetEventService(component);
      return evtBind.GetEventProperties(TypeDescriptor.GetEvents(component));
    }
    
    private IEventBindingService GetEventService(object component)
    {
      IComponent comp = component as IComponent;
      if(comp == null || comp.Site == null)
        throw new Exception("Check whether a tab can display a component before displaying it");
      IEventBindingService evtBind =(IEventBindingService) comp.Site.GetService(typeof(IEventBindingService));
      if(evtBind == null)
        throw new Exception("Check whether a tab can display a component before displaying it");
      return evtBind;
    }
  }
}