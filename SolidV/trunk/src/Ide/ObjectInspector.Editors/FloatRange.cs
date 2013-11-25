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

	[InspectorEditorType(typeof(Double))]
	[InspectorEditorType(typeof(Single))]
	public class FloatRange : Gtk.SpinButton, IInspectorEditor
	{
		Type propType;
		
		public FloatRange(): base(0, 0, 0.01)
		{
		}
		
		public void Initialize(EditSession session)
		{
			propType = session.Inspector.PropertyType;
			
			double min, max;
			
			if(propType == typeof(double)) {
				min = Double.MinValue;
				max = Double.MaxValue;
			} else if(propType == typeof(float)) {
				min = float.MinValue;
				max = float.MaxValue;
			} else
				throw new ApplicationException("FloatRange editor does not support editing values of type " + propType);
			
			SetRange(min, max);
			
			Digits = 2;
		}
		
		object IInspectorEditor.Value {
			get { return Convert.ChangeType(base.Value, propType); }
			set { base.Value =(double) Convert.ChangeType(value, typeof(double)); }
		}
	}
}
