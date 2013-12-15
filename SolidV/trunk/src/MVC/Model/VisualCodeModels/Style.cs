/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using Cairo;

namespace SolidV.MVC
{
  /// <summary>
  /// Encapsulates the look-and-feel of an object.
  /// </summary>
  /// 
  [Serializable]
  public class Style
  {
    public static Style DefaultStyle;
    
    static Style() {
      DefaultStyle = new Style();
      DefaultStyle.border = new SolidPattern(0,0,0);
      //DefaultStyle.border = new LinearGradient(0,0,100,100);
      //(DefaultStyle.border as LinearGradient).AddColorStop(0, new Color(0,0,0));
      //(DefaultStyle.border as LinearGradient).AddColorStop(1, new Color(1,1,1));
      DefaultStyle.fill = new SolidPattern(1,1,1);
      DefaultStyle.borderWidth = 1;
    }
    
    private Pattern fill;    
    public virtual Pattern Fill {
      get { return fill; }
      set { fill = value; }
    }
    
    private Pattern border;
    public virtual Pattern Border {
      get { return border; }
      set { border = value; }
    }
    
    private int borderWidth;
    public int BorderWidth {
      get { return borderWidth; }
      set { borderWidth = value; }
    }
    
  }
}
