// /*
//  * $Id:
//  * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
//  * For further details see the nearest License.txt
//  */
//
using System;
using Cairo;

namespace SolidV.MVC
{
    public class Style
    {
        public static Style DefaultStyle;

        static Style() {
            DefaultStyle = new Style();
            DefaultStyle.borderColor = new Color(0,0,0);
            DefaultStyle.fillColor = new Color(1,1,1);
            DefaultStyle.borderWidth = 1;
        }

        private Color fillColor;    
        public virtual Color FillColor {
            get { return fillColor; }
            set { fillColor = value; }
        }
        
        private Color borderColor;
        public virtual Color BorderColor {
            get { return borderColor; }
            set { borderColor = value; }
        }
        
        private int borderWidth;
        public int BorderWidth {
            get { return borderWidth; }
            set { borderWidth = value; }
        }
    }

}

