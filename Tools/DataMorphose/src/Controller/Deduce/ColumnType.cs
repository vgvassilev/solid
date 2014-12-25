/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;

using DataMorphose.Model;

namespace DataMorphose.Deduce
{
  /// <summary>
  /// Class used to resolve the closest type to the contents of a column.
  /// </summary>
  public static class ColumnType
  {
    public static void ResolveColumnType(Column column) {
      column.Meta.Type = ResolveType(column.Values.ToArray());
    }

    public static Type ResolveType(object[] values) {
      HashSet<Type> types = new HashSet<Type>();
      Type result = null;
      for (int i = 0; i < values.Length; i++) {
        result = ColumnType.ResolveType(values[i]);
        types.Add(result);
      }
      // When we have just one element type just return it.
      if (types.Count == 1)
        return result;
      else {
        // null when inconclusive
        return null;
        //TODO: In the case of many we have to check whether they are similar to one another.
        // For example: When they are Int32 and Int64 we should return the enclosing type.
      }
    }

    public static Type ResolveType(object val) {
      Int32 int32Result;
      Int64 int64Result;
      DateTime dateTimeResult;

      if (Int32.TryParse(val.ToString(), out int32Result))
        return typeof(Int32);
      else if (Int64.TryParse(val.ToString(), out int64Result))
        return typeof(Int64);
      else if (DateTime.TryParse(val.ToString(), out dateTimeResult))
        return typeof(DateTime);

      return typeof(string);
    }
  }
}

