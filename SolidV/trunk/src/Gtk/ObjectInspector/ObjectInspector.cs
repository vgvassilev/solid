/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

// Inspired (copied and modified) by Monodevelop with MIT license.

using System;
using System.Collections.Generic;
using System.ComponentModel;

/// <summary>
/// Contains SolidV-specific extensions of the Gtk library.
/// </summary>
/// 
namespace SolidV.Gtk
{
  using Cairo = global::Cairo; // We need those because the lookup won't fall back into the global
  using Gtk = global::Gtk; // namespace.

  /// <summary>
  /// A Gtk-based implementation of the popular object inspector. It shows the properties of
  /// objects. If the properties are not read only the user could modify their values.
  /// </summary>
  /// 
  public class ObjectInspector : Gtk.VBox
  {
    private object currentObject;

    /// <summary>
    /// Gets or sets the current object whose properties are being inspected. After the current
    /// object was set, the object inspector's information gets updated.
    /// </summary>
    /// <value>The current object.</value>
    /// 
    public object CurrentObject {
      get { return currentObject; }
      set { 
        currentObject = value;
        Populate();
      }
    }

    /// <summary>
    /// The property table, containing the property list and the values.
    /// </summary>
    private ObjectInspectorTable tree;

    public ObjectInspector() {
      tree = new ObjectInspectorTable();
      this.Add(tree);
    }

    /// <summary>
    /// Updates the property lists.
    /// </summary>
    /// 
    private void Populate () {
      PropertyDescriptorCollection properties;      
      tree.Clear ();
      //tree.PropertySort = propertySort;
      properties = GetProperties(currentObject);
      tree.Populate (properties, currentObject);
    }

    /// <summary>
    /// Gets the default property along with its description and value.
    /// </summary>
    /// <returns>The default property.</returns>
    /// <param name="component">Component.</param>
    public PropertyDescriptor GetDefaultProperty (object component) {
      if (component == null)
        return null;
      return TypeDescriptor.GetDefaultProperty (component);     
    }

    /// <summary>
    /// Gets the properties along with their description and values.
    /// </summary>
    /// <returns>The properties.</returns>
    /// <param name="component">Component.</param>
    /// 
    public PropertyDescriptorCollection GetProperties (object component) {
      if (component == null)
        return new PropertyDescriptorCollection (new PropertyDescriptor[] {});
      return TypeDescriptor.GetProperties (component);
    }
  }
}
 


