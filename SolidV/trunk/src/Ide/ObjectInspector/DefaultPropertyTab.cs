/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using Gtk;
using System;
using System.ComponentModel;
 
namespace SolidV.Gtk.InspectorGrid
{
	public class DefaultInspectorTab : InspectorTab
	{
		public DefaultInspectorTab()
			: base()
		{
		}
		
		public override string TabName {
			get {return "Properties"; }
		}
		
		public override bool CanExtend(object extendee)
		{
			return true;
		}
		
		public override PropertyDescriptor GetDefaultProperty(object component)
		{
			if(component == null)
				return null;
			return TypeDescriptor.GetDefaultProperty(component);			
		}
		
		public override PropertyDescriptorCollection GetProperties(object component, Attribute[] attributes)
		{
			if(component == null)
				return new PropertyDescriptorCollection(new PropertyDescriptor[] {});
			return TypeDescriptor.GetProperties(component);
		}
	}
	
	public abstract class InspectorTab
	{
		public abstract string TabName { get; }
		public abstract bool CanExtend(object extendee);
		public abstract PropertyDescriptor GetDefaultProperty(object component);
		public abstract PropertyDescriptorCollection GetProperties(object component, Attribute[] attributes);
		
		public PropertyDescriptorCollection GetProperties(object component)
		{
			return GetProperties(component, null);
		}
		
//		public Gdk.Pixbuf GetIcon()
//		{
//			using(var stream = GetType().Assembly.GetManifestResourceStream(GetType().FullName + ".bmp")) {
//				if(stream != null) {
//					try {
//						return new Gdk.Pixbuf(stream);
//					} catch(Exception e) {
//						LoggingService.LogError("Can't create pixbuf from resource:" + GetType().FullName + ".bmp", e);
//					}
//				}
//			}
//			return null;
//		}
	}
}
