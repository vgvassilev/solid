/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace SolidV.Gtk.InspectorGrid.InspectorEditors {
  using Gtk = global::Gtk;

	[InspectorEditorType(typeof(System.Enum))]
	public class EnumerationEditorCell: InspectorEditorCell
	{
		protected override string GetValueText()
		{
			if(Value == null)
				return "";

			string val = Value.ToString();
			
			// If the enum value has a Description attribute, return the description
			foreach(FieldInfo f in Inspector.PropertyType.GetFields()) {
				if(f.Name == val) {
					DescriptionAttribute att =(DescriptionAttribute) Attribute.GetCustomAttribute(f, typeof(DescriptionAttribute));
					if(att != null)
						return att.Description;
					else
						return val;
				}
			}
			return val;
		}
		
		protected override IInspectorEditor CreateEditor(Gdk.Rectangle cell_area, Gtk.StateType state)
		{
			return new EnumerationEditor();
		}
	}
	
	public class EnumerationEditor : Gtk.HBox, IInspectorEditor {

		Gtk.EventBox ebox;
		Gtk.ComboBoxEntry combo;
		Array values;

		public EnumerationEditor() : base(false, 0)
		{
		}
		
		public void Initialize(EditSession session)
		{
			PropertyDescriptor prop = session.Inspector;
			
			if(!prop.PropertyType.IsEnum)
				throw new ApplicationException("Enumeration editor does not support editing values of type " + prop.PropertyType);
			
			values = System.Enum.GetValues(prop.PropertyType);
			Hashtable names = new Hashtable();
			foreach(FieldInfo f in prop.PropertyType.GetFields()) {
				DescriptionAttribute att =(DescriptionAttribute) Attribute.GetCustomAttribute(f, typeof(DescriptionAttribute));
				if(att != null)
					names [f.Name] = att.Description;
				else
					names [f.Name] = f.Name;
			}
				       
			
			ebox = new Gtk.EventBox();
			ebox.Show();
			PackStart(ebox, true, true, 0);

			combo = Gtk.ComboBoxEntry.NewText();
			combo.Changed += combo_Changed;
			combo.Entry.IsEditable = false;
			combo.Entry.CanFocus = false;
			combo.Entry.HasFrame = false;
			combo.Entry.HeightRequest = combo.SizeRequest().Height;
			combo.Show();
			ebox.Add(combo);

			foreach(object value in values) {
				string str = prop.Converter.ConvertToString(value);
				if(names.Contains(str))
					str =(string) names [str];
				combo.AppendText(str);
			}
		}
		
		protected override void OnDestroyed()
		{
			base.OnDestroyed();
			((IDisposable)this).Dispose();
		}

		void IDisposable.Dispose()
		{
		}

		public object Value {
			get {
				return values.GetValue(combo.Active);
			}
			set {
				int i = Array.IndexOf(values, value);
				if(i != -1)
					combo.Active = i;
			}
		}

		public event EventHandler ValueChanged;

		void combo_Changed(object o, EventArgs args)
		{
			if(ValueChanged != null)
				ValueChanged(this, EventArgs.Empty);
			ebox.TooltipText = Value != null ? Value.ToString() : null;
		}
	}
}
