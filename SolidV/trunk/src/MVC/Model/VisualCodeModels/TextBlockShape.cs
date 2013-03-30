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
        private string blockText;

        public string BlockText {
          get { return blockText; }
          set { blockText = value;}
        }
        public TextBlockShape() {
        }
    }
}

