/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using Gtk;
using Gdk;
using System.Text;
using System.ComponentModel;

namespace SolidV.Gtk.InspectorGrid.InspectorEditors 
{
  using Gtk = global::Gtk;

	[InspectorEditorType(typeof(TimeSpan))]
	public class TimeSpanEditorCell: InspectorEditorCell
	{
		protected override string GetValueText()
		{
			return((TimeSpan)Value).ToString();
		}
		
		protected override IInspectorEditor CreateEditor(Gdk.Rectangle cell_area, Gtk.StateType state)
		{
			return new TimeSpanEditor();
		}
	}
	
	public class TimeSpanEditor: Gtk.HBox, IInspectorEditor
	{
		Gtk.Entry entry;
		TimeSpan time;
		
		public TimeSpanEditor()
		{
			entry = new Gtk.Entry();
			entry.Changed += OnChanged;
			entry.HasFrame = false;
			PackStart(entry, true, true, 0);
			ShowAll();
		}
		
		public void Initialize(EditSession session)
		{
		}
		
		public object Value {
			get { return time; }
			set {
				time =(TimeSpan) value;
				entry.Changed -= OnChanged;
				entry.Text = time.ToString();
				entry.Changed += OnChanged;
			}
		}
		
		void OnChanged(object o, EventArgs a)
		{
			string s = entry.Text;
			
			try {
				time = TimeSpan.Parse(s);
				if(ValueChanged != null)
					ValueChanged(this, a);
			} catch {
			}
		}
		
		public event EventHandler ValueChanged;
	}
}
