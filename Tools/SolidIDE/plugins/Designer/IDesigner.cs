/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using SolidOpt.Services;
using SolidV.MVC;

namespace SolidIDE.Plugins.Designer
{
  public interface IDesigner : IService
  {
    #region High level interface methods

    ISheet AddSheet(string Label, ISheet sheet);

    /// <summary>
    /// Adds the shapes to sheet.
    /// </summary>
    /// <param name="objects">Objects.</param>
    void AddShapes(params Shape[] objects);

    ISheet CurrentSheet { get; set; }

    void AttachToolController(IController<Gdk.Event, Cairo.Context, Model> controller);
    void DetachToolController(IController<Gdk.Event, Cairo.Context, Model> controller);

    #endregion

    #region Low level methods

    /// <summary>
    /// Gets the designer notebook.
    /// </summary>
    /// <returns>The notebook.</returns>
    Gtk.Notebook GetNotebook();

    #endregion
  }
}
