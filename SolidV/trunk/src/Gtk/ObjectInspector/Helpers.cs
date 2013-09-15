/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

// Inspired (copied and modified) by Monodevelop with MIT license.

using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace SolidV.Gtk
{
  public enum PropertySort {
    NoSort = 0,
    Alphabetical,
    Categorized,
    CategorizedAlphabetical
  }
  
  class TableRow {
    public bool IsCategory;
    public string Label;
    public object Instance;
    public PropertyDescriptor Property;
    public List<TableRow> ChildRows;
    public bool Expanded;
    public bool Enabled = true;
    public Gdk.Rectangle EditorBounds;
    public bool AnimatingExpand;
    public int AnimationHeight;
    public int ChildrenHeight;
    public uint AnimationHandle;
  }

}
