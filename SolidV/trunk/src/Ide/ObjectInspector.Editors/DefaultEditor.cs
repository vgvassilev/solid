/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using Gtk;
using System.ComponentModel;

namespace SolidV.Gtk.InspectorGrid.InspectorEditors
{
  using Gtk = global::Gtk;
	public class DefaultEditor : InspectorEditorCell
	{
		protected override string GetValueMarkup()
		{
			return "<span foreground=\"grey\">&lt;" + Inspector.PropertyType.ToString() 
                                                                    + "&gt;</span>";
		}
	}
}
