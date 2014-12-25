/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SolidV.MVC
{
  public static partial class ShapeHelper
  {
    private static SurrogateSelector surrogates = new SurrogateSelector();
    public static SurrogateSelector Surrogates {
      get { return surrogates; }
      set { surrogates = value; }
    }

    public static readonly StreamingContext CloneSC = new StreamingContext(StreamingContextStates.Clone);

    static ShapeHelper()
    {
      Cairo.SerializationSurrogates.RegisterSurrogates(surrogates);
    }

    /// <summary>
    /// Deep copy the object graph.
    /// </summary>
    /// <returns>The copy of object.</returns>
    /// <param name="obj">Object to copy. It must be serializible (including all object graph types).</param>
    public static object DeepCopy(this Object obj)
    {
      using (MemoryStream ms = new MemoryStream()) {
        BinaryFormatter bf = new BinaryFormatter(Surrogates, CloneSC);
        bf.Serialize(ms, obj);
        ms.Position = 0;
        return bf.Deserialize(ms);
      }
    }

    /// <summary>
    /// Deep copy the object graph.
    /// </summary>
    /// <returns>The copy of object.</returns>
    /// <param name="obj">Object to copy. It must be serializible (including all object graph types).</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public static T DeepCopy<T>(this T obj)
    {
      using (MemoryStream ms = new MemoryStream()) {
        BinaryFormatter bf = new BinaryFormatter(Surrogates, CloneSC);
        bf.Serialize(ms, obj);
        ms.Position = 0;
        return (T)bf.Deserialize(ms);
      }
    }
  }
}

