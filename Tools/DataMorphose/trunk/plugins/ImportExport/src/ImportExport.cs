// /*
//  * $Id$
//  * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
//  * For further details see the nearest License.txt
//  */
using System;
using System.IO;
using SolidOpt.Services;
using MonoDevelop.Components.Docking;

namespace DataMorphose
{
  public class ImportExport : IPlugin
  {
    public ImportExport() {
    }

    #region IPlugin implementation
    void IPlugin.Init (object context)
    {
      DockItem dockItem = null;
      IDataMorphose reflector = context as IDataMorphose;
      var MainWindow = reflector.GetMainWindow();
      
      Gtk.MenuBar mainMenuBar = reflector.GetMainMenu();

      Gtk.MenuItem fileMenu = null;
      // Find the File menu if present
      foreach(Gtk.Widget w in mainMenuBar.Children)
        if (w.Name == "FileAction") {
          fileMenu = w as Gtk.MenuItem;
          break;
        }
      // If not present - create it
      if (fileMenu == null) {
        Gtk.Menu menu = new Gtk.Menu();
        fileMenu = new Gtk.MenuItem("File");
        fileMenu.Submenu = menu;
        mainMenuBar.Append(fileMenu);
      }
        
      // Setting up the Open menu item in File
      Gtk.MenuItem import = new Gtk.MenuItem("Import");
      (fileMenu.Submenu as Gtk.Menu).Prepend(import);
      
      Gtk.MenuItem export = new Gtk.MenuItem("Export");
      (fileMenu.Submenu as Gtk.Menu).Prepend(export);

      // Attaching the current dockItem in the DockFrame
      dockItem = MainWindow.DockFrame.AddItem("AssemblyBrowser");
      dockItem.DrawFrame = true;
      dockItem.Label = "Assembly";

     
    }

    void IPlugin.UnInit (object context)
    {
      throw new NotImplementedException ();
    }
    #endregion


  }
}

