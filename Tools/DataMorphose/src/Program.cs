/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using Gtk;

namespace DataMorphose
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
