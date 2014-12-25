/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;

using DataMorphose.Model;
using DataMorphose.Model.Meta;

namespace DataMorphose.Deduce
{
  public class ColumnConstraints
  {
    public ColumnConstraints() {
    }

    public void DeduceConstraints(Database db) {
      foreach (Table table in db.Tables) {
        foreach (Column column in table.Columns) {
          DeduceUniqueValues(column);
        }
      }
    }

    public void DeducePrimaryKey(Table table) {
      foreach (Column column in table.Columns) {
        DeducePrimaryKey(column);
      }
    }

    private void DeducePrimaryKey(Column column) {
      // FIXME: Extract out in separate class implementing the strategy pattern.
      if (column.Meta.Name.ToLower().EndsWith("id"))
        column.Meta.Constraints.Add(ConstraintKind.PrimaryKey, new PrimaryKey(column));
    }

    private void DeduceUniqueValues(Column column) {
      HashSet<object> values = new HashSet<object>();
      foreach (object val in column.Values)
        if (!values.Add(val))
          return;
      column.Meta.Constraints.Add(ConstraintKind.Unique, null);
    }
  }
}

