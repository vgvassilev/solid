/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

namespace SolidV.Gtk.InspectorGrid
{

  [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
  public class SurrogateUITypeEditorAttribute : Attribute
  {
    public Type Type;
    public SurrogateUITypeEditorAttribute(Type myType)
    {
      this.Type = myType; 
    }
  }

  /* TODO: Surrogates for...
   * 
   * System.Drawing.Design.FontEditor
   * System.Drawing.Design.ImageEditor
   * System.Web.UI.Design.DataBindingCollectionEditor
   * System.Web.UI.Design.UrlEditor
   * System.Web.UI.Design.WebControls.DataGridColumnCollectionEditor
   * System.Web.UI.Design.WebControls.RegexTypeEditor
   * System.Web.UI.Design.XmlFileEditor
   * System.Web.UI.Design.TreeNodeCollectionEditor *STUPID: isn't based on CollectionEditor*
   */
}
