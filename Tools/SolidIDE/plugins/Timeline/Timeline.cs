/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using SolidIDE;
using SolidOpt.Services;

using SolidV.Ide.Dock;
using SolidV.MVC;
using Cairo;

using SolidIDE.Plugins.Designer;

namespace SolidIDE.Plugins.Timeline
{
  public class Timeline : IPlugin, IPad, ITimeline
  {
    // External global objects (form Main program and other plugins)
    private ISolidIDE solidIDE;
    private MainWindow mainWindow;

    // Plugin global objects
    private DockItem timelineDockItem;
    private Gtk.HScale timeline;

    #region IPlugin implementation

    void IPlugin.Init(object context) {
      solidIDE = context as ISolidIDE;
      mainWindow = solidIDE.GetMainWindow();

      // Dock with timeline
      Gtk.ScrolledWindow timelineScrollWindow = new Gtk.ScrolledWindow();
      Gtk.Viewport timelineViewport = new Gtk.Viewport();
      timelineScrollWindow.Add(timelineViewport);

      timeline = new global::Gtk.HScale(null);
      timeline.CanFocus = true;
      timeline.Name = "Timeline";
      timeline.Adjustment.Upper = 100;
      timeline.Adjustment.PageIncrement = 10;
      timeline.Adjustment.StepIncrement = 1;
      timeline.DrawValue = true;
      timeline.Digits = 0;
      timeline.ValuePos = Gtk.PositionType.Bottom;

      timelineViewport.Add(timeline);
      timelineScrollWindow.ShowAll();

      timelineDockItem = mainWindow.DockFrame.AddItem("Timeline");
      timelineDockItem.Behavior = DockItemBehavior.Normal;
      timelineDockItem.Expand = true;
      timelineDockItem.DrawFrame = false;
      timelineDockItem.Label = "Timeline";
      timelineDockItem.Content = timelineScrollWindow;
      timelineDockItem.DefaultVisible = true;
      timelineDockItem.Visible = true;

      // Menu
      var viewMenuItem = solidIDE.GetMenuItem<Gtk.ImageMenuItem>("View");
      var timelineMenuItem = solidIDE.GetMenuItem<Gtk.ImageMenuItem>("View", "Timeline");
      timelineMenuItem.Activated += HandleShowTimelineActivated;
      var runMenuItem = solidIDE.GetMenuItem<Gtk.ImageMenuItem>("Run");
      var startMenuItem = solidIDE.GetMenuItem<Gtk.ImageMenuItem>("Run", "Start");
      startMenuItem.Activated += StartClicked;

      // Toolbar
      var tb1 = solidIDE.GetToolbarItem<Gtk.SeparatorToolItem>("");
      var startTool = (Gtk.ToolButton)solidIDE.GetToolbarItem(new Gtk.ToolButton("gtk-media-play"), "Start");
      startTool.Clicked += StartClicked;
      var tb2 = solidIDE.GetToolbarItem<Gtk.SeparatorToolItem>("");
    }

    void IPlugin.UnInit(object context) {
      // throw new NotImplementedException();
    }

    #endregion

    #region IPad implementation

    void IPad.Init(DockFrame frame) {
      // throw new NotImplementedException();
    }

    #endregion

    #region ITimeline implementation
    #endregion

    protected void HandleShowTimelineActivated(object sender, EventArgs e) {
      timelineDockItem.Visible = true;
    }

    protected void StartClicked(object sender, EventArgs e) {
      //
    }

  }
}
