/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SolidV.Gtk
{
  using Cairo = global::Cairo; // We need those because the lookup won't fall back into the global
  using Gtk = global::Gtk; // namespace.
  public class PropertyGridTable : Gtk.EventBox {
    private List<TableRow> rows = new List<TableRow>();
    private TableRow currentEditorRow;

    public PropertySort PropertySort { get; set; }

    public virtual void Clear() {
      heightMeasured = false;
      //StopAllAnimations ();
      //EndEditing ();
      rows.Clear();
      //QueueDraw ();
      //QueueResize ();
    }

    internal void Populate(PropertyDescriptorCollection properties, object instance) {
      //bool categorised = PropertySort == PropertySort.Categorized;
      bool categorised = false;
      
      //transcribe browsable properties
      var sorted = new List<PropertyDescriptor>();
      
      foreach (PropertyDescriptor descriptor in properties)
        if (descriptor.IsBrowsable)
          sorted.Add (descriptor);
      
      if (sorted.Count == 0)
        return;
      
      if (!categorised) {
        if (PropertySort != PropertySort.NoSort)
          sorted.Sort ((a,b) => a.DisplayName.CompareTo (b.DisplayName));
        foreach (PropertyDescriptor pd in sorted)
          AppendProperty (rows, pd, instance);
      }
      else {
        sorted.Sort ((a,b) => {
          var c = a.Category.CompareTo (b.Category);
          return c != 0 ? c : a.DisplayName.CompareTo (b.DisplayName);
        });
        TableRow lastCat = null;
        List<TableRow> rowList = rows;
        
        foreach (PropertyDescriptor pd in sorted) {
          if (!string.IsNullOrEmpty (pd.Category) && (lastCat == null || pd.Category != lastCat.Label)) {
            TableRow row = new TableRow ();
            row.IsCategory = true;
            row.Expanded = true;
            row.Label = pd.Category;
            row.ChildRows = new List<TableRow> ();
            rows.Add (row);
            lastCat = row;
            rowList = row.ChildRows;
          }
          AppendProperty (rowList, pd, instance);
        }
      }
      QueueDraw ();
      QueueResize ();
    }

    void AppendProperty (PropertyDescriptor prop, object instance)
    {
      AppendProperty (rows, prop, instance);
    }
    
    void AppendProperty (List<TableRow> rowList, PropertyDescriptor prop, object instance)
    {
      TableRow row = new TableRow () {
        IsCategory = false,
        Property = prop,
        Label = prop.DisplayName,
        Instance = instance
      };
      rowList.Add (row);
      
      TypeConverter tc = prop.Converter;
      if (typeof (ExpandableObjectConverter).IsAssignableFrom (tc.GetType ())) {
        object cob = prop.GetValue (instance);
        row.ChildRows = new List<TableRow> ();
        foreach (PropertyDescriptor cprop in TypeDescriptor.GetProperties (cob))
          AppendProperty (row.ChildRows, cprop, cob);
      }
    }
    double dividerPosition = 0.5;
    static readonly Cairo.Color LabelBackgroundColor = new Cairo.Color (250d/255d, 250d/255d, 250d/255d);
    static readonly Cairo.Color DividerColor = new Cairo.Color (217d/255d, 217d/255d, 217d/255d);
    const int PropertyLeftPadding = 8;
    bool heightMeasured = false;
    const int CategoryTopBottomPadding = 6;
    const int PropertyTopBottomPadding = 5;
    const int PropertyContentLeftPadding = 8;

    internal void MeasureHeight (IEnumerable<TableRow> rowList, ref int y) {
      heightMeasured = true;
      Pango.Layout layout = new Pango.Layout(PangoContext);
      foreach (var r in rowList) {
        layout.SetText (r.Label);
        int w,h;
        layout.GetPixelSize (out w, out h);
        if (r.IsCategory) {
          r.EditorBounds = new Gdk.Rectangle (0, y, Allocation.Width, h + CategoryTopBottomPadding * 2);
          y += h + CategoryTopBottomPadding * 2;
        }
        else {
          int eh;
          int dividerX = (int)((double)Allocation.Width * dividerPosition);
          var cell = GetCell(r);
          cell.GetSize (Allocation.Width - dividerX, out w, out eh);
          eh = Math.Max (h + PropertyTopBottomPadding * 2, eh);
          r.EditorBounds = new Gdk.Rectangle (dividerX + PropertyContentLeftPadding, y, Allocation.Width - dividerX - PropertyContentLeftPadding, eh);
          y += eh;
        }
        if (r.ChildRows != null && (r.Expanded || r.AnimatingExpand)) {
          int py = y;
          MeasureHeight (r.ChildRows, ref y);
          r.ChildrenHeight = y - py;
          if (r.AnimatingExpand)
            y = py + r.AnimationHeight;
        }
      }
      layout.Dispose ();
    }


    protected override bool OnExposeEvent (Gdk.EventExpose evnt)
    {
      bool result = base.OnExposeEvent (evnt);
      using (Cairo.Context ctx = Gdk.CairoHelper.Create (evnt.Window)) {
        int dx = (int)((double)Allocation.Width * dividerPosition);
        ctx.LineWidth = 1;
        ctx.Rectangle (0, 0, dx, Allocation.Height);
        ctx.Color = LabelBackgroundColor;
        ctx.Fill ();
        ctx.Rectangle (dx, 0, Allocation.Width - dx, Allocation.Height);
        ctx.Color = new Cairo.Color (1, 1, 1);
        ctx.Fill ();
        ctx.MoveTo (dx + 0.5, 0);
        ctx.RelLineTo (0, Allocation.Height);
        ctx.Color = DividerColor;
        ctx.Stroke ();
        
        int y = 0;
        Draw (ctx, rows, dx, PropertyLeftPadding, ref y);
      }
      return result;
    }

    protected override void OnSizeRequested (ref Gtk.Requisition requisition) {
      requisition.Width = 20;
      
      int dx = (int)((double)Allocation.Width * dividerPosition) - PropertyContentLeftPadding;
      if (dx < 0) dx = 0;
      int y = 0;
      MeasureHeight (rows, ref y);
      requisition.Height = y;
      
      //foreach (var c in children)
      //  c.Key.SizeRequest ();
    }
    
    protected override void OnSizeAllocated (Gdk.Rectangle allocation) {
      base.OnSizeAllocated (allocation);
      int y = 0;
      MeasureHeight (rows, ref y);
      //if (currentEditorRow != null)
      //  children [currentEditor] = currentEditorRow.EditorBounds;
      //foreach (var cr in children) {
      //  var r = cr.Value;
      //  cr.Key.SizeAllocate (new Gdk.Rectangle (r.X, r.Y, r.Width, r.Height));
      //}
    }


    static readonly Cairo.Color CategoryLabelColor = new Cairo.Color (128d/255d, 128d/255d, 128d/255d);

    PropertyEditorCell GetCell (TableRow row) {
      //var e = editorManager.GetEditor (row.Property);
      var e = new PropertyEditorCell();
      e.Initialize(this, row.Property, row.Instance);
      return e;
    }


    void Draw (Cairo.Context ctx, List<TableRow> rowList, int dividerX, int x, ref int y)
    {
      if (!heightMeasured) {
        int dummyY = 0;
        MeasureHeight(rows, ref dummyY);
      }

      Pango.Layout layout = new Pango.Layout (PangoContext);
      TableRow lastCategory = null;
      
      foreach (var r in rowList) {
        int w,h;
        layout.SetText (r.Label);
        layout.GetPixelSize (out w, out h);
        int indent = 0;
        
        if (r.IsCategory) {
          var rh = h + CategoryTopBottomPadding*2;
          ctx.Rectangle (0, y, Allocation.Width, rh);
          Cairo.LinearGradient gr = new Cairo.LinearGradient (0, y, 0, rh);
          gr.AddColorStop (0, new Cairo.Color (248d/255d, 248d/255d, 248d/255d));
          gr.AddColorStop (1, new Cairo.Color (240d/255d, 240d/255d, 240d/255d));
          ctx.Pattern = gr;
          ctx.Fill ();
          
          if (lastCategory == null || lastCategory.Expanded || lastCategory.AnimatingExpand) {
            ctx.MoveTo (0, y + 0.5);
            ctx.LineTo (Allocation.Width, y + 0.5);
          }
          ctx.MoveTo (0, y + rh - 0.5);
          ctx.LineTo (Allocation.Width, y + rh - 0.5);
          ctx.Color = DividerColor;
          ctx.Stroke ();
          
          ctx.MoveTo (x, y + CategoryTopBottomPadding);
          ctx.Color = CategoryLabelColor;
          Pango.CairoHelper.ShowLayout (ctx, layout);

          //FIXME: Implement
          //var img = r.Expanded ? discloseUp : discloseDown;
          //CairoHelper.SetSourcePixbuf (ctx, img, Allocation.Width - img.Width - CategoryTopBottomPadding,
          //                             y + (rh - img.Height) / 2);
          ctx.Paint ();
          
          y += rh;
          lastCategory = r;
        }
        else {
          var cell = GetCell(r);
          r.Enabled = !r.Property.IsReadOnly; //|| cell.EditsReadOnlyObject;
          var state = r.Enabled ? State : Gtk.StateType.Insensitive;
          ctx.Save();
          ctx.Rectangle(0, y, dividerX, h + PropertyTopBottomPadding*2);
          ctx.Clip();
          ctx.MoveTo(x, y + PropertyTopBottomPadding);
          //ctx.Color = Gtk.Style.Text(state).ToCairoColor ();
          ctx.Color = new Cairo.Color(0,0,0);
          Pango.CairoHelper.ShowLayout(ctx, layout);
          ctx.Restore();
          
          if (r != currentEditorRow)
            cell.Render(GdkWindow, r.EditorBounds, state);
          
          y += r.EditorBounds.Height;
          //indent = PropertyIndent;
        }
        
        if (r.ChildRows != null && r.ChildRows.Count > 0 && (r.Expanded || r.AnimatingExpand)) {
          int py = y;
          if (r.AnimatingExpand) {
            ctx.Save ();
            ctx.Rectangle (0, y, Allocation.Width, r.AnimationHeight);
            ctx.Clip ();
          }
          
          Draw (ctx, r.ChildRows, dividerX, x + indent, ref y);
          
          if (r.AnimatingExpand) {
            ctx.Restore ();
            y = py + r.AnimationHeight;
            // Repaing the background because the cairo clip doesn't work for gdk primitives
            int dx = (int)((double)Allocation.Width * dividerPosition);
            ctx.Rectangle (0, y, dx, Allocation.Height - y);
            ctx.Color = LabelBackgroundColor;
            ctx.Fill ();
            ctx.Rectangle (dx + 1, y, Allocation.Width - dx - 1, Allocation.Height - y);
            ctx.Color = new Cairo.Color (1, 1, 1);
            ctx.Fill ();
          }
        }
      }
    }



  }

  public enum PropertySort
  {
    NoSort = 0,
    Alphabetical,
    Categorized,
    CategorizedAlphabetical
  }

  class TableRow
  {
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


  public class PropertyGrid : Gtk.VBox
  {
    private object currentObject;
    public object CurrentObject {
      get { return currentObject; }
      set { 
        currentObject = value;
        Populate();
      }
    }

    private PropertyGridTable tree;
    public PropertyGrid() {
      tree = new PropertyGridTable();
      this.Add(tree);
    }

    public void Populate ()
    {
      PropertyDescriptorCollection properties;
      
      //tree.SaveStatus ();
      tree.Clear ();
      //tree.PropertySort = propertySort;
      properties = GetProperties(currentObject);
      tree.Populate (properties, currentObject);
      if (currentObject == null) {
        //properties = new PropertyDescriptorCollection (new PropertyDescriptor[0] {});
        //tree.Populate (properties, currentObject);
      }
      else {
        //foreach (object prov in propertyProviders) {
        //  properties = selectedTab.GetProperties (prov);
        //  tree.Populate (properties, prov);
        //}
      }
      //tree.RestoreStatus ();
      tree.ShowAll();
      this.ShowAll();
      base.ShowAll();
    }

    public PropertyDescriptor GetDefaultProperty (object component) {
      if (component == null)
        return null;
      return TypeDescriptor.GetDefaultProperty (component);     
    }

    public PropertyDescriptorCollection GetProperties (object component) {
      if (component == null)
        return new PropertyDescriptorCollection (new PropertyDescriptor[] {});
      return TypeDescriptor.GetProperties (component);
    }

  }

  public class PropertyEditorCell {
    private Pango.Layout layout;
    private PropertyDescriptor property; 
    private object obj;
    private Gtk.Widget container;

    internal void Initialize (Gtk.Widget container, PropertyDescriptor property, object obj) {
      this.container = container;

      layout = new Pango.Layout (container.PangoContext);
      layout.Width = -1;
      
      Pango.FontDescription des = container.Style.FontDescription.Copy();
      layout.FontDescription = des;
      
      this.property = property;
      this.obj = obj;
      Initialize();
    }

    protected virtual void Initialize() {
      string s = null;//GetValueMarkup();
      if (s != null)
        layout.SetMarkup(GetNormalizedText(s));
      else
        layout.SetText (GetNormalizedText(GetValueText()));
    }

    protected virtual string GetValueText() {
      string result = "";
      if (obj == null)
        return result;
      object val = property.GetValue(obj);
      if (val != null)
        result = property.Converter.ConvertToString(val);

      return result;
    }

    private string GetNormalizedText (string s) {
      if (s == null)
        return "";
      
      int i = s.IndexOf ('\n');
      if (i == -1)
        return s;
      
      s = s.TrimStart ('\n',' ','\t');
      i = s.IndexOf ('\n');
      if (i != -1)
        return s.Substring (0, i) + "...";
      else
        return s;
    }

    public virtual void GetSize (int availableWidth, out int width, out int height)
    {
      layout.GetPixelSize (out width, out height);
    }

    public virtual void Render (Gdk.Drawable window, Gdk.Rectangle bounds, Gtk.StateType state)
    {
      int w, h;
      layout.GetPixelSize (out w, out h);
      int dy = (bounds.Height - h) / 2;
      window.DrawLayout (container.Style.TextGC (state), bounds.X, dy + bounds.Y, layout);
    }

  }


}
 


