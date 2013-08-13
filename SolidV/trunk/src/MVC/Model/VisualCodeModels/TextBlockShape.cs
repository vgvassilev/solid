/*
 * $Id:
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

namespace SolidV.MVC
{
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
        
        public TextBlockShape() {
          this.autoSize = false;
        }

        public TextBlockShape(bool autoSize) {
          this.autoSize = autoSize;
        }

        public override string ToString() {
          string result = "";
          for (int i = 0, e = blockText.Length; i < e; i++) {
            result += blockText[i];
          }
          return result;
        }
  }
}

