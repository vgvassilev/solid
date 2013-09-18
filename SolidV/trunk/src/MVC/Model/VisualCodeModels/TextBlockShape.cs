/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

using Cairo;

namespace SolidV.MVC
{
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
                  if (longestLine < blockText[i].Length)
                    longestLine = blockText[i].Length;
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
          double width = 100;
          double height = LineCount;
          if (LongestLine < 100) {
            width = LongestLine * 8;
          }
          if (LineCount < 28) {
            height = LineCount * 16;
          }
          Rectangle = new Rectangle(Location.X, Location.Y, width, height);
        }
  }
}
