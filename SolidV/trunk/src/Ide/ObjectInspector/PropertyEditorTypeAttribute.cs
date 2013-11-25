/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

namespace SolidV.Gtk.InspectorGrid
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class InspectorEditorTypeAttribute : Attribute
	{
		private Type type;
		private bool inherits = false;
		
		public InspectorEditorTypeAttribute(Type type)
		{
			this.type = type;
		}
		
		public InspectorEditorTypeAttribute(Type myType, bool inherits)
		{
			this.type = myType;
			this.inherits = inherits;
		}
		
		public bool Inherits {
			get { return inherits; }
		}
		
		public Type Type {
			get {return type; }
		}
	}
}
