/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using Gtk;

namespace SolidIDE
{
  class MainClass
  {
    public static int Main(string[] args) {
      Application.Init();
      MainWindow mainWindow = new MainWindow();
      // Workaround the problem of not appearing main window on Mac OS X
      mainWindow.KeepAbove = true;
      mainWindow.KeepAbove = false;
      //
      mainWindow.ShowAll();
      Application.Run();
      return 0;
    }
  }
}
