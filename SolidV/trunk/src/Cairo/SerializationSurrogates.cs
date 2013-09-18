// /*
//  * $Id$
//  * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
//  * For further details see the nearest License.txt
//  */
using System;
using System.Runtime.Serialization;

using Cairo;

namespace SolidV.Cairo
{
  public static class SerializationSurrogates
  {
    private static readonly StreamingContext CloneSC = new StreamingContext(StreamingContextStates.Clone);

    public static void RegisterSurrogates(SurrogateSelector ss)
    {
      ss.AddSurrogate(typeof(global::Cairo.Rectangle), CloneSC, new CairoRectangleSerializationSurrogate());
      ss.AddSurrogate(typeof(global::Cairo.Matrix), CloneSC, new CairoMatrixSerializationSurrogate());
      ss.AddSurrogate(typeof(global::Cairo.SolidPattern), CloneSC, new CairoSolidPatternSerializationSurrogate());
      ss.AddSurrogate(typeof(global::Cairo.Color), CloneSC, new CairoColorSerializationSurrogate());
    }

    public sealed class CairoRectangleSerializationSurrogate: ISerializationSurrogate 
    {
      public void GetObjectData(Object obj, SerializationInfo info, StreamingContext context) 
      {
        global::Cairo.Rectangle o = (global::Cairo.Rectangle)obj;
        info.AddValue("X", o.X);
        info.AddValue("Y", o.Y);
        info.AddValue("Width", o.Width);
        info.AddValue("Height", o.Height);
      }
      
      public Object SetObjectData(Object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
      {
        //global::Cairo.Rectangle o = (global::Cairo.Rectangle)obj;
        //o.X = info.GetDouble("X");
        //o.Y = info.GetDouble("Y");
        //o.Width = info.GetDouble("Width");
        //o.Height = info.GetDouble("Height");
        //return null;
        return new global::Cairo.Rectangle(info.GetDouble("X"), info.GetDouble("Y"), info.GetDouble("Width"), info.GetDouble("Height"));
      }
    }

    public sealed class CairoMatrixSerializationSurrogate: ISerializationSurrogate 
    {
      public void GetObjectData(Object obj, SerializationInfo info, StreamingContext context) 
      {
        global::Cairo.Matrix o = (global::Cairo.Matrix)obj;
        info.AddValue("Xx", o.Xx);
        info.AddValue("Yx", o.Yx);
        info.AddValue("Xy", o.Xy);
        info.AddValue("Yy", o.Yy);
        info.AddValue("X0", o.X0);
        info.AddValue("Y0", o.Y0);
      }
      
      public Object SetObjectData(Object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
      {
        global::Cairo.Matrix o = (global::Cairo.Matrix)obj;
        o.Init(info.GetDouble("Xx"),
               info.GetDouble("Yx"),
               info.GetDouble("Xy"),
               info.GetDouble("Yy"),
               info.GetDouble("X0"),
               info.GetDouble("Y0"));
        return null;
      }
    }
    
    internal sealed class CairoSolidPatternSerializationSurrogate: ISerializationSurrogate 
    {
      public void GetObjectData(Object obj, SerializationInfo info, StreamingContext context) 
      {
        global::Cairo.SolidPattern o = (global::Cairo.SolidPattern)obj;
        info.AddValue("Color", o.Color, typeof(global::Cairo.Color));
      }
      
      public Object SetObjectData(Object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
      {
        return new global::Cairo.SolidPattern((global::Cairo.Color)info.GetValue("Color", typeof(global::Cairo.Color)));
      }
    }
    
    internal sealed class CairoColorSerializationSurrogate: ISerializationSurrogate 
    {
      public void GetObjectData(Object obj, SerializationInfo info, StreamingContext context) 
      {
        global::Cairo.Color o = (global::Cairo.Color)obj;
        info.AddValue("R", o.R);
        info.AddValue("G", o.G);
        info.AddValue("B", o.B);
        info.AddValue("A", o.A);
      }
      
      public Object SetObjectData(Object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
      {
        return new global::Cairo.Color(info.GetDouble("R"), info.GetDouble("G"), info.GetDouble("B"), info.GetDouble("A"));
      }
    }
  }
}

