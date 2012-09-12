/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;

namespace DataMorphose.Model.Meta
{

  public enum ConstraintKind {
    ForeignKey,
    NotNull,
    PrimaryKey,
    Unique
  }

  /// <summary>
  /// Represents logical relations such as foreign keys.
  /// </summary>
  public abstract class Constraint
  {
  }

  public class ForeignKeyConstraint : Constraint
  {
    private Table table;
    private Column column;
  }

  /// <summary>
  /// Represents the logical constraint primary key. It may be simple or complex PK.
  /// </summary>
  public class PrimaryKey : Constraint
  {
    private List<Column> columns = new List<Column>(2);

    public PrimaryKey(params Column[] columns) {
      this.columns.AddRange(columns);
    }

    /// <summary>
    /// The PK might be constituted from more than one column. This is complex PK.
    /// </summary>
    /// <returns>
    /// The complex key.
    /// </returns>
    public bool isComplexKey() {
      return columns.Count > 1;
    }
  }
}
