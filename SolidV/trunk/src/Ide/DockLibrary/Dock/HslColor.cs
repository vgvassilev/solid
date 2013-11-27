/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using Gdk;
using Cairo;
using SolidV.GdkExtensions;

namespace SolidV.Ide.Dock
{
  using Cairo = global::Cairo;
  public struct HslColor
  {
    public double H {
      get;
      set;
    }
    
    public double S {
      get;
      set;
    }
    
    public double L {
      get;
      set;
    }
    
    static Gdk.Color black = new  Gdk.Color (0, 0, 0);
    public static implicit operator Gdk.Color (HslColor hsl)
    {
      if (hsl.L > 1) hsl.L = 1;
      if (hsl.L < 0) hsl.L = 0;
      if (hsl.H > 1) hsl.H = 1;
      if (hsl.H < 0) hsl.H = 0;
      if (hsl.S > 1) hsl.S = 1;
      if (hsl.S < 0) hsl.S = 0;
      
      double r = 0, g = 0, b = 0;
      
      if (hsl.L == 0)
        return black;
      
      if (hsl.S == 0) {
        r = g = b = hsl.L;
      } else {
        double temp2 = hsl.L <= 0.5 ? hsl.L * (1.0 + hsl.S) : hsl.L + hsl.S -(hsl.L * hsl.S);
        double temp1 = 2.0 * hsl.L - temp2;
        
        double[] t3 = new double[] { hsl.H + 1.0 / 3.0, hsl.H, hsl.H - 1.0 / 3.0};
        double[] clr= new double[] { 0, 0, 0};
        for (int i = 0; i < 3; i++) {
          if (t3[i] < 0)
            t3[i] += 1.0;
          if (t3[i] > 1)
            t3[i]-=1.0;
          if (6.0 * t3[i] < 1.0)
            clr[i] = temp1 + (temp2 - temp1) * t3[i] * 6.0;
          else if (2.0 * t3[i] < 1.0)
            clr[i] = temp2;
          else if (3.0 * t3[i] < 2.0)
            clr[i] = (temp1 + (temp2 - temp1) * ((2.0 / 3.0) - t3[i]) * 6.0);
          else
            clr[i] = temp1;
        }
        
        r = clr[0];
        g = clr[1];
        b = clr[2];
      }
      return new Gdk.Color ((byte)(255 * r), 
                        (byte)(255 * g), 
                        (byte)(255 * b));
    }
    
    public static implicit operator Cairo.Color (HslColor hsl)
    {
      return ((Gdk.Color)hsl).ToCairoColor ();
    }
    
    public static implicit operator HslColor (Gdk.Color color)
    {
      return new HslColor (color);
    }
    
    public HslColor (Gdk.Color color) : this ()
    {
      double r = color.Red   / (double)ushort.MaxValue;
      double g = color.Green / (double)ushort.MaxValue;
      double b = color.Blue  / (double)ushort.MaxValue;

      double v = System.Math.Max (r, g);
      v = System.Math.Max (v, b);

      double m = System.Math.Min (r, g);
      m = System.Math.Min (m, b);
      
      this.L = (m + v) / 2.0;
      if (this.L <= 0.0)
        return;
      double vm = v - m;
      this.S = vm;
      
      if (this.S > 0.0) {
        this.S /= (this.L <= 0.5) ? (v + m) : (2.0 - v - m);
      } else {
        return;
      }
      
      double r2 = (v - r) / vm;
      double g2 = (v - g) / vm;
      double b2 = (v - b) / vm;
      
      if (r == v) {
        this.H = (g == m ? 5.0 + b2 : 1.0 - g2);
      } else if (g == v) {
        this.H = (b == m ? 1.0 + r2 : 3.0 - b2);
      } else {
        this.H = (r == m ? 3.0 + g2 : 5.0 - r2);
      }
      this.H /= 6.0;
    }
    
    public static double Brightness (Gdk.Color c)
    {
      double r = c.Red / (double)ushort.MaxValue;
      double g = c.Green / (double)ushort.MaxValue;
      double b = c.Blue / (double)ushort.MaxValue;
      return System.Math.Sqrt (r * .241 + g * .691 + b * .068);
    }
    
    public override string ToString ()
    {
      return string.Format ("[HslColor: H={0}, S={1}, L={2}]", H, S, L);
    }
  }
}
