/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.IO;
using System.Collections;
using System.ComponentModel;

using Gtk;
using Gdk;

using System.Collections.Generic;
using System.Linq;

//using MonoDevelop.Components;

namespace SolidV.Gtk.InspectorGrid
{
  using Gtk = global::Gtk;

	[System.ComponentModel.Category("MonoDevelop.Components")]
	[System.ComponentModel.ToolboxItem(true)]
	public class InspectorGrid: Gtk.VBox
	{
		object currentObject;
		object[] propertyProviders;

		InspectorGridTable tree;
		HSeparator helpSeparator;
		HSeparator toolbarSeparator;
		VPaned vpaned;
		
		IToolbarProvider toolbar;
		RadioButton catButton;
		RadioButton alphButton;

		string descTitle, descText;
		Label descTitleLabel;
		TextView descTextView;
		Gtk.Widget descFrame;
		
		EditorManager editorManager;
		
		InspectorSort propertySort = InspectorSort.Categorized;
		
		const string PROP_HELP_KEY = "MonoDevelop.InspectorPad.ShowHelp";
		
		public InspectorGrid(): this(new EditorManager())
		{
		}
		
		internal InspectorGrid(EditorManager editorManager)
		{
			this.editorManager = editorManager;
			
			#region Toolbar
			
			InspectorGridToolbar tb = new InspectorGridToolbar();
			base.PackStart(tb, false, false, 0);
			toolbar = tb;
			
			catButton = new RadioButton((Gtk.RadioButton)null);
			catButton.DrawIndicator = false;
			catButton.Relief = ReliefStyle.None;
//			Gdk.Pixbuf pixbuf = null;
//			try {
//				pixbuf = new Gdk.Pixbuf(typeof(InspectorGrid).Assembly, "MonoDevelop.Components.InspectorGrid.SortByCat.png");
//			} catch(Exception e) {
//				LoggingService.LogError("Can't create pixbuf from resource: MonoDevelop.Components.InspectorGrid.SortByCat.png", e);
//			}
//			if(pixbuf != null) {
//				catButton.Image = new Gtk.Image(pixbuf);
//				catButton.Image.Show();
//			}
//			catButton.TooltipText = GettextCatalog.GetString("Sort in categories");
//			catButton.Toggled += new EventHandler(toolbarClick);
//			toolbar.Insert(catButton, 0);
			
			alphButton = new RadioButton(catButton);
			alphButton.DrawIndicator = false;
			alphButton.Relief = ReliefStyle.None;
			alphButton.Image = new Gtk.Image(Stock.SortAscending, IconSize.Menu);
			alphButton.Image.Show();
//			alphButton.TooltipText = GettextCatalog.GetString("Sort alphabetically");
			alphButton.Clicked += new EventHandler(toolbarClick);
//			toolbar.Insert(alphButton, 1);
			
			catButton.Active = true;
			
			#endregion

			vpaned = new VPaned();

			tree = new InspectorGridTable(editorManager, this);
			tree.Changed += delegate {
				Update();
			};

			ScrolledWindow sw = new ScrolledWindow();
			sw.AddWithViewport(tree);
			((Gtk.Viewport)sw.Child).ShadowType = Gtk.ShadowType.None;
			sw.ShadowType = Gtk.ShadowType.None;
			sw.HscrollbarPolicy = PolicyType.Never;
			sw.VscrollbarPolicy = PolicyType.Automatic;

			VBox tbox = new VBox();
			toolbarSeparator = new HSeparator();
			toolbarSeparator.Visible = true;
			tbox.PackStart(toolbarSeparator, false, false, 0);
			tbox.PackStart(sw, true, true, 0);
			helpSeparator = new HSeparator();
			tbox.PackStart(helpSeparator, false, false, 0);
			helpSeparator.NoShowAll = true;
			vpaned.Pack1(tbox, true, true);
			
			AddInspectorTab(new DefaultInspectorTab());
			AddInspectorTab(new EventInspectorTab());

			base.PackEnd(vpaned);
			base.FocusChain = new Gtk.Widget [] { vpaned };
			
			Populate();
			UpdateTabs();
		}
		
		public void SetToolbarProvider(IToolbarProvider toolbarProvider)
		{
			InspectorGridToolbar t = toolbar as InspectorGridToolbar;
			if(t == null)
				throw new InvalidOperationException("Custom toolbar provider already set");
			Remove(t);
			foreach(Widget w in t.Children) {
				t.Remove(w);
				toolbarProvider.Insert(w, -1);
			}
			t.Destroy();
			toolbarSeparator.Hide();
			toolbar = toolbarProvider;
			UpdateTabs();
		}
		
		public event EventHandler Changed {
			add { tree.Changed += value; }
			remove { tree.Changed -= value; }
		}
			
		internal EditorManager EditorManager {
			get { return editorManager; }
		}
		
		#region Toolbar state and handlers
		
		private const int FirstTabIndex = 3;
		
		void toolbarClick(object sender, EventArgs e)
		{
			if(sender == alphButton)
				InspectorSort = InspectorSort.Alphabetical;
			else if(sender == catButton)
				InspectorSort = InspectorSort.Categorized;
			else {
				TabRadioToolButton button =(TabRadioToolButton) sender;
				if(selectedTab == button.Tab) return;
				selectedTab = button.Tab;
				Populate();
			}
			// If the tree is re-populated while a value is being edited, the focus that the value editor had
			// is not returned back to the tree. We need to explicitly get it.
			tree.GrabFocus();
		}
		
		public InspectorSort InspectorSort {
			get { return propertySort; }
			set {
				if(value != propertySort) {
					propertySort = value;
					Populate();
				}
			}
		}
		
		ArrayList propertyTabs = new ArrayList();
		InspectorTab selectedTab;
		
		public InspectorTab SelectedTab {
			get { return selectedTab; }
		}
		
		TabRadioToolButton firstTab;
		SeparatorToolItem tabSectionSeparator;
		
		private void AddInspectorTab(InspectorTab tab)
		{
			TabRadioToolButton rtb;
			if(propertyTabs.Count == 0) {
				selectedTab = tab;
				rtb = new TabRadioToolButton(null);
				rtb.Active = true;
				firstTab = rtb;
				toolbar.Insert(tabSectionSeparator = new SeparatorToolItem(), FirstTabIndex - 1);
			}
			else
				rtb = new TabRadioToolButton(firstTab);
			
			//load image from InspectorTab's bitmap
//			var icon = tab.GetIcon();
//			if(icon != null)
//				rtb.Image = new Gtk.Image(icon);
//			else
//				rtb.Image = new Gtk.Image(Stock.MissingImage, IconSize.Menu);
			
			rtb.Tab = tab;
			rtb.TooltipText = tab.TabName;
			rtb.Toggled += new EventHandler(toolbarClick);	
			
			toolbar.Insert(rtb, propertyTabs.Count + FirstTabIndex);
			
			propertyTabs.Add(tab);
		}
			
		#endregion
		
		public object CurrentObject {
			get { return currentObject; }
			set { SetCurrentObject(value, new object[] {value}); }
		}
		
		public void SetCurrentObject(object obj, object[] propertyProviders)
		{
			if(this.currentObject == obj)
				return;
			this.currentObject = obj;
			this.propertyProviders = propertyProviders;
			UpdateTabs();
			Populate();
		}
		
		public void CommitPendingChanges()
		{
			tree.CommitChanges();
		}
		
		void UpdateTabs()
		{
			bool visible = currentObject != null && toolbar.Children.OfType<TabRadioToolButton>().Count(but => but.Tab.CanExtend(currentObject)) > 1;
			foreach(var w in toolbar.Children.OfType<TabRadioToolButton>())
				w.Visible = visible;
			if(tabSectionSeparator != null)
				tabSectionSeparator.Visible = visible;
		}
	
		//TODO: add more intelligence for editing state etc. Maybe need to know which property has changed, then 
		//just update that
		public void Refresh()
		{
			QueueDraw();
		}
		
		void Populate()
		{
			PropertyDescriptorCollection properties;
			
			tree.SaveStatus();
			tree.Clear();
			tree.InspectorSort = propertySort;
			
			if(currentObject == null) {
				properties = new PropertyDescriptorCollection(new PropertyDescriptor[0] {});
				tree.Populate(properties, currentObject);
			}
			else {
				foreach(object prov in propertyProviders) {
					properties = selectedTab.GetProperties(prov);
					tree.Populate(properties, prov);
				}
			}
			tree.RestoreStatus();
		}
		
		void Update()
		{
			PropertyDescriptorCollection properties;
			
			if(currentObject == null) {
				properties = new PropertyDescriptorCollection(new PropertyDescriptor[0] {});
				tree.Update(properties, currentObject);
			}
			else {
				foreach(object prov in propertyProviders) {
					properties = selectedTab.GetProperties(prov);
					tree.Update(properties, prov);
				}
			}
		}
		
		public bool ShowToolbar {
			get { return toolbar.Visible; }
			set { toolbar.Visible = toolbarSeparator.Visible = value; }
		}
		
		public ShadowType ShadowType {
			get { return tree.ShadowType; }
			set { tree.ShadowType = value; }
		}
		
		#region Hel Pane
		
		public bool ShowHelp
		{
			get { return descFrame != null; }
			set {
				// Disable for now. Documentation is shown in tooltips
/*				if(value == ShowHelp)
					return;
				if(value) {
					AddHelpPane();
					helpSeparator.Show();
				} else {
					vpaned.Remove(descFrame);
					descFrame.Destroy();
					descFrame = null;
					descTextView = null;
					descTitleLabel = null;
					helpSeparator.Hide();
				}*/
			}
		}
		
		void AddHelpPane()
		{
			VBox desc = new VBox(false, 0);

			descTitleLabel = new Label();
			descTitleLabel.SetAlignment(0, 0);
			descTitleLabel.SetPadding(5, 2);
			descTitleLabel.UseMarkup = true;
			desc.PackStart(descTitleLabel, false, false, 0);

			ScrolledWindow textScroll = new ScrolledWindow();
			textScroll.HscrollbarPolicy = PolicyType.Never;
			textScroll.VscrollbarPolicy = PolicyType.Automatic;
			
			desc.PackEnd(textScroll, true, true, 0);
			
			//TODO: Use label, but wrapping seems dodgy.
			descTextView = new TextView();
			descTextView.WrapMode = WrapMode.Word;
			descTextView.WidthRequest = 1;
			descTextView.HeightRequest = 70;
			descTextView.Editable = false;
			descTextView.LeftMargin = 5;
			descTextView.RightMargin = 5;
			
			Pango.FontDescription font = Style.FontDescription.Copy();
			font.Size =(font.Size * 8) / 10;
			descTextView.ModifyFont(font);
			
			textScroll.Add(descTextView);
			
			descFrame = desc;
			vpaned.Pack2(descFrame, false, true);
			descFrame.ShowAll();
			UpdateHelp();
		}
		
		public void SetHelp(string title, string description)
		{
			descTitle = title;
			descText = description;
			UpdateHelp();
		}
		
		void UpdateHelp()
		{
			if(!ShowHelp)
				return;
			descTextView.Buffer.Clear();
			if(descText != null)
				descTextView.Buffer.InsertAtCursor(descText);
			descTitleLabel.Markup = descTitle != null?
				"<b>" + descTitle + "</b>" : string.Empty;
		}

		public void ClearHelp()
		{
			descTitle = descText = null;
			UpdateHelp();
		}
		
		public interface IToolbarProvider
		{
			void Insert(Widget w, int pos);
			Widget[] Children { get; }
			void ShowAll();
			bool Visible { get; set; }
		}
		
		class InspectorGridToolbar: HBox, IToolbarProvider
		{
			public InspectorGridToolbar()
			{
				Spacing = 3;
			}
			
			public void Insert(Widget w, int pos)
			{
				PackStart(w, false, false, 0);
				if(pos != -1) {
					Box.BoxChild bc =(Box.BoxChild) this [w];
					bc.Position = pos;
				}
			}
		}
		
		#endregion
	}
	
	class TabRadioToolButton: RadioButton
	{
		public TabRadioToolButton(RadioButton group): base(group)
		{
			DrawIndicator = false;
			Relief = ReliefStyle.None;
			NoShowAll = true;
		}
		
		public InspectorTab Tab;
	}
	
	public enum InspectorSort
	{
		NoSort = 0,
		Alphabetical,
		Categorized,
		CategorizedAlphabetical
	}
}
