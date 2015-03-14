/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using SolidOpt.Services;

namespace SolidIDE.Plugins.Designer
{
  public interface IDesigner : IService
  {
    Gtk.Notebook GetNotebook();
  }
}
