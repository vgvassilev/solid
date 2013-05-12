/*
 * $Id: Program.cs 902 2013-03-30 16:31:25Z mvassilev $
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using Gtk;

namespace SolidReflector
{
 class MainClass
 {
  public static void Main (string[] args)
  {
   Application.Init();
   MainWindow win = new MainWindow();
   win.Show();
   Application.Run();
  }
 }
}
