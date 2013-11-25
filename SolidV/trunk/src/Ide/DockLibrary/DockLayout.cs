using System;
using System.Xml;
using System.Collections.Generic;

namespace MonoDevelop.Components.Docking
{
	class DockLayout: DockGroup
	{
		string name;
		int layoutWidth = 1024;
		int layoutHeight = 768;
		
		public DockLayout (DockFrame frame): base (frame, DockGroupType.Horizontal)
		{
		}
		
		public string Name {
			get {
				return name;
			}
			set {
				name = value;
			}
		}
		
		internal override void Write (XmlWriter writer)
		{
			writer.WriteStartElement ("layout");
			writer.WriteAttributeString ("name", name);
			writer.WriteAttributeString ("width", layoutWidth.ToString ());
			writer.WriteAttributeString ("height", layoutHeight.ToString ());
			base.Write (writer);
			writer.WriteEndElement ();
		}
		
		internal override void Read (XmlReader reader)
		{
			name = reader.GetAttribute ("name");
			string s = reader.GetAttribute ("width");
			if (s != null)
				layoutWidth = int.Parse (s);
			s = reader.GetAttribute ("height");
			if (s != null)
				layoutHeight = int.Parse (s);
			base.Read (reader);
		}
		
		public static DockLayout Read (DockFrame frame, XmlReader reader)
		{
			DockLayout layout = new DockLayout (frame);
			layout.Read (reader);
			return layout;
		}
		
		public override void SizeAllocate (Gdk.Rectangle rect)
		{
			Size = rect.Width;
			base.SizeAllocate (rect);
		}

		internal override void StoreAllocation ()
		{
			base.StoreAllocation ();
			layoutWidth = Allocation.Width;
			layoutHeight = Allocation.Height;
		}
		
		internal override void RestoreAllocation ()
		{
			Allocation = new Gdk.Rectangle (0, 0, layoutWidth, layoutHeight);
			base.RestoreAllocation ();
		}

	}
}
