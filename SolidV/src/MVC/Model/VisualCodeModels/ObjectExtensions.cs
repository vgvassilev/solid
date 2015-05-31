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
  public static partial class ObjectExtensions
  {
    private static SurrogateSelector surrogates = new SurrogateSelector();
    public static SurrogateSelector Surrogates {
      get { return surrogates; }
      set { surrogates = value; }
    }

    public static readonly StreamingContext CloneSC = new StreamingContext(StreamingContextStates.Clone);

    static ObjectExtensions()
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
        if (obj == null) return default(T);
        BinaryFormatter bf = new BinaryFormatter(Surrogates, CloneSC);
        bf.Serialize(ms, obj);
        ms.Position = 0;
        return (T)bf.Deserialize(ms);
      }
    }

    public static byte[] ToArray(this Object obj)
    {
      using (MemoryStream ms = new MemoryStream()) {
        if (obj == null) return new byte[0];
        BinaryFormatter bf = new BinaryFormatter(Surrogates, CloneSC);
        bf.Serialize(ms, obj);
        ms.Position = 0;
        return ms.ToArray();
      }
    }

    public static Object FromArray(this byte[] array)
    {
      if (array.Length == 0) return null;
      MemoryStream ms = new MemoryStream();
      BinaryFormatter bf = new BinaryFormatter(Surrogates, CloneSC);
      ms.Write(array, 0, array.Length);
      ms.Position = 0;
      return bf.Deserialize(ms);
    }

  }
}

