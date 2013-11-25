/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections;
using System.Reflection;
using System.ComponentModel;
using SolidV.Gtk.InspectorGrid.InspectorEditors;
using System.Drawing.Design;

namespace SolidV.Gtk.InspectorGrid
{
  using Gtk = global::Gtk;
	internal class EditorManager
	{
		private Hashtable editors = new Hashtable();
		private Hashtable inheritingEditors = new Hashtable();
		private Hashtable surrogates = new Hashtable();
		static InspectorEditorCell Default = new InspectorEditorCell();
		static Hashtable cellCache = new Hashtable();

		internal EditorManager()
		{
			LoadEditor(Assembly.GetAssembly(typeof(EditorManager)));
		}

		public void LoadEditor(Assembly editorAssembly)
		{
			foreach(Type t in editorAssembly.GetTypes()) {
				foreach(Attribute currentAttribute in Attribute.GetCustomAttributes(t)) {
					if(currentAttribute.GetType() == typeof(InspectorEditorTypeAttribute)) {
						InspectorEditorTypeAttribute peta =(InspectorEditorTypeAttribute)currentAttribute;
						Type editsType = peta.Type;
						if(t.IsSubclassOf(typeof(InspectorEditorCell)))
							if(peta.Inherits)
								inheritingEditors.Add(editsType, t);
							else
								editors.Add(editsType, t);
					}
					else if(currentAttribute.GetType() == typeof(SurrogateUITypeEditorAttribute)) {
						Type editsType =(currentAttribute as SurrogateUITypeEditorAttribute).Type;
						surrogates.Add(editsType, t);
					}
				}
			}
		}

		public InspectorEditorCell GetEditor(PropertyDescriptor pd)
		{
			InspectorEditorCell cell = pd.GetEditor(typeof(InspectorEditorCell)) as InspectorEditorCell;
			if(cell != null)
				return cell;
			
			Type editorType = GetEditorType(pd);
			if(editorType == null)
				return Default;
			
			if(typeof(IInspectorEditor).IsAssignableFrom(editorType)) {
				if(!typeof(Gtk.Widget).IsAssignableFrom(editorType))
					throw new Exception("The property editor '" + editorType + "' must be a Gtk Widget");
				return Default;
			}

			cell = cellCache [editorType] as InspectorEditorCell;
			if(cell != null)
				return cell;

			if(!typeof(InspectorEditorCell).IsAssignableFrom(editorType))
				throw new Exception("The property editor '" + editorType + "' must be a subclass of " +
                      "Stetic.InspectorEditorCell or implement Stetic.IInspectorEditor");

			cell =(InspectorEditorCell) Activator.CreateInstance(editorType);
			cellCache [editorType] = cell;
			return cell;
		}
		
		public Type GetEditorType(PropertyDescriptor pd)
		{
			//try to find a custom editor
			//TODO: Find a way to provide a IWindowsFormsEditorService so this can work directly
			//for now, substitute GTK#-based editors
			/*
			UITypeEditor UITypeEd =(UITypeEditor) pd.GetEditor(typeof(System.Drawing.Design.UITypeEditor));//first, does it have custom editors?
			if(UITypeEd != null)
				if(surrogates.Contains(UITypeEd.GetType()))
					return instantiateEditor((Type) surrogates[UITypeEd.GetType()], parentRow);
			*/

			//does a registered GTK# editor support this natively?
			Type editType = pd.PropertyType;
			if(editors.Contains(editType))
				return(Type) editors [editType];
			
			//editors that edit derived types
			foreach(DictionaryEntry de in inheritingEditors)
				if(editType.IsSubclassOf((Type) de.Key))
					return(Type) de.Value;
			
			if(pd.PropertyType.IsEnum) {
				if(pd.PropertyType.IsDefined(typeof(FlagsAttribute), true))
					return typeof(InspectorEditors.FlagsEditorCell);
				else
					return typeof(InspectorEditors.EnumerationEditorCell);
			}
			
			//collections with items of single type that aren't just objects
			if(typeof(IList).IsAssignableFrom(editType)) {
				// Iterate through all properties since there may be more than one indexer.
				if(GetCollectionItemType(editType) != null)
          return null;//typeof(CollectionEditor);
			}
			
			//TODO: support simple SWF collection editor derivatives that just override Types available
			// and reflect protected Type[] NewItemTypes {get;} to get types
			//if(UITypeEd is System.ComponentModel.Design.CollectionEditor)
			//	((System.ComponentModel.Design.CollectionEditor)UITypeEd).

			//can we use a type converter with a built-in editor?
			TypeConverter tc = pd.Converter;
			
			if(typeof(ExpandableObjectConverter).IsAssignableFrom(tc.GetType()))
        return null;//typeof(ExpandableObjectEditor);

			//This is a temporary workaround *and* and optimisation
			//First, most unknown types will be most likely to convert to/from strings
			//Second, System.Web.UI.WebControls/UnitConverter.cs dies on non-strings
			if(tc.CanConvertFrom(typeof(string)) && tc.CanConvertTo(typeof(string)))
				return typeof(TextEditor);
			
			foreach(DictionaryEntry editor in editors)
				if(tc.CanConvertFrom((Type) editor.Key) && tc.CanConvertTo((Type) editor.Key))
					return(Type) editor.Value;
					
			foreach(DictionaryEntry de in inheritingEditors)
				if(tc.CanConvertFrom((Type) de.Key) && tc.CanConvertTo((Type) de.Key))
					return(Type) de.Value;

			//nothing found - just display type
			return null;
		}
		
		public static Type GetCollectionItemType(Type colType)
		{
			foreach(PropertyInfo member in colType.GetProperties()) {
				if(member.Name == "Item") {
					if(member.PropertyType != typeof(object))
						return member.PropertyType;
				}
			}
			return null;
		}
	}
}
