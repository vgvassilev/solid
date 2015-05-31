/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;

using Cairo;

namespace SolidV.MVC
{
  /// <summary>
  /// A shape containing text.
  /// </summary>
  [Serializable]
  public class TextBlockShape : Shape
  {
    private int longestLine = -1;
    public int LongestLine {
      get { return longestLine; }
    }        

    public int LineCount {
      get { return blockText.Length; }
    }

    private uint fontSize = 12;
    public uint FontSize {
      get { return fontSize; }
      set { fontSize = value; }
    }         

    private string[] blockText = new string[]{""};
    public string[] Lines {
      get { return blockText; }
    }
    public string BlockText {
      get { return ToString(); }
      set { 
        blockText = value.Split(new string[] { Environment.NewLine },
                                StringSplitOptions.None); 
        for (int i = 0, e = blockText.Length; i < e; i++)
          if (longestLine < blockText[i].Length){
            longestLine = blockText[i].Length;
          }
        UpdateAutoSize();
      }
    }

    private bool autoSize;
    public bool AutoSize {
      get { return autoSize; }
      set { autoSize = value; }
    }

    private string title = null;
    public string Title {
      get { return title; }
      set { title = value; }
    }

    public TextBlockShape(Rectangle rectangle): base(rectangle) {
      this.autoSize = false;
    }

    public TextBlockShape(Rectangle rectangle, bool autoSize): this(rectangle) {
      this.autoSize = autoSize;
    }

    public override string ToString() {
      string result = "";
      for (int i = 0, e = blockText.Length; i < e; i++) {
        result += blockText[i];
      }
      return result;
    }

    private void UpdateAutoSize() {
      if (!AutoSize)
        return;
      Pango.Layout layout = new Pango.Layout(Gdk.PangoHelper.ContextGet());
      Pango.FontDescription font = new Pango.FontDescription();
      font.Family = "Arial";
      font.Size = (int)fontSize * (int)Pango.Scale.PangoScale;
      layout.FontDescription = font;

      double width, height;
      int w, h;          
      int maxW = int.MinValue;          
      
      for (int i = 0; i < blockText.Length; i++) {
        layout.SetText(blockText[i]);            
        layout.GetPixelSize(out w, out h);
        if (maxW < w)
          maxW = w;
      }

      layout.GetPixelSize(out w, out h);

      // FIXME: Very strange! The height somehow is not accurate. It is too big. 
      Width = maxW;
      Height = Title.ToString() != null ? (h - 4) * LineCount + 25 : (h - 4) * LineCount;
    }
  }
}
