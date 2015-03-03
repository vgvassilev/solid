/*
 * $Id: Program.cs 902 2013-03-30 16:31:25Z mvassilev $
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using Gtk;

namespace SampleTool
{
  class MainClass
  {
    public static void Main (string[] args)
    {
      Application.Init();
      MainWindow mainWindow = new MainWindow();
      // Workaround the problem of not appearing main window on Mac OS X
      mainWindow.KeepAbove = true;
      mainWindow.KeepAbove = false;
      //
      mainWindow.Show();
      Application.Run();
    }
  }
}
