/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.ComponentModel;

namespace SolidV.Gtk.InspectorGrid
{
  public class InspectorValueChangedEventArgs
  {
    private PropertyDescriptor changedItem;
    private object oldValue;
    private object newValue;

    public InspectorValueChangedEventArgs(PropertyDescriptor changedItem, object oldValue, 
                                                                          object newValue)
    {
      this.changedItem = changedItem;
      this.oldValue = oldValue;
      this.newValue = newValue;
    }

    public object OldValue {
      get { return oldValue; }
    }

    public object NewValue
    {
      get { return newValue; }
    }

    public PropertyDescriptor ChangedItem {
      get { return changedItem; }
    }
  }
}
